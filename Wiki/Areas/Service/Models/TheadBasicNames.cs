namespace Wiki.Areas.Service.Models
{
    public class TheadBasicNames
    {
        private string edit = "Ред.";
        private string view = "См.";
        private string paragraph = "№ п/п";
        private string name = "Наименование";
        private string active = "Активная запись";
        private string meaning = "Значение";

        public string Meaning { get => meaning; }
        protected string Edit { get => edit; }
        protected string View { get => view; }
        protected string Paragraph { get => paragraph; }
        protected string Name { get => name; }
        protected string Active { get => active; }
    }
}