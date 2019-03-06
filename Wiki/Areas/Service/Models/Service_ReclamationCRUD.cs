using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Wiki.Areas.Service.Models
{
    public class Service_ReclamationCRUD
    {
        private PortalKATEKEntities _db = new PortalKATEKEntities();

        public PortalKATEKEntities Db { get => _db; }

        public List<Service_Reclamation> ListAll()
        {
            return Db.Service_Reclamation.ToList();
        }

        public List<Service_Reclamation> GetListReclamationBeforeSerilise()
        {
            return Db.Service_Reclamation.ToList();
        }

        public int Add(Service_Reclamation reclamation, int[] service_TypeReclamation, int[] pZ_PlanZakaz)
        {
            reclamation.dateAdd = DateTime.Now;
            reclamation.addMission = false;
            Db.Service_Reclamation.Add(reclamation);
            Db.SaveChanges();
            AddService_ReclamationType(service_TypeReclamation, reclamation);
            AddService_ReclamationPZ(pZ_PlanZakaz, reclamation);

            return 1;
        }

        public int Update(Service_Reclamation reclamation, int[] service_TypeReclamation, int[] pZ_PlanZakaz)
        {
            Service_Reclamation updateReclamation = Db.Service_Reclamation.Find(reclamation.id);
            bool updateData = false;
            if (updateReclamation.id_Service_ReclamationWhoAdd != reclamation.id_Service_ReclamationWhoAdd)
            {
                updateReclamation.id_Service_ReclamationWhoAdd = reclamation.id_Service_ReclamationWhoAdd;
                updateData = true;
            }
            if (updateReclamation.id_Service_TypeDocument != reclamation.id_Service_TypeDocument)
            {
                updateReclamation.id_Service_TypeDocument = reclamation.id_Service_TypeDocument;
                updateData = true;
            }
            if (updateReclamation.numberDocument != reclamation.numberDocument)
            {
                updateReclamation.numberDocument = reclamation.numberDocument;
                updateData = true;
            }
            if (updateReclamation.dateDocument != reclamation.dateDocument)
            {
                updateReclamation.dateDocument = reclamation.dateDocument;
                updateData = true;
            }
            if (updateReclamation.description != reclamation.description)
            {
                updateReclamation.description = reclamation.description;
                updateData = true;
            }
            if (updateData == true) 
            {
                Db.Entry(updateReclamation).State = EntityState.Modified;
                Db.SaveChanges();
            }
            var listLastPz = Db.Service_ReclamationPZ.Where(d => d.id_Service_Reclamation == reclamation.id).ToList();
            foreach (var lastService_ReclamationPZ in listLastPz)
            {
                if (pZ_PlanZakaz.Where(d => d == lastService_ReclamationPZ.id_PZ_PlanZakaz).Count() == 0)
                {
                    Db.Service_ReclamationPZ.Remove(lastService_ReclamationPZ);
                    Db.SaveChanges();
                }
            }
            listLastPz = Db.Service_ReclamationPZ.Where(d => d.id_Service_Reclamation == reclamation.id).ToList();
            foreach (var newService_ReclamationPZ in pZ_PlanZakaz)
            {
                if (listLastPz.Where(d => d.id_PZ_PlanZakaz == newService_ReclamationPZ).Count() == 0)
                {
                    Service_ReclamationPZ newServiceReclamationPz = new Service_ReclamationPZ();
                    newServiceReclamationPz.id_PZ_PlanZakaz = newService_ReclamationPZ;
                    newServiceReclamationPz.id_Service_Reclamation = reclamation.id;
                    Db.Service_ReclamationPZ.Add(newServiceReclamationPz);
                    Db.SaveChanges();
                }
            }
            var listLastTypeList = Db.Service_ReclamationType.Where(d => d.id_Service_Reclamation == reclamation.id).ToList();
            foreach (var lastService_ReclamationType in listLastTypeList)
            {
                if (service_TypeReclamation.Where(d => d == lastService_ReclamationType.id_Service_TypeReclamation).Count() == 0)
                {
                    Db.Service_ReclamationType.Remove(lastService_ReclamationType);
                    Db.SaveChanges();
                }
            }
            listLastTypeList = Db.Service_ReclamationType.Where(d => d.id_Service_Reclamation == reclamation.id).ToList();
            foreach (var newType in service_TypeReclamation)
            {
                if (listLastTypeList.Where(d => d.id_Service_TypeReclamation == newType).Count() == 0)
                {
                    Service_ReclamationType newService_ReclamationType = new Service_ReclamationType();
                    newService_ReclamationType.id_Service_TypeReclamation = newType;
                    newService_ReclamationType.id_Service_Reclamation = reclamation.id;
                    Db.Service_ReclamationType.Add(newService_ReclamationType);
                    Db.SaveChanges();
                }
            }
            
            return 1;
        }

        private void AddService_ReclamationType(int[] service_TypeReclamation, Service_Reclamation reclamation)
        {
            foreach (var data in service_TypeReclamation)
            {
                Service_ReclamationType type = new Service_ReclamationType();
                type.id_Service_Reclamation = reclamation.id;
                type.id_Service_TypeReclamation = data;
                Db.Service_ReclamationType.Add(type);
                Db.SaveChanges();
            }
        }

        private void AddService_ReclamationPZ(int[] pZ_PlanZakaz, Service_Reclamation reclamation)
        {
            foreach (var data in pZ_PlanZakaz)
            {
                Service_ReclamationPZ type = new Service_ReclamationPZ();
                type.id_Service_Reclamation = reclamation.id;
                type.id_PZ_PlanZakaz = data;
                Db.Service_ReclamationPZ.Add(type);
                Db.SaveChanges();
            }
        }
    }
}