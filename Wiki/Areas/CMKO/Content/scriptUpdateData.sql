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
SET @cost1N = 4.9;
SET @managerOrderPercent = 0.04;
SET @percentMKO = 0.025;
SET @percentMKBE = 0.11;
SET @percentMKBM = 0.08;

--DELETE [PortalKATEK].[dbo].[CMKO_ThisPeriod]
--insert into [PortalKATEK].[dbo].[CMKO_ThisPeriod] select @periodQua

--DELETE PortalKATEK.dbo.CMKO_BujetList
--insert into PortalKATEK.dbo.CMKO_BujetList
--select
--ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID
--,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID
--,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWBS
--,ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
--,ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceName
--,concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.', (month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate) + 2) / 3) as [quartalFinishTask]
--,iif(len(month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)) = 1, concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.0',month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)), concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.',month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate))) as [monthFinishTask]
--,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskPercentCompleted
--,PortalKATEK.dbo.PZ_PlanZakaz.id as id_PZ_PlanZakaz
--,ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
--,isnull(ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО], 0) as [counterKO]
--,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskName
--,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate
--,substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) as [Devision]
--,substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 4, 5) as [DevisionKBE]
--,isnull(PortalKATEK.dbo.PZ_TEO.SSM, 0)
--,isnull(PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker, 0) as [bujetWorkers]
--,isnull(PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager, 0) as [bujetManagers]
--,MSP_EpmTask_UserView.TaskWork
--,isnull(ProjectWebApp.dbo.MSP_EpmTask_UserView.нк, 0) as [normH]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM], 0), 0) as [normHOrderKBM]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE], 0), 0) as [normHOrderKBE]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM]), 0), 0) as [cost1NHWorkerKBM]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM]), 0), 0) as [cost1NHManagerKBM]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedWorkerForTaskKBM]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBM] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedManagerForTaskKBM]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE]), 0), 0) as [cost1NHWorkerKBE]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE]), 0), 0) as [cost1NHManagerKBE]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedWorkerForTaskKBE]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] = 0, 0, (PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager) / ReportKATEK.dbo.CMKO_TimeWorkOnOrder.[normHourKBE] * ProjectWebApp.dbo.MSP_EpmTask_UserView.НК), 0), 0) as [accruedManagerForTaskKBE]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coefBujetNWorker, 0), 0), 0) as [accruedWorkerForNTaskKBM]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coenBujetNManager, 0), 0), 0) as [accruedManagerForNTaskKBM]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coefBujetNWorker, 0), 0), 0) as [accruedWorkerForNTaskKBE]
--,isnull(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', iif(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%Задание%' or ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like '%НИОКР%', ProjectWebApp.dbo.MSP_EpmTask_UserView.нк * @cost1N * @coenBujetNManager, 0), 0), 0) as [accruedManagerForNTaskKBE]
--,PortalKATEK.dbo.RKD_GIP.id_UserKBM
--,PortalKATEK.dbo.RKD_GIP.id_UserKBE
--FROM
--ProjectWebApp.dbo.MSP_EpmProject_UserView
--INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID
--LEFT JOIN ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
--LEFT JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
--LEFT JOIN ReportKATEK.dbo.CMKO_TimeWorkOnOrder on ReportKATEK.dbo.CMKO_TimeWorkOnOrder.numberOrder = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
--left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz like ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
--left join PortalKATEK.dbo.PZ_TEO on PortalKATEK.dbo.PZ_TEO.Id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
--left join PortalKATEK.dbo.RKD_Order on PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
--left join PortalKATEK.dbo.RKD_GIP on PortalKATEK.dbo.RKD_GIP.id_RKD_Order = PortalKATEK.dbo.RKD_Order.id
--where
--(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')

--Delete PortalKATEK.dbo.CMKO_ThisOverflowsBujet
--insert into PortalKATEK.dbo.CMKO_ThisOverflowsBujet
--SELECT
--PortalKATEK.dbo.PZ_PlanZakaz.Id
--,sum(isnull(PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker, 0) + isnull(PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager, 0) - isnull(KBMFactBujet.bujet, 0) - isnull(ThisBujetList.bujetKBM, 0)) as KBM
--,sum(isnull(PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetWorker, 0) + isnull(PortalKATEK.dbo.PZ_TEO.SSM * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] * @coefBujetManager, 0) - isnull(KBEFactBujet.bujet, 0) - isnull(ThisBujetList.bujetKBE, 0)) as KBE
--FROM
--ProjectWebApp.dbo.MSP_EpmProject_UserView
--left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.ProjectUID = ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID
--left join (select id_PZ_PlanZakaz, sum([data]) as bujet from PortalKATEK.dbo.CMKO_ProjectFactBujet where devision like 'КБМ' group by id_PZ_PlanZakaz) as KBMFactBujet on KBMFactBujet.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
--left join (select id_PZ_PlanZakaz, sum([data]) as bujet from PortalKATEK.dbo.CMKO_ProjectFactBujet where devision like 'КБЭ' group by id_PZ_PlanZakaz) as KBEFactBujet on KBEFactBujet.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
--left join (select id_PZ_PlanZakaz, sum(accruedWorkerForTaskKBM) + sum(accruedManagerForTaskKBM) as [bujetKBM], sum(accruedWorkerForTaskKBE) + sum(accruedManagerForTaskKBE) as [bujetKBE] from PortalKATEK.dbo.CMKO_BujetList where PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask = @periodQua group by id_PZ_PlanZakaz) as ThisBujetList on ThisBujetList.id_PZ_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
--left join PortalKATEK.dbo.PZ_TEO on PortalKATEK.dbo.PZ_TEO.Id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
--WHERE
--concat(year(PortalKATEK.dbo.PZ_PlanZakaz.dataOtgruzkiBP),'.', (month(PortalKATEK.dbo.PZ_PlanZakaz.dataOtgruzkiBP) + 2) / 3) = @periodQua
--group by PortalKATEK.dbo.PZ_PlanZakaz.Id

