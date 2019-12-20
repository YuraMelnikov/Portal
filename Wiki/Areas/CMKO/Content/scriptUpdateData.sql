DECLARE @periodQua NVARCHAR(6);
DECLARE @periodM1 NVARCHAR(7);
DECLARE @periodM2 NVARCHAR(7);
DECLARE @periodM3 NVARCHAR(7);
DECLARE @periodMP1 NVARCHAR(7);
DECLARE @periodMP2 NVARCHAR(7);
DECLARE @periodMP3 NVARCHAR(7);
DECLARE @coefConvertCalendarNorm float;
DECLARE	@coefBujetManager float;
DECLARE	@coefBujetWorker float;
DECLARE @coenBujetNManager float;
DECLARE @coefBujetNWorker float;
DECLARE @cost1N float;
DECLARE @managerOrderPercent float;
DECLARE @percentMKO float;
DECLARE @percentMKBE float;
DECLARE @percentMKBM float;
DECLARE @sizeQualityBonus float;
DECLARE @sizeSpeed1 float;
DECLARE @sizeSpeed2 float;
DECLARE @coefErrorGip float;


SET @coefErrorGip = 1000.0;
SET @coefConvertCalendarNorm = 0.9;
SET @periodQua ='2019.4';
SET @periodM1 ='2019.10';
SET @periodM2 ='2019.11';
SET @periodM3 ='2019.12';
SET @periodMP1 ='2019.10';
SET @periodMP2 ='2019.11';
SET @periodMP3 ='2019.12';
SET @coefBujetWorker = 0.0105;
SET @coefBujetManager = 0.0020;
SET @coenBujetNManager = 0.16;
SET @coefBujetNWorker = 0.84;
SET @cost1N = 10;
SET @managerOrderPercent = 0.04;
SET @percentMKO = 0.025;
SET @percentMKBE = 0.11;
SET @percentMKBM = 0.08;
SET @sizeQualityBonus = 200;
SET @sizeSpeed1 = 100;
SET @sizeSpeed2 = 200;


update PortalKATEK.dbo.PZ_TEO
set PortalKATEK.dbo.PZ_TEO.SSMToBYN = PortalKATEK.dbo.CurencyBYN.USD * PortalKATEK.dbo.PZ_TEO.SSM
from
PortalKATEK.dbo.PZ_TEO left join
PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.Id = PortalKATEK.dbo.PZ_TEO.Id_PlanZakaz left join
PortalKATEK.dbo.CurencyBYN on PortalKATEK.dbo.PZ_PlanZakaz.DateCreate = PortalKATEK.dbo.CurencyBYN.[date]


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
,isnull(PortalKATEK.dbo.PZ_TEO.SSMToBYN, 0)
,isnull(PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker, 0) as [bujetWorkers]
,isnull(PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager, 0) as [bujetManagers]
,MSP_EpmTask_UserView.TaskWork
,isnull(ProjectWebApp.dbo.MSP_EpmTask_UserView.нк, 0) as [normH]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM], 0), 0) as [normHOrderKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE], 0), 0) as [normHOrderKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM]), 0), 0) as [cost1NHWorkerKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM]), 0), 0) as [cost1NHManagerKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedWorkerForTaskKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedManagerForTaskKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE]), 0), 0) as [cost1NHWorkerKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE]), 0), 0) as [cost1NHManagerKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedWorkerForTaskKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedManagerForTaskKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coefBujetNWorker, 0), 0), 0) as [accruedWorkerForNTaskKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coenBujetNManager, 0), 0), 0) as [accruedManagerForNTaskKBM]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coefBujetNWorker, 0), 0), 0) as [accruedWorkerForNTaskKBE]
,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coenBujetNManager, 0), 0), 0) as [accruedManagerForNTaskKBE]
,PortalKATEK.dbo.RKD_GIP.id_UserKBM
,PortalKATEK.dbo.RKD_GIP.id_UserKBE
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView
INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID
LEFT JOIN ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
LEFT JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
LEFT JOIN ReportKATEK.dbo.CMKO_TimeWorkOnOrder on ReportKATEK.dbo.CMKO_TimeWorkOnOrder.numberOrder = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz like ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
left join PortalKATEK.dbo.PZ_TEO on PortalKATEK.dbo.PZ_TEO.Id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join PortalKATEK.dbo.RKD_Order on PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join PortalKATEK.dbo.RKD_GIP on PortalKATEK.dbo.RKD_GIP.id_RKD_Order = PortalKATEK.dbo.RKD_Order.id
left join (select * from PortalKATEK.dbo.Debit_WorkBit where PortalKATEK.dbo.Debit_WorkBit.id_TaskForPZ = 45) as TableWorkKBM on TableWorkKBM.id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join (select * from PortalKATEK.dbo.Debit_WorkBit where PortalKATEK.dbo.Debit_WorkBit.id_TaskForPZ = 46) as TableWorkKBE on TableWorkKBE.id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
where
(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
and (convert(int, isnull(TableWorkKBM.[close], 1)) * convert(int, isnull(TableWorkKBE.[close], 1)) != 0)


Delete PortalKATEK.dbo.CMKO_ThisOverflowsBujet
insert into PortalKATEK.dbo.CMKO_ThisOverflowsBujet
SELECT
PortalKATEK.dbo.PZ_PlanZakaz.Id
,sum(isnull(PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker, 0) + isnull(PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager, 0) - isnull(KBMFactBujet.bujet, 0) - isnull(ThisBujetList.bujetKBM, 0)) as KBM
,sum(isnull(PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker, 0) + isnull(PortalKATEK.dbo.PZ_TEO.SSMToBYN * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager, 0) - isnull(KBEFactBujet.bujet, 0) - isnull(ThisBujetList.bujetKBE, 0)) as KBE
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView
left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.ProjectUID = ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID
left join (select id_PZ_PlanZakaz, sum([data]) as bujet from PortalKATEK.dbo.CMKO_ProjectFactBujet where devision like 'КБМ' group by id_PZ_PlanZakaz) as KBMFactBujet on KBMFactBujet.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join (select id_PZ_PlanZakaz, sum([data]) as bujet from PortalKATEK.dbo.CMKO_ProjectFactBujet where devision like 'КБЭ' group by id_PZ_PlanZakaz) as KBEFactBujet on KBEFactBujet.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join (select id_PZ_PlanZakaz, sum(accruedWorkerForTaskKBM) + sum(accruedManagerForTaskKBM) as [bujetKBM], sum(accruedWorkerForTaskKBE) + sum(accruedManagerForTaskKBE) as [bujetKBE] from PortalKATEK.dbo.CMKO_BujetList where PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask = @periodQua group by id_PZ_PlanZakaz) as ThisBujetList on ThisBujetList.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join PortalKATEK.dbo.PZ_TEO on PortalKATEK.dbo.PZ_TEO.Id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join (select * from PortalKATEK.dbo.Debit_WorkBit where PortalKATEK.dbo.Debit_WorkBit.id_TaskForPZ = 45) as TableWorkKBM on TableWorkKBM.id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join (select * from PortalKATEK.dbo.Debit_WorkBit where PortalKATEK.dbo.Debit_WorkBit.id_TaskForPZ = 46) as TableWorkKBE on TableWorkKBE.id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
WHERE
concat(year(PortalKATEK.dbo.PZ_PlanZakaz.dataOtgruzkiBP),'.', (month(PortalKATEK.dbo.PZ_PlanZakaz.dataOtgruzkiBP) + 2) / 3) = @periodQua
and (convert(int, isnull(TableWorkKBM.[close], 1)) * convert(int, isnull(TableWorkKBE.[close], 1)) != 0)
group by PortalKATEK.dbo.PZ_PlanZakaz.Id


DELETE PortalKATEK.dbo.CMKO_ThisWageFund
INSERT INTO PortalKATEK.dbo.CMKO_ThisWageFund
SELECT
sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) as wageFundMPlan
,sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM, 0)) as wageFundMFact
,sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) as wageFundEPlan
,sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE, 0)) as wageFundEFact
,sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) as wageFundWorkerMPlan
,sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM, 0)) as wageFundWorkerMFact
,sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) as wageFundWorkerEPlan
,sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE, 0)) as wageFundWorkerEFact
,sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) as wageFundManagerMPlan
,sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM, 0)) as wageFundManagerMFact
,sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) as wageFundManagerEPlan
,sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE, 0)) as wageFundManagerEFact
,(max(Overflows.SumKBM) * @coefBujetNWorker) + (max(Overflows.SumKBM) * @coefBujetManager) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) as wageFundOverflowsMPlan
,(max(Overflows.SumKBM) * @coefBujetNWorker) + (max(Overflows.SumKBM) * @coefBujetManager) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM, 0)) as wageFundOverflowsMFact
,(max(Overflows.SumKBE) * @coefBujetNWorker) + (max(Overflows.SumKBE) * @coefBujetManager) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) as wageFundOverflowsEPlan
,(max(Overflows.SumKBE) * @coefBujetNWorker) + (max(Overflows.SumKBE) * @coefBujetManager) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE, 0)) as wageFundOverflowsEFact
,(max(Overflows.SumKBM) * @coefBujetNWorker) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) as wageFundOverflowsWorkerMPlan
,(max(Overflows.SumKBM) * @coefBujetNWorker) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM, 0)) as wageFundOverflowsWorkerMFact
,(max(Overflows.SumKBE) * @coefBujetNWorker) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) as wageFundOverflowsWorkerEPlan
,(max(Overflows.SumKBE) * @coefBujetNWorker) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE, 0)) as wageFundOverflowsWorkerEFact
,(max(Overflows.SumKBM) * @coefBujetManager) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) as wageFundOverflowsManagerMPlan
,(max(Overflows.SumKBM) * @coefBujetManager) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM, 0)) as wageFundOverflowsManagerMFact
,(max(Overflows.SumKBE) * @coefBujetManager) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) as wageFundOverflowsManagerEPlan
,(max(Overflows.SumKBE) * @coefBujetManager) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE, 0)) as wageFundOverflowsManagerEFact
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBM is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0), 0)) + max(Overflows.SumKBM)) * @managerOrderPercent as wageFundGMPlan
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBM is not null, isnull(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0), 0) + isnull(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0), 0), 0)) + max(Overflows.SumKBM)) * @managerOrderPercent as wageFundGMFact
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBE is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0), 0)) + max(Overflows.SumKBE)) * @managerOrderPercent as wageFundGEPlan
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBE is not null, isnull(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0), 0) + isnull(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0), 0), 0)) + max(Overflows.SumKBE)) * @managerOrderPercent as wageFundGEFact
FROM
[PortalKATEK].[dbo].[CMKO_BujetList] 
left join (select MAX(id) as id, sum(KBM) as [SumKBM], sum(KBE) as [SumKBE] from PortalKATEK.dbo.CMKO_ThisOverflowsBujet) as Overflows on Overflows.id > 0
where
[PortalKATEK].[dbo].[CMKO_BujetList].quartalFinishTask = @periodQua


