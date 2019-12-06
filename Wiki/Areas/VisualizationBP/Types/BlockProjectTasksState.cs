namespace Wiki.Areas.VisualizationBP.Types
{
    public class BlockProjectTasksState
    {
        ElementProjectTasksState[] elementProjectTasksStates;

        public BlockProjectTasksState(int countElements)
        {
            ElementProjectTasksStates = new ElementProjectTasksState[countElements];
            for (int i = 0; i < countElements; i++)
            {
                ElementProjectTasksStates[i] = new ElementProjectTasksState();
            }
        }

        public ElementProjectTasksState[] ElementProjectTasksStates { get => elementProjectTasksStates; set => elementProjectTasksStates = value; }
    }
}