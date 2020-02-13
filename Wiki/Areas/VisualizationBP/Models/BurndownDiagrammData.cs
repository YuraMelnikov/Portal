namespace Wiki.Areas.VisualizationBP.Models
{
    public class BurndownDiagrammData
    {
        string week;
        int valueBP;
        int valueP;
        int valueI;

        public string Week { get => week; set => week = value; }
        public int ValueBP { get => valueBP; set => valueBP = value; }
        public int ValueP { get => valueP; set => valueP = value; }
        public int ValueI { get => valueI; set => valueI = value; }
    }
}