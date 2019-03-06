using System.Linq;

namespace Wiki.Models
{
    public class CMO_PreOrderView
    {
        int id;
        string namePositions = "";

        public CMO_PreOrderView(CMO_PreOrder cMO_PreOrder)
        {
            this.id = cMO_PreOrder.id;
            this.namePositions = GetPositionName(cMO_PreOrder);
        }

        public int Id { get => id; set => id = value; }
        public string NamePositions { get => namePositions; set => namePositions = value; }


        string GetPositionName(CMO_PreOrder cMO_PreOrder)
        {
            foreach(var data in cMO_PreOrder.CMO_PositionPreOrder.ToList())
            {
                namePositions += data.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + data.CMO_TypeProduct.name + "; ";
            }
            return namePositions;
        }
    }
}