--DELETE PortalKATEK.dbo.CMKO_ThisBujetDevision
--INSERT INTO PortalKATEK.dbo.CMKO_ThisBujetDevision
--SELECT
--sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) as accruedAllKBM
--,sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) as accruedAllKBE
--,sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) as accruedWorkerKBM
--,sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) as accruedWorkerKBE
--,sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) as accruedManagerKBM
--,sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) as accruedManagerKBE
--,(max(Overflows.SumKBM) * @coefBujetNWorker) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) as accruedWorkerOverflowsKBM
--,(max(Overflows.SumKBE) * @coefBujetNWorker) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) as accruedWorkerOverflowsKBE
--,(max(Overflows.SumKBM) * @coenBujetNManager) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) as accruedManagerOverflowsKBM
--,(max(Overflows.SumKBE) * @coenBujetNManager) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) as accruedManagerOverflowsKBE
--,sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) + max(Overflows.SumKBM) as accruedOverflowsKBM
--,sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) + max(Overflows.SumKBE) as accruedOverflowsKBE
--,sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBM is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0), 0)) as accruedKBMG
--,sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBE is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0), 0)) as accruedKBEG
--,sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBM is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0), 0)) + max(Overflows.SumKBM) as accruedOverflowsKBMG
--,sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBE is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0), 0)) + max(Overflows.SumKBE) as accruedOverflowsKBEG
--,(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) + max(Overflows.SumKBM)) * @percentMKO as bujetMKOFromM
--,(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) + max(Overflows.SumKBE)) * @percentMKO as bujetMKOFromE
--,(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBM) + max(Overflows.SumKBM)) * @percentMKBM as bujetMKBM
--,(sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForNTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE) + sum(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForNTaskKBE) + max(Overflows.SumKBE)) * @percentMKBE as bujetMKBE
--,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBM is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBM, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBM, 0), 0)) + max(Overflows.SumKBM)) * @managerOrderPercent as bujetGM
--,(sum(iif(PortalKATEK.dbo.CMKO_BujetList.id_RKD_GIP_KBE is not null, isnull(PortalKATEK.dbo.CMKO_BujetList.accruedWorkerForTaskKBE, 0) + isnull(PortalKATEK.dbo.CMKO_BujetList.accruedManagerForTaskKBE, 0), 0)) + max(Overflows.SumKBE)) * @managerOrderPercent as bujetGE
--,0 as bonusMFinal
--,0 as bonusEFinal
--FROM
--[PortalKATEK].[dbo].[CMKO_BujetList] 
--left join (select MAX(id) as id, sum(KBM) as [SumKBM], sum(KBE) as [SumKBE] from PortalKATEK.dbo.CMKO_ThisOverflowsBujet) as Overflows on Overflows.id > 0
--where
--[PortalKATEK].[dbo].[CMKO_BujetList].quartalFinishTask = @periodQua

