delete [PortalKATEK].[dbo].[DashboardTV_BasicPlanData] 
insert into [PortalKATEK].[dbo].[DashboardTV_BasicPlanData] 
select
sum(iif(MSP_EpmAssignmentByDay_UserView.TimeByDay < getdate(), [PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 0)) 
,sum(iif(MSP_EpmAssignmentByDay_UserView.TimeByDay < getdate(), [PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 0)) / [PortalKATEK].[dbo].[DashboardTV_MonthPlan].[data] * 100 
,sum([PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) 
,sum([PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) / [PortalKATEK].[dbo].[DashboardTV_MonthPlan].[data] * 100 
,sum(iif(MSP_EpmAssignmentByDay_UserView.TimeByDay < getdate(), ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 0)) 
,T2.[count]
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView
INNER JOIN
ProjectWebApp.dbo.MSP_EpmTask_UserView
ON
ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView
ON
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID
AND
ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignment_UserView
ON
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID
AND
ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmResource_UserView
ON
ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
left join [PortalKATEK].[dbo].[PZ_PlanZakaz] on convert(varchar,[PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz) = MSP_EpmProject_UserView.[¹ çàêàçà]
left join [PortalKATEK].[dbo].[PZ_TEO] on [PortalKATEK].[dbo].[PZ_TEO].Id_PlanZakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].id
left join [exportImport].[dbo].[planZakaz] on [exportImport].[dbo].[planZakaz].Zakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz
left join [ReportKATEK].[dbo].[ProjectWorkPO] on [ReportKATEK].[dbo].[ProjectWorkPO].[¹ çàêàçà] = MSP_EpmProject_UserView.[¹ çàêàçà]
left join [PortalKATEK].[dbo].[DashboardTV_MonthPlan] on [PortalKATEK].[dbo].[DashboardTV_MonthPlan].[data] > 0
left join (select count(*) as [count]
From (select convert(date, MSP_EpmAssignmentByDay_UserView.TimeByDay) as [count]
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView
INNER JOIN
ProjectWebApp.dbo.MSP_EpmTask_UserView
ON
ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView
ON
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID
AND
ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignment_UserView
ON
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID
AND
ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmResource_UserView
ON
ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
WHERE
(iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay)) = 1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.0', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay))) = 
iif(len(month(getdate())) = 1, concat(year(getdate()), '.0', 
month(getdate())), concat(year(getdate()), '.', 
month(getdate()))))
and
(MSP_EpmResource_UserView.[ÑÄÐåñ] like '%ÓÑ%' or MSP_EpmResource_UserView.[ÑÄÐåñ] like '%ÓÈ%' or MSP_EpmResource_UserView.[ÑÄÐåñ] like '%ÝÓ%')
and
(MSP_EpmAssignmentByDay_UserView.TimeByDay < getdate())
and
(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork > 0)
group by convert(date, MSP_EpmAssignmentByDay_UserView.TimeByDay)) as T1) as T2 on t2.[count] > 0
WHERE
(iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay)) = 1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.0', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay))) = 
iif(len(month(getdate())) = 1, concat(year(getdate()), '.0', 
month(getdate())), concat(year(getdate()), '.', 
month(getdate()))))
and
(MSP_EpmResource_UserView.[ÑÄÐåñ] like '%ÓÑ%' or MSP_EpmResource_UserView.[ÑÄÐåñ] like '%ÓÈ%' or MSP_EpmResource_UserView.[ÑÄÐåñ] like '%ÝÓ%')
and
(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork > 0)
group by
[PortalKATEK].[dbo].[DashboardTV_MonthPlan].[data]
,T2.[count]