DELETE PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund
INSERT INTO PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund
SELECT
(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) + max(Overflows.SumKBM)) * @percentMKO as [deductionsMKOMPlan]
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM, 0)) + max(Overflows.SumKBM)) * @percentMKO as [deductionsMKOMFact]
,(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) + max(Overflows.SumKBE)) * @percentMKO as [deductionsMKOEPlan]
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE, 0)) + max(Overflows.SumKBE)) * @percentMKO as [deductionsMKOEFact]
,(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) + max(Overflows.SumKBM)) * @percentMKBM as [deductionsMMPlan]
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM, 0)) + max(Overflows.SumKBM)) * @percentMKBM as [deductionsMMFact]
,(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) + max(Overflows.SumKBE)) * @percentMKBE as [deductionsMEPlan]
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0)) + sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE, 0)) + max(Overflows.SumKBE)) * @percentMKBE as [deductionsMEFact]
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBM is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0), 0)) + max(Overflows.SumKBM)) * @managerOrderPercent as [deductionsGMPlan]
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBM is not null, isnull(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0), 0) + isnull(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0), 0), 0)) + max(Overflows.SumKBM)) * @managerOrderPercent as [deductionsGMFact]
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBE is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0), 0)) + max(Overflows.SumKBE)) * @managerOrderPercent as [deductionsGEPlan]
,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBE is not null, isnull(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0), 0) + isnull(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0), 0), 0)) + max(Overflows.SumKBE)) * @managerOrderPercent as [deductionsGEFact]
,0 as [balanceBonusFundMPlan]
,0 as [balanceBonusFundMFact]
,0 as [balanceBonusFundEPlan]
,0 as [balanceBonusFundEFact]FROM
[PortalKATEK].[dbo].[CMKO_BujetList] 
left join (select MAX(id) as id, sum(KBM) as [SumKBM], sum(KBE) as [SumKBE] from PortalKATEK.dbo.CMKO_ThisOverflowsBujet) as Overflows on Overflows.id > 0
where
[PortalKATEK].[dbo].[CMKO_BujetList].quartalFinishTask = @periodQua


update PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund set 
balanceBonusFundMPlan = PortalKATEK.dbo.CMKO_ThisWageFund.wageFundOverflowsManagerMPlan -
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMKOMPlan - 
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMMPlan - 
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsGMPlan 
,balanceBonusFundMFact = PortalKATEK.dbo.CMKO_ThisWageFund.wageFundOverflowsManagerMFact -
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMKOMFact - 
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMMFact - 
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsGMFact 
,balanceBonusFundEPlan = PortalKATEK.dbo.CMKO_ThisWageFund.wageFundOverflowsManagerEPlan -
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMKOEPlan - 
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMEPlan - 
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsGEPlan  
,balanceBonusFundEFact = PortalKATEK.dbo.CMKO_ThisWageFund.wageFundOverflowsManagerEFact -
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMKOEFact - 
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMEFact - 
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsGEFact  
from
PortalKATEK.dbo.CMKO_ThisWageFund join
PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund on PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.id > 0


