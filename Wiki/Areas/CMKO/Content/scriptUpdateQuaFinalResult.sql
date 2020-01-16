DECLARE @periodQua NVARCHAR(6);
SET @periodQua ='2019.4';

delete PortalKATEK.dbo.CMKO_SummaryResultToMonth where PortalKATEK.dbo.CMKO_SummaryResultToMonth.id_CMKO_PeriodResult = @periodQua
insert into PortalKATEK.dbo.CMKO_SummaryResultToMonth
select PortalKATEK.dbo.CMKO_PeriodResult.[period] as [id_CMKO_PeriodResult]
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers as [id_AspNetUsers]
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefManager
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefError
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.coefErrorG
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhFact
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.nhGFact
,PortalKATEK.dbo.CMKO_ThisAccrued.accruedTotalFact as [ordersAccrue]
,iif(PortalKATEK.dbo.CMKO_ThisAccrued.bonusFact < 0, 0, PortalKATEK.dbo.CMKO_ThisAccrued.bonusFact) as [remainingBonusAccrue]
,PortalKATEK.dbo.CMKO_ThisAccruedG.accruedTotalFact as [gAccrue]
,iif(PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers = '4ebb8e70-7637-40b4-8c6e-3cd30a451d76',
     PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMKOMFact 
	 + PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMKOEFact
	 + PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMEFact * (TableNHE.NH1 / (TableNHE.NH1 + TableNHE.NH2)), 0) + 
 iif(PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers = '5ba3227f-ac84-4d65-ad87-632044217841',
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMEFact * (TableNHE.NH2 / (TableNHE.NH1 + TableNHE.NH2)), 0) + 
 iif(PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers = '8294e987-b175-4444-b300-8cb729448b38',
 PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.deductionsMMFact, 0) as [managerAccrue]
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.teach as [teachAccrue]
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.optimization as [optimizationAccrue]
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.speed1 + PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.speed2 + PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.speed3 as [timeUpAccrue]
,PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.qualityBonus as [qualityAccrue]
,0 as [rate]
,0 as [tax]
,0 as [result]
,0 as [resultToMonth]
from PortalKATEK.dbo.CMKO_PeriodResult left join
PortalKATEK.dbo.CMKO_ThisIndicatorsUsers on PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers is not null left join
PortalKATEK.dbo.CMKO_ThisAccrued on PortalKATEK.dbo.CMKO_ThisAccrued.id_AspNetUsers = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers left join
PortalKATEK.dbo.CMKO_ThisAccruedG on PortalKATEK.dbo.CMKO_ThisAccruedG.id_AspNetUsers = PortalKATEK.dbo.CMKO_ThisIndicatorsUsers.id_AspNetUsers left join
PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund on PortalKATEK.dbo.CMKO_ThisDeductionsBonusFund.id is not null left join
PortalKATEK.dbo.CMKO_ThisFinalBonus on PortalKATEK.dbo.CMKO_ThisFinalBonus.id is not null left join
(select sum(iif(PortalKATEK.dbo.CMKO_BujetList.DevisionKBE = 1, PortalKATEK.dbo.CMKO_BujetList.normH, 0)) as NH1
		,sum(iif(PortalKATEK.dbo.CMKO_BujetList.DevisionKBE = 2, PortalKATEK.dbo.CMKO_BujetList.normH, 0)) as NH2
	from PortalKATEK.dbo.CMKO_BujetList 
	where PortalKATEK.dbo.CMKO_BujetList.quartalFinishTask = @periodQua and (PortalKATEK.dbo.CMKO_BujetList.DevisionKBE > 0)) as TableNHE on TableNHE.NH1 is not null 
where PortalKATEK.dbo.CMKO_PeriodResult.[period] = @periodQua 

update PortalKATEK.dbo.CMKO_SummaryResultToMonth
set result = ordersAccrue + remainingBonusAccrue + gAccrue + managerAccrue + teachAccrue + optimizationAccrue + timeUpAccrue + qualityAccrue - rate - tax
where PortalKATEK.dbo.CMKO_SummaryResultToMonth.id_CMKO_PeriodResult = @periodQua

update PortalKATEK.dbo.CMKO_SummaryResultToMonth
set resultToMonth = result / 3
where PortalKATEK.dbo.CMKO_SummaryResultToMonth.id_CMKO_PeriodResult = @periodQua