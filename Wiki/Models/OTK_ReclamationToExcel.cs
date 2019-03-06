namespace Wiki.Models
{
    public class OTK_ReclamationToExcel
    {
        public int PlanZakaz { get; set; }
        public string Type { get; set; }
        public bool Close { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public float WorkOtk { get; set; }
        public float WorkPo { get; set; }
        public string Answer { get; set; }
        public string Devision { get; set; }
    }
}