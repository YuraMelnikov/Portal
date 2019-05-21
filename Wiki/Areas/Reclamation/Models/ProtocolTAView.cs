namespace Wiki.Areas.Reclamation.Models
{
    public class ProtocolTAView
    {
        int id_Protocol;
        string linkToView;
        string linkToWord;
        string dateProtocol;
        int countReclamation;

        public int Id_Protocol { get => id_Protocol; set => id_Protocol = value; }
        public string LinkToView { get => linkToView; set => linkToView = value; }
        public string LinkToWord { get => linkToWord; set => linkToWord = value; }
        public string DateProtocol { get => dateProtocol; set => dateProtocol = value; }
        public int CountReclamation { get => countReclamation; set => countReclamation = value; }
    }
}