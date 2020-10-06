--Издержки заказов (тыс. / привязка к тр-там производства)
delete [ReportKATEK].[dbo].[DashboardDH]
insert into [ReportKATEK].[dbo].[DashboardDH]
SELECT [Месяц] as [Month]
,[ReportKATEK].[dbo].[TEOInWorkPO].[year]
,isnull(sum([ХВыручка]), 0) as Rate
,isnull(sum([ХСС]), 0) as [SSM]
,isnull(sum([ХССЗП]), 0) as [SSW]
,isnull(sum([ХИК]), 0) as [IK]
,isnull(sum([ХППК]), 0) as [PK]
,isnull(sum([ХПИ]), 0) as [PI]
,isnull(sum([ХНОП]), 0) as [Profit]
,isnull(sum([RSSM]), 0) as [SSMR]
FROM [ReportKATEK].[dbo].[TEOInWorkPO]
where [year] > year(getdate()) - 3
group by [Месяц]
,[ReportKATEK].[dbo].[TEOInWorkPO].[year]
order by [Месяц]


--Прибыль с накоплением (тыс. / привязка к тр-там производства)
delete [ReportKATEK].[dbo].[DashboardDHCustomer]
insert into [ReportKATEK].[dbo].[DashboardDHCustomer]
SELECT year([PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP) as [year]
,iif(SUBSTRING([PortalKATEK].[dbo].[PZ_Client].NameSort,0,CHARINDEX('-',[PortalKATEK].[dbo].[PZ_Client].NameSort,0)) = '', [PortalKATEK].[dbo].[PZ_Client].NameSort, SUBSTRING([PortalKATEK].[dbo].[PZ_Client].NameSort,0,CHARINDEX('-',[PortalKATEK].[dbo].[PZ_Client].NameSort,0))) as customer
,isnull(sum([PortalKATEK].[dbo].[PZ_TEO].Rate), 0) as Rate
,isnull(sum([PortalKATEK].[dbo].[PZ_TEO].SSM), 0) as  SSM
,isnull(sum([PortalKATEK].[dbo].[PZ_TEO].SSR), 0) as  SSR
,isnull(sum([PortalKATEK].[dbo].[PZ_TEO].IzdKom), 0) as  IK
,isnull(sum([PortalKATEK].[dbo].[PZ_TEO].IzdPPKredit), 0) as  PK
,isnull(sum([PortalKATEK].[dbo].[PZ_TEO].[PI]), 0) as  [PI]
,isnull(sum([PortalKATEK].[dbo].[PZ_TEO].NOP), 0) as  [Profit]
,isnull(sum([exportImport].[dbo].[planZakaz].SSfact), 0) as [FSSM]
FROM [PortalKATEK].[dbo].[PZ_PlanZakaz] 
left join [PortalKATEK].[dbo].[PZ_TEO] on [PortalKATEK].[dbo].[PZ_TEO].Id_PlanZakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].Id
left join [exportImport].[dbo].[planZakaz] on [exportImport].[dbo].[planZakaz].Zakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz
left join [PortalKATEK].[dbo].[PZ_Client] on [PortalKATEK].[dbo].[PZ_Client].id = [PortalKATEK].[dbo].[PZ_PlanZakaz] .Client
where year([PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP) = year(getdate())
group by year([PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP)
,iif(SUBSTRING([PortalKATEK].[dbo].[PZ_Client].NameSort,0,CHARINDEX('-',[PortalKATEK].[dbo].[PZ_Client].NameSort,0)) = '', [PortalKATEK].[dbo].[PZ_Client].NameSort, SUBSTRING([PortalKATEK].[dbo].[PZ_Client].NameSort,0,CHARINDEX('-',[PortalKATEK].[dbo].[PZ_Client].NameSort,0)))


--Средняя взвешенная НОП
delete [ReportKATEK].[dbo].[DashboardDPercent]
insert into [ReportKATEK].[dbo].[DashboardDPercent]
SELECT iif(SUBSTRING([PortalKATEK].[dbo].[PZ_Client].NameSort,0,CHARINDEX('-',[PortalKATEK].[dbo].[PZ_Client].NameSort,0)) = '',
[PortalKATEK].[dbo].[PZ_Client].NameSort, SUBSTRING([PortalKATEK].[dbo].[PZ_Client].NameSort,0,CHARINDEX('-',[PortalKATEK].[dbo].[PZ_Client].NameSort,0))) as customer
,sum([PortalKATEK].[dbo].[PZ_TEO].NOP)/sum([PortalKATEK].[dbo].[PZ_TEO].Rate)*100 as [data]
FROM [PortalKATEK].[dbo].[PZ_PlanZakaz] 
left join [PortalKATEK].[dbo].[PZ_TEO] on [PortalKATEK].[dbo].[PZ_TEO].Id_PlanZakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].Id
left join [exportImport].[dbo].[planZakaz] on [exportImport].[dbo].[planZakaz].Zakaz = [PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz
left join [PortalKATEK].[dbo].[PZ_Client] on [PortalKATEK].[dbo].[PZ_Client].id = [PortalKATEK].[dbo].[PZ_PlanZakaz] .Client
where year([PortalKATEK].[dbo].[PZ_PlanZakaz].DateCreate) = year(getdate())
and [PortalKATEK].[dbo].[PZ_TEO].Rate > 0
group by iif(SUBSTRING([PortalKATEK].[dbo].[PZ_Client].NameSort,0,CHARINDEX('-',[PortalKATEK].[dbo].[PZ_Client].NameSort,0)) = '',
 [PortalKATEK].[dbo].[PZ_Client].NameSort,
 SUBSTRING([PortalKATEK].[dbo].[PZ_Client].NameSort,0,CHARINDEX('-',[PortalKATEK].[dbo].[PZ_Client].NameSort,0)))


 delete [ReportKATEK].[dbo].[DashboardDN]
insert into [ReportKATEK].[dbo].[DashboardDN]
SELECT [ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[Месяц] as [Month]
,[ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[НОП] as [Profit]
,[ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[Выручка] as [Rate]
,[ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[НОП] / [ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[Выручка] * 100 as [SN]
,sum([НОП]) over (order by [ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[Месяц]) / sum([Выручка]) over (order by [ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[Месяц]) * 100 as [SVN]
,iif([ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[Месяц] = iif(len(month(getdate())) = 1, concat(year(getdate()), '.0', month(getdate())), concat(year(getdate()), '.', month(getdate()))),1,0) + iif(SUBSTRING([ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[Месяц],6,2)=12,1,0) as [Filter]
FROM [ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears]
where [ReportKATEK].[dbo].[NOPOpenOrderLastTheerYears].[год] > year(getdate()) - 2