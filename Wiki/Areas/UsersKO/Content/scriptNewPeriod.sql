insert into [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan]
SELECT 
[PortalKATEK].[dbo].[AspNetUsers].Id,
[PortalKATEK].[dbo].[ProductionCalendar].id
,iif(substring([PortalKATEK].[dbo].[ProductionCalendar].period, 7, 1) = 7, isnull([ProjectWebApp].[dbo].[MSP_EpmResource_UserView].yul / 100, 0), iif(substring([PortalKATEK].[dbo].[ProductionCalendar].period, 7, 1) = 8, isnull([ProjectWebApp].[dbo].[MSP_EpmResource_UserView].aug / 100,0), isnull([ProjectWebApp].[dbo].[MSP_EpmResource_UserView].sen / 100,0)))
  FROM 
  [PortalKATEK].[dbo].[ProductionCalendar] join [PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].Id is not null left join
  [ProjectWebApp].[dbo].[MSP_EpmResource_UserView] on [ProjectWebApp].[dbo].[MSP_EpmResource_UserView].ResourceUID = [PortalKATEK].[dbo].[AspNetUsers].ResourceUID
  where		
  ([PortalKATEK].[dbo].[AspNetUsers].Devision = 3 or [PortalKATEK].[dbo].[AspNetUsers].Devision = 15 or [PortalKATEK].[dbo].[AspNetUsers].Devision = 16)
  and
   [PortalKATEK].[dbo].[AspNetUsers].LockoutEnabled = 1


   SELECT
[PortalKATEK].[dbo].[ProductionCalendar].[period],
[PortalKATEK].[dbo].[AspNetUsers].CiliricalName as ciliricalName,
[PortalKATEK].[dbo].[Devision].[name] as devisionName
      ,[PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k]
	  ,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] as [plan]
	  ,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.10 as [plan10]
	  ,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.20 as [plan20]
	  ,[PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] + [PortalKATEK].[dbo].[ProductionCalendar].timeToOnePerson * [PortalKATEK].[dbo].[DashboardKO_UsersMonthPlan].[k] * 0.30 as [plan30]
	  ,tableResult.НК as [normHoure]
	  into DashboardKOMP3
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
group by
iif(len(month(MSP_EpmAssignmentByDay_UserView.TimeByDay))=1,
concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.0',month(MSP_EpmAssignmentByDay_UserView.TimeByDay)),
concat(year(MSP_EpmAssignmentByDay_UserView.TimeByDay),'.',month(MSP_EpmAssignmentByDay_UserView.TimeByDay))),
substring(MSP_EpmResource_UserView.СДРес, 0, 4), MSP_EpmResource_UserView.ResourceUID) as tableResult on tableResult.[Месяц завершения задачи] = [PortalKATEK].[dbo].[ProductionCalendar].period and tableResult.ResourceUID = [PortalKATEK].[dbo].AspNetUsers.ResourceUID