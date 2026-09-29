SELECT 
	   [BI].[Id]
	  ,[T].[Id] AS TaskId
      ,[T].[External_Id] AS TaskExternalId
  FROM 
  [bilboard].[BilboardItem] AS [BI] (NOLOCK)
  INNER JOIN [bilboard].[Task] AS [T] (NOLOCK) ON [BI].[Task_Id] = [T].[Id]
  WHERE
  [BI].[IsActive] = 1
  AND [T].[IsActive] = 1
  AND [BI].[Id] = @BilboardItemId