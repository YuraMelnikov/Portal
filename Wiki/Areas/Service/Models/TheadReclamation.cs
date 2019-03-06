namespace Wiki.Areas.Service.Models
{
    public class TheadReclamation : Thead
    {
        public void SetColumnListAdminTypeReclamation()
        {
            columnList.Add(base.Edit);
            columnList.Add(base.View);
            columnList.Add(base.Paragraph);
            columnList.Add(base.Meaning);
            columnList.Add(base.Active);
        }
    }
}