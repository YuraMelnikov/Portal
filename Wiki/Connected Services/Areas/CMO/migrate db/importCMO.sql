insert into [PortalKATEK_TEST].[dbo].[CMO2_Order]
SELECT 
[PortalKATEK_TEST].[dbo].[CMO_PreOrder].userCreate,
[PortalKATEK_TEST].[dbo].[CMO_PreOrder].dateCreate
,0
,null
,0
,0
,0
,null
,0
,0
,0
,null
,0
,0
,[PortalKATEK_TEST].[dbo].[CMO_PreOrder].folder
,0
,null
,[PortalKATEK_TEST].[dbo].[CMO_PreOrder].id
,null
  FROM [PortalKATEK_TEST].[dbo].[CMO_PreOrder]
  where [firstTenderStart] = 0
insert into [PortalKATEK_TEST].[dbo].[CMO2_Position]
select
    [PortalKATEK_TEST].[dbo].CMO2_Order.id
    ,[PortalKATEK_TEST].[dbo].CMO_PositionPreOrder.id_PZ_PlanZakaz
    ,[PortalKATEK_TEST].[dbo].CMO_PositionPreOrder.id_CMO_TypeProduct
FROM [PortalKATEK_TEST].[dbo].CMO2_Order left join
[PortalKATEK_TEST].[dbo].CMO_PositionPreOrder on [PortalKATEK_TEST].[dbo].CMO2_Order.idtPO = [PortalKATEK_TEST].[dbo].CMO_PositionPreOrder.id_CMO_PreOrder
insert into [PortalKATEK_TEST].[dbo].[CMO2_Order]
SELECT 
[PortalKATEK_TEST].[dbo].[CMO_Order].userCreate, --id_AspNetUsers_Create
[PortalKATEK_TEST].[dbo].[CMO_Order].dateCreate --dateTimeCreate
,1 --workIn
,[Expr1]--workDateTime
,1--workComplitet
,iif([Expr3] is null, 0, [Expr3])--workCost
,iif([Expr2] is not null, 1, 0)--manufIn
,[Expr2]--manufDate
,iif([Expr2] is not null, 1, 0)--manufComplited
,iif([Expr3] is null, 0, [Expr3])--manufCost
,0--finIn
,null--finDate
,0--finComplited
,0--finCost
,[PortalKATEK_TEST].[dbo].[CMO_Order].folder--folder
,0--reOrder
,[PortalKATEK_TEST].[dbo].[CMO_Order].companyWin--id_CMO_Company
,null--idtPO
,[PortalKATEK_TEST].[dbo].[CMO_Order].id--idtO
  FROM [PortalKATEK_TEST].[dbo].[CMO_Order] left join 
  [PortalKATEK_TEST].[dbo].[CMO_TMP] on [PortalKATEK_TEST].[dbo].[CMO_TMP].Id_Order = [PortalKATEK_TEST].[dbo].[CMO_Order].id
  where
  [dateCloseOrder] is null
  and [datetimeFirstTenderFinish] is not null

insert into [PortalKATEK_TEST].[dbo].[CMO2_Position]
select
    [PortalKATEK_TEST].[dbo].CMO2_Order.id
    ,[PortalKATEK_TEST].[dbo].CMO_PositionOrder.id_PZ_PlanZakaz
    ,[PortalKATEK_TEST].[dbo].CMO_PositionOrder.id_CMO_TypeProduct
FROM [PortalKATEK_TEST].[dbo].CMO2_Order left join
[PortalKATEK_TEST].[dbo].CMO_PositionOrder on [PortalKATEK_TEST].[dbo].CMO2_Order.idtO = [PortalKATEK_TEST].[dbo].CMO_PositionOrder.id_CMO_Order
where [PortalKATEK_TEST].[dbo].CMO2_Order.idtO is not null