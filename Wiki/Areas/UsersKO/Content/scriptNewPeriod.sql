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