DECLARE @periodQua NVARCHAR(6);
DECLARE @periodM1 NVARCHAR(7);
DECLARE @periodM2 NVARCHAR(7);
DECLARE @periodM3 NVARCHAR(7);
SET @periodQua ='2019.3';
SET @periodM1 ='2019.7';
SET @periodM2 ='2019.8';
SET @periodM3 ='2019.9';
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
sum(MSP_EpmTask_UserView.[НК]) as [Нормачасы],
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
sum(MSP_EpmTask_UserView.[НК]) as [Нормачасы],
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
--DashboardKORemainingWork
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
and
(MSP_EpmTask_UserView.TaskPercentCompleted < 100)
and
(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%НИОКР%')
and
(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%Задание%')
and
(ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%Прочие%')
and
(MSP_EpmTask_UserView.TaskIsMarked = 0)
group by
MSP_EpmResource_UserView.[СДРес],
MSP_EpmResource_UserView.ResourceName
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
--DashboardKORemainingWorkAll
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
and
(MSP_EpmTask_UserView.TaskPercentCompleted < 100)
group by
MSP_EpmResource_UserView.[СДРес],
MSP_EpmResource_UserView.ResourceName

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
(MSP_EpmAssignmentByDay_UserView.TimeByDay > getdate() - 30)
group by
MSP_EpmAssignmentByDay_UserView.TimeByDay,
MSP_EpmResource_UserView.ResourceName