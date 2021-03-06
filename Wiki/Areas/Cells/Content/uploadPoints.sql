--getPoints
delete [SCells].[dbo].[SectionMap]
insert into [SCells].[dbo].[SectionMap]
SELECT [SCells].[dbo].[Section].idS
,t1.[id2]
,0
  FROM [SCells].[dbo].[Section]  join
  (select [SCells].[dbo].[Section].idS as [id2] from [SCells].[dbo].[Section]) as t1 
  on [SCells].[dbo].[Section].idS != t1.[id2]

--getRand
update [SCells].[dbo].[SectionMap] set 
[distance] = RAND(CHECKSUM(NEWID())) * 5


--input data: <"xxxx";"xxxx";>
--output data: <"0.1";"xxxx";"xxxx";>

delete [SCells].[dbo].[TierMap]
insert into SCells.dbo.TierMap
select
SCells.dbo.Tier.id
,T1.id
,isnull(SCells.dbo.SectionMap.distance, 0)
from SCells.dbo.Tier left join (select * from SCells.dbo.Tier) as T1 on t1.id != SCells.dbo.Tier.id 
left join SCells.dbo.Section on SCells.dbo.Section.idS = SCells.dbo.Tier.idS
left join (select * from SCells.dbo.Section) as T2 on T1.idS = T2.idS
left join SCells.dbo.SectionMap on SCells.dbo.SectionMap.sectionIdStart = SCells.dbo.Section.idS
								and SCells.dbo.SectionMap.sectionIdFinish = T2.idS



USE [SCells]
GO

DECLARE	@return_value int

EXEC	@return_value = [dbo].[UploadDocdata]
		@List = N'2846;2892;3385;2983;',
		@DocNum = N'testDoc04'
SELECT	'Return Value' = @return_value
GO

select * 
from SCells.dbo.DocData