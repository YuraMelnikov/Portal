delete [PortalKATEK].[dbo].[Debit_CostUpdate]
delete [PortalKATEK].[dbo].[Debit_WorkBit] where [id_TaskForPZ] = 15
insert into [PortalKATEK].[dbo].[Debit_WorkBit]
SELECT 
15,
getdate(),
getdate(),
null,
0,
[PortalKATEK].[dbo].[PZ_PlanZakaz].Id,
getdate()
  FROM [PortalKATEK].[dbo].[д] left join [PortalKATEK].[dbo].[PZ_PlanZakaz]
  on [PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz = [Заказ]
  where
  [ДЗ] not like ''
  
insert into [PortalKATEK].[dbo].[Debit_WorkBit]
SELECT 
15,
getdate(),
[PortalKATEK].[dbo].[д].[Дата оплаты],
[PortalKATEK].[dbo].[д].[Дата оплаты],
1,
[PortalKATEK].[dbo].[PZ_PlanZakaz].Id,
getdate()
  FROM [PortalKATEK].[dbo].[д] left join [PortalKATEK].[dbo].[PZ_PlanZakaz]
  on [PortalKATEK].[dbo].[PZ_PlanZakaz].PlanZakaz = [Заказ]
  where
  [ДЗ] like ''
  and 
  [PortalKATEK].[dbo].[PZ_PlanZakaz].Id is not null
  update [PortalKATEK].[dbo].[Debit_WorkBit] set [PortalKATEK].[dbo].[Debit_WorkBit].datePlan = 
	  iif([PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP > getdate(), 
			DATEADD(day, 14 + [PortalKATEK].[dbo].[PZ_Setup].UslovieOplatyInt, [PortalKATEK].[dbo].[PZ_PlanZakaz].dataOtgruzkiBP), 
			iif(DATEADD(day, [PortalKATEK].[dbo].[PZ_Setup].UslovieOplatyInt, TableOp.dateClose) is not null, DATEADD(day, [PortalKATEK].[dbo].[PZ_Setup].UslovieOplatyInt, TableOp.dateClose),
			DATEADD(day, [PortalKATEK].[dbo].[PZ_Setup].UslovieOplatyInt, getdate())))
  FROM [PortalKATEK].[dbo].[Debit_WorkBit] left join 
  (select * from [PortalKATEK].[dbo].[Debit_WorkBit]  where [id_TaskForPZ] = 28) as TableOp on TableOp.id_PlanZakaz = [PortalKATEK].[dbo].[Debit_WorkBit].[id_PlanZakaz]
  left join [PortalKATEK].[dbo].[PZ_Setup] on [PortalKATEK].[dbo].[PZ_Setup].id_PZ_PlanZakaz = [PortalKATEK].[dbo].[Debit_WorkBit].[id_PlanZakaz]
  left join [PortalKATEK].[dbo].[PZ_PlanZakaz] on [PortalKATEK].[dbo].[PZ_PlanZakaz].Id = [PortalKATEK].[dbo].[Debit_WorkBit].[id_PlanZakaz]
  where
  [PortalKATEK].[dbo].[Debit_WorkBit].[id_TaskForPZ] = 15
  and [PortalKATEK].[dbo].[Debit_WorkBit].[close] = 0
insert into [PortalKATEK].[dbo].[Debit_CostUpdate]
select 
[PortalKATEK].[dbo].[PZ_TEO].Id_PlanZakaz
,[PortalKATEK].[dbo].[PZ_TEO].OtpuskChena + [PortalKATEK].[dbo].[PZ_TEO].NDS
,[PortalKATEK].[dbo].[Debit_WorkBit].dateClose
,[PortalKATEK].[dbo].[Debit_WorkBit].dateClose
 from
[PortalKATEK].[dbo].[Debit_WorkBit]
left join [PortalKATEK].[dbo].[PZ_TEO] on [PortalKATEK].[dbo].[PZ_TEO].Id_PlanZakaz = [PortalKATEK].[dbo].[Debit_WorkBit].id_PlanZakaz
 where [PortalKATEK].[dbo].[Debit_WorkBit].id_TaskForPZ = 15 and [PortalKATEK].[dbo].[Debit_WorkBit].[close] = 1
 and ([PortalKATEK].[dbo].[PZ_TEO].OtpuskChena + [PortalKATEK].[dbo].[PZ_TEO].NDS) is not null