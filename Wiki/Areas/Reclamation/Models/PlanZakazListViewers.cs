﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class PlanZakazListViewers
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        List<PlanZakazViwers> planZakazViwers;

        public List<PlanZakazViwers> PlanZakazViwers { get => planZakazViwers; set => planZakazViwers = value; }

        void InitializationList()
        {
            planZakazViwers = new List<PlanZakazViwers>();
        }

        public void GetPlanZakazs()
        {
            InitializationList();
            foreach (var data in db.PZ_PlanZakaz.ToList())
            {
                PlanZakazViwers planZakazViwers = new PlanZakazViwers(data);
                AddPlanZakazViwersList(planZakazViwers);
            }
        }

        public void GetPlanZakazs(int id_Devision)
        {
            InitializationList();
            foreach (var data in db.PZ_PlanZakaz.ToList())
            {
                PlanZakazViwers planZakazViwers = new PlanZakazViwers(data, id_Devision);
                AddPlanZakazViwersList(planZakazViwers);
            }
        }

        public void GetPlanZakazs(int id_Devision, bool sh)
        {
            InitializationList();
            List<PZ_PlanZakaz> list = new List<PZ_PlanZakaz>();
            if (sh == true)
                list = db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP < DateTime.Now).ToList();
            else
                list = db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP >= DateTime.Now).ToList();
            foreach (var data in list)
            {
                PlanZakazViwers planZakazViwers = new PlanZakazViwers(data, id_Devision);
                AddPlanZakazViwersList(planZakazViwers);
            }
        }

        bool AddPlanZakazViwersList(PlanZakazViwers planZakazViwers)
        {
            if (planZakazViwers.ReclamationCount > 0)
                PlanZakazViwers.Add(planZakazViwers);
            return true;
        }
    }
}