delete [PortalKATEK].[dbo].[DashboardTV_DataForProjectPortfolio]
insert into [PortalKATEK].[dbo].[DashboardTV_DataForProjectPortfolio]
SELECT
MSP_EpmProject_UserView.[¹ çàêàçà]
,isnull(convert(int,sum([PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork)), 0)
,min(MSP_EpmAssignmentByDay_UserView.TimeByDay)
,max(MSP_EpmAssignmentByDay_UserView.TimeByDay)
,[PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP
,ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectRemainingDuration
,ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectDuration
,ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectPercentCompleted
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView
 join [PortalKATEK].[dbo].[PZ_PlanZakaz] on convert(varchar,[PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz) = MSP_EpmProject_UserView.[¹ çàêàçà]
INNER JOIN
ProjectWebApp.dbo.MSP_EpmTask_UserView
ON
ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView
ON
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID
AND
ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignment_UserView
ON
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID
AND
ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmResource_UserView
ON
ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
left join [PortalKATEK].[dbo].[PZ_TEO] on [PortalKATEK].[dbo].[PZ_TEO].Id_PlanZakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].id
left join [exportImport].[dbo].[planZakaz] on [exportImport].[dbo].[planZakaz].Zakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz
left join [ReportKATEK].[dbo].[ProjectWorkPO] on [ReportKATEK].[dbo].[ProjectWorkPO].[¹ çàêàçà] = MSP_EpmProject_UserView.[¹ çàêàçà]
WHERE
([PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP > getdate())
and
(MSP_EpmResource_UserView.[ÑÄÐåñ] like '%ÓÑ%' or MSP_EpmResource_UserView.[ÑÄÐåñ] like '%ÓÈ%' or MSP_EpmResource_UserView.[ÑÄÐåñ] like '%ÝÓ%')
and
(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork > 0)
and
([PortalKATEK].[dbo].[PZ_TEO].SSM > 0)
group by
iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay)) = 1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.0', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay))), 
MSP_EpmProject_UserView.[¹ çàêàçà]
,[PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP
,ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectRemainingDuration
,ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectDuration
,ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectPercentCompleted

delete [PortalKATEK].[dbo].[WBS_BP]
  insert into [PortalKATEK].[dbo].[WBS_BP] 
  select
  [PortalKATEK].[dbo].[PZ_PlanZakaz].Id
  ,[PortalKATEK].[dbo].[WBS].id
  ,0
  ,[PortalKATEK].[dbo].[PWA_EmpTask].TaskStartDate
  ,[PortalKATEK].[dbo].[PWA_EmpTask].TaskFinishDate
  ,[PortalKATEK].[dbo].[PWA_EmpTask].TaskWork
  ,[PortalKATEK].[dbo].[PWA_EmpTask].TaskPercentWorkCompleted
  ,[PortalKATEK].[dbo].[PWA_EmpTask].TaskPercentWorkCompleted
  FROM [PortalKATEK].[dbo].[PWA_EmpTask] left join [PortalKATEK].[dbo].[PZ_PlanZakaz] on 
  [PortalKATEK].[dbo].[PZ_PlanZakaz].ProjectUID = [PortalKATEK].[dbo].[PWA_EmpTask].ProjectUID
  left join [PortalKATEK].[dbo].[WBS] on [PortalKATEK].[dbo].[WBS].WBSName = [PortalKATEK].[dbo].[PWA_EmpTask].TaskWBS
  where
  [PortalKATEK].[dbo].[WBS].id is not null 
  and [PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP > getdate()

update [PortalKATEK].[dbo].[PlanVerificationItems] set [PortalKATEK].[dbo].[PlanVerificationItems].verificationDateInPrj = [PortalKATEK].[dbo].[WBS_BP].[start]
  FROM [PortalKATEK].[dbo].[PZ_PlanZakaz]
  left join [PortalKATEK].[dbo].[WBS_BP] on [PortalKATEK].[dbo].[WBS_BP].id_PZ_PlanZakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].Id
  left join [PortalKATEK].[dbo].[PlanVerificationItems] on [PortalKATEK].[dbo].[PlanVerificationItems].id_PZ_PlanZakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].Id
  where [PortalKATEK].[dbo].[WBS_BP].id_WBS = 79