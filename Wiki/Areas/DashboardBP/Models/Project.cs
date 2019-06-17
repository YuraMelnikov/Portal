using System;

namespace Wiki.Areas.DashboardBP.Models
{
    public struct Project
    {
        public string name;
        public string id;
        public string owner;
        public ulong start;
        public ulong end;
        public Complited completed;
        public ulong contractDate;
        public int y;
    }
}