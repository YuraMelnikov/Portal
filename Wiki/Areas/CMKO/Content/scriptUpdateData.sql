DECLARE @periodQua NVARCHAR(6);
--DECLARE @periodM1 NVARCHAR(7);
--DECLARE @periodM2 NVARCHAR(7);
--DECLARE @periodM3 NVARCHAR(7);
--DECLARE @periodMP1 NVARCHAR(7);
--DECLARE @periodMP2 NVARCHAR(7);
--DECLARE @periodMP3 NVARCHAR(7);
--DECLARE @coefConvertCalendarNorm float;
DECLARE	@coefBujetManager float;
DECLARE	@coefBujetWorker float;
declare @coenBujetNManager float;
declare @coefBujetNWorker float;
declare @cost1N float;

--SET @coefConvertCalendarNorm = 0.9;
SET @periodQua ='2019.4';
--SET @periodM1 ='2019.10';
--SET @periodM2 ='2019.11';
--SET @periodM3 ='2019.12';
--SET @periodMP1 ='2019.10';
--SET @periodMP2 ='2019.11';
--SET @periodMP3 ='2019.12';
SET @coefBujetWorker = 0.0105;
SET @coefBujetManager = 0.0020;
SET @coenBujetNManager = 0.16;
SET @coefBujetNWorker = 0.84;
SET @cost1N = 4.9;

DELETE [PortalKATEK].[dbo].[CMKO_ThisPeriod]
insert into [PortalKATEK].[dbo].[CMKO_ThisPeriod] select @periodQua

DELETE PortalKATEK.dbo.CMKO_BujetList
insert into PortalKATEK.dbo.CMKO_BujetList
select
ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWBS
,ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
,ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceName
,concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.', (month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate) + 2) / 3) as [quartalFinishTask]
,iif(len(month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)) = 1, concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.0',month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)), concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.',month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate))) as [monthFinishTask]
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskPercentCompleted
,PortalKATEK.dbo.PZ_PlanZakaz.id as id_PZ_PlanZakaz
,ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
,isnull(ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО], 0) as [counterKO]
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskName
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate
,substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) as [Devision]
,substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 4, 5) as [DevisionKBE]
,isnull(PortalKATEK.dbo.PZ_TEO.SSM, 0)
,isnull(PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker, 0) as [bujetWorkers]
,isnull(PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager, 0) as [bujetManagers]
,MSP_EpmTask_UserView.TaskWork
,isnull(ProjectWebApp.dbo.MSP_EpmTask_UserView.нк, 0) as [normH]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM], 0), 0) as [normHOrderKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE], 0), 0) as [normHOrderKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM]), 0), 0) as [cost1NHWorkerKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM]), 0), 0) as [cost1NHManagerKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedWorkerForTaskKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedManagerForTaskKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE]), 0), 0) as [cost1NHWorkerKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE]), 0), 0) as [cost1NHManagerKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedWorkerForTaskKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedManagerForTaskKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coefBujetNWorker, 0), 0), 0) as [accruedWorkerForNTaskKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coenBujetNManager, 0), 0), 0) as [accruedManagerForNTaskKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coefBujetNWorker, 0), 0), 0) as [accruedWorkerForNTaskKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coenBujetNManager, 0), 0), 0) as [accruedManagerForNTaskKBE]
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


SELECT 
AspNetUsers.id
,PortalKATEK.dbo.CMKO_PeriodResult.period
,isnull(PortalKATEK.dbo.CMKO_TaxCatigories.salary, 0)
,isnull(AspNetUsers.tax, 0)
,AspNetUsers.CiliricalName
FROM [PortalKATEK].[dbo].[CMKO_PeriodResult] left join
(select * from PortalKATEK.dbo.AspNetUsers where PortalKATEK.dbo.AspNetUsers.LockoutEnabled = 1  and (PortalKATEK.dbo.AspNetUsers.Devision = 3 or PortalKATEK.dbo.AspNetUsers.Devision = 15 or PortalKATEK.dbo.AspNetUsers.Devision = 16))
   as AspNetUsers on AspNetUsers.LockoutEnabled > 0 
left join [PortalKATEK].[dbo].[CMKO_TaxFact] on [PortalKATEK].[dbo].[CMKO_TaxFact].id_AspNetUsers = AspNetUsers.Id
left join PortalKATEK.dbo.CMKO_TaxCatigories on PortalKATEK.dbo.CMKO_TaxCatigories.id = AspNetUsers.id_CMKO_TaxCatigories
where [PortalKATEK].[dbo].[CMKO_TaxFact].id is null