namespace Wiki.Areas.VisualizationBP.Types
{
    public class BlockProjectTasksState
    {
        string name;
        ElementProjectTasksState[] elementProjectTasksStates;

        public BlockProjectTasksState(int countElements)
        {
            ElementProjectTasksStates = new ElementProjectTasksState[countElements];
            for (int i = 0; i < countElements; i++)
            {
                ElementProjectTasksStates[i] = new ElementProjectTasksState();
            }
        }

        public BlockProjectTasksState(int countElements, string name)
        {
            this.name = name;
            ElementProjectTasksStates = new ElementProjectTasksState[countElements];
            for (int i = 0; i < countElements; i++)
            {
                ElementProjectTasksStates[i] = new ElementProjectTasksState();
            }
        }

        public ElementProjectTasksState[] ElementProjectTasksStates { get => elementProjectTasksStates; set => elementProjectTasksStates = value; }
        public string Name { get => name; set => name = value; }
    }
}