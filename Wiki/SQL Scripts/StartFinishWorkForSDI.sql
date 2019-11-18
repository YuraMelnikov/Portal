select
ManufacturingTable.[№заказа],
ManufacturingTable.[Начало производства],
ManufacturingTable.[Окончание производства],
KBMStartTable.[Начало разработки] as [Начало разработки КБМ],
KBMFinishTable.[Окончание разработки] as [Окончание разработки КБМ],
KBEStartTable.[Начало разработки] as [Начало разработки КБЭ],
KBEFinishTable.[Окончание разработки] as [Окончание разработки КБЭ],
StartWorkToRKDTable.[Дата готовности первой версии РКД],
ComplitedRKDTable.[Дата согласования РКД]
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
	and MSP_EpmProject_UserView.[№ заказа] not like '%НИОКР%'
	and MSP_EpmProject_UserView.[№ заказа] not like '%Прочие%'
	and MSP_EpmProject_UserView.[№ заказа] not like '%Задан%'
	group by MSP_EpmProject_UserView.[№ заказа]) as ManufacturingTable left join
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
	group by MSP_EpmProject_UserView.[№ заказа]) as KBMStartTable on ManufacturingTable.[№заказа] = KBMStartTable.[№заказа] left join
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
	group by MSP_EpmProject_UserView.[№ заказа]) as KBMFinishTable on ManufacturingTable.[№заказа] = KBMFinishTable.[№заказа] left join
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
	group by MSP_EpmProject_UserView.[№ заказа]) as KBEStartTable on ManufacturingTable.[№заказа] = KBEStartTable.[№заказа] left join
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
	group by MSP_EpmProject_UserView.[№ заказа]) as KBEFinishTable on ManufacturingTable.[№заказа] = KBEFinishTable.[№заказа] left join
(SELECT [PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz] as [№заказа]
    ,min([PortalKATEK].[dbo].[View_RKD_ListMailVersion].[dateEvent]) as [Дата готовности первой версии РКД]
	FROM [PortalKATEK].[dbo].[View_RKD_ListMailVersion]
    where
	[PortalKATEK].[dbo].[View_RKD_ListMailVersion].[text] like 'Отправлена черновая версия РКД в ТП'
	group by [PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz]) as StartWorkToRKDTable on ManufacturingTable.[№заказа] = StartWorkToRKDTable.[№заказа] left join
(SELECT [PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz] as [№заказа]
    ,min([PortalKATEK].[dbo].[View_RKD_ListMailVersion].[dateEvent]) as [Дата согласования РКД]
	FROM [PortalKATEK].[dbo].[View_RKD_ListMailVersion]
	where [PortalKATEK].[dbo].[View_RKD_ListMailVersion].[text] like 'Получено согласование РКД'
	group by [PortalKATEK].[dbo].[View_RKD_ListMailVersion].[PlanZakaz]) as ComplitedRKDTable on ManufacturingTable.[№заказа] = ComplitedRKDTable.[№заказа]