--update PortalKATEK.dbo.CMKO_ThisBujetDevision set
--bonusMFinal = accruedManagerOverflowsKBM - bujetMKOFromM - bujetMKBM - bujetGM
--,bonusEFinal = accruedManagerOverflowsKBE - bujetMKOFromE - bujetMKBE - bujetGE

--delete [PortalKATEK].[dbo].[DashboardKOMP1]
--insert into [PortalKATEK].[dbo].[DashboardKOMP1]
--SELECT
--[PortalKATEK].[dbo].[ProductionCalendar].[period],
--[PortalKATEK].[dbo].[AspNetUsers].CiliricalName as ciliricalName,
--[PortalKATEK].[dbo].[Devision].[name] as devisionName
--,[PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] as [plan]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.10 as [plan10]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.20 as [plan20]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.30 as [plan30]
--,tableResPower.SumAssignmentActualWork
--,tableResPower.SumAssignmentWork
--FROM [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan] left join 
--[PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].Id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_AspNetUsers] left join
--[PortalKATEK].[dbo].[Devision] on [PortalKATEK].[dbo].[Devision].id = [PortalKATEK].[dbo].[AspNetUsers].Devision left join
--[PortalKATEK].[dbo].[ProductionCalendar] on [PortalKATEK].[dbo].[ProductionCalendar].id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_ProductionCalendar] left join
--(SELECT iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)))  as [Месяц завершения задачи]
--,MSP_EpmResource_UserView.ResourceUID
--,sum(iif(ProjectWebApp.dbo.MSP_EpmTask_UserView.НК = 0, 0, ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork * MSP_EpmAssignmentByDay_UserView.AssignmentWork)) as НК
--FROM
--ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN
--ProjectWebApp.dbo.MSP_EpmTask_UserView ON ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN
--ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
--ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN
--ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID 
--WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0) and (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
--group by iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)),
--concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay))),
--substring(MSP_EpmResource_UserView.СДРес, 0, 4), MSP_EpmResource_UserView.ResourceUID) 
--as tableResult on tableResult.[Месяц завершения задачи] = [PortalKATEK].[dbo].[ProductionCalendar].period and tableResult.ResourceUID = [PortalKATEK].[dbo].AspNetUsers.ResourceUID
--left join
--(SELECT ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID ,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork) as SumAssignmentActualWork
--,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) as SumAssignmentWork 
--FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
--ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON
--ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignment_UserView
--ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
--LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
--WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.НК > 0) and ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0 AND (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
--and (concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay)) = @periodM1)
--group by ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID) as tableResPower on tableResPower.ResourceUID = [PortalKATEK].[dbo].[AspNetUsers].ResourceUID
--where ([PortalKATEK].[dbo].[ProductionCalendar].[period] = @periodMP1)