delete [PortalKATEK].[dbo].[DashboardKOMP1]
insert into [PortalKATEK].[dbo].[DashboardKOMP1]
SELECT
[PortalKATEK].[dbo].[ProductionCalendar].[period],
[PortalKATEK].[dbo].[AspNetUsers].CiliricalName as ciliricalName,
[PortalKATEK].[dbo].[Devision].[name] as devisionName
,[PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] as [plan]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.10 as [plan10]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.20 as [plan20]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.30 as [plan30]
,tableResPower.SumAssignmentActualWork
,tableResPower.SumAssignmentWork
FROM [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan] left join 
[PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].Id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_AspNetUsers] left join
[PortalKATEK].[dbo].[Devision] on [PortalKATEK].[dbo].[Devision].id = [PortalKATEK].[dbo].[AspNetUsers].Devision left join
[PortalKATEK].[dbo].[ProductionCalendar] on [PortalKATEK].[dbo].[ProductionCalendar].id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_ProductionCalendar] left join
(SELECT iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)))  as [Месяц завершения задачи]
,MSP_EpmResource_UserView.ResourceUID
,sum(iif(ProjectWebApp.dbo.MSP_EpmTask_UserView.НК = 0, 0, ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork * MSP_EpmAssignmentByDay_UserView.AssignmentWork)) as НК
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN
ProjectWebApp.dbo.MSP_EpmTask_UserView ON ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID 
WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0) and (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
group by iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)),
concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay))),
substring(MSP_EpmResource_UserView.СДРес, 0, 4), MSP_EpmResource_UserView.ResourceUID) 
as tableResult on tableResult.[Месяц завершения задачи] = [PortalKATEK].[dbo].[ProductionCalendar].period and tableResult.ResourceUID = [PortalKATEK].[dbo].AspNetUsers.ResourceUID
left join
(SELECT ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID ,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork) as SumAssignmentActualWork
,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) as SumAssignmentWork 
FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignment_UserView
ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.НК > 0) and ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0 AND (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
and (concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay)) = @periodM1)
group by ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID) as tableResPower on tableResPower.ResourceUID = [PortalKATEK].[dbo].[AspNetUsers].ResourceUID
where ([PortalKATEK].[dbo].[ProductionCalendar].[period] = @periodMP1)


delete [PortalKATEK].[dbo].[DashboardKOMP2]
insert into [PortalKATEK].[dbo].[DashboardKOMP2]
SELECT
[PortalKATEK].[dbo].[ProductionCalendar].[period],
[PortalKATEK].[dbo].[AspNetUsers].CiliricalName as ciliricalName,
[PortalKATEK].[dbo].[Devision].[name] as devisionName
,[PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] as [plan]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.10 as [plan10]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.20 as [plan20]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.30 as [plan30]
,isnull(tableResPower.SumAssignmentActualWork, 0)
,isnull(tableResPower.SumAssignmentWork, 0)
FROM [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan] left join 
[PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].Id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_AspNetUsers] left join
[PortalKATEK].[dbo].[Devision] on [PortalKATEK].[dbo].[Devision].id = [PortalKATEK].[dbo].[AspNetUsers].Devision left join
[PortalKATEK].[dbo].[ProductionCalendar] on [PortalKATEK].[dbo].[ProductionCalendar].id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_ProductionCalendar] left join
(SELECT iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)))  as [Месяц завершения задачи]
,MSP_EpmResource_UserView.ResourceUID
,sum(iif(ProjectWebApp.dbo.MSP_EpmTask_UserView.НК = 0, 0, ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork * MSP_EpmAssignmentByDay_UserView.AssignmentWork)) as НК
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN
ProjectWebApp.dbo.MSP_EpmTask_UserView ON ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID 
WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0) and (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
group by iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)),
concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay))),
substring(MSP_EpmResource_UserView.СДРес, 0, 4), MSP_EpmResource_UserView.ResourceUID) 
as tableResult on tableResult.[Месяц завершения задачи] = [PortalKATEK].[dbo].[ProductionCalendar].period and tableResult.ResourceUID = [PortalKATEK].[dbo].AspNetUsers.ResourceUID
left join
(SELECT ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID ,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork) as SumAssignmentActualWork
,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) as SumAssignmentWork 
FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignment_UserView
ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.НК > 0) and ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0 AND (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
and (concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay)) = @periodM2)
group by ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID) as tableResPower on tableResPower.ResourceUID = [PortalKATEK].[dbo].[AspNetUsers].ResourceUID
where ([PortalKATEK].[dbo].[ProductionCalendar].[period] = @periodMP2)


delete [PortalKATEK].[dbo].[DashboardKOMP3]
insert into [PortalKATEK].[dbo].[DashboardKOMP3]
SELECT
[PortalKATEK].[dbo].[ProductionCalendar].[period],
[PortalKATEK].[dbo].[AspNetUsers].CiliricalName as ciliricalName,
[PortalKATEK].[dbo].[Devision].[name] as devisionName
,[PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] as [plan]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.10 as [plan10]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.20 as [plan20]
,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.30 as [plan30]
,tableResPower.SumAssignmentActualWork
,tableResPower.SumAssignmentWork
FROM [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan] left join 
[PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].Id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_AspNetUsers] left join
[PortalKATEK].[dbo].[Devision] on [PortalKATEK].[dbo].[Devision].id = [PortalKATEK].[dbo].[AspNetUsers].Devision left join
[PortalKATEK].[dbo].[ProductionCalendar] on [PortalKATEK].[dbo].[ProductionCalendar].id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_ProductionCalendar] left join
(SELECT iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)))  as [Месяц завершения задачи]
,MSP_EpmResource_UserView.ResourceUID
,sum(iif(ProjectWebApp.dbo.MSP_EpmTask_UserView.НК = 0, 0, ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork * MSP_EpmAssignmentByDay_UserView.AssignmentWork)) as НК
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN
ProjectWebApp.dbo.MSP_EpmTask_UserView ON ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID 
WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0) and (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
group by iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)),
concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay))),
substring(MSP_EpmResource_UserView.СДРес, 0, 4), MSP_EpmResource_UserView.ResourceUID) 
as tableResult on tableResult.[Месяц завершения задачи] = [PortalKATEK].[dbo].[ProductionCalendar].period and tableResult.ResourceUID = [PortalKATEK].[dbo].AspNetUsers.ResourceUID
left join
(SELECT ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID ,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork) as SumAssignmentActualWork
,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) as SumAssignmentWork 
FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignment_UserView
ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.НК > 0) and ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0 AND (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
and (concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay)) = @periodM3)
group by ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID) as tableResPower on tableResPower.ResourceUID = [PortalKATEK].[dbo].[AspNetUsers].ResourceUID
where ([PortalKATEK].[dbo].[ProductionCalendar].[period] = @periodMP3)


