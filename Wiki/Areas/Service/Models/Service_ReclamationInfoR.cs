namespace Wiki.Areas.Service.Models
{
    public class Service_ReclamationInfoR
    {
        private int id;
        private string textData;

        public int Id { get => id; set => id = value; }
        public string TextData { get => textData; set => textData = value; }


        public Service_ReclamationInfoR(Service_ReclamationInfo reclamationInfo)
        {
            Id = reclamationInfo.id;
            TextData = reclamationInfo.textData;
        }
    }
}