--delete [PortalKATEK].[dbo].[DashboardKOMP2]
--insert into [PortalKATEK].[dbo].[DashboardKOMP2]
--SELECT
--[PortalKATEK].[dbo].[ProductionCalendar].[period],
--[PortalKATEK].[dbo].[AspNetUsers].CiliricalName as ciliricalName,
--[PortalKATEK].[dbo].[Devision].[name] as devisionName
--,[PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] as [plan]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.10 as [plan10]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.20 as [plan20]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.30 as [plan30]
--,tableResPower.SumAssignmentActualWork
--,tableResPower.SumAssignmentWork
--FROM [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan] left join 
--[PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].Id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_AspNetUsers] left join
--[PortalKATEK].[dbo].[Devision] on [PortalKATEK].[dbo].[Devision].id = [PortalKATEK].[dbo].[AspNetUsers].Devision left join
--[PortalKATEK].[dbo].[ProductionCalendar] on [PortalKATEK].[dbo].[ProductionCalendar].id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_ProductionCalendar] left join
--(SELECT iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)))  as [Месяц завершения задачи]
--,MSP_EpmResource_UserView.ResourceUID
--,sum(iif(ProjectWebApp.dbo.MSP_EpmTask_UserView.НК = 0, 0, ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork * MSP_EpmAssignmentByDay_UserView.AssignmentWork)) as НК
--FROM
--ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN
--ProjectWebApp.dbo.MSP_EpmTask_UserView ON ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN
--ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
--ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN
--ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID 
--WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0) and (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
--group by iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)),
--concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay))),
--substring(MSP_EpmResource_UserView.СДРес, 0, 4), MSP_EpmResource_UserView.ResourceUID) 
--as tableResult on tableResult.[Месяц завершения задачи] = [PortalKATEK].[dbo].[ProductionCalendar].period and tableResult.ResourceUID = [PortalKATEK].[dbo].AspNetUsers.ResourceUID
--left join
--(SELECT ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID ,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork) as SumAssignmentActualWork
--,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) as SumAssignmentWork 
--FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
--ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON
--ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignment_UserView
--ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
--LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
--WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.НК > 0) and ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0 AND (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
--and (concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay)) = @periodM2)
--group by ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID) as tableResPower on tableResPower.ResourceUID = [PortalKATEK].[dbo].[AspNetUsers].ResourceUID
--where ([PortalKATEK].[dbo].[ProductionCalendar].[period] = @periodMP2)

--delete [PortalKATEK].[dbo].[DashboardKOMP3]
--insert into [PortalKATEK].[dbo].[DashboardKOMP3]
--SELECT
--[PortalKATEK].[dbo].[ProductionCalendar].[period],
--[PortalKATEK].[dbo].[AspNetUsers].CiliricalName as ciliricalName,
--[PortalKATEK].[dbo].[Devision].[name] as devisionName
--,[PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] as [plan]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.10 as [plan10]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.20 as [plan20]
--,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * @coefConvertCalendarNorm * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.30 as [plan30]
--,tableResPower.SumAssignmentActualWork
--,tableResPower.SumAssignmentWork
--FROM [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan] left join 
--[PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].Id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_AspNetUsers] left join
--[PortalKATEK].[dbo].[Devision] on [PortalKATEK].[dbo].[Devision].id = [PortalKATEK].[dbo].[AspNetUsers].Devision left join
--[PortalKATEK].[dbo].[ProductionCalendar] on [PortalKATEK].[dbo].[ProductionCalendar].id = [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[id_ProductionCalendar] left join
--(SELECT iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)))  as [Месяц завершения задачи]
--,MSP_EpmResource_UserView.ResourceUID
--,sum(iif(ProjectWebApp.dbo.MSP_EpmTask_UserView.НК = 0, 0, ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork * MSP_EpmAssignmentByDay_UserView.AssignmentWork)) as НК
--FROM
--ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN
--ProjectWebApp.dbo.MSP_EpmTask_UserView ON ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN
--ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN
--ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID LEFT OUTER JOIN
--ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID 
--WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0) and (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
--group by iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)),
--concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay))),
--substring(MSP_EpmResource_UserView.СДРес, 0, 4), MSP_EpmResource_UserView.ResourceUID) 
--as tableResult on tableResult.[Месяц завершения задачи] = [PortalKATEK].[dbo].[ProductionCalendar].period and tableResult.ResourceUID = [PortalKATEK].[dbo].AspNetUsers.ResourceUID
--left join
--(SELECT ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID ,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentActualWork) as SumAssignmentActualWork
--,sum((ProjectWebApp.dbo.MSP_EpmTask_UserView.НК / ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork) * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork) as SumAssignmentWork 
--FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
--ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView ON
--ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.ProjectUID LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmAssignment_UserView
--ON ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
--LEFT OUTER JOIN ProjectWebApp.dbo.MSP_EpmResource_UserView ON ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
--WHERE (ProjectWebApp.dbo.MSP_EpmTask_UserView.НК > 0) and ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork > 0 AND (ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
--and (concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay)) = @periodM3)
--group by ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID) as tableResPower on tableResPower.ResourceUID = [PortalKATEK].[dbo].[AspNetUsers].ResourceUID
--where ([PortalKATEK].[dbo].[ProductionCalendar].[period] = @periodMP3)

