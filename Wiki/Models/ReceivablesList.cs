using System.Collections.Generic;
using System.Linq;

namespace Wiki.Models
{
    public class ReceivablesList
    {
        private PortalKATEKEntities _db = new PortalKATEKEntities();
        List<Receivables> list = new List<Receivables>();
        readonly int idTask = 28;

        public List<Receivables> List { get => list; set => list = value; }

        public List<Receivables> GetActiveWork()
        {
            var reclamationList = GetReclamationList();
            foreach (var debitWork in GetActiveDebit_WorkBit())
            {
                tmpRecl recl = _db.tmpRecl.Where(d => d.Order == debitWork.PZ_PlanZakaz.PlanZakaz).FirstOrDefault();
                Receivables receivables = new Receivables(debitWork, recl);
                list.Add(receivables);
            }

            return list;
        }

        public List<Receivables> GetCloseWork()
        {
            var reclamationList = GetReclamationList();
            foreach (var debitWork in GetActiveDebit_WorkBit())
            {
                tmpRecl recl = _db.tmpRecl.Where(d => d.Order == debitWork.PZ_PlanZakaz.PlanZakaz).FirstOrDefault();
                Receivables receivables = new Receivables(debitWork, recl);
                list.Add(receivables);
            }

            return list;
        }

        List<Debit_WorkBit> GetActiveDebit_WorkBit()
        {
            return _db.Debit_WorkBit
                        .Where(d => d.close == false)
                        .Where(d => d.id_TaskForPZ == idTask)
                        .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                        .ToList();
        }

        List<Debit_WorkBit> GetCloseDebit_WorkBit()
        {
            return _db.Debit_WorkBit
                        .Where(d => d.close == true)
                        .Where(d => d.id_TaskForPZ == idTask)
                        .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                        .ToList();
        }

        List<tmpRecl> GetReclamationList()
        {
            return _db.tmpRecl.ToList();
        }
    }
}