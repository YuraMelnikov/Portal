--Reclamation_Type
insert into [PortalKATEK_TEST].[dbo].[Reclamation_Type]
select 
[PortalKATEK_TEST].[dbo].[OTK_TypeReclamation].Name
,[PortalKATEK_TEST].[dbo].[OTK_TypeReclamation].KO
,[PortalKATEK_TEST].[dbo].[OTK_TypeReclamation].KO
from [PortalKATEK_TEST].[dbo].[OTK_TypeReclamation]
update [PortalKATEK_TEST].[dbo].[Reclamation_Type]
set [PortalKATEK_TEST].[dbo].[Reclamation_Type].activeOTK = 1
--Reclamation_CountError
insert into [PortalKATEK_TEST].[dbo].[Reclamation_CountError]
select
[PortalKATEK_TEST].[dbo].[OTK_CounterErrorKO].name
,1
,[PortalKATEK_TEST].[dbo].[OTK_CounterErrorKO].count
from [PortalKATEK_TEST].[dbo].[OTK_CounterErrorKO]
