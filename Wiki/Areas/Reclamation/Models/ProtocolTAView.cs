namespace Wiki.Areas.Reclamation.Models
{
    public class ProtocolTAView
    {
        int id_Protocol;
        string linkToView;
        string linkToWord;
        string dateProtocol;
        int countReclamation;

        string firstPartLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getID('";
        string secondPartLink = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
        string firstPartLinkToWord = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return downloadProtocol('";
        string secondPartLinkToWord = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-download-alt" + '\u0022' + "></span></a></td>";

        public int Id_Protocol { get => id_Protocol; set => id_Protocol = value; }
        public string LinkToView { get => linkToView; set => linkToView = value; }
        public string LinkToWord { get => linkToWord; set => linkToWord = value; }
        public string DateProtocol { get => dateProtocol; set => dateProtocol = value; }
        public int CountReclamation { get => countReclamation; set => countReclamation = value; }

        public ProtocolTAView(Reclamation_TechnicalAdviceProtocol protocol)
        {
            int id_Protocol = protocol.id;
            this.id_Protocol = id_Protocol;
            dateProtocol = protocol.date.ToString().Substring(0, 10);
            countReclamation = protocol.Reclamation_TechnicalAdviceProtocolPosition.Count;
            linkToView = firstPartLink + id_Protocol + secondPartLink;
            linkToWord = GetLinkToWord(id_Protocol);
        }

        string GetLinkToWord(int id_Reclamation)
        {
            string link = "";
            //< button type = "button" class="btn-xs btn-primary" data-toggle="modal" data-target="#tablesModal" onclick="clearTextBoxTableOrders();">Сформировать таблички</button>
            return link;
        }

    }
}