delete [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers]
insert into [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers]
Select
PortalKATEK.dbo.AspNetUsers.Id as id_AspNetUsers
,isnull(max(OptimizationTable.countIdea), 0) as optimization
,isnull(max(SpeedUser1.plan10), 0) + isnull(max(SpeedUser1.plan20), 0) as speed1
,isnull(max(SpeedUser2.plan10), 0) + isnull(max(SpeedUser2.plan20), 0) as speed2
,isnull(max(SpeedUser3.plan10), 0) + isnull(max(SpeedUser3.plan20), 0) as speed3
,isnull(max(TeachTable.cost), 0) as teach
,max(PortalKATEK.dbo.CMKO_TaxCatigories.salary) as rate1
,max(PortalKATEK.dbo.CMKO_TaxCatigories.salary) as rate2
,max(PortalKATEK.dbo.CMKO_TaxCatigories.salary) as rate3
,max(PortalKATEK.dbo.AspNetUsers.tax) as tax1
,max(PortalKATEK.dbo.AspNetUsers.tax) as tax2
,max(PortalKATEK.dbo.AspNetUsers.tax) as tax3
,isnull(1 - ((400 * ReclamationCounter.countError) / (100 * sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.normH, 0)))), 1) as coefError
,isnull(1 - ((400 * ReclamationCounter.countErrorG) / (100 * sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.normH, 0)))), 1) as coefErrorG
,iif(1 - ((400 * ReclamationCounter.countError) / (100 * sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.normH, 0)))) >= 0.99, @sizeQualityBonus, 0) as qualityBonus
,sum(PortalKATEK.dbo.CMKO_BujetList.normH) as nhPlan
,sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.normH, 0)) as nhFact
,0 as nhGPlan
,0 as nhGFact
,PortalKATEK.dbo.CMKO_ThisCoefManager.coef
from
PortalKATEK.dbo.CMKO_BujetList
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.ResourceUID = PortalKATEK.dbo.CMKO_BujetList.ResourceUID
left join (select COUNT(id) as countIdea, id_AspNetUsersIdea from PortalKATEK.dbo.CMKO_Optimization group by PortalKATEK.dbo.CMKO_Optimization.id_AspNetUsersIdea) as OptimizationTable on OptimizationTable.id_AspNetUsersIdea = PortalKATEK.dbo.AspNetUsers.Id
left join (select iif(plan20 < normHoureFact, @sizeSpeed2, 0) as plan20, iif(plan10 < normHoureFact, @sizeSpeed1, 0) as plan10, ciliricalName from PortalKATEK.dbo.DashboardKOMP1) as SpeedUser1 on SpeedUser1.ciliricalName = PortalKATEK.dbo.AspNetUsers.CiliricalName
left join (select iif(plan20 < normHoureFact, @sizeSpeed2, 0) as plan20, iif(plan10 < normHoureFact, @sizeSpeed1, 0) as plan10, ciliricalName from PortalKATEK.dbo.DashboardKOMP2) as SpeedUser2 on SpeedUser2.ciliricalName = PortalKATEK.dbo.AspNetUsers.CiliricalName
left join (select iif(plan20 < normHoureFact, @sizeSpeed2, 0) as plan20, iif(plan10 < normHoureFact, @sizeSpeed1, 0) as plan10, ciliricalName from PortalKATEK.dbo.DashboardKOMP3) as SpeedUser3 on SpeedUser3.ciliricalName = PortalKATEK.dbo.AspNetUsers.CiliricalName
left join PortalKATEK.dbo.CMKO_TaxCatigories on PortalKATEK.dbo.CMKO_TaxCatigories.id = PortalKATEK.dbo.AspNetUsers.id_CMKO_TaxCatigories
left join (select TableRes.id_AspNetUsersError as id_AspNetUsersError
                ,SUM(TableRes.countError) as countError
                ,0 as countErrorG
                from (select 
                PortalKATEK.dbo.Reclamation.id_AspNetUsersError
                ,iif(PortalKATEK.dbo.Reclamation.gip = 0, PortalKATEK.dbo.Reclamation_CountError.[count], 0) as countError
                ,0 as countErrorG 
                ,ROW_NUMBER() OVER(PARTITION BY PortalKATEK.dbo.Reclamation.id ORDER BY PortalKATEK.dbo.Reclamation.id ASC) as rowNum
                from PortalKATEK.dbo.Reclamation 
                left join PortalKATEK.dbo.Reclamation_PZ on PortalKATEK.dbo.Reclamation_PZ.id_Reclamation = PortalKATEK.dbo.Reclamation.id
                left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.Id = PortalKATEK.dbo.Reclamation_PZ.id_PZ_PlanZakaz
                left join ProjectWebApp.dbo.MSP_EpmProject_UserView on ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz
                left join PortalKATEK.dbo.Reclamation_CountError on PortalKATEK.dbo.Reclamation_CountError.id = PortalKATEK.dbo.Reclamation.id_Reclamation_CountErrorFirst
                left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Id = PortalKATEK.dbo.Reclamation.id_AspNetUsersError
                where (PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 3 or PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 15 or PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 16)
                and concat(year(Portalkatek.dbo.Reclamation.dateTimeCreate),'.', (month(Portalkatek.dbo.Reclamation.dateTimeCreate) + 2) / 3) = @periodQua
                and ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] != 0
                and PortalKATEK.dbo.Reclamation.id_AspNetUsersError is not null
                and PortalKATEK.dbo.Reclamation.closeMKO = 1) as TableRes
                where TableRes.rowNum = 1
                group by TableRes.id_AspNetUsersError) as ReclamationCounter on ReclamationCounter.id_AspNetUsersError = PortalKATEK.dbo.AspNetUsers.Id
left join (select * from PortalKATEK.dbo.CMKO_Teach where PortalKATEK.dbo.CMKO_Teach.id_CMKO_PeriodResult = @periodQua) as TeachTable on TeachTable.id_AspNetUsersTeacher = PortalKATEK.dbo.AspNetUsers.Id
left join PortalKATEK.dbo.CMKO_ThisCoefManager on PortalKATEK.dbo.CMKO_ThisCoefManager.id_CMKO_PeriodResult = @periodQua and PortalKATEK.dbo.CMKO_ThisCoefManager.id_AspNetUsers = PortalKATEK.dbo.AspNetUsers.Id
where PortalKATEK.dbo.AspNetUsers.LockoutEnabled = 1
and (PortalKATEK.dbo.AspNetUsers.Devision = 3 or PortalKATEK.dbo.AspNetUsers.Devision = 15 or PortalKATEK.dbo.AspNetUsers.Devision = 16)
and PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask = @periodQua
group by
PortalKATEK.dbo.AspNetUsers.Id
,ReclamationCounter.countError
,ReclamationCounter.countErrorG
,PortalKATEK.dbo.CMKO_ThisCoefManager.coef