delete [PortalKATEK].[dbo].[CMKO_ThisBujetUsersBonus]
insert into [PortalKATEK].[dbo].[CMKO_ThisBujetUsersBonus]
Select
PortalKATEK.dbo.AspNetUsers.Id as id_AspNetUsers
,isnull(max(OptimizationTable.countIdea), 0) as optimization
,isnull(max(SpeedUser1.plan10) + max(SpeedUser1.plan20), 0) as speed1
,isnull(max(SpeedUser2.plan10) + max(SpeedUser2.plan20), 0) as speed2
,isnull(max(SpeedUser3.plan10) + max(SpeedUser3.plan20), 0) as speed3
,isnull(sum(TeachTable.cost), 0) as teach
,max(PortalKATEK.dbo.CMKO_TaxCatigories.salary) / Curency1.USD as rate1
,max(PortalKATEK.dbo.CMKO_TaxCatigories.salary) / iif(Curency2.USD is null, Curency1.USD, Curency2.USD) as rate2
,max(PortalKATEK.dbo.CMKO_TaxCatigories.salary) / iif(Curency3.USD is null, iif(Curency2.USD is null, Curency1.USD, Curency2.USD), Curency3.USD) as rate3
,max(PortalKATEK.dbo.AspNetUsers.tax) / Curency1.USD as tax1
,max(PortalKATEK.dbo.AspNetUsers.tax) / iif(Curency2.USD is null, Curency1.USD, Curency2.USD) as tax2
,max(PortalKATEK.dbo.AspNetUsers.tax) / iif(Curency3.USD is null, iif(Curency2.USD is null, Curency1.USD, Curency2.USD), Curency3.USD) as tax3
,1 - ((400 * ReclamationCounter.countError) / (100 * sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.normH, 0)))) as coefError
,1 - ((400 * ReclamationCounter.countError) / (100 * sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.normH, 0)))) as coefErrorG
,iif(1 - ((400 * ReclamationCounter.countError) / (100 * sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.normH, 0)))) >= 0.99, 100, 0) as qualityBonus
,sum(PortalKATEK.dbo.CMKO_BujetList.normH) as nhPlan
,sum(iif(PortalKATEK.dbo.CMKO_BujetList.TaskPercentCompleted = 100, PortalKATEK.dbo.CMKO_BujetList.normH, 0)) as nhFact
,0 as nhGPlan
,0 as nhGFact
from
PortalKATEK.dbo.CMKO_BujetList
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.ResourceUID = PortalKATEK.dbo.CMKO_BujetList.ResourceUID
left join (select COUNT(id) as countIdea, id_AspNetUsersIdea from PortalKATEK.dbo.CMKO_Optimization group by PortalKATEK.dbo.CMKO_Optimization.id_AspNetUsersIdea) as OptimizationTable on OptimizationTable.id_AspNetUsersIdea = PortalKATEK.dbo.AspNetUsers.Id
left join (select iif(plan10 < normHoureFact, 50, 0) as plan10, iif(plan20 < normHoureFact, 100, 0) as plan20, ciliricalName from PortalKATEK.dbo.DashboardKOMP1) as SpeedUser1 on SpeedUser1.ciliricalName = PortalKATEK.dbo.AspNetUsers.CiliricalName
left join (select iif(plan10 < normHoureFact, 50, 0) as plan10, iif(plan20 < normHoureFact, 100, 0) as plan20, ciliricalName from PortalKATEK.dbo.DashboardKOMP2) as SpeedUser2 on SpeedUser2.ciliricalName = PortalKATEK.dbo.AspNetUsers.CiliricalName
left join (select iif(plan10 < normHoureFact, 50, 0) as plan10, iif(plan20 < normHoureFact, 100, 0) as plan20, ciliricalName from PortalKATEK.dbo.DashboardKOMP3) as SpeedUser3 on SpeedUser3.ciliricalName = PortalKATEK.dbo.AspNetUsers.CiliricalName
left join PortalKATEK.dbo.CMKO_TaxCatigories on PortalKATEK.dbo.CMKO_TaxCatigories.id = PortalKATEK.dbo.AspNetUsers.id_CMKO_TaxCatigories
left join (select PortalKATEK.dbo.Reclamation.id_AspNetUsersError, sum(iif(PortalKATEK.dbo.Reclamation.gip = 0, PortalKATEK.dbo.Reclamation_CountError.[count], 0)) as countError
			,sum(iif(PortalKATEK.dbo.Reclamation.gip = 1, PortalKATEK.dbo.Reclamation_CountError.[count], 0)) as countErrorG 
			from PortalKATEK.dbo.Reclamation 
			left join PortalKATEK.dbo.Reclamation_PZ on PortalKATEK.dbo.Reclamation_PZ.id_Reclamation = PortalKATEK.dbo.Reclamation.id
			left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.Id = PortalKATEK.dbo.Reclamation_PZ.id_PZ_PlanZakaz
			left join ProjectWebApp.dbo.MSP_EpmProject_UserView on ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] like PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz
			left join PortalKATEK.dbo.Reclamation_CountError on PortalKATEK.dbo.Reclamation_CountError.id = PortalKATEK.dbo.Reclamation.id_Reclamation_CountErrorFirst
			where (PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 3 or PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 15 or PortalKATEK.dbo.Reclamation.id_DevisionReclamation = 16)
			and concat(year(Portalkatek.dbo.Reclamation.dateTimeCreate),'.', (month(Portalkatek.dbo.Reclamation.dateTimeCreate) + 2) / 3) = @periodQua
			and ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во КО] != 0
			and PortalKATEK.dbo.Reclamation.id_AspNetUsersError is not null
			group by 
			PortalKATEK.dbo.Reclamation.id_AspNetUsersError) as ReclamationCounter on ReclamationCounter.id_AspNetUsersError = PortalKATEK.dbo.AspNetUsers.Id
