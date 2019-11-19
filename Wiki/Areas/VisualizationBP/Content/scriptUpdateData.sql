DECLARE	@planHSSToYear int;


SET @planHSSToYear = 10000000;


DELETE [PortalKATEK].[dbo].[DashboardBP_ProjectTasks]
DELETE PortalKATEK.dbo.DashboardBP_ProjectList
INSERT INTO PortalKATEK.dbo.DashboardBP_ProjectList
SELECT
[PortalKATEK].dbo.PZ_PlanZakaz.id,
PortalKATEK.dbo.DashboardBP_State.id,
PortalKATEK.dbo.PZ_PlanZakaz.DateShipping,
PortalKATEK.dbo.PZ_PlanZakaz.dataOtgruzkiBP
,isnull(PortalKATEK.dbo.ProjectMSP_EpmProject_UserView.ProjectStartDate, getdate())
,isnull(PortalKATEK.dbo.ProjectMSP_EpmProject_UserView.ProjectDuration, 0)
,isnull(PortalKATEK.dbo.ProjectMSP_EpmProject_UserView.ProjectPercentCompleted, 0)
FROM
[PortalKATEK].[dbo].[PZ_PlanZakaz] 
left join [PortalKATEK].[dbo].[DashboardBP_State] on [PortalKATEK].[dbo].[DashboardBP_State].id > 0
left join [PortalKATEK].dbo.ProjectMSP_EpmProject_UserView on PortalKATEK.dbo.ProjectMSP_EpmProject_UserView.ProjectUID = PortalKATEK.dbo.PZ_PlanZakaz.ProjectUID
WHERE
[PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP > GETDATE()
and [PortalKATEK].dbo.DashboardBP_State.active = 1


DELETE [PortalKATEK].[dbo].[DashboardBP_HSSPO]
INSERT INTO [PortalKATEK].[dbo].[DashboardBP_HSSPO]
SELECT     
[PortalKATEK].[dbo].[DashboardBP_State].id,
PortalKatek.dbo.PZ_PlanZakaz.Id AS OrderPZ, 
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID, 
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskName, 
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay, 
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 
PortalKatek.dbo.PZ_TEO.Rate AS RatePlan, 
PortalKatek.dbo.PZ_TEO.SSM AS SSMPlan, 
iif(exportImport.dbo.planZakaz.SSfact is null, 0, exportImport.dbo.planZakaz.SSfact) AS SSMFact, 
PortalKatek.dbo.PZ_TEO.SSR AS SSRPlan, 
PortalKatek.dbo.PZ_TEO.IzdKom AS ik, 
PortalKatek.dbo.PZ_TEO.IzdPPKredit AS ppk, 
PortalKatek.dbo.PZ_TEO.PI, 
PortalKatek.dbo.PZ_TEO.NOP, 
PortalKatek.dbo.PZ_TEO.Rate / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] AS RateHoure,
PortalKatek.dbo.PZ_TEO.SSM / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] AS ssmHoure, 
iif(exportImport.dbo.planZakaz.SSfact is null, 0, exportImport.dbo.planZakaz.SSfact) / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] AS ssmFactHoure,
PortalKatek.dbo.PZ_TEO.SSR / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] AS ssrHoure, 
PortalKatek.dbo.PZ_TEO.IzdKom / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] AS ikHoure, 
PortalKatek.dbo.PZ_TEO.IzdPPKredit / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] AS ppkHoure, 
PortalKatek.dbo.PZ_TEO.PI / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] AS piHoure, 
PortalKatek.dbo.PZ_TEO.NOP / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] AS nopHoure, 
PortalKatek.dbo.PZ_TEO.Rate / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xRate, 
PortalKatek.dbo.PZ_TEO.SSM / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xSSM, 
iif(exportImport.dbo.planZakaz.SSfact is null, 0, exportImport.dbo.planZakaz.SSfact) / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xSSMFact, 
PortalKatek.dbo.PZ_TEO.SSR / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xSSR, 
PortalKatek.dbo.PZ_TEO.IzdKom / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xIK, 
PortalKatek.dbo.PZ_TEO.IzdPPKredit / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xPPK, 
PortalKatek.dbo.PZ_TEO.PI / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xPI, 
PortalKatek.dbo.PZ_TEO.NOP / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xNOP
,iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay)) = 1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.0', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay))) as [month], 
year(MSP_EpmAssignmentByDay_UserView.TimeByDay) as [year],
concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', 
(month(MSP_EpmAssignmentByDay_UserView.TimeByDay) + 2) / 3) AS [quartal], 
iiF(len(datepart(ISO_WEEK, ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay))=1,
concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',datepart(ISO_WEEK, ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay)),
concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',datepart(ISO_WEEK, ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay))) as [week]
FROM         ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN
                      ProjectWebApp.dbo.MSP_EpmTask_UserView ON 
                      ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN
                      ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON 
                      ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND 
                      ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
                      ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON 
                      ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND 
                      ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN
                      ProjectWebApp.dbo.MSP_EpmResource_UserView ON 
                      ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID LEFT OUTER JOIN
                      PortalKatek.dbo.PZ_PlanZakaz ON CONVERT(varchar, PortalKatek.dbo.PZ_PlanZakaz.PlanZakaz) = ProjectWebApp.dbo.MSP_EpmProject_UserView.[� ������] LEFT OUTER JOIN
                      PortalKatek.dbo.PZ_TEO ON PortalKatek.dbo.PZ_TEO.Id_PlanZakaz = PortalKatek.dbo.PZ_PlanZakaz.Id LEFT OUTER JOIN
                      exportImport.dbo.planZakaz ON exportImport.dbo.planZakaz.Zakaz = PortalKatek.dbo.PZ_PlanZakaz.PlanZakaz LEFT OUTER JOIN
                      ReportKATEK.dbo.ProjectWorkPO ON ReportKATEK.dbo.ProjectWorkPO.[� ������] = ProjectWebApp.dbo.MSP_EpmProject_UserView.[� ������] left join
					  [PortalKATEK].[dbo].[DashboardBP_State] on [PortalKATEK].[dbo].[DashboardBP_State].id > 0
WHERE     
[PortalKATEK].[dbo].[DashboardBP_State].active = 1
and
(ProjectWebApp.dbo.MSP_EpmProject_UserView.[� ������] NOT LIKE '%�����%') AND 
                      (ProjectWebApp.dbo.MSP_EpmProject_UserView.[� ������] NOT LIKE '%�����%') AND 
                      (ProjectWebApp.dbo.MSP_EpmProject_UserView.[� ������] NOT LIKE '%����%') AND (ProjectWebApp.dbo.MSP_EpmProject_UserView.[���-�� ��] > 0) AND 
                      (ProjectWebApp.dbo.MSP_EpmResource_UserView.����� LIKE '%��%' OR
                      ProjectWebApp.dbo.MSP_EpmResource_UserView.����� LIKE '%��%' OR
                      ProjectWebApp.dbo.MSP_EpmResource_UserView.����� LIKE '%��%') AND (ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork > 0)


DELETE [PortalKATEK].[dbo].[DashboardBP_ProjectTasks]
INSERT INTO [PortalKATEK].[dbo].[DashboardBP_ProjectTasks]
SELECT
  PortalKATEK.dbo.PWA_TasksForBP.TaskBaseline0Duration,
  PortalKATEK.dbo.PWA_TasksForBP.TaskBaseline0StartDate
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskBaseline0FinishDate
  ,PortalKATEK.dbo.PWA_TasksForBP.AssignmentBaseline0Work
  ,isnull(PortalKATEK.dbo.PWA_TasksForBP.TaskDuration, 0)
  ,isnull(PortalKATEK.dbo.PWA_TasksForBP.TaskRemainingDuration, 0)
  ,isnull(PortalKATEK.dbo.PWA_TasksForBP.TaskActualDuration, 0)
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskStartDate
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskFinishDate
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskWork
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskRemainingWork
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskActualWork
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskPercentWorkCompleted
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskPercentCompleted
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskPhysicalPercentCompleted
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskDeadline
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskIsCritical
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskPriority
  ,PortalKATEK.dbo.PWA_TasksForBP.��
  ,PortalKATEK.dbo.WBS.id as i1
  ,PortalKATEK.dbo.DashboardBP_ProjectList.id
  ,PortalKATEK.dbo.PWA_TasksForBP.TaskUID
  ,PortalKATEK.dbo.AspNetUsers.Id
  from PortalKATEK.dbo.DashboardBP_ProjectList left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.Id = PortalKATEK.dbo.DashboardBP_ProjectList.id_PZ_PlanZakaz
  left join PortalKATEK.dbo.PWA_TasksForBP on PortalKATEK.dbo.PWA_TasksForBP.ProjectUID = PortalKATEK.dbo.PZ_PlanZakaz.ProjectUID
  left join PortalKATEK.dbo.WBS on PortalKATEK.dbo.wbs.WBSName = PortalKATEK.dbo.PWA_TasksForBP.TaskWBS
  left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.ResourceUID = PortalKATEK.dbo.PWA_TasksForBP.ResourceUID
  where
  (PortalKATEK.dbo.PWA_TasksForBP.TaskDuration is not null)
  delete [PortalKATEK].[dbo].[DashboardBP_ProjectTasks] where [PortalKATEK].[dbo].[DashboardBP_ProjectTasks].id_WBS is null


DELETE PortalKATEK.dbo.DashboardBP_HSSPOSmall
insert into PortalKATEK.dbo.DashboardBP_HSSPOSmall
SELECT 
PortalKATEK.dbo.DashboardBP_State.id
,PortalKATEK.dbo.DashboardBP_HSSPO.year
,month(PortalKATEK.dbo.DashboardBP_HSSPO.timeByDay)
,convert(int,sum(PortalKATEK.dbo.DashboardBP_HSSPO.xSsm))
  FROM [PortalKATEK].[dbo].[DashboardBP_State]
  left join PortalKATEK.dbo.DashboardBP_HSSPO on PortalKATEK.dbo.DashboardBP_HSSPO.id_DashboardBP_State = PortalKATEK.dbo.DashboardBP_State.id
  where
  active = 1
  group by
  PortalKATEK.dbo.DashboardBP_State.id
,PortalKATEK.dbo.DashboardBP_HSSPO.year
,month(PortalKATEK.dbo.DashboardBP_HSSPO.timeByDay)


DELETE PortalKATEK.dbo.DashboardHSSPlan
INSERT INTO PortalKATEK.dbo.DashboardHSSPlan
select  sum(PortalKATEK.dbo.DashboardBP_HSSPOSmall.[data]), @planHSSToYear
from PortalKATEK.dbo.DashboardBP_HSSPOSmall left join PortalKATEK.dbo.DashboardBP_State on PortalKATEK.dbo.DashboardBP_State.id = PortalKATEK.dbo.DashboardBP_HSSPOSmall.id_DashboardBP_State
where PortalKATEK.dbo.DashboardBP_HSSPOSmall.year = year(GETDATE()) and PortalKATEK.dbo.DashboardBP_State.id is not null


