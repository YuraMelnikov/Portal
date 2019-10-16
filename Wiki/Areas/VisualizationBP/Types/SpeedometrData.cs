namespace Wiki.Areas.VisualizationBP.Types
{
    public class SpeedometrData
    {
        string period;
        int data;

        public SpeedometrData(string period, int data)
        {
            this.Period = period;
            this.Data = data;
        }

        public string Period { get => period; set => period = value; }
        public int Data { get => data; set => data = value; }
    }
}