left join (select * from PortalKATEK.dbo.CMKO_Teach where PortalKATEK.dbo.CMKO_Teach.id_CMKO_PeriodResult = @periodQua) as TeachTable on TeachTable.id_AspNetUsersTeacher = PortalKATEK.dbo.AspNetUsers.Id
left join (select top 1 * from PortalKATEK.dbo.CurencyBYN where YEAR(PortalKATEK.dbo.CurencyBYN.[date]) = SUBSTRING(@periodM1, 0, 5) and month(PortalKATEK.dbo.CurencyBYN.[date]) = SUBSTRING(@periodM1, 6, 3) order by PortalKATEK.dbo.CurencyBYN.date desc) as Curency1 on Curency1.USD > 0
left join (select top 1 * from PortalKATEK.dbo.CurencyBYN where YEAR(PortalKATEK.dbo.CurencyBYN.[date]) = SUBSTRING(@periodM2, 0, 5) and month(PortalKATEK.dbo.CurencyBYN.[date]) = SUBSTRING(@periodM2, 6, 3) order by PortalKATEK.dbo.CurencyBYN.date desc) as Curency2 on Curency1.USD > 0
left join (select top 1 * from PortalKATEK.dbo.CurencyBYN where YEAR(PortalKATEK.dbo.CurencyBYN.[date]) = SUBSTRING(@periodM3, 0, 5) and month(PortalKATEK.dbo.CurencyBYN.[date]) = SUBSTRING(@periodM3, 6, 3) order by PortalKATEK.dbo.CurencyBYN.date desc) as Curency3 on Curency1.USD > 0
where PortalKATEK.dbo.AspNetUsers.LockoutEnabled = 1
and (PortalKATEK.dbo.AspNetUsers.Devision = 3 or PortalKATEK.dbo.AspNetUsers.Devision = 15 or PortalKATEK.dbo.AspNetUsers.Devision = 16)
and PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask = @periodQua
group by
PortalKATEK.dbo.AspNetUsers.Id
,ReclamationCounter.countError
,Curency1.USD 
,Curency2.USD 
,Curency3.USD 


