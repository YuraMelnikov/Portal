namespace Wiki.Areas.VisualizationBP.Types
{
    public class ElementProjectTasksState
    {
        string name;
        string wbs;
        string wbs3;
        int leavel;
        ElementDataProjectTasksState[] elementDataProjectTasksStates;

        public ElementProjectTasksState()
        {

        }

        public ElementProjectTasksState(string name, int countTask, int leavel)
        {
            this.leavel = leavel;
            this.name = name;
            elementDataProjectTasksStates = new ElementDataProjectTasksState[countTask];
            for (int i = 0; i < countTask; i++)
            {
                elementDataProjectTasksStates[i] = new ElementDataProjectTasksState();
            }
        }

        public string WBS { get => wbs; set => wbs = value; }
        public string Name { get => name; set => name = value; }
        public ElementDataProjectTasksState[] ElementDataProjectTasksStates { get => elementDataProjectTasksStates; set => elementDataProjectTasksStates = value; }
        public string Wbs3 { get => wbs3; set => wbs3 = value; }
        public int Leavel { get => leavel; set => leavel = value; }
    }
}