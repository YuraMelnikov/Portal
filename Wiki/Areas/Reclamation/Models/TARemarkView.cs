namespace Wiki.Areas.Reclamation.Models
{
    public class TARemarkView
    {
        int id_Reclamation;
        string linkToEdit;
        string linkToView;
        string orders;
        string textReclamation;
        string descriptionReclamation;
        string answers;
        string decision;
        string userToTA;
        string userCreate;
        string devisionReclamation;
        string leavelReclamation;
        string lastLeavelReclamation;

        public int Id_Reclamation { get => id_Reclamation; set => id_Reclamation = value; }
        public string LinkToEdit { get => linkToEdit; set => linkToEdit = value; }
        public string LinkToView { get => linkToView; set => linkToView = value; }
        public string Orders { get => orders; set => orders = value; }
        public string TextReclamation { get => textReclamation; set => textReclamation = value; }
        public string DescriptionReclamation { get => descriptionReclamation; set => descriptionReclamation = value; }
        public string Answers { get => answers; set => answers = value; }
        public string Decision { get => decision; set => decision = value; }
        public string UserToTA { get => userToTA; set => userToTA = value; }
        public string UserCreate { get => userCreate; set => userCreate = value; }
        public string DevisionReclamation { get => devisionReclamation; set => devisionReclamation = value; }
        public string LeavelReclamation { get => leavelReclamation; set => leavelReclamation = value; }
        public string LastLeavelReclamation { get => lastLeavelReclamation; set => lastLeavelReclamation = value; }
    }
}