update [PortalKATEK].[dbo].[CMKO_ThisBujetUsersBonus] set
[PortalKATEK].[dbo].[CMKO_ThisBujetUsersBonus].nhGPlan = TableNorm.normPlan
,[PortalKATEK].[dbo].[CMKO_ThisBujetUsersBonus].nhGFact = TableNorm.normFact
from
(select 
PortalKATEK.dbo.CMKO_ThisBujetUsersBonus.id_AspNetUsers
,isnull(sum(BujetListM.[normH]), 0) + isnull(sum(BujetListE.normH), 0) as normPlan
,isnull(sum(iif(BujetListM.TaskPercentCompleted = 100, BujetListM.[normH], 0)), 0) + isnull(sum(iif(BujetListE.TaskPercentCompleted = 100, BujetListE.[normH], 0)), 0) as normFact
from PortalKATEK.dbo.CMKO_ThisBujetUsersBonus
left join (select * from PortalKATEK.dbo.CMKO_BujetList where PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask = @periodQua) as BujetListM on BujetListM.id_RKD_GIP_KBM = PortalKATEK.dbo.CMKO_ThisBujetUsersBonus.id_AspNetUsers
left join (select * from PortalKATEK.dbo.CMKO_BujetList where PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask = @periodQua) as BujetListE on BujetListE.id_RKD_GIP_KBE = PortalKATEK.dbo.CMKO_ThisBujetUsersBonus.id_AspNetUsers
group by
PortalKATEK.dbo.CMKO_ThisBujetUsersBonus.id_AspNetUsers) as TableNorm
where [PortalKATEK].[dbo].[CMKO_ThisBujetUsersBonus].id_AspNetUsers = TableNorm.id_AspNetUsers

--SELECT 
--AspNetUsers.id
--,PortalKATEK.dbo.CMKO_PeriodResult.period
--,isnull(PortalKATEK.dbo.CMKO_TaxCatigories.salary, 0)
--,isnull(AspNetUsers.tax, 0)
--,AspNetUsers.CiliricalName
--FROM [PortalKATEK].[dbo].[CMKO_PeriodResult] left join
--(select * from PortalKATEK.dbo.AspNetUsers where PortalKATEK.dbo.AspNetUsers.LockoutEnabled = 1  and (PortalKATEK.dbo.AspNetUsers.Devision = 3 or PortalKATEK.dbo.AspNetUsers.Devision = 15 or PortalKATEK.dbo.AspNetUsers.Devision = 16))
--   as AspNetUsers on AspNetUsers.LockoutEnabled > 0 
--left join [PortalKATEK].[dbo].[CMKO_TaxFact] on [PortalKATEK].[dbo].[CMKO_TaxFact].id_AspNetUsers = AspNetUsers.Id
--left join PortalKATEK.dbo.CMKO_TaxCatigories on PortalKATEK.dbo.CMKO_TaxCatigories.id = AspNetUsers.id_CMKO_TaxCatigories
--where [PortalKATEK].[dbo].[CMKO_TaxFact].id is null
