using System;

namespace Wiki.Models
{
    public class RKD_ForTaskList
    {
        //typeTask for color string report
        //1 - this is desparthing (white)

        int id_RKD_Order;
        int planZakazName;
        DateTime dateEvent;
        string textData;
        int typeTask;
        string userID;
        string taskFinishDate;

        public RKD_ForTaskList(int id_RKD_Order, int planZakazName, DateTime dateEvent, string textData, int typeTask, string userID, string taskFinishDate)
        {
            Id_RKD_Order = id_RKD_Order;
            PlanZakazName = planZakazName;
            DateEvent = dateEvent;
            TextData = textData;
            TypeTask = typeTask;
            UserID = userID;
            TaskFinishDate = taskFinishDate;
        }

        public int Id_RKD_Order { get => id_RKD_Order; set => id_RKD_Order = value; }
        public int PlanZakazName { get => planZakazName; set => planZakazName = value; }
        public DateTime DateEvent { get => dateEvent; set => dateEvent = value; }
        public string TextData { get => textData; set => textData = value; }
        public int TypeTask { get => typeTask; set => typeTask = value; }
        public string UserID { get => userID; set => userID = value; }
        public string TaskFinishDate { get => taskFinishDate; set => taskFinishDate = value; }
    }
}