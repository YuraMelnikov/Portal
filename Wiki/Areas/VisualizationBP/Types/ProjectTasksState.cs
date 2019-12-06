namespace Wiki.Areas.VisualizationBP.Types
{
    public class ProjectTasksState
    {
        BlockProjectTasksState[] blockProjectTasksStates;

        public ProjectTasksState(int countElements)
        {
            int countBlocks = 5;
            BlockProjectTasksStates = new BlockProjectTasksState[countBlocks];
            for (int i = 0; i < countBlocks; i++)
            {
                BlockProjectTasksStates[i] = new BlockProjectTasksState(countElements);
            }
        }

        public BlockProjectTasksState[] BlockProjectTasksStates { get => blockProjectTasksStates; set => blockProjectTasksStates = value; }
    }
}