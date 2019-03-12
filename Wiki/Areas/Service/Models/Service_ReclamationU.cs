using Newtonsoft.Json;
using System.Linq;

namespace Wiki.Areas.Service.Models
{
    public class Service_ReclamationU 
    {
        JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        public int id { get; set; }
        public int id_Service_TypeDocument { get; set; }
        public string dateAdd { get; set; }
        public int id_Service_ReclamationWhoAdd { get; set; }
        public string numberDocument { get; set; }
        public string dateDocument { get; set; }
        public string description { get; set; }
        public string dateClose { get; set; }
        public bool addMission { get; set; }
        public string[] pZ_PlanZakaz { get; set; }
        public string[] service_TypeReclamation { get; set; }
        public Service_ReclamationInfoR[] service_ReclamationInfo { get; set; }
        public Service_ReclamationCorrespondence[] service_ReclamationCorrespondence { get; set; }


        public Service_ReclamationU(Service_Reclamation reclamation)
        {
            id = reclamation.id;
            id_Service_TypeDocument = reclamation.id_Service_TypeDocument;
            dateAdd = JsonConvert.SerializeObject(reclamation.dateAdd, settings).Replace(@"""", "");
            id_Service_ReclamationWhoAdd = reclamation.id_Service_ReclamationWhoAdd;
            numberDocument = reclamation.numberDocument;
            dateDocument = JsonConvert.SerializeObject(reclamation.dateDocument, settings).Replace(@"""", "");
            description = reclamation.description;
            dateClose = JsonConvert.SerializeObject(reclamation.dateClose, settings).Replace(@"""", "");
            addMission = reclamation.addMission;
            var listPZ = reclamation.Service_ReclamationPZ.ToList();
            pZ_PlanZakaz = new string[listPZ.Count];
            for (int i = 0; i < listPZ.Count; i++)
            {
                pZ_PlanZakaz[i] = listPZ[i].id_PZ_PlanZakaz.ToString();
            }
            var listTypeReclamation = reclamation.Service_ReclamationType.ToList();
            service_TypeReclamation = new string[listPZ.Count];
            for (int i = 0; i < listTypeReclamation.Count; i++)
            {
                service_TypeReclamation[i] = listTypeReclamation[i].id_Service_TypeReclamation.ToString();
            }
            var reclamationInfoArray = reclamation.Service_ReclamationInfo.ToArray();
            service_ReclamationInfo = new Service_ReclamationInfoR[reclamationInfoArray.Length];
            for (int i = 0; i < reclamationInfoArray.Length; i++)
            {
                service_ReclamationInfo[i] = new Service_ReclamationInfoR(reclamationInfoArray[i]);
            }
        }
    }
}