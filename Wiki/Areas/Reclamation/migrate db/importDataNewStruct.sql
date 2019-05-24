--Reclamation PO
insert into [PortalKATEK_TEST].[dbo].[Reclamation]
select
PortalKATEK_TEST.[dbo].[Reclamation_Type].id
,PortalKATEK_TEST.[dbo].[Devision].id
,[PortalKATEK_TEST].[dbo].[Reclamation_CountError].id
,[PortalKATEK_TEST].[dbo].[Reclamation_CountError].id
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].userCreate
,PortalKATEK_TEST.[dbo].[Devision].id
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].dateCreate
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].textReclamation
,''
,0
,0
,1--close
,0--gip
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].id
,null
,1--closeDevision
,''--PCAM
,0--editManuf
,null
,21
,0--TA
,null
,1--closeMKO
,1--fixExpert
from [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO] left join
[PortalKATEK_TEST].[dbo].[OTK_TypeReclamation] on [PortalKATEK_TEST].[dbo].[OTK_TypeReclamation].id = [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].typeReclamation left join
PortalKATEK_TEST.[dbo].[Reclamation_Type] on PortalKATEK_TEST.[dbo].[Reclamation_Type].name = [PortalKATEK_TEST].[dbo].[OTK_TypeReclamation].Name left join
PortalKATEK_TEST.[dbo].[AspNetUsers] on PortalKATEK_TEST.[dbo].[AspNetUsers].Id = [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].userCreate left join
PortalKATEK_TEST.[dbo].[Devision] on PortalKATEK_TEST.[dbo].[Devision].id = PortalKATEK_TEST.[dbo].[AspNetUsers].Devision left join
PortalKATEK_TEST.[dbo].[OTK_CounterErrorKO] on PortalKATEK_TEST.[dbo].[OTK_CounterErrorKO].id = [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].countError left join
[PortalKATEK_TEST].[dbo].[Reclamation_CountError] on [PortalKATEK_TEST].[dbo].[Reclamation_CountError].name = PortalKATEK_TEST.[dbo].[OTK_CounterErrorKO].name
order by
PortalKATEK_TEST.[dbo].[Devision].id

update [PortalKATEK_TEST].[dbo].[Reclamation] set
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = PortalKATEK_TEST.[dbo].[Devision].id
,[PortalKATEK_TEST].[dbo].[Reclamation].id_AspNetUsersError = [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].userError
from [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO] left join
PortalKATEK_TEST.[dbo].[AspNetUsers] on PortalKATEK_TEST.[dbo].[AspNetUsers].Id = [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].userError left join
PortalKATEK_TEST.[dbo].[Devision] on PortalKATEK_TEST.[dbo].[Devision].id = PortalKATEK_TEST.[dbo].[AspNetUsers].Devision left join 
[PortalKATEK_TEST].[dbo].[Reclamation] on [PortalKATEK_TEST].[dbo].[Reclamation].timePO = [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].id

insert into [PortalKATEK_TEST].[dbo].[Reclamation_Answer]
select 
[PortalKATEK_TEST].[dbo].[Reclamation].id
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].userUpdate
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].textAnswer
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].dateUpdate
,0
from 
[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO] left join
[PortalKATEK_TEST].[dbo].[Reclamation] on [PortalKATEK_TEST].[dbo].[Reclamation].timePO = [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].id
where
[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].userUpdate not like ''

insert into [PortalKATEK_TEST].[dbo].[Reclamation_Answer]
select 
[PortalKATEK_TEST].[dbo].[Reclamation].id
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].userUpdate
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].descriptionManagerKO
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].dateUpdate
,0
from 
[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO] left join
[PortalKATEK_TEST].[dbo].[Reclamation] on [PortalKATEK_TEST].[dbo].[Reclamation].timePO = [PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].id
where
[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].userUpdate not like ''
and
[PortalKATEK_TEST].[dbo].[OTK_ReclamationKO].descriptionManagerKO is not null

insert into [PortalKATEK_test].[dbo].[Reclamation_PZ]
select
[PortalKATEK_TEST].[dbo].OTK_ReclamationKO.[order],
[PortalKATEK_TEST].[dbo].[Reclamation].id
from
[PortalKATEK_test].[dbo].[Reclamation] left join
[PortalKATEK_TEST].[dbo].OTK_ReclamationKO on [PortalKATEK_TEST].[dbo].OTK_ReclamationKO.id = [PortalKATEK_test].[dbo].[Reclamation].timePO
where [PortalKATEK_test].[dbo].[Reclamation].timePO is not null

