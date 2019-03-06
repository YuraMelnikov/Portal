
namespace Wiki.Models
{
    public class CMO_Description
    {
        public string planZakaz;
        public string typeProduct;

        public CMO_Description(string planZakaz, string typeProduct)
        {
            this.planZakaz = planZakaz;
            this.typeProduct = typeProduct;
        }
    }
}