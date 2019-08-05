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