--ServiceRemarksActions
--ServiceRemarksCauses
--ServiceRemarksPlanZakazs
--ServiceRemarksTypes

 --SELECT
 --   ud.tp_ID
 --   , ud.tp_ListId
 --   , ud.tp_Author
	--,ud.tp_ColumnSet
 --   , ud.tp_ColumnSet.value('(text())[1]', 'nvarchar(32)') as Title
 --   FROM dbo.UserData ud
 --   WHERE (ud.tp_ListId =  'B443637F-31A6-4CFF-9C0C-8C801CD812CE

  --insert into [PortalKATEK].[dbo].[ServiceRemarks]
  --SELECT 
  --[text],
  --[description],
  --[dateTimeCreate],
  --[userCreate],
  --[datePutToService],
  --[dateClose],
  --'',
  --[timeId]
  --FROM [PortalKATEK_TEST].[dbo].[imptRec]

  --update [PortalKATEK].[dbo].[ServiceRemarks]
  --set [PortalKATEK].[dbo].[ServiceRemarks].dateClose = null
  --where
  --year([PortalKATEK].[dbo].[ServiceRemarks].dateClose) < 2000

--  insert into [PortalKATEK].[dbo].[ServiceRemarksActions]
--SELECT 
--[PortalKATEK].[dbo].[ServiceRemarks].id,
--rEPLACE([actionText], '?', '')
--,[create]
--,[user]
--  FROM [PortalKATEK_test].[dbo].[importData] left join 
--  [PortalKATEK].[dbo].[ServiceRemarks] on [PortalKATEK].[dbo].[ServiceRemarks].timeId = [idRec]
--  where [PortalKATEK].[dbo].[ServiceRemarks].id is not null
--  order by
--  [create]

--insert into  [PortalKATEK].[dbo].[ServiceRemarksCauses]
--SELECT [PortalKATEK_TEST].[dbo].[importData].[dataCau],[PortalKATEK_TEST].[dbo].[ServiceRemarks].id
--  FROM [PortalKATEK_TEST].[dbo].[importData] left join [PortalKATEK_TEST].[dbo].[ServiceRemarks] on 
--  [PortalKATEK_TEST].[dbo].[ServiceRemarks].timeId = [PortalKATEK_TEST].[dbo].[importData].[timeId]
--  where [PortalKATEK_TEST].[dbo].[ServiceRemarks].id is not null

--insert into [PortalKATEK].[dbo].[ServiceRemarksPlanZakazs]
--SELECT [PortalKATEK_TEST].[dbo].[PZ_PlanZakaz].Id
--      ,[PortalKATEK_TEST].[dbo].[ServiceRemarks].id
--  FROM [PortalKATEK_TEST].[dbo].[importData] left join
--  [PortalKATEK_TEST].[dbo].[ServiceRemarks] on [PortalKATEK_TEST].[dbo].[ServiceRemarks].timeId = [PortalKATEK_TEST].[dbo].[importData].[id] left join
--  [PortalKATEK_TEST].[dbo].[PZ_PlanZakaz] on [PortalKATEK_TEST].[dbo].[PZ_PlanZakaz].PlanZakaz = [PortalKATEK_TEST].[dbo].[importData].[txt]
--  where
--  [PortalKATEK_TEST].[dbo].[PZ_PlanZakaz].Id is not null and [PortalKATEK_TEST].[dbo].[ServiceRemarks].id is not null

--insert into [PortalKATEK].[dbo].[ServiceRemarksTypes]
--SELECT [PortalKATEK_TEST].[dbo].[importData].txt
--      ,[PortalKATEK_TEST].[dbo].[ServiceRemarks].id
--  FROM [PortalKATEK_TEST].[dbo].[importData] left join 
--  [PortalKATEK_TEST].[dbo].[ServiceRemarks] on [PortalKATEK_TEST].[dbo].[ServiceRemarks].timeId = [PortalKATEK_TEST].[dbo].[importData].[id]
--  where [PortalKATEK_TEST].[dbo].[importData].txt is not null and [PortalKATEK_TEST].[dbo].[ServiceRemarks].id is not null