update [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers] set
[PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].nhGPlan = TableNorm.normPlan
,[PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].nhGFact = TableNorm.normFact
from
(select 
[PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers
,isnull(sum(BujetListM.[normH]), 0) + isnull(sum(BujetListE.normH), 0) as normPlan
,isnull(sum(iif(BujetListM.TaskPercentCompleted = 100, BujetListM.[normH], 0)), 0) + isnull(sum(iif(BujetListE.TaskPercentCompleted = 100, BujetListE.[normH], 0)), 0) as normFact
from [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers]
left join (select * from PortalKATEK.dbo.CMKO_BujetList where PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask = @periodQua) as BujetListM on BujetListM.id_RKD_GIP_KBM = [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers
left join (select * from PortalKATEK.dbo.CMKO_BujetList where PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask = @periodQua) as BujetListE on BujetListE.id_RKD_GIP_KBE = [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers
group by
[PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers) as TableNorm
where [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers = TableNorm.id_AspNetUsers


update [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers]
set [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].coefErrorG = 1 - iif(isnull(TableGipCoef.[count], 0) = 0, 0, ((TableGipCoef.[count] * @coefErrorGip) / (100 * [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].nhGFact)))    
from 
[PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers] left join
(select TableResult.id_UserKBE,
sum(TableResult.[count]) as [count]
from (select PortalKATEK.dbo.RKD_GIP.id_UserKBE
		,PortalKATEK.dbo.Reclamation_CountError.[count]
		,ROW_NUMBER() OVER(PARTITION BY PortalKATEK.dbo.Reclamation.id ORDER BY PortalKATEK.dbo.Reclamation.id ASC) as rowNum
		from PortalKATEK.dbo.Reclamation
		left join PortalKATEK.dbo.Reclamation_CountError on PortalKATEK.dbo.Reclamation_CountError.id = PortalKATEK.dbo.Reclamation.id_Reclamation_CountErrorFirst
		left join PortalKATEK.dbo.Reclamation_PZ on PortalKATEK.dbo.Reclamation_PZ.id_Reclamation = PortalKATEK.dbo.Reclamation.id
		left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.id = PortalKATEK.dbo.Reclamation_PZ.id_PZ_PlanZakaz
		left join ProjectWebApp.dbo.MSP_EpmProject_UserView on PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz like ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
		left join PortalKATEK.dbo.RKD_Order on PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
		left join PortalKATEK.dbo.RKD_GIP on PortalKATEK.dbo.RKD_GIP.id_RKD_Order = PortalKATEK.dbo.RKD_Order.id
		where (PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 16 or PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 3)
		and PortalKATEK.dbo.Reclamation.closeMKO = 1 
		and concat(year(Portalkatek.dbo.Reclamation.dateTimeCreate),'.', (month(Portalkatek.dbo.Reclamation.dateTimeCreate) + 2) / 3) = @periodQua 
		and PortalKATEK.dbo.Reclamation.gip = 1
		and PortalKATEK.dbo.RKD_GIP.id_UserKBE is not null) as TableResult 
		where TableResult.rowNum = 1
		group by TableResult.id_UserKBE) as TableGipCoef on TableGipCoef.id_UserKBE = [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Id = [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers
left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision
where (PortalKATEK.dbo.Devision.id = 3 or PortalKATEK.dbo.Devision.id = 16)


update [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers]
set [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].coefErrorG = 0
from
[PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers] left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Id = [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers
left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision
where [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].nhGFact = 0
and (PortalKATEK.dbo.Devision.id = 3 or PortalKATEK.dbo.Devision.id = 16)


update [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers]
set [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].coefErrorG = 1 - iif(isnull(TableGipCoef.[count], 0) = 0, 0, ((TableGipCoef.[count] * @coefErrorGip) / (100 * [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].nhGFact)))    
from 
[PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers] left join
(select 
TableResut.id_UserKBM
,sum(TableResut.count) as [count]
 from
(select UsersTable.Id as id_UserKBM
		,PortalKATEK.dbo.Reclamation_CountError.[count] as [count]
		,ROW_NUMBER() OVER(PARTITION BY PortalKATEK.dbo.Reclamation.id ORDER BY PortalKATEK.dbo.Reclamation.id ASC) as rowNum
		from PortalKATEK.dbo.Reclamation
		left join PortalKATEK.dbo.Reclamation_CountError on PortalKATEK.dbo.Reclamation_CountError.id = PortalKATEK.dbo.Reclamation.id_Reclamation_CountErrorFirst
		left join PortalKATEK.dbo.Reclamation_PZ on PortalKATEK.dbo.Reclamation_PZ.id_Reclamation = PortalKATEK.dbo.Reclamation.id
		left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.id = PortalKATEK.dbo.Reclamation_PZ.id_PZ_PlanZakaz
		left join ProjectWebApp.dbo.MSP_EpmProject_UserView on PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz like ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
		left join (select
					ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] as orderNumber
					,PortalKATEK.dbo.AspNetUsers.Id
					from
					ProjectWebApp.dbo.MSP_EpmProject_UserView left join
					ProjectWebApp.dbo.MSP_EpmTask on ProjectWebApp.dbo.MSP_EpmTask.ProjectUID = ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID left join
					ProjectWebApp.dbo.MSP_EpmAssignment on ProjectWebApp.dbo.MSP_EpmAssignment.TaskUID = ProjectWebApp.dbo.MSP_EpmTask.TaskUID left join
					PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.ResourceUID = ProjectWebApp.dbo.MSP_EpmAssignment.ResourceUID
					where ProjectWebApp.dbo.MSP_EpmTask.TaskName like '%Сформировать комплект РКД (КБМ)/Подсборка%' or ProjectWebApp.dbo.MSP_EpmTask.TaskName like '%Подсобрать 3D-модели. Сформировать комплект РКД (КБМ)%'
					and (PortalKATEK.dbo.AspNetUsers.Id is not null)) as UsersTable on ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] = UsersTable.orderNumber
		where PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 15
		and PortalKATEK.dbo.Reclamation.closeMKO = 1 
		and concat(year(Portalkatek.dbo.Reclamation.dateTimeCreate),'.', (month(Portalkatek.dbo.Reclamation.dateTimeCreate) + 2) / 3) = @periodQua 
		and PortalKATEK.dbo.Reclamation.gip = 1
		and UsersTable.Id is not null) as TableResut
		where TableResut.rowNum = 1
		group by TableResut.id_UserKBM) as TableGipCoef on TableGipCoef.id_UserKBM = [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Id = [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers
left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision
where PortalKATEK.dbo.Devision.id = 15


update [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers]
set [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].coefErrorG = 0
from
[PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers] left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Id = [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].id_AspNetUsers
left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision
where [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers].nhGFact = 0
and PortalKATEK.dbo.Devision.id = 15


delete PortalKATEK.dbo.CMKO_ThisAccruedG
insert into PortalKATEK.dbo.CMKO_ThisAccruedG
select 
PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers
,(PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGPlan / TableNh.sumNhGPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEPlan]) as [accruedPlan]
,(PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGFact / TableNh.sumNhGFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEFact]) as [accruedFact]
,((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGPlan / TableNh.sumNhGPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEPlan])) - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGPlan / TableNh.sumNhGPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEPlan])) * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefErrorG) as [withheldPlan]
,((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGFact / TableNh.sumNhGFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEFact])) - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGFact / TableNh.sumNhGFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEFact])) * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefErrorG) as [withheldFact]
,((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGPlan / TableNh.sumNhGPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEPlan]))
  - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGPlan / TableNh.sumNhGPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEPlan])) - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGPlan / TableNh.sumNhGPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEPlan])) * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefErrorG)) as [accruedTotalPlan]
,((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGFact / TableNh.sumNhGFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEFact]))
  - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGFact / TableNh.sumNhGFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEFact])) - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGFact / TableNh.sumNhGFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundGEFact])) * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefErrorG)) as [accruedTotalFact]
from
PortalKATEK.dbo.CMKO_ThisIndicatorsUsers 
left join [PortalKATEK].[dbo].[CMKO_ThisWageFund] on [PortalKATEK].[dbo].[CMKO_ThisWageFund].id > 0
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.id = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers
left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision
left join (select sum([nhGPlan]) as [sumNhGPlan], sum([nhGFact]) as [sumNhGFact], SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) as devisionSubstringName from PortalKATEK.dbo.CMKO_ThisIndicatorsUsers left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.id = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision group by SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4)) as TableNh on TableNh.devisionSubstringName = SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4)