--Reclamation OTK
insert into [PortalKATEK_TEST].[dbo].[Reclamation]
select
[PortalKATEK_TEST].[dbo].[Reclamation_Type].id
,[PortalKATEK_TEST].[dbo].[OTK_Reclamation].Devision
,1
,1
,[PortalKATEK_TEST].[dbo].[OTK_Reclamation].[User]
,6
,[PortalKATEK_TEST].[dbo].[OTK_Reclamation].DateTimeCreate
,[PortalKATEK_TEST].[dbo].[OTK_Reclamation].ReclamationText
,[PortalKATEK_TEST].[dbo].[OTK_Reclamation].Description
,[PortalKATEK_TEST].[dbo].[OTK_Reclamation].LaborCostOTK
,[PortalKATEK_TEST].[dbo].[OTK_Reclamation].LaborCostManufacturing
,[PortalKATEK_TEST].[dbo].[OTK_Reclamation].Complited --close
,0--gip
,null
,[PortalKATEK_TEST].[dbo].[OTK_Reclamation].id
,1--closeDevision
,'' --PCAM
,0--editManuf
,null
,21
,0--TA
,null
,1--closeMKO
,1--fixExpert
from [PortalKATEK_TEST].[dbo].[OTK_Reclamation] left join
[PortalKATEK_TEST].[dbo].[OTK_TypeReclamation] on [PortalKATEK_TEST].[dbo].[OTK_TypeReclamation].id = [PortalKATEK_TEST].[dbo].[OTK_Reclamation].TypeReclamation left join
[PortalKATEK_TEST].[dbo].[Reclamation_Type] on [PortalKATEK_TEST].[dbo].[Reclamation_Type].name = [PortalKATEK_TEST].[dbo].[OTK_TypeReclamation].Name

insert into [PortalKATEK_test].[dbo].[Reclamation_PZ]
select
[PortalKATEK_TEST].[dbo].[OTK_ChaeckList].[Order],
[PortalKATEK_TEST].[dbo].[Reclamation].id
from
[PortalKATEK_test].[dbo].[Reclamation] left join
[PortalKATEK_TEST].[dbo].[OTK_Reclamation] on [PortalKATEK_TEST].[dbo].[OTK_Reclamation].id = [PortalKATEK_test].[dbo].[Reclamation].timeOTK left join
[PortalKATEK_TEST].[dbo].[OTK_ChaeckList] on [PortalKATEK_TEST].[dbo].[OTK_ChaeckList].id = [PortalKATEK_TEST].[dbo].[OTK_Reclamation].CheckList
where [PortalKATEK_test].[dbo].[Reclamation].timeOTK is not null

