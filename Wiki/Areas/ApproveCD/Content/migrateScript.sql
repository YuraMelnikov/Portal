delete PortalKATEK.dbo.ApproveCDTasks
delete PortalKATEK.dbo.ApproveCDActions
delete PortalKATEK.dbo.ApproveCDVersions
delete PortalKATEK.dbo.ApproveCDQuestionCorr
delete PortalKATEK.dbo.ApproveCDQuestions
delete PortalKATEK.dbo.ApproveCDOrders


insert into PortalKATEK.dbo.ApproveCDOrders
SELECT [PortalKATEK].[dbo].[RKD_Order].[id_PZ_PlanZakaz]
,PortalKATEK.dbo.RKD_GIP.id_UserKBM
,PortalKATEK.dbo.RKD_GIP.id_UserKBE
FROM [PortalKATEK].[dbo].[RKD_Order] left join PortalKATEK.dbo.RKD_GIP on PortalKATEK.dbo.RKD_GIP.id_RKD_Order = PortalKATEK.dbo.RKD_Order.id
where PortalKATEK.dbo.RKD_GIP.id is not null


insert into PortalKATEK.dbo.ApproveCDQuestions
select
PortalKATEK.dbo.ApproveCDOrders.id
,PortalKATEK.dbo.RKD_Question.dateUpload
,PortalKATEK.dbo.RKD_Question.userUpload
,PortalKATEK.dbo.RKD_Question.textQuestion
,PortalKATEK.dbo.RKD_Question.complited
from PortalKATEK.dbo.RKD_Question left join PortalKATEK.dbo.RKD_Order on PortalKATEK.dbo.RKD_Order.id = PortalKATEK.dbo.RKD_Question.id_RKD_Order left join 
PortalKATEK.dbo.ApproveCDOrders on PortalKATEK.dbo.ApproveCDOrders.id_PZ_PlanZakaz = PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz


insert into PortalKATEK.dbo.ApproveCDQuestionCorr
select 
PortalKATEK.dbo.ApproveCDQuestions.id
,PortalKATEK.dbo.RKD_QuestionData.dateUpload
,PortalKATEK.dbo.RKD_QuestionData.userUpload
,PortalKATEK.dbo.RKD_QuestionData.textData
from PortalKATEK.dbo.ApproveCDQuestions left join
PortalKATEK.dbo.RKD_Question on PortalKATEK.dbo.RKD_Question.textQuestion = PortalKATEK.dbo.ApproveCDQuestions.textQuestion left join
PortalKATEK.dbo.RKD_QuestionData on PortalKATEK.dbo.RKD_QuestionData.id_RKD_Question = PortalKATEK.dbo.RKD_Question.id
where PortalKATEK.dbo.RKD_QuestionData.dateUpload is not null


insert into PortalKATEK.dbo.ApproveCDVersions
select PortalKATEK.dbo.ApproveCDOrders.id
,PortalKATEK.dbo.RKD_Version.id_RKD_VersionWork
,PortalKATEK.dbo.RKD_Version.numberVersion1
,PortalKATEK.dbo.RKD_Version.numberVersion2
,PortalKATEK.dbo.RKD_Version.activeVersion
from PortalKATEK.dbo.RKD_Version left join 
PortalKATEK.dbo.RKD_Order on PortalKATEK.dbo.RKD_Order.id = PortalKATEK.dbo.RKD_Version.id_RKD_Order left join
PortalKATEK.dbo.ApproveCDOrders on PortalKATEK.dbo.ApproveCDOrders.id_PZ_PlanZakaz = PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz left join
PortalKATEK.dbo.PZ_PlanZakaz on PortalKATEK.dbo.PZ_PlanZakaz.Id = PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz
where PortalKATEK.dbo.ApproveCDOrders.id is not null


insert into PortalKATEK.dbo.ApproveCDActions
select 
PortalKATEK.dbo.ApproveCDVersions.id
,PortalKATEK.dbo.RKD_Mail_Version.id_TypeRKD_Mail_Version
,PortalKATEK.dbo.RKD_Mail_Version.id_AspNetUser_Upload
,PortalKATEK.dbo.RKD_Mail_Version.dateTimeUpload
from [PortalKATEK].[dbo].[RKD_Mail_Version] left join
PortalKATEK.dbo.RKD_Version on PortalKATEK.dbo.RKD_Version.id = PortalKATEK.dbo.RKD_Mail_Version.id_RKD_Version left join
PortalKATEK.dbo.RKD_Order on PortalKATEK.dbo.RKD_Order.id = PortalKATEK.dbo.RKD_Version.id_RKD_Order left join
PortalKATEK.dbo.ApproveCDOrders on PortalKATEK.dbo.ApproveCDOrders.id_PZ_PlanZakaz = PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz 
left join
PortalKATEK.dbo.ApproveCDVersions 
on PortalKATEK.dbo.ApproveCDVersions.id_ApproveCDOrders = PortalKATEK.dbo.ApproveCDOrders.id
and PortalKATEK.dbo.RKD_Version.numberVersion1 = PortalKATEK.dbo.ApproveCDVersions.numberVersion1
and PortalKATEK.dbo.RKD_Version.numberVersion2 = PortalKATEK.dbo.ApproveCDVersions.numberVersion2
and PortalKATEK.dbo.RKD_Version.id_RKD_VersionWork = PortalKATEK.dbo.ApproveCDVersions.id_RKD_VersionWork
where PortalKATEK.dbo.ApproveCDOrders.id is not null
and PortalKATEK.dbo.ApproveCDVersions.id is not null

insert into PortalKATEK.dbo.ApproveCDTasks
select PortalKATEK.dbo.ApproveCDOrders.id
,PortalKATEK.dbo.RKD_Despatching.dateEvent
,PortalKATEK.dbo.RKD_Despatching.[text]
,PortalKATEK.dbo.RKD_Despatching.dateTaskFinishDate
,PortalKATEK.dbo.RKD_Despatching.id_AspNetUsers
from PortalKATEK.dbo.RKD_Despatching left join
PortalKATEK.dbo.RKD_Order on PortalKATEK.dbo.RKD_Order.id = PortalKATEK.dbo.RKD_Despatching.id_RKD_Order left join
PortalKATEK.dbo.ApproveCDOrders on PortalKATEK.dbo.ApproveCDOrders.id_PZ_PlanZakaz = PortalKATEK.dbo.RKD_Order.id_PZ_PlanZakaz
where  PortalKATEK.dbo.ApproveCDOrders.id is not null