delete PortalKATEK.dbo.CMKO_ThisWithheldToBonusFund
insert into PortalKATEK.dbo.CMKO_ThisWithheldToBonusFund
select 
0 as [reclamationMPlan]
,0 as [reclamationEPlan]
,0 as [reclamationMFact]
,0 as [reclamationEFact]
,isnull(sum(iif(PortalKATEK.dbo.AspNetUsers.Devision = 15, PortalKATEK.dbo.CMKO_ThisAccruedG.withheldPlan, 0)), 0) as [reclamationMGPlan]
,isnull(sum(iif(PortalKATEK.dbo.AspNetUsers.Devision != 15, PortalKATEK.dbo.CMKO_ThisAccruedG.withheldPlan, 0)), 0) as [reclamationEGPlan]
,isnull(sum(iif(PortalKATEK.dbo.AspNetUsers.Devision = 15, PortalKATEK.dbo.CMKO_ThisAccruedG.withheldFact, 0)), 0) as [reclamationMGFact]
,isnull(sum(iif(PortalKATEK.dbo.AspNetUsers.Devision != 15, PortalKATEK.dbo.CMKO_ThisAccruedG.withheldFact, 0)), 0) as [reclamationEGFact]
from
PortalKATEK.dbo.CMKO_ThisAccruedG 
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Id = PortalKATEK.dbo.CMKO_ThisAccruedG.id_AspNetUsers

delete PortalKATEK.dbo.CMKO_ThisAccrued
insert into PortalKATEK.dbo.CMKO_ThisAccrued
select 
PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers
,(PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhPlan / TableNh.sumNhPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEPlan]) as [accruedPlan]
,(PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhFact / TableNh.sumNhFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEFact]) as [accruedFact]
,((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhPlan / TableNh.sumNhPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEPlan])) - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhPlan / TableNh.sumNhPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEPlan])) * (PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefError * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefManager)) as [withheldPlan]
,((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhFact / TableNh.sumNhFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEFact])) - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhFact / TableNh.sumNhFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEFact])) * (PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefError * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefManager)) as [withheldFact]
,((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhPlan / TableNh.sumNhPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEPlan]))
  - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhPlan / TableNh.sumNhPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEPlan])) - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhPlan / TableNh.sumNhPlan) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMPlan], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEPlan])) * (PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefError * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefManager))) as [accruedTotalPlan]
,((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhFact / TableNh.sumNhFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEFact]))
  - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhFact / TableNh.sumNhFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEFact])) - (((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhFact / TableNh.sumNhFact) *  iif(SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) like '%КБМ%', [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsMFact], [PortalKATEK].[dbo].[CMKO_ThisWageFund].[wageFundOverflowsEFact])) * (PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefError * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefManager))) as [accruedTotalFact]
,0 as [bonusPlan]
,0 as [bonusFact]
from
PortalKATEK.dbo.CMKO_ThisIndicatorsUsers 
left join [PortalKATEK].[dbo].[CMKO_ThisWageFund] on [PortalKATEK].[dbo].[CMKO_ThisWageFund].id > 0
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.id = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers
left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision
left join (select sum([nhPlan]) as [sumNhPlan], sum([nhFact]) as [sumNhFact], SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) as devisionSubstringName from PortalKATEK.dbo.CMKO_ThisIndicatorsUsers left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.id = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision group by SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4)) as TableNh on TableNh.devisionSubstringName = SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4)


update PortalKATEK.dbo.CMKO_ThisWithheldToBonusFund set 
PortalKATEK.dbo.CMKO_ThisWithheldToBonusFund.reclamationMPlan = isnull(TableResult.planMData, 0)
,PortalKATEK.dbo.CMKO_ThisWithheldToBonusFund.reclamationEPlan = isnull(TableResult.planEData, 0)
,PortalKATEK.dbo.CMKO_ThisWithheldToBonusFund.reclamationMFact = isnull(TableResult.factMData, 0)
,PortalKATEK.dbo.CMKO_ThisWithheldToBonusFund.reclamationEFact = isnull(TableResult.factEData, 0)
from
(select sum(iif(devision = 15, PortalKATEK.dbo.CMKO_ThisAccrued.withheldPlan, 0)) as planMData, sum(iif(devision = 15, PortalKATEK.dbo.CMKO_ThisAccrued.withheldFact, 0)) factMData, sum(iif(devision != 15, PortalKATEK.dbo.CMKO_ThisAccrued.withheldPlan, 0)) as planEData, sum(iif(devision != 15, PortalKATEK.dbo.CMKO_ThisAccrued.withheldFact, 0)) factEData from PortalKATEK.dbo.CMKO_ThisAccrued left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Id = PortalKATEK.dbo.CMKO_ThisAccrued.id_AspNetUsers) as TableResult


delete PortalKATEK.dbo.CMKO_ThisFinalBonus
insert into PortalKATEK.dbo.CMKO_ThisFinalBonus
SELECT 
[PortalKATEK].[dbo].[CMKO_ThisWithheldToBonusFund].[reclamationMPlan] + [PortalKATEK].[dbo].[CMKO_ThisWithheldToBonusFund].[reclamationMGPlan] + [PortalKATEK].[dbo].[CMKO_ThisDeductionsBonusFund].[balanceBonusFundMPlan] - TableUsers.optimizationM - TableUsers.speedM - TableUsers.qualityBonusM as mPlan
,[PortalKATEK].[dbo].[CMKO_ThisWithheldToBonusFund].[reclamationMFact] + [PortalKATEK].[dbo].[CMKO_ThisWithheldToBonusFund].[reclamationMGFact] + [PortalKATEK].[dbo].[CMKO_ThisDeductionsBonusFund].[balanceBonusFundMFact] - TableUsers.optimizationM - TableUsers.speedM - TableUsers.qualityBonusM as mFact
,[PortalKATEK].[dbo].[CMKO_ThisWithheldToBonusFund].[reclamationEPlan] + [PortalKATEK].[dbo].[CMKO_ThisWithheldToBonusFund].[reclamationEGPlan] + [PortalKATEK].[dbo].[CMKO_ThisDeductionsBonusFund].[balanceBonusFundEPlan] - TableUsers.optimizationE - TableUsers.speedE - TableUsers.qualityBonusE as ePlan
,[PortalKATEK].[dbo].[CMKO_ThisWithheldToBonusFund].[reclamationEFact] + [PortalKATEK].[dbo].[CMKO_ThisWithheldToBonusFund].[reclamationEGFact] + [PortalKATEK].[dbo].[CMKO_ThisDeductionsBonusFund].[balanceBonusFundEFact] - TableUsers.optimizationE - TableUsers.speedE - TableUsers.qualityBonusE AS eFact
FROM [PortalKATEK].[dbo].[CMKO_ThisWithheldToBonusFund]
left join [PortalKATEK].[dbo].[CMKO_ThisDeductionsBonusFund] on PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.id > 0
left join (SELECT 
sum(iif(PortalKATEK.dbo.AspNetUsers.Devision = 15, [optimization], 0)) as [optimizationM]
,sum(iif(PortalKATEK.dbo.AspNetUsers.Devision != 15, [optimization], 0)) as [optimizationE]
,sum(iif(PortalKATEK.dbo.AspNetUsers.Devision = 15, [speed1], 0)) + sum(iif(PortalKATEK.dbo.AspNetUsers.Devision = 15, [speed2], 0)) + sum(iif(PortalKATEK.dbo.AspNetUsers.Devision = 15, [speed3], 0)) as [speedM]
,sum(iif(PortalKATEK.dbo.AspNetUsers.Devision != 15, [speed1], 0)) + sum(iif(PortalKATEK.dbo.AspNetUsers.Devision != 15, [speed2], 0)) + sum(iif(PortalKATEK.dbo.AspNetUsers.Devision != 15, [speed3], 0)) as [speedE]
,sum(iif(PortalKATEK.dbo.AspNetUsers.Devision = 15, [qualityBonus], 0)) as [qualityBonusM]
,sum(iif(PortalKATEK.dbo.AspNetUsers.Devision != 15, [qualityBonus], 0)) as [qualityBonusE]
  FROM [PortalKATEK].[dbo].[CMKO_ThisIndicatorsUsers] left join 
  PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Id = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers) as TableUsers on TableUsers.optimizationE >= 0


