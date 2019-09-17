-- Every month
delete [PortalKATEK].[dbo].[DashboardTV_MonthPlan]
insert into [PortalKATEK].[dbo].[DashboardTV_MonthPlan]
SELECT
sum([PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) as [ÕÑÑ]
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
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork > 0
group by
iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay)) = 1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.0', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay)))

--basic plan
delete [PortalKATEK].[dbo].[DashboardTV_BasicPlanData] 
insert into [PortalKATEK].[dbo].[DashboardTV_BasicPlanData] 
select
sum(iif(MSP_EpmAssignmentByDay_UserView.TimeByDay < getdate(), [PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 0)) 
,sum(iif(MSP_EpmAssignmentByDay_UserView.TimeByDay < getdate(), [PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 0)) / [PortalKATEK].[dbo].[DashboardTV_MonthPlan].[data] * 100 
,sum([PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) 
,sum([PortalKATEK].[dbo].[PZ_TEO].SSM / [ReportKATEK].[dbo].[ProjectWorkPO].[AssignmentWork] * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Êîë-âî ÏÎ] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) / [PortalKATEK].[dbo].[DashboardTV_MonthPlan].[data] * 100 
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