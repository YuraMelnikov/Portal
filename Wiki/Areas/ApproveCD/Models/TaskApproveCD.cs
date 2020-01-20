using System;

namespace Wiki.Areas.ApproveCD.Models
{
    public class TaskApproveCD
    {
        public DateTime dateTime;
        public string action;
        public string user;
        public DateTime? deadline;
        public string order;
        public int typeTask;

        public TaskApproveCD(DateTime dateTime, string action, string user, 
            DateTime? deadline, string order, int typeTask)
        {
            this.dateTime = dateTime;
            this.action = action;
            this.user = user;
            this.deadline = deadline;
            this.order = order;
            this.typeTask = typeTask;
        }
    }
}