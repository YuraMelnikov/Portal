insert into [PortalKATEK_TEST].[dbo].[DashboardBP_HSSPO]
SELECT     
[PortalKATEK_test].[dbo].[DashboardBP_State].id,
dbo.PZ_PlanZakaz.Id AS OrderPZ, 
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID, 
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskName, 
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay, 
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 
dbo.PZ_TEO.Rate AS RatePlan, 
dbo.PZ_TEO.SSM AS SSMPlan, 
iif(exportImport.dbo.planZakaz.SSfact is null, 0, exportImport.dbo.planZakaz.SSfact) AS SSMFact, 
dbo.PZ_TEO.SSR AS SSRPlan, 
dbo.PZ_TEO.IzdKom AS ik, 
dbo.PZ_TEO.IzdPPKredit AS ppk, 
dbo.PZ_TEO.PI, 
dbo.PZ_TEO.NOP, 
dbo.PZ_TEO.Rate / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS RateHoure,
dbo.PZ_TEO.SSM / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ssmHoure, 
iif(exportImport.dbo.planZakaz.SSfact is null, 0, exportImport.dbo.planZakaz.SSfact) / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ssmFactHoure,
dbo.PZ_TEO.SSR / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ssrHoure, 
dbo.PZ_TEO.IzdKom / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ikHoure, 
dbo.PZ_TEO.IzdPPKredit / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ppkHoure, 
dbo.PZ_TEO.PI / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS piHoure, 
dbo.PZ_TEO.NOP / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS nopHoure, 
dbo.PZ_TEO.Rate / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xRate, 
dbo.PZ_TEO.SSM / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xSSM, 
iif(exportImport.dbo.planZakaz.SSfact is null, 0, exportImport.dbo.planZakaz.SSfact) / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xSSMFact, 
dbo.PZ_TEO.SSR / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xSSR, 
dbo.PZ_TEO.IzdKom / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xIK, 
dbo.PZ_TEO.IzdPPKredit / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xPPK, 
dbo.PZ_TEO.PI / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xPI, 
dbo.PZ_TEO.NOP / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xNOP
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
                      dbo.PZ_PlanZakaz ON CONVERT(varchar, dbo.PZ_PlanZakaz.PlanZakaz) = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] LEFT OUTER JOIN
                      dbo.PZ_TEO ON dbo.PZ_TEO.Id_PlanZakaz = dbo.PZ_PlanZakaz.Id LEFT OUTER JOIN
                      exportImport.dbo.planZakaz ON exportImport.dbo.planZakaz.Zakaz = dbo.PZ_PlanZakaz.PlanZakaz LEFT OUTER JOIN
                      ReportKATEK.dbo.ProjectWorkPO ON ReportKATEK.dbo.ProjectWorkPO.[№ заказа] = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] left join
					  [PortalKATEK_test].[dbo].[DashboardBP_State] on [PortalKATEK_test].[dbo].[DashboardBP_State].id > 0
WHERE     
[PortalKATEK_test].[dbo].[DashboardBP_State].active = 1
and
(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] NOT LIKE '%НИОКР%') AND 
                      (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] NOT LIKE '%Задан%') AND 
                      (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] NOT LIKE '%Проч%') AND (ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] > 0) AND 
                      (ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%УС%' OR
                      ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%УИ%' OR
                      ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%ЭУ%') AND (ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork > 0)