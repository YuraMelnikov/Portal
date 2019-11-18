select
*
From
(SELECT MSP_EpmProject_UserView.[№ заказа] as [№заказа], min(MSP_EpmAssignmentByDay_UserView.TimeByDay) as [Начало производства], max(MSP_EpmAssignmentByDay_UserView.TimeByDay) as [Окончание производства]
	FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
	ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView
	ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID
	AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
	ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND
	ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView
	ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
	WHERE
	(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork > 0) AND
	(ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес like '%ЭУ%' or 
	ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес like '%УСМК%' or 
	ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес like '%УСС%' or 
	ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес like '%УСР%' or 
	ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес like '%Уиш%')
group by MSP_EpmProject_UserView.[№ заказа]) as ManufacturingTable

select
*
From
(SELECT MSP_EpmProject_UserView.[№ заказа] as [№заказа], min(MSP_EpmAssignmentByDay_UserView.TimeByDay) as [Начало разработки]
	FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
	ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView
	ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID
	AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
	ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND
	ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView
	ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
	WHERE
	(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork > 0) AND
	(ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес like '%КБМ%')
group by MSP_EpmProject_UserView.[№ заказа]) as KBMStartTable

select
*
From
(SELECT MSP_EpmProject_UserView.[№ заказа] as [№заказа], max(MSP_EpmAssignmentByDay_UserView.TimeByDay) as [Окончание разработки]
	FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
	ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView
	ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID
	AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
	ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND
	ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView
	ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
	WHERE
	(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork > 0) AND
	(ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес like '%КБМ%' and ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskName like '%ведомость%')
group by MSP_EpmProject_UserView.[№ заказа]) as KBMFinishTable

select
*
From
(SELECT MSP_EpmProject_UserView.[№ заказа] as [№заказа], min(MSP_EpmAssignmentByDay_UserView.TimeByDay) as [Начало разработки]
	FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
	ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView
	ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID
	AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
	ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND
	ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView
	ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
	WHERE
	(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork > 0) AND
	(ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес like '%КБЭ%')
group by MSP_EpmProject_UserView.[№ заказа]) as KBEStartTable

select
*
From
(SELECT MSP_EpmProject_UserView.[№ заказа] as [№заказа], max(MSP_EpmAssignmentByDay_UserView.TimeByDay) as [Окончание разработки]
	FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
	ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView
	ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID
	AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
	ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND
	ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView
	ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
	WHERE
	(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork > 0) AND
	(ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес like '%КБЭ%' and ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskName like '%ведомость%')
group by MSP_EpmProject_UserView.[№ заказа]) as KBEFinishTable

select
*
from
(
SELECT [PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz] as [№заказа]
      ,min([PortalKATEK].[dbo].[View_RKD_ListMailVersion].[dateEvent]) as [Дата готовности первой версии РКД]
      ,[PortalKATEK].[dbo].[View_RKD_ListMailVersion].[text]
FROM [PortalKATEK].[dbo].[View_RKD_ListMailVersion]
where
[PortalKATEK].[dbo].[View_RKD_ListMailVersion].[text] like 'Отправлена черновая версия РКД в ТП'
group by [PortalKATEK].[dbo].[View_RKD_ListMailVersion].[text]
,[PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz]
order by
[PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz]
) as StartWorkToRKDTable

select
*
from
(SELECT [PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz] as [№заказа]
      ,min([PortalKATEK].[dbo].[View_RKD_ListMailVersion].[dateEvent]) as [Дата согласования РКД]
      ,[PortalKATEK].[dbo].[View_RKD_ListMailVersion].[text]
FROM [PortalKATEK].[dbo].[View_RKD_ListMailVersion]
where
[PortalKATEK].[dbo].[View_RKD_ListMailVersion].[text] like 'Получено согласование РКД'
group by [PortalKATEK].[dbo].[View_RKD_ListMailVersion].[text]
,[PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz]
order by
[PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz]) as ComplitedRKDTable