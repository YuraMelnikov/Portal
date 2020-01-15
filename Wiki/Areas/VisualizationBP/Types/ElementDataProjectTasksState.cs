using System;

namespace Wiki.Areas.VisualizationBP.Types
{
    public class ElementDataProjectTasksState
    {
        double work;
        double remainingWork;
        DateTime startDate;
        DateTime finishDate;
        string name;
        string users;

        public double Work { get => work; set => work = value; }
        public double RemainingWork { get => remainingWork; set => remainingWork = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime FinishDate { get => finishDate; set => finishDate = value; }
        public string Name { get => name; set => name = value; }
        public string Users { get => users; set => users = value; }

        public ElementDataProjectTasksState()
        {

        }

        public ElementDataProjectTasksState(DashboardBPTaskInsert dashboardBPTaskInsert)
        {
            this.work = dashboardBPTaskInsert.TaskWork.Value;
            this.remainingWork = dashboardBPTaskInsert.TaskRemainingWork.Value;
            this.startDate = dashboardBPTaskInsert.TaskStartDate;
            this.finishDate = dashboardBPTaskInsert.TaskfinishDate;
            this.name = GetShortTaskName(dashboardBPTaskInsert.TaskWBS1);
            this.users = dashboardBPTaskInsert.AspNetUsers.CiliricalName.Substring(0, dashboardBPTaskInsert.AspNetUsers.CiliricalName.IndexOf(' '));
        }

        string GetShortTaskName(string wbsName)
        {
            if (wbsName == "ПР")
            {
                return "Предразраб. КБМ";
            }
            else if (wbsName == "ПЭ")
            {
                return "Предразраб. КБЭ";
            }
            else if (wbsName == "РМ")
            {
                return "Компл. РКД КБМ";
            }
            else if (wbsName == "РЭ")
            {
                return "Компл. РКД КБЭ";
            }
            else if (wbsName == "СМ")
            {
                return "Согл. РКД КБМ";
            }
            else if (wbsName == "СЭ")
            {
                return "Согл. РКД КБЭ";
            }
            else
            {
                return "";
            }
        }
    }
}