﻿namespace Wiki.Areas.VisualizationBP.Types
{
    public class ElementProjectTasksState
    {
        string name;
        ElementDataProjectTasksState[] elementDataProjectTasksStates;

        public ElementProjectTasksState()
        {

        }

        public ElementProjectTasksState(string name, int countTask)
        {
            this.name = name;
            elementDataProjectTasksStates = new ElementDataProjectTasksState[countTask];
            for (int i = 0; i < countTask; i++)
            {
                elementDataProjectTasksStates[i] = new ElementDataProjectTasksState();
            }

        }

        public string Name { get => name; set => name = value; }
        public ElementDataProjectTasksState[] ElementDataProjectTasksStates { get => elementDataProjectTasksStates; set => elementDataProjectTasksStates = value; }
    }
}