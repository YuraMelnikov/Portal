using System;

namespace Wiki.Areas.PZ.Models
{
    public class CorrectOrderList
    {
        private DateTime sortDate;
        private PZ_PlanZakaz pZ;

        public CorrectOrderList(PZ_PlanZakaz pZ_PlanZakazs)
        {
            this.PZ = pZ_PlanZakazs;
            if (pZ_PlanZakazs.timeArrDate != null)
            {
                SortDate = pZ_PlanZakazs.timeArrDate.Value;
            }
            else
            {
                SortDate = pZ_PlanZakazs.timeContractDate.Value;
            }
        }

        public DateTime SortDate { get => sortDate; set => sortDate = value; }
        public PZ_PlanZakaz PZ { get => pZ; set => pZ = value; }
    }
}