update PortalKATEK.dbo.CMKO_ThisAccrued set 
PortalKATEK.dbo.CMKO_ThisAccrued.[bonusPlan] = iif(PortalKATEK.dbo.AspNetUsers.Devision = 15, ((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefError * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefManager) * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhPlan) / TableNh.sumNhPlan * [PortalKATEK].[dbo].[CMKO_ThisFinalBonus].[mPlan], ((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefError * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefManager) * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhPlan) / TableNh.sumNhPlan * [PortalKATEK].[dbo].[CMKO_ThisFinalBonus].[ePlan])
,PortalKATEK.dbo.CMKO_ThisAccrued.[bonusFact] = iif(PortalKATEK.dbo.AspNetUsers.Devision = 15, ((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefError * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefManager) * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhFact) / TableNh.sumNhFact * [PortalKATEK].[dbo].[CMKO_ThisFinalBonus].[mFact], ((PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefError * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefManager) * PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhFact) / TableNh.sumNhFact * [PortalKATEK].[dbo].[CMKO_ThisFinalBonus].[eFact])
from
PortalKATEK.dbo.CMKO_ThisIndicatorsUsers
join PortalKATEK.dbo.CMKO_ThisAccrued on PortalKATEK.dbo.CMKO_ThisAccrued.id_AspNetUsers = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers
left join [PortalKATEK].[dbo].[CMKO_ThisFinalBonus] on [PortalKATEK].[dbo].[CMKO_ThisFinalBonus].id > 0
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.id = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers
left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision
left join (select sum([nhPlan] * coefError) as [sumNhPlan], sum([nhFact] * coefError) as [sumNhFact], SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4) as devisionSubstringName from PortalKATEK.dbo.CMKO_ThisIndicatorsUsers left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.id = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision group by SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4)) as TableNh on TableNh.devisionSubstringName = SUBSTRING(PortalKATEK.dbo.Devision.[name], 0, 4)
where PortalKATEK.dbo.CMKO_ThisAccrued.id_AspNetUsers = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers


delete PortalKatek.dbo.DashboardKOHssPO
insert into PortalKatek.dbo.DashboardKOHssPO
SELECT [Dashboard].[dbo].[1910_DB_прибыль].[quart]
      ,sum([Dashboard].[dbo].[1910_DB_прибыль].[ХСС]) as hss
FROM [Dashboard].[dbo].[1910_DB_прибыль]
where
[Dashboard].[dbo].[1910_DB_прибыль].[year] > year(getdate()) - 2
group by [Dashboard].[dbo].[1910_DB_прибыль].[quart]


delete PortalKATEK.dbo.CMKO_ThisHSS
insert into PortalKATEK.dbo.CMKO_ThisHSS
select 
PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask
,(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM)) / (@coefBujetWorker + @coefBujetManager)
,(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE)) / (@coefBujetWorker + @coefBujetManager)
from
PortalKATEK.dbo.CMKO_BujetList
where YEAR(PortalKATEK.dbo.CMKO_BujetList.TaskFinishDate) > YEAR(GETDATE()) - 2
group by
PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask
order by
PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask


delete PortalKatek.dbo.DashboardKORemainingWorkAll
insert into PortalKatek.dbo.DashboardKORemainingWorkAll
select
sum(MSP_EpmAssignment_UserView.AssignmentRemainingWork) as [остТрты]
,substring(MSP_EpmResource_UserView.[СДРес],0,5) as [СДРес]
,MSP_EpmResource_UserView.ResourceName as [Сотрудник]
                FROM
                ProjectWebApp.dbo.MSP_EpmProject_UserView
                INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
                MSP_EpmProject_UserView.ProjectUID = MSP_EpmTask_UserView.ProjectUID
                LEFT OUTER JOIN
                ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON
                MSP_EpmTask_UserView.TaskUID = MSP_EpmAssignment_UserView.TaskUID
                AND MSP_EpmTask_UserView.ProjectUID = MSP_EpmAssignment_UserView.ProjectUID
                LEFT OUTER JOIN
                ProjectWebApp.dbo.MSP_EpmResource_UserView ON
                MSP_EpmAssignment_UserView.ResourceUID = MSP_EpmResource_UserView.ResourceUID
where
(MSP_EpmResource_UserView.[СДРес] like '%КБ%')
and (MSP_EpmResource_UserView.ResourceName not like '%КБМ%')
and (MSP_EpmResource_UserView.ResourceName not like '%КБЭ%')
and (MSP_EpmTask_UserView.TaskPercentCompleted < 100)
group by
MSP_EpmResource_UserView.[СДРес],
MSP_EpmResource_UserView.ResourceName


delete PortalKatek.dbo.DashboardKORemainingWork
insert into PortalKatek.dbo.DashboardKORemainingWork
select
sum(MSP_EpmAssignment_UserView.AssignmentRemainingWork) as [остТрты]
,substring(MSP_EpmResource_UserView.[СДРес],0,5) as [СДРес]
,MSP_EpmResource_UserView.ResourceName as [Сотрудник]
                FROM
                ProjectWebApp.dbo.MSP_EpmProject_UserView
                INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
                MSP_EpmProject_UserView.ProjectUID = MSP_EpmTask_UserView.ProjectUID
                LEFT OUTER JOIN
                ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON
                MSP_EpmTask_UserView.TaskUID = MSP_EpmAssignment_UserView.TaskUID
                AND MSP_EpmTask_UserView.ProjectUID = MSP_EpmAssignment_UserView.ProjectUID
                LEFT OUTER JOIN
                ProjectWebApp.dbo.MSP_EpmResource_UserView ON
                MSP_EpmAssignment_UserView.ResourceUID = MSP_EpmResource_UserView.ResourceUID