insert into [PortalKATEK_TEST].[dbo].[Reclamation_Answer]
select 
[PortalKATEK_TEST].[dbo].[Reclamation].id
--,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].[Text]
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].DateUpdate
,0
from
[PortalKATEK_TEST].[dbo].[Reclamation] left join
[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer] on [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].ReclamationId = [PortalKATEK_TEST].[dbo].[Reclamation].timeOTK
where
[PortalKATEK_TEST].[dbo].[Reclamation].timeOTK is not null
and
([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate is not null)
and 
([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError is not null)

insert into [PortalKATEK_TEST].[dbo].[Reclamation_Answer]
select 
[PortalKATEK_TEST].[dbo].[Reclamation].id
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate as [data]
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].[Text]
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].DateUpdate
,0
from
[PortalKATEK_TEST].[dbo].[Reclamation] left join
[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer] on [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].ReclamationId = [PortalKATEK_TEST].[dbo].[Reclamation].timeOTK
left join [PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].id = [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate
where
[PortalKATEK_TEST].[dbo].[Reclamation].timeOTK is not null
and
([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate is not null)
and 
([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError is null)
and [PortalKATEK].[dbo].[AspNetUsers].Id is not null
order by
[PortalKATEK].[dbo].[AspNetUsers].Id

insert into [PortalKATEK_TEST].[dbo].[Reclamation_Answer]
select 
[PortalKATEK_TEST].[dbo].[Reclamation].id
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate as [data]
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].descriptionManagerKO
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].DateUpdate
,0
from
[PortalKATEK_TEST].[dbo].[Reclamation] left join
[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer] on [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].ReclamationId = [PortalKATEK_TEST].[dbo].[Reclamation].timeOTK
left join [PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].id = [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate
where
[PortalKATEK_TEST].[dbo].[Reclamation].timeOTK is not null
and
([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate is not null)
and 
([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError is null)
and [PortalKATEK].[dbo].[AspNetUsers].Id is not null
and ([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].descriptionManagerKO is not null)

insert into [PortalKATEK_TEST].[dbo].[Reclamation_Answer]
select 
[PortalKATEK_TEST].[dbo].[Reclamation].id
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError as [data]
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].descriptionManagerKO
,[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].DateUpdate
,0
from
[PortalKATEK_TEST].[dbo].[Reclamation] left join
[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer] on [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].ReclamationId = [PortalKATEK_TEST].[dbo].[Reclamation].timeOTK
left join [PortalKATEK].[dbo].[AspNetUsers] on [PortalKATEK].[dbo].[AspNetUsers].id = [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate
where
[PortalKATEK_TEST].[dbo].[Reclamation].timeOTK is not null
and
([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].UserUpdate is not null)
and 
([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError is not null)
and [PortalKATEK].[dbo].[AspNetUsers].Id is not null
and ([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].descriptionManagerKO is not null)
order by
[PortalKATEK].[dbo].[AspNetUsers].Id

update [PortalKATEK_TEST].[dbo].[Reclamation]
set [PortalKATEK_TEST].[dbo].[Reclamation].id_Reclamation_CountErrorFirst = [PortalKATEK_TEST].[dbo].[Reclamation_CountError].id
,[PortalKATEK_TEST].[dbo].[Reclamation].id_Reclamation_CountErrorFinal = [PortalKATEK_TEST].[dbo].[Reclamation_CountError].id
from
[PortalKATEK_TEST].[dbo].[Reclamation] left join
[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer] on [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].ReclamationId = [PortalKATEK_TEST].[dbo].[Reclamation].timeOTK left join
[PortalKATEK_TEST].[dbo].[OTK_CounterErrorKO] on [PortalKATEK_TEST].[dbo].[OTK_CounterErrorKO].id = [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].countError left join
[PortalKATEK_TEST].[dbo].[Reclamation_CountError] on [PortalKATEK_TEST].[dbo].[Reclamation_CountError].name = [PortalKATEK_TEST].[dbo].[OTK_CounterErrorKO].name
where
[PortalKATEK_TEST].[dbo].[Reclamation].timeOTK > 0
and
([PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 3 or
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 15 or
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 16)
and
([PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].Devision = 3 or
[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].Devision = 15 or
[PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].Devision = 16)
and ([PortalKATEK_TEST].[dbo].[Reclamation_CountError].id is not null)

delete from [PortalKATEK_TEST].[dbo].[Reclamation_Answer] where [answer] like ''

update [PortalKATEK_TEST].[dbo].[Reclamation] set
[PortalKATEK_TEST].[dbo].[Reclamation].closeDevision = 1
FROM [PortalKATEK_TEST].[dbo].[Reclamation] left join [PortalKATEK_TEST].[dbo].[Reclamation_Answer] on [PortalKATEK_TEST].[dbo].[Reclamation_Answer].id_Reclamation = [PortalKATEK_TEST].[dbo].[Reclamation].id
where [PortalKATEK_TEST].[dbo].[Reclamation_Answer].id is not null

update [PortalKATEK_TEST].[dbo].[Reclamation]
set [PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 6
from [PortalKATEK_TEST].[dbo].[Reclamation]
where [PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 26

update [PortalKATEK_TEST].[dbo].[Reclamation] set [id_PF] = 21

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = null
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 6

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = 'e27c9470-9316-4957-b848-62c8bee898f2'
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 7

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = '3a065e5d-35c9-402a-982b-2e296096f216'
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 8

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = 'ce0af4d6-3cc6-48aa-9b63-f83ce8454be8'
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 9

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = '55c7db94-9317-4043-b5b9-c890ccc5b64d'
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 10

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = '67d16a86-19a5-411c-a7e6-590de391e585'
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 12

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = '266ca9be-6981-453f-8251-fff856f460f8'
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 18

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = '99fdfb7c-48db-4a3b-a300-4ec689880dbc'
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 20

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = '0997dac8-5e26-4c1a-a4a4-9079224cf0aa'
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 22

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = '9a1b2fd6-d4a8-4576-98f1-d472b011372f'
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 25

update [PortalKATEK_TEST].[dbo].[Reclamation]
set id_AspNetUsersError = null
from
[PortalKATEK_TEST].[dbo].[Reclamation]
where
[PortalKATEK_TEST].[dbo].[Reclamation].id_DevisionReclamation = 26


--dbug
update [PortalKATEK_TEST].[dbo].[Reclamation] set
[PortalKATEK_TEST].[dbo].[Reclamation].id_AspNetUsersError = [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError
  FROM [PortalKATEK_TEST].[dbo].[Reclamation]
  left join [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer] on [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].ReclamationId = [timeOTK]
  where id_DevisionReclamation = 15 and [timeOTK] is not null
  and [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError is not null
  and [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].countError is not null

  update [PortalKATEK_TEST].[dbo].[Reclamation] set
[PortalKATEK_TEST].[dbo].[Reclamation].id_AspNetUsersError = [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError
  FROM [PortalKATEK_TEST].[dbo].[Reclamation]
  left join [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer] on [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].ReclamationId = [timeOTK]
  where id_DevisionReclamation = 3 and [timeOTK] is not null
  and [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError is not null
  and [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].countError is not null

  update [PortalKATEK_TEST].[dbo].[Reclamation] set
[PortalKATEK_TEST].[dbo].[Reclamation].id_AspNetUsersError = [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError
  FROM [PortalKATEK_TEST].[dbo].[Reclamation]
  left join [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer] on [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].ReclamationId = [timeOTK]
  where id_DevisionReclamation = 16 and [timeOTK] is not null
  and [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].userError is not null
  and [PortalKATEK_TEST].[dbo].[OTK_ReclamationAnswer].countError is not null