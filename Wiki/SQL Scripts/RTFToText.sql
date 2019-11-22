SELECT 
[ProjectWebApp].[pub].[MSP_TASKS].TASK_HAS_NOTES
,[ProjectWebApp].[pub].[MSP_TASKS].TASK_NOTES
,[ProjectWebApp].[pub].[MSP_TASKS].TASK_RTF_NOTES
  FROM [ProjectWebApp].[pub].[MSP_TASKS]
  where [ProjectWebApp].[pub].[MSP_TASKS].TASK_HAS_NOTES > 0