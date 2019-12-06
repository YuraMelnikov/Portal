DECLARE	@planHSSToYear int;
DECLARE @ratePlanToYear int;


SET @planHSSToYear = 10000000;
SET @ratePlanToYear = 16500000;


DELETE [PortalKATEK].[dbo].[DashboardBP_ProjectTasks]
DELETE PortalKATEK.dbo.DashboardBP_ProjectList
INSERT INTO PortalKATEK.dbo.DashboardBP_ProjectList
SELECT
[PortalKATEK].dbo.PZ_PlanZakaz.id,
PortalKATEK.dbo.DashboardBP_State.id,
PortalKATEK.dbo.PZ_PlanZakaz.DateShipping,
PortalKATEK.dbo.PZ_PlanZakaz.dataOtgruzkiBP
,isnull(PortalKATEK.dbo.ProjectMSP_EpmProject_UserView.ProjectStartDate, getdate())
,isnull(PortalKATEK.dbo.ProjectMSP_EpmProject_UserView.ProjectDuration, 0)
,isnull(PortalKATEK.dbo.ProjectMSP_EpmProject_UserView.ProjectPercentCompleted, 0)
FROM
[PortalKATEK].[dbo].[PZ_PlanZakaz] 
left join [PortalKATEK].[dbo].[DashboardBP_State] on [PortalKATEK].[dbo].[DashboardBP_State].id > 0
left join [PortalKATEK].dbo.ProjectMSP_EpmProject_UserView on PortalKATEK.dbo.ProjectMSP_EpmProject_UserView.ProjectUID = PortalKATEK.dbo.PZ_PlanZakaz.ProjectUID
WHERE
[PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP > GETDATE()
and [PortalKATEK].dbo.DashboardBP_State.active = 1


DELETE [PortalKATEK].[dbo].[DashboardBP_HSSPO]
INSERT INTO [PortalKATEK].[dbo].[DashboardBP_HSSPO]
SELECT     
[PortalKATEK].[dbo].[DashboardBP_State].id,
PortalKatek.dbo.PZ_PlanZakaz.Id AS OrderPZ, 
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID, 
ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskName, 
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay, 
ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 
PortalKatek.dbo.PZ_TEO.Rate AS RatePlan, 
PortalKatek.dbo.PZ_TEO.SSM AS SSMPlan, 
iif(exportImport.dbo.planZakaz.SSfact is null, 0, exportImport.dbo.planZakaz.SSfact) AS SSMFact, 
PortalKatek.dbo.PZ_TEO.SSR AS SSRPlan, 
PortalKatek.dbo.PZ_TEO.IzdKom AS ik, 
PortalKatek.dbo.PZ_TEO.IzdPPKredit AS ppk, 
PortalKatek.dbo.PZ_TEO.PI, 
PortalKatek.dbo.PZ_TEO.NOP, 
PortalKatek.dbo.PZ_TEO.Rate / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS RateHoure,
PortalKatek.dbo.PZ_TEO.SSM / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ssmHoure, 
iif(exportImport.dbo.planZakaz.SSfact is null, 0, exportImport.dbo.planZakaz.SSfact) / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ssmFactHoure,
PortalKatek.dbo.PZ_TEO.SSR / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ssrHoure, 
PortalKatek.dbo.PZ_TEO.IzdKom / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ikHoure, 
PortalKatek.dbo.PZ_TEO.IzdPPKredit / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS ppkHoure, 
PortalKatek.dbo.PZ_TEO.PI / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS piHoure, 
PortalKatek.dbo.PZ_TEO.NOP / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] AS nopHoure, 
PortalKatek.dbo.PZ_TEO.Rate / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xRate
,iif(convert(int, isnull(TableWorkKBM.[close], 1)) + convert(int, isnull(TableWorkKBE.[close], 1)) = 2, PortalKatek.dbo.PZ_TEO.SSM / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 0) AS xSSM, 
iif(exportImport.dbo.planZakaz.SSfact is null, 0, exportImport.dbo.planZakaz.SSfact) / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xSSMFact, 
PortalKatek.dbo.PZ_TEO.SSR / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xSSR, 
PortalKatek.dbo.PZ_TEO.IzdKom / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xIK, 
PortalKatek.dbo.PZ_TEO.IzdPPKredit / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xPPK, 
PortalKatek.dbo.PZ_TEO.PI / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xPI, 
PortalKatek.dbo.PZ_TEO.NOP / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork AS xNOP
,iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay)) = 1, concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.0', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay)), concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', 
month(MSP_EpmAssignmentByDay_UserView.TimeByDay))) as [month], 
year(MSP_EpmAssignmentByDay_UserView.TimeByDay) as [year],
concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay), '.', 
(month(MSP_EpmAssignmentByDay_UserView.TimeByDay) + 2) / 3) AS [quartal], 
iiF(len(datepart(ISO_WEEK, ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay))=1,
concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',datepart(ISO_WEEK, ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay)),
concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',datepart(ISO_WEEK, ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.TimeByDay))) as [week]
,iif(convert(int, isnull(TableWorkKBM.[close], 1)) + convert(int, isnull(TableWorkKBE.[close], 1)) != 2, PortalKatek.dbo.PZ_TEO.SSM / ReportKATEK.dbo.ProjectWorkPO.AssignmentWork * ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] * ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork, 0) as [xHSSNoplaningwork]
FROM ProjectWebApp.dbo.MSP_EpmProject_UserView INNER JOIN
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
PortalKatek.dbo.PZ_PlanZakaz ON CONVERT(varchar, PortalKatek.dbo.PZ_PlanZakaz.PlanZakaz) = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] LEFT OUTER JOIN
PortalKatek.dbo.PZ_TEO ON PortalKatek.dbo.PZ_TEO.Id_PlanZakaz = PortalKatek.dbo.PZ_PlanZakaz.Id LEFT OUTER JOIN
exportImport.dbo.planZakaz ON exportImport.dbo.planZakaz.Zakaz = PortalKatek.dbo.PZ_PlanZakaz.PlanZakaz LEFT OUTER JOIN
ReportKATEK.dbo.ProjectWorkPO ON ReportKATEK.dbo.ProjectWorkPO.[№ заказа] = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] left join
[PortalKATEK].[dbo].[DashboardBP_State] on [PortalKATEK].[dbo].[DashboardBP_State].id > 0
left join (select * from PortalKATEK.dbo.Debit_WorkBit where PortalKATEK.dbo.Debit_WorkBit.id_TaskForPZ = 45) as TableWorkKBM on TableWorkKBM.id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join (select * from PortalKATEK.dbo.Debit_WorkBit where PortalKATEK.dbo.Debit_WorkBit.id_TaskForPZ = 46) as TableWorkKBE on TableWorkKBE.id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
WHERE [PortalKATEK].[dbo].[DashboardBP_State].active = 1
AND (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] NOT LIKE '%НИОКР%') 
AND (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] NOT LIKE '%Задан%') 
AND (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] NOT LIKE '%Проч%') 
AND (ProjectWebApp.dbo.MSP_EpmProject_UserView.[Кол-во ПО] > 0) 
AND (ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%УС%' 
		OR ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%УИ%' 
		OR ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%ЭУ%') 
AND (ProjectWebApp.dbo.MSP_EpmAssignmentByDay_UserView.AssignmentWork > 0)


DELETE [PortalKATEK].[dbo].[DashboardBP_ProjectTasks]
INSERT INTO [PortalKATEK].[dbo].[DashboardBP_ProjectTasks]
SELECT
PortalKATEK.dbo.PWA_TasksForBP.TaskBaseline0Duration,
PortalKATEK.dbo.PWA_TasksForBP.TaskBaseline0StartDate
,PortalKATEK.dbo.PWA_TasksForBP.TaskBaseline0FinishDate
,PortalKATEK.dbo.PWA_TasksForBP.AssignmentBaseline0Work
,isnull(PortalKATEK.dbo.PWA_TasksForBP.TaskDuration, 0)
,isnull(PortalKATEK.dbo.PWA_TasksForBP.TaskRemainingDuration, 0)
,isnull(PortalKATEK.dbo.PWA_TasksForBP.TaskActualDuration, 0)
,PortalKATEK.dbo.PWA_TasksForBP.TaskStartDate
,PortalKATEK.dbo.PWA_TasksForBP.TaskFinishDate
,PortalKATEK.dbo.PWA_TasksForBP.TaskWork
,PortalKATEK.dbo.PWA_TasksForBP.TaskRemainingWork
,PortalKATEK.dbo.PWA_TasksForBP.TaskActualWork
,PortalKATEK.dbo.PWA_TasksForBP.TaskPercentWorkCompleted
,PortalKATEK.dbo.PWA_TasksForBP.TaskPercentCompleted
,PortalKATEK.dbo.PWA_TasksForBP.TaskPhysicalPercentCompleted
,PortalKATEK.dbo.PWA_TasksForBP.TaskDeadline
,PortalKATEK.dbo.PWA_TasksForBP.TaskIsCritical
,PortalKATEK.dbo.PWA_TasksForBP.TaskPriority
,PortalKATEK.dbo.PWA_TasksForBP.НК
,PortalKATEK.dbo.WBS.id as i1
,PortalKATEK.dbo.DashboardBP_ProjectList.id
,PortalKATEK.dbo.PWA_TasksForBP.TaskUID
,PortalKATEK.dbo.AspNetUsers.Id
from PortalKATEK.dbo.DashboardBP_ProjectList left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.Id = PortalKATEK.dbo.DashboardBP_ProjectList.id_PZ_PlanZakaz
left join PortalKATEK.dbo.PWA_TasksForBP on PortalKATEK.dbo.PWA_TasksForBP.ProjectUID = PortalKATEK.dbo.PZ_PlanZakaz.ProjectUID
left join PortalKATEK.dbo.WBS on PortalKATEK.dbo.wbs.WBSName = PortalKATEK.dbo.PWA_TasksForBP.TaskWBS
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.ResourceUID = PortalKATEK.dbo.PWA_TasksForBP.ResourceUID
where
(PortalKATEK.dbo.PWA_TasksForBP.TaskDuration is not null)
delete [PortalKATEK].[dbo].[DashboardBP_ProjectTasks] where [PortalKATEK].[dbo].[DashboardBP_ProjectTasks].id_WBS is null


DELETE PortalKATEK.dbo.DashboardBP_HSSPOSmall
insert into PortalKATEK.dbo.DashboardBP_HSSPOSmall
SELECT 
PortalKATEK.dbo.DashboardBP_State.id
,PortalKATEK.dbo.DashboardBP_HSSPO.year
,month(PortalKATEK.dbo.DashboardBP_HSSPO.timeByDay)
,convert(int,sum(PortalKATEK.dbo.DashboardBP_HSSPO.xSsm))
FROM [PortalKATEK].[dbo].[DashboardBP_State]
left join PortalKATEK.dbo.DashboardBP_HSSPO on PortalKATEK.dbo.DashboardBP_HSSPO.id_DashboardBP_State = PortalKATEK.dbo.DashboardBP_State.id
where active = 1
group by
PortalKATEK.dbo.DashboardBP_State.id
,PortalKATEK.dbo.DashboardBP_HSSPO.year
,month(PortalKATEK.dbo.DashboardBP_HSSPO.timeByDay)


DELETE PortalKATEK.dbo.DashboardHSSPlan
INSERT INTO PortalKATEK.dbo.DashboardHSSPlan
select  sum(PortalKATEK.dbo.DashboardBP_HSSPOSmall.[data]), @planHSSToYear
from PortalKATEK.dbo.DashboardBP_HSSPOSmall left join PortalKATEK.dbo.DashboardBP_State on PortalKATEK.dbo.DashboardBP_State.id = PortalKATEK.dbo.DashboardBP_HSSPOSmall.id_DashboardBP_State
where PortalKATEK.dbo.DashboardBP_HSSPOSmall.year = year(GETDATE()) and PortalKATEK.dbo.DashboardBP_State.id is not null


delete PortalKATEK.dbo.DashboardRemaining
insert into PortalKATEK.dbo.DashboardRemaining
SELECT sum([xSsm])
FROM [PortalKATEK].[dbo].[DashboardBP_HSSPO]
where PortalKATEK.dbo.DashboardBP_HSSPO.timeByDay >= getdate()


insert into PortalKATEK.dbo.DashboardBPComments
select
[ProjectWebApp].[dbo].[MSP_EpmTask_UserView].TaskUID
,[ProjectWebApp].[dbo].[MSP_EpmProject_UserView].[№ заказа]
,[ProjectWebApp].[dbo].[MSP_EpmTask_UserView].TaskName
,[ProjectWebApp].[dbo].[MSP_EpmResource_UserView].ResourceName
,0
,0
,[ReportKATEK].[dbo].RTF2Text(convert(varchar(max),convert(varbinary(max),[TASK_RTF_NOTES]))) AS [Прим.]
FROM [ProjectWebApp].[dbo].[MSP_EpmProject_UserView]
INNER JOIN [ProjectWebApp].[dbo].[MSP_EpmTask_UserView] ON [ProjectWebApp].[dbo].[MSP_EpmProject_UserView].[ProjectUID] = [ProjectWebApp].[dbo].[MSP_EpmTask_UserView].[ProjectUID]
LEFT OUTER JOIN [ProjectWebApp].[dbo].[MSP_EpmAssignment_UserView] ON [ProjectWebApp].[dbo].[MSP_EpmTask_UserView].[TaskUID] = [ProjectWebApp].[dbo].[MSP_EpmAssignment_UserView].[TaskUID]
AND [ProjectWebApp].[dbo].[MSP_EpmTask_UserView].[ProjectUID] = [ProjectWebApp].[dbo].[MSP_EpmAssignment_UserView].[ProjectUID]
LEFT OUTER JOIN [ProjectWebApp].[dbo].[MSP_EpmResource_UserView] ON MSP_EpmAssignment_UserView.ResourceUID = MSP_EpmResource_UserView.ResourceUID
left join ProjectWebApp.pub.[MSP_TASKS] on ProjectWebApp.pub.[MSP_TASKS].TASK_UID = [ProjectWebApp].[dbo].[MSP_EpmTask_UserView].[TaskUID]
left join PortalKATEK.dbo.DashboardBPComments on PortalKATEK.dbo.DashboardBPComments.taskUID = [ProjectWebApp].[dbo].[MSP_EpmTask_UserView].TaskUID
where
[ProjectWebApp].[dbo].[MSP_EpmProject_UserView].ProjectPercentCompleted < 100 
and [ProjectWebApp].[dbo].[MSP_EpmTask_UserView].[TaskPercentCompleted] < 100
and [ProjectWebApp].[dbo].[MSP_EpmProject_UserView].[№ заказа] not like '%Задание%'
and [ProjectWebApp].[dbo].[MSP_EpmProject_UserView].[№ заказа] not like '%Прочие%'
and [ProjectWebApp].[dbo].[MSP_EpmProject_UserView].[№ заказа] not like '%НИОКР%'
and (ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%УСР%'
OR ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%УИШ%'
OR ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%УСМК%'
OR ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%УСС%'
OR ProjectWebApp.dbo.MSP_EpmResource_UserView.СДРес LIKE '%ЭУ%')
and ([ReportKATEK].[dbo].RTF2Text(convert(varchar(max),convert(varbinary(max),[TASK_RTF_NOTES]))) is not null)
and (PortalKATEK.dbo.DashboardBPComments.taskUID is null)


delete PortalKATEK.dbo.DashboardBPComments
from ProjectWebApp.dbo.MSP_EpmTask  join PortalKATEK.dbo.DashboardBPComments on PortalKATEK.dbo.DashboardBPComments.taskUID = ProjectWebApp.dbo.MSP_EpmTask.TaskUID
where ProjectWebApp.dbo.MSP_EpmTask.TaskPercentCompleted = 100


update PortalKATEK.dbo.DashboardBPComments set
counterState1 = counterState2
where counterState2 != 0


update PortalKATEK.dbo.DashboardBPComments set
counterState2 = LEN(PortalKATEK.dbo.DashboardBPComments.notes)


delete PortalKATEK.dbo.DashboardBPManpowerManuf
insert into PortalKATEK.dbo.DashboardBPManpowerManuf
select
PortalKATEK.dbo.DashboardBPDevisionCoef.id as id_DashboardBPDevisionCoef
,TablePlanPowerResource.manpower as manpowerPrj
,TableAssigments.SumAssignmentWork as planWork
,PortalKATEK.dbo.ProductionCalendar.timeToOnePerson / 8 as workday
,TableAssigments.SumAssignmentWork * PortalKATEK.dbo.DashboardBPDevisionCoef.coefDefaultWork / PortalKATEK.dbo.ProductionCalendar.timeToOnePerson / 8 as workMode
,0 as manpowerResult
,0 as workModeResult
,PortalKATEK.dbo.ProductionCalendar.id as id_ProductionCalendar
from PortalKATEK.dbo.DashboardBPDevisionCoef 
left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.DashboardBPDevisionCoef.id_Devision
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.Devision = PortalKATEK.dbo.Devision.id
left join ProjectWebApp.dbo.MSP_EpmResource on ProjectWebApp.dbo.MSP_EpmResource.ResourceUID = PortalKATEK.dbo.AspNetUsers.ResourceUID
left join (SELECT ROW_NUMBER() OVER(PARTITION BY [ProjectWebApp].[dbo].[MSP_EpmResourceByDay_UserView].ResourceUID ORDER BY [ProjectWebApp].[dbo].[MSP_EpmResourceByDay_UserView].TimeByDay ASC) AS RowNumber,
			[ProjectWebApp].[dbo].[MSP_EpmResourceByDay_UserView].TimeByDay,
			[ProjectWebApp].[dbo].[MSP_EpmResourceByDay_UserView].ResourceUID,
			[ProjectWebApp].[dbo].[MSP_EpmResourceByDay_UserView].[BaseCapacity] / 8 as manpower
			FROM [ProjectWebApp].[dbo].[MSP_EpmResourceByDay_UserView]
			where [ProjectWebApp].[dbo].[MSP_EpmResourceByDay_UserView].TimeByDay > getdate()
			and ([ProjectWebApp].[dbo].[MSP_EpmResourceByDay_UserView].BaseCapacity > 0)) as TablePlanPowerResource on TablePlanPowerResource.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource.ResourceUID and TablePlanPowerResource.RowNumber = 1
left join PortalKATEK.dbo.ProductionCalendar on PortalKATEK.dbo.ProductionCalendar.id > 0
left join (select sum(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.AssignmentWork) as [SumAssignmentWork]
,[ProjectWebApp].dbo.MSP_EpmAssignment_UserView.ResourceUID
,concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.TimeByDay), '.', iif(month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.TimeByDay) < 10, '0', ''), month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.TimeByDay)) as [period]
			from ProjectWebApp.dbo.MSP_EpmAssignmentByDay
			left join [ProjectWebApp].dbo.MSP_EpmAssignment_UserView on [ProjectWebApp].dbo.MSP_EpmAssignment_UserView.AssignmentUID = ProjectWebApp.dbo.MSP_EpmAssignmentByDay.AssignmentUID
			where concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.TimeByDay), '.', iif(month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.TimeByDay) < 10, '0', ''), month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.TimeByDay))
			>= concat(year(getdate()), '.', iif(month(getdate()) < 10, '0', ''), month(getdate()))
			and (ProjectWebApp.dbo.MSP_EpmAssignmentByDay.AssignmentWork > 0)
			group by
			[ProjectWebApp].dbo.MSP_EpmAssignment_UserView.ResourceUID
			,concat(year(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.TimeByDay), '.', iif(month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.TimeByDay) < 10, '0', ''), month(ProjectWebApp.dbo.MSP_EpmAssignmentByDay.TimeByDay))) as TableAssigments on TableAssigments.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource.ResourceUID and TableAssigments.[period] = PortalKATEK.dbo.ProductionCalendar.[period]
