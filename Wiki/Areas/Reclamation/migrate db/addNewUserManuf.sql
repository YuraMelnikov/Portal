insert into [PortalKATEK].[dbo].[AspNetUsers]
  SELECT 
  NEWID ( ),
  '23@katek.by'
      ,[EmailConfirmed]
      ,[PasswordHash]
      ,[SecurityStamp]
      ,[PhoneNumber]
      ,[PhoneNumberConfirmed]
      ,[TwoFactorEnabled]
      ,[LockoutEndDateUtc]
      ,0
      ,[AccessFailedCount]
      ,'23@katek.by'
      ,'Мацкевич А.А.'
      ,[Devision]
      ,[ResourceUID]
  FROM [PortalKATEK].[dbo].[AspNetUsers]
  where [CiliricalName] like '%Корхов%'