where
(MSP_EpmResource_UserView.[СДРес] like '%КБ%')
and (MSP_EpmTask_UserView.TaskPercentCompleted < 100)
and (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%НИОКР%')
and (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%Задание%')
and (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%Прочие%')
and (MSP_EpmTask_UserView.TaskIsMarked = 0)
and (MSP_EpmResource_UserView.ResourceName not like '%КБМ%')
and (MSP_EpmResource_UserView.ResourceName not like '%КБЭ%')
group by
MSP_EpmResource_UserView.[СДРес],
MSP_EpmResource_UserView.ResourceName


delete PortalKATEK.dbo.CMKO_RemarksList
insert into PortalKATEK.dbo.CMKO_RemarksList
select 
TableRes.id
,TableRes.id_Reclamation_CountErrorFirst
,TableRes.id_AspNetUsersError
from(select PortalKATEK.dbo.Reclamation.id
,ROW_NUMBER() OVER(PARTITION BY PortalKATEK.dbo.Reclamation.id ORDER BY PortalKATEK.dbo.Reclamation.id ASC) as rowNum
,PortalKATEK.dbo.Reclamation.id_Reclamation_CountErrorFirst
,PortalKATEK.dbo.Reclamation.id_AspNetUsersError
from PortalKATEK.dbo.Reclamation 
left join PortalKATEK.dbo.Reclamation_PZ on PortalKATEK.dbo.Reclamation_PZ.id_Reclamation = PortalKATEK.dbo.Reclamation.id
left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.Id = PortalKATEK.dbo.Reclamation_PZ.id_PZ_PlanZakaz
left join ProjectWebApp.dbo.MSP_EpmProject_UserView on ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz
left join PortalKATEK.dbo.Reclamation_CountError on PortalKATEK.dbo.Reclamation_CountError.id = PortalKATEK.dbo.Reclamation.id_Reclamation_CountErrorFirst
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Id = PortalKATEK.dbo.Reclamation.id_AspNetUsersError
where (PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 3 or PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 15 or PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 16)
and concat(year(Portalkatek.dbo.Reclamation.dateTimeCreate),'.', (month(Portalkatek.dbo.Reclamation.dateTimeCreate) + 2) / 3) = @periodQua
and ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] != 0
and PortalKATEK.dbo.Reclamation.id_AspNetUsersError is not null
and PortalKATEK.dbo.Reclamation.closeMKO = 1) as TableRes
where TableRes.rowNum = 1 and TableRes.id_Reclamation_CountErrorFirst != 4


delete PortalKATEK.dbo.CMKO_RemarksListG
insert into PortalKATEK.dbo.CMKO_RemarksListG
select TableResult.id
,TableResult.counId
,TableResult.id_UserKBE
from (select 
		PortalKATEK.dbo.Reclamation.id
		,PortalKATEK.dbo.Reclamation_CountError.id as [counId]
		,PortalKATEK.dbo.RKD_GIP.id_UserKBE
		,ROW_NUMBER() OVER(PARTITION BY PortalKATEK.dbo.Reclamation.id ORDER BY PortalKATEK.dbo.Reclamation.id ASC) as rowNum
		from PortalKATEK.dbo.Reclamation
		left join PortalKATEK.dbo.Reclamation_CountError on PortalKATEK.dbo.Reclamation_CountError.id = PortalKATEK.dbo.Reclamation.id_Reclamation_CountErrorFirst
		left join PortalKATEK.dbo.Reclamation_PZ on PortalKATEK.dbo.Reclamation_PZ.id_Reclamation = PortalKATEK.dbo.Reclamation.id
		left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.id = PortalKATEK.dbo.Reclamation_PZ.id_PZ_PlanZakaz
		left join ProjectWebApp.dbo.MSP_EpmProject_UserView on PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz like ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
		left join PortalKATEK.dbo.RKD_Order on PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
		left join PortalKATEK.dbo.RKD_GIP on PortalKATEK.dbo.RKD_GIP.id_RKD_Order = PortalKATEK.dbo.RKD_Order.id
		where (PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 16 or PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 3)
		and PortalKATEK.dbo.Reclamation.closeMKO = 1 
		and concat(year(Portalkatek.dbo.Reclamation.dateTimeCreate),'.', (month(Portalkatek.dbo.Reclamation.dateTimeCreate) + 2) / 3) = @periodQua 
		and PortalKATEK.dbo.Reclamation.gip = 1
		and PortalKATEK.dbo.RKD_GIP.id_UserKBE is not null) as TableResult 
		where TableResult.rowNum = 1


insert into PortalKATEK.dbo.CMKO_RemarksListG
select TableResult.id
,TableResult.counId
,TableResult.id_UserKBM
from (select 
		PortalKATEK.dbo.Reclamation.id
		,PortalKATEK.dbo.Reclamation_CountError.id as [counId]
		,PortalKATEK.dbo.RKD_GIP.id_UserKBM
		,ROW_NUMBER() OVER(PARTITION BY PortalKATEK.dbo.Reclamation.id ORDER BY PortalKATEK.dbo.Reclamation.id ASC) as rowNum
		from PortalKATEK.dbo.Reclamation
		left join PortalKATEK.dbo.Reclamation_CountError on PortalKATEK.dbo.Reclamation_CountError.id = PortalKATEK.dbo.Reclamation.id_Reclamation_CountErrorFirst
		left join PortalKATEK.dbo.Reclamation_PZ on PortalKATEK.dbo.Reclamation_PZ.id_Reclamation = PortalKATEK.dbo.Reclamation.id
		left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.id = PortalKATEK.dbo.Reclamation_PZ.id_PZ_PlanZakaz
		left join ProjectWebApp.dbo.MSP_EpmProject_UserView on PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz like ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
		left join PortalKATEK.dbo.RKD_Order on PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
		left join PortalKATEK.dbo.RKD_GIP on PortalKATEK.dbo.RKD_GIP.id_RKD_Order = PortalKATEK.dbo.RKD_Order.id
		where (PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 15)
		and PortalKATEK.dbo.Reclamation.closeMKO = 1 
		and concat(year(Portalkatek.dbo.Reclamation.dateTimeCreate),'.', (month(Portalkatek.dbo.Reclamation.dateTimeCreate) + 2) / 3) = @periodQua 
		and PortalKATEK.dbo.Reclamation.gip = 1
		and PortalKATEK.dbo.RKD_GIP.id_UserKBM is not null) as TableResult 
		where TableResult.rowNum = 1


insert into PortalKATEK.dbo.CMKO_ThisCoefManager
select PortalKATEK.dbo.CMKO_PeriodResult.[period]
,PortalKATEK.dbo.AspNetUsers.Id
,1
from PortalKATEK.dbo.CMKO_PeriodResult 
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.LockoutEnabled = 1 and (PortalKATEK.dbo.AspNetUsers.Devision = 3 or PortalKATEK.dbo.AspNetUsers.Devision = 15 or PortalKATEK.dbo.AspNetUsers.Devision = 16)
left join PortalKATEK.dbo.CMKO_ThisCoefManager on PortalKATEK.dbo.CMKO_ThisCoefManager.id_CMKO_PeriodResult = PortalKATEK.dbo.CMKO_PeriodResult.[period] and PortalKATEK.dbo.CMKO_ThisCoefManager.id_AspNetUsers = PortalKATEK.dbo.AspNetUsers.Id
where PortalKATEK.dbo.CMKO_ThisCoefManager.id is null


