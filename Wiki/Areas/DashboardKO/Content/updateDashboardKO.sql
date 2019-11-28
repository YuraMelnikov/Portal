DECLARE @periodQua NVARCHAR(6);
DECLARE @periodM1 NVARCHAR(7);
DECLARE @periodM2 NVARCHAR(7);
DECLARE @periodM3 NVARCHAR(7);
DECLARE @periodMP1 NVARCHAR(7);
DECLARE @periodMP2 NVARCHAR(7);
DECLARE @periodMP3 NVARCHAR(7);
DECLARE @coefConvertCalendarNorm float;
SET @coefConvertCalendarNorm = 0.9;
SET @periodQua ='2019.4';
SET @periodM1 ='2019.10';
SET @periodM2 ='2019.11';
SET @periodM3 ='2019.12';
SET @periodMP1 ='2019.10';
SET @periodMP2 ='2019.11';
SET @periodMP3 ='2019.12';
--DashboardKOQuaHumen
delete PortalKatek.dbo.DashboardKOQuaHumen
insert into PortalKatek.dbo.DashboardKOQuaHumen
select
sum(MSP_EpmTask_UserView.[НК]) as [Нормачасы],
MSP_EpmResource_UserView.ResourceName as [Сотрудник]
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
and
(MSP_EpmTask_UserView.TaskPercentCompleted=100)
and
(concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',(month(MSP_EpmTask_UserView.TaskFinishDate)+2)/3) like @periodQua)
and
(MSP_EpmTask_UserView.TaskName not like '%отпуск%')
group by
MSP_EpmResource_UserView.ResourceName,
concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',(month(MSP_EpmTask_UserView.TaskFinishDate)+2)/3)
--DashboardKOM1
delete PortalKatek.dbo.DashboardKOM1
insert into PortalKatek.dbo.DashboardKOM1
select
concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',month(MSP_EpmTask_UserView.TaskFinishDate)) as [month],
isnull(sum(MSP_EpmTask_UserView.[НК]), 0) as [Нормачасы],
MSP_EpmResource_UserView.ResourceName as [Сотрудник]
,substring(MSP_EpmResource_UserView.[СДРес],0,5) as [СДРес]
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
and
(MSP_EpmTask_UserView.TaskPercentCompleted=100)
and
(concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',month(MSP_EpmTask_UserView.TaskFinishDate)) like @periodM1)
and
(MSP_EpmTask_UserView.TaskName not like '%отпуск%')
group by
MSP_EpmResource_UserView.ResourceName,
concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',month(MSP_EpmTask_UserView.TaskFinishDate))
,MSP_EpmResource_UserView.[СДРес]
--DashboardKOM2
delete PortalKatek.dbo.DashboardKOM2

insert into PortalKatek.dbo.DashboardKOM2
select
concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',month(MSP_EpmTask_UserView.TaskFinishDate)) as [month],
isnull(sum(MSP_EpmTask_UserView.[НК]), 0) as [Нормачасы],
MSP_EpmResource_UserView.ResourceName as [Сотрудник]
,substring(MSP_EpmResource_UserView.[СДРес],0,5) as [СДРес]
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
and
(MSP_EpmTask_UserView.TaskPercentCompleted=100)
and
(concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',month(MSP_EpmTask_UserView.TaskFinishDate)) like @periodM2)
and
(MSP_EpmTask_UserView.TaskName not like '%отпуск%')
group by
MSP_EpmResource_UserView.ResourceName,
concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',month(MSP_EpmTask_UserView.TaskFinishDate))
,MSP_EpmResource_UserView.[СДРес]
--DashboardKOM3
delete PortalKatek.dbo.DashboardKOM3
insert into PortalKatek.dbo.DashboardKOM3
select
concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',month(MSP_EpmTask_UserView.TaskFinishDate)) as [month],
isnull(sum(MSP_EpmTask_UserView.[НК]), 0) as [Нормачасы],
MSP_EpmResource_UserView.ResourceName as [Сотрудник]
,substring(MSP_EpmResource_UserView.[СДРес],0,5) as [СДРес]
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
and
(MSP_EpmTask_UserView.TaskPercentCompleted=100)
and
(concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',month(MSP_EpmTask_UserView.TaskFinishDate)) like @periodM3)
and
(MSP_EpmTask_UserView.TaskName not like '%отпуск%')
group by
MSP_EpmResource_UserView.ResourceName,
concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',month(MSP_EpmTask_UserView.TaskFinishDate))
,MSP_EpmResource_UserView.[СДРес]

--DashboardKOQuartal
delete PortalKatek.dbo.DashboardKOQuartal
insert into PortalKatek.dbo.DashboardKOQuartal
select
concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',(month(MSP_EpmTask_UserView.TaskFinishDate)+2)/3) as [quartal],
iif(sum(MSP_EpmTask_UserView.[НК]) is null, 0, sum(MSP_EpmTask_UserView.[НК])) as [Нормачасы],
MSP_EpmResource_UserView.ResourceName as [Сотрудник]
,substring(MSP_EpmResource_UserView.[СДРес],0,5) as [СДРес]
,MSP_EpmTask_UserView.TaskFinishDate as [Окончание]
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
and
(MSP_EpmTask_UserView.TaskPercentCompleted=100)
and
(concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',(month(MSP_EpmTask_UserView.TaskFinishDate)+2)/3) like @periodQua)
and
(MSP_EpmTask_UserView.TaskName not like '%отпуск%')
group by
MSP_EpmResource_UserView.ResourceName,
concat(year(MSP_EpmTask_UserView.TaskFinishDate),'.',(month(MSP_EpmTask_UserView.TaskFinishDate)+2)/3)
,MSP_EpmResource_UserView.[СДРес]
,MSP_EpmTask_UserView.TaskFinishDate
--DashboardKOHssPO
delete PortalKatek.dbo.DashboardKOHssPO
insert into PortalKatek.dbo.DashboardKOHssPO
SELECT [Dashboard].[dbo].[1910_DB_прибыль].[quart]
      ,sum([Dashboard].[dbo].[1910_DB_прибыль].[ХСС]) as hss
FROM [Dashboard].[dbo].[1910_DB_прибыль]
where
[Dashboard].[dbo].[1910_DB_прибыль].[year] > year(getdate()) - 2
group by [Dashboard].[dbo].[1910_DB_прибыль].[quart]
--DashboardKOKBHss
delete PortalKatek.dbo.DashboardKOKBHss
insert into PortalKatek.dbo.DashboardKOKBHss
select
concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.',(month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)+2)/3) as [Quart]
,sum(iif(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБМ%',ProjectWebApp.dbo.MSP_EpmTask_UserView.НК*[Dashboard].[dbo].[2016_katek_KO_1normahour].[1нч$],0)) as [KBM]
,sum(iif(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБЭ%',ProjectWebApp.dbo.MSP_EpmTask_UserView.НК*[Dashboard].[dbo].[2016_katek_KO_1normahour].[1нч$КБЭ],0)) as [KBE]
,0
,0
				FROM
                ProjectWebApp.dbo.MSP_EpmProject_UserView
                INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
                ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID
                LEFT OUTER JOIN
                ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON
                ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID
                AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
                LEFT OUTER JOIN
                ProjectWebApp.dbo.MSP_EpmResource_UserView ON
                ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
				LEFT OUTER JOIN
                [Dashboard].[dbo].[2016_katek_KO_1normahour] ON
                ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] = [Dashboard].[dbo].[2016_katek_KO_1normahour].[№ заказа]
				inner join
				[Dashboard].[dbo].[2016_KATEK_KBE] on
				[Dashboard].[dbo].[2016_KATEK_KBE].[№ заказа] = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] 
where
(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate) > year(getdate()) - 2)
and
(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%НИОКР%')
and
(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%Задание%')
and
(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
and
(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskIsSummary = 0)
group by concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.',(month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)+2)/3)


update [PortalKATEK].[dbo].[DashboardBP_State] set [PortalKATEK].[dbo].[DashboardBP_State].active = 0
insert into [PortalKATEK].[dbo].[DashboardBP_State](datetime, numberWeek, active)
VALUES (getdate(), DATEPART (wk, getdate()), 1);

delete PortalKATEK.dbo.DashboardKOTimesheet
insert into PortalKATEK.dbo.DashboardKOTimesheet
SELECT
MSP_EpmAssignmentByDay_UserView.TimeByDay as [date],
MSP_EpmResource_UserView.ResourceName as [user],
sum(MSP_EpmAssignmentByDay_UserView.AssignmentActualWork) as work
FROM
ProjectWebApp.dbo.MSP_EpmProject_UserView
INNER JOIN
ProjectWebApp.dbo.MSP_EpmTask_UserView
ON
MSP_EpmProject_UserView.ProjectUID = MSP_EpmTask_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView
ON
MSP_EpmTask_UserView.TaskUID = MSP_EpmAssignmentByDay_UserView.TaskUID
AND
MSP_EpmTask_UserView.ProjectUID = MSP_EpmAssignmentByDay_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmAssignment_UserView
ON
MSP_EpmTask_UserView.TaskUID = MSP_EpmAssignment_UserView.TaskUID
AND
MSP_EpmTask_UserView.ProjectUID = MSP_EpmAssignment_UserView.ProjectUID
LEFT OUTER JOIN
ProjectWebApp.dbo.MSP_EpmResource_UserView
ON
MSP_EpmAssignment_UserView.ResourceUID = MSP_EpmResource_UserView.ResourceUID
WHERE
(MSP_EpmAssignmentByDay_UserView.AssignmentActualWork > 0)
AND
(MSP_EpmResource_UserView.[СДРес] like '%КБ%')
and
(MSP_EpmAssignmentByDay_UserView.TimeByDay > getdate() - 14)
group by
MSP_EpmAssignmentByDay_UserView.TimeByDay,
MSP_EpmResource_UserView.ResourceName

update [PortalKATEK].[dbo].[DashboardKOKBHss] set 
[PortalKATEK].[dbo].[DashboardKOKBHss].KBER = [PortalKATEK].[dbo].[DashboardKOKBHss].KBE,
[PortalKATEK].[dbo].[DashboardKOKBHss].KBMR = [PortalKATEK].[dbo].[DashboardKOKBHss].KBM

update [PortalKATEK].[dbo].[DashboardKOKBHss] set 
[PortalKATEK].[dbo].[DashboardKOKBHss].KBER = isnull([PortalKATEK].[dbo].[DashboardKOKBHss].KBE,0) - isnull([TableResult].KBE,0),
[PortalKATEK].[dbo].[DashboardKOKBHss].KBMR = isnull([PortalKATEK].[dbo].[DashboardKOKBHss].KBM,0) - isnull([TableResult].KBM,0)
from
(select 
[qua],
sum([KBM]) as [KBM]
,sum([KBE]) as [KBE]
 from 
(SELECT 
concat(year([PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP), '.', datepart(q,[PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP)) AS [qua]
,sum(HSS_KBM.KBM) + sum(Fact_KBM.payment) - ([PortalKATEK].[dbo].[PZ_TEO].SSM / 1000 * 12.5) as [KBM]
,sum(HSS_KBM.KBE) + sum(Fact_KBE.payment) - ([PortalKATEK].[dbo].[PZ_TEO].SSM / 1000 * 12.5) as [KBE]
FROM [PortalKATEK].[dbo].[PZ_PlanZakaz]
left join (select
ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
,sum(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБМ%', ProjectWebApp.dbo.MSP_EpmTask_UserView.НК*[Dashboard].[dbo].[2016_katek_KO_1normahour].[1нч$], 0)) as [KBM]
,sum(iif(substring(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес], 0, 4) like '%КБЭ%', ProjectWebApp.dbo.MSP_EpmTask_UserView.НК*[Dashboard].[dbo].[2016_katek_KO_1normahour].[1нч$КБЭ], 0)) as [KBE]
                FROM
                ProjectWebApp.dbo.MSP_EpmProject_UserView
                INNER JOIN ProjectWebApp.dbo.MSP_EpmTask_UserView ON
                ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID
                LEFT OUTER JOIN
                ProjectWebApp.dbo.MSP_EpmAssignment_UserView ON
                ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID
                AND ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ProjectUID
                LEFT OUTER JOIN
                ProjectWebApp.dbo.MSP_EpmResource_UserView ON
                ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
				LEFT OUTER JOIN
                [Dashboard].[dbo].[2016_katek_KO_1normahour] ON
                ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] = [Dashboard].[dbo].[2016_katek_KO_1normahour].[№ заказа]
				LEFT OUTER JOIN
				[Dashboard].[dbo].[2016_KATEK_KBE] on
				[Dashboard].[dbo].[2016_KATEK_KBE].[№ заказа] = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] 
where
(ProjectWebApp.dbo.MSP_EpmResource_UserView.[СДРес] like '%КБ%')
and
(concat(year(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate),'.',(month(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskFinishDate)+2)/3) >= @periodQua)
group by
ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]) as HSS_KBM on HSS_KBM.[№ заказа] like [PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz
left join (SELECT sum([data]) as payment ,[id_PZ_PlanZakaz] FROM [PortalKATEK].[dbo].[CMKO_ProjectFactBujet] where [devision] like '%КБМ%' group by [id_PZ_PlanZakaz])
	as Fact_KBM on Fact_KBM.[id_PZ_PlanZakaz] = [PortalKATEK].[dbo].[PZ_PlanZakaz].Id
left join (SELECT sum([data]) as payment ,[id_PZ_PlanZakaz] FROM [PortalKATEK].[dbo].[CMKO_ProjectFactBujet] where [devision] like '%КБЭ%' group by [id_PZ_PlanZakaz])
	as Fact_KBE on Fact_KBE.id_PZ_PlanZakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].Id
left join [PortalKATEK].[dbo].[PZ_TEO] on [PortalKATEK].[dbo].[PZ_TEO].Id_PlanZakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].Id
WHERE
concat(year([PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP), '.', datepart(q,[PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP)) >= @periodQua
group by [PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz
,concat(year([PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP), '.', datepart(q,[PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP))
,[PortalKATEK].[dbo].[PZ_TEO].SSM / 1000 * 12.5) as T1 group by [qua]) as [TableResult]
where [TableResult].qua = [PortalKATEK].[dbo].[DashboardKOKBHss].Quart

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
and (concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay)) = @periodM3)
group by ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID) as tableResPower on tableResPower.ResourceUID = [PortalKATEK].[dbo].[AspNetUsers].ResourceUID
where ([PortalKATEK].[dbo].[ProductionCalendar].[period] = @periodMP3)