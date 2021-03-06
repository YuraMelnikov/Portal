DECLARE @xml NVARCHAR(MAX)
DECLARE @body NVARCHAR(MAX)

SET @xml = CAST(( 
SELECT
PortalKATEK.dbo.PZ_PlanZakaz.PlanZakaz AS 'td','',
PortalKATEK.dbo.PlanVerificationItems.planDate AS 'td','',
PortalKATEK.dbo.PlanVerificationItems.planDescription AS 'td','',
PortalKATEK.dbo.PlanVerificationItems.factDescription AS 'td',''
from
PortalKATEK.dbo.PlanVerificationItems
left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.id = PortalKATEK.dbo.PlanVerificationItems.id_PZ_PlanZakaz
where PortalKATEK.dbo.PlanVerificationItems.factDate is null
and PortalKATEK.dbo.PlanVerificationItems.planDate <= getdate() - 1

FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))
SET @body ='<html><body><H3>Отчет о передаче изделия на проверку: ' + convert(varchar, GETDATE()) + '</H3>
<table border = 1> 
<tr>
<th> № заказа </th> <th> Плановая дата </th> <th> Прим. ГИ </th><th> Прим. РП </th> </tr>'    
SET @body = @body + @xml +'</table></body></html>'
IF (SELECT count (*) 
from
PortalKATEK.dbo.PlanVerificationItems
left join PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.id = PortalKATEK.dbo.PlanVerificationItems.id_PZ_PlanZakaz
where PortalKATEK.dbo.PlanVerificationItems.factDate is null
and PortalKATEK.dbo.PlanVerificationItems.planDate <= getdate() - 1) > 0
EXEC msdb.dbo.sp_send_dbmail
@profile_name = 'Основной', -- replace with your SQL Database Mail Profile 
@body = @body,
@body_format ='HTML',
@recipients = 'myi@katek.by',
@subject = 'Отчет о передаче изделия на проверку';