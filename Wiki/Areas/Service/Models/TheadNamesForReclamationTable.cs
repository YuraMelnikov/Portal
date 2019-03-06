namespace Wiki.Areas.Service.Models
{
    public class TheadNamesForReclamationTable
    {
        protected PortalKATEKEntities _db = new PortalKATEKEntities();
        private string view = "См.";
        private string edit = "Ред.";
        private string paragraphNumber = "№ п/п";
        private string orders = "Заказ/ы";
        private string dateCreate = "Создана";
        private string type = "Тип";
        private string executor = "Ответственный";
        private string folder = "Папка";

        public string View
        {
            get => view;
        }
        public string Edit
        {
            get => edit;
        }
        public string ParagraphNumber
        {
            get => paragraphNumber;
        }
        public string Orders
        {
            get => orders;
        }
        public string DateCreate
        {
            get => dateCreate;
        }
        public string ShotDescription { get; } = "Описание";
        public string Type
        {
            get => type;
        }
        public string Executor
        {
            get => executor;
        }
        public string Folder
        {
            get => folder;
        }
    }
}