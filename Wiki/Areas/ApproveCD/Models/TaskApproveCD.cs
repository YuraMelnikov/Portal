using System;

namespace Wiki.Areas.ApproveCD.Models
{
    public class TaskApproveCD
    {
        public DateTime dateTime;
        public string action;
        public string user;
        public DateTime? deadline;

        public TaskApproveCD(DateTime dateTime, string action, string user, DateTime? deadline)
        {
            this.dateTime = dateTime;
            this.action = action;
            this.user = user;
            this.deadline = deadline;
        }
    }
}