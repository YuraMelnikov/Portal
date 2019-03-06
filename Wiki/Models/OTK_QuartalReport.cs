using System;

namespace Wiki.Models
{
    public class OTK_QuartalReport
    {
        public DateTime DateShip { get; set; }
        public int PlanZakaz { get; set; }
        public int CountBefore4 { get; set; }
        public int CountBefore8 { get; set; }
        public int CountBefore16 { get; set; }
        public int CountBeforeLarge { get; set; }
        public int CountBeforeNotClose { get; set; }
        public int CountBeforeAll { get; set; }
        public float ResultPercent { get; set; }
    }
}