where PortalKATEK.dbo.AspNetUsers.LockoutEnabled = 1
and (PortalKATEK.dbo.AspNetUsers.Email != 'dms@katek.by')
and (concat(year(getdate()), '.', iif(month(getdate()) < 10, '0', ''), month(getdate())) <= PortalKATEK.dbo.ProductionCalendar.[period])


delete PortalKATEK.dbo.DashboardBPTaskInsert 
insert into PortalKATEK.dbo.DashboardBPTaskInsert 
select
PortalKATEK.dbo.PZ_PlanZakaz.Id as [id_PZ_PlanZakaz]
,exportImport.dbo.planZakaz.OboznacIzdelia as [productName]
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskIsSummary
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskOutlineLevel
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskOutlineNumber
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWBS
,Substring(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWBS, 0, 3) as [TaskWBS1]
,iif(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskOutlineLevel > 1, Substring(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWBS, 4, 4), '') as [TaskWBS2]
,iif(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskOutlineLevel > 2, Substring(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWBS, 9, 4), '') as [TaskWBS3]
,iif(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskOutlineLevel > 3, Substring(ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWBS, 14, 4), '') as [TaskWBS4]
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskName
,PortalKATEK.dbo.AspNetUsers.Id as id_AspNetUsers
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskStartDate
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskBaseline0StartDate
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskfinishDate
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskBaseline0FinishDate
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskClientUniqueId
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskPriority
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskPercentWorkCompleted
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskPercentCompleted
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWork
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskRemainingWork
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskBaseline0Work
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskDuration
,ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskIndex
from ProjectWebApp.dbo.MSP_EpmProject_UserView
left join ProjectWebApp.dbo.MSP_EpmTask_UserView on ProjectWebApp.dbo.MSP_EpmTask_UserView.ProjectUID = ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID
left join ProjectWebApp.dbo.MSP_EpmAssignment_UserView on ProjectWebApp.dbo.MSP_EpmAssignment_UserView.TaskUID = ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskUID
left join ProjectWebApp.dbo.MSP_EpmResource_UserView on ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID = ProjectWebApp.dbo.MSP_EpmAssignment_UserView.ResourceUID
left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.ProjectUID = ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectUID
left join [exportImport].[dbo].[planZakaz] on exportImport.dbo.planZakaz.Zakaz = ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа]
left join PortalKATEK.dbo.PZ_Client on PortalKATEK.dbo.PZ_Client.id = PortalKATEK.dbo.PZ_PlanZakaz.Client
left join PortalKATEK.dbo.AspNetUsers on PortalKATEK.dbo.AspNetUsers.ResourceUID = ProjectWebApp.dbo.MSP_EpmResource_UserView.ResourceUID
left join PortalKATEK.dbo.Devision on PortalKATEK.dbo.Devision.id = PortalKATEK.dbo.AspNetUsers.Devision
left join (select * from PortalKATEK.dbo.Debit_WorkBit where PortalKATEK.dbo.Debit_WorkBit.id_TaskForPZ = 45) as TableWorkKBM on TableWorkKBM.id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
left join (select * from PortalKATEK.dbo.Debit_WorkBit where PortalKATEK.dbo.Debit_WorkBit.id_TaskForPZ = 46) as TableWorkKBE on TableWorkKBE.id_PlanZakaz = PortalKATEK.dbo.PZ_PlanZakaz.Id
where ProjectWebApp.dbo.MSP_EpmProject_UserView.ProjectPercentCompleted < 100
and (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] is not null)
and (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%Задани%')
and (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%НИОКР%')
and (ProjectWebApp.dbo.MSP_EpmProject_UserView.[№ заказа] not like '%Прочи%')
and (PortalKATEK.dbo.PZ_PlanZakaz.id is not null)
and (PortalKATEK.dbo.PZ_Client.id != 39)
and (ProjectWebApp.dbo.MSP_EpmTask_UserView.TaskWBS not like '')
and (TableWorkKBE.[close] = 1)
and (TableWorkKBM.[close] = 1)