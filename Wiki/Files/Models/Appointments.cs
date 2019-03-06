using System;

namespace Wiki.Models
{
    public class Appointments
    {
        public string ConversationTopic { get; set; }
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Organizer { get; set; }
        public int ReminderMinutesBeforeStart { get; set; }
        public string RequiredAttendees { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}