select
ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWBS
,ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
,ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceName
,concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.', (month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)+2) / 3) as [quartalFinishTask]
,iif(len(month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)) = 1,
concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.0',month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)),
concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.',month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate))) as [monthFinishTask],
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskPercentCompleted,
PortalKATEK.dbo.PZ_PlanZakaz.id as id_PZ_PlanZakaz,
ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] as [orderNumber],
ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] as [counterKO],
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskName,
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate,
substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) as [Devision],
substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 4, 5) as [DevisionKBE]
,PortalKATEK.dbo.PZ_TEO.SSM
,PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0105 as [bujetWorkers]
,PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0020 as [bujetManagers]
,MSP_EpmTask_UserView.TaskWork
,ProjectWebApp.dbo.MSP_EpmTask_UserView.нк as [normH]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM], 0) as [normHOrderKBM]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0105) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM]), 0) as [cost1NHWorkerKBM]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0020) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM]), 0) as [cost1NHManagerKBM]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0105) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0) as [accruedWorkerForTaskKBM]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0020) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0) as [accruedManagerForTaskKBM]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE], 0) as [normHOrderKBE]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0105) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE]), 0) as [cost1NHWorkerKBE]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0020) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE]), 0) as [cost1NHManagerKBE]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0105) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0) as [accruedWorkerForTaskKBE]
,iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * 0.0020) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0) as [accruedManagerForTaskKBE]
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView
INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID
LEFT JOIN ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
LEFT JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
LEFT JOIN ReportKATEK.dbo.CMKO_TimeWorkOnOrder on ReportKATEK.dbo.CMKO_TimeWorkOnOrder.numberOrder = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz like ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
left join PortalKATEK.dbo.PZ_TEO on PortalKATEK.dbo.PZ_TEO.Id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
where
(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')