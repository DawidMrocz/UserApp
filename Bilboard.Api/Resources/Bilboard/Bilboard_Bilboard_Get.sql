SELECT
       [Id]
      ,[User_Id] AS UserId
	  ,(
		  SELECT 
			 [BI].[Id] AS BilboardItemId
			,[BI].[Hours]
			,[T].[Title]
			,[T].[Deadline]
			,[ST].[Name] AS Status
			,[T].[EstimatedHours]
			,[T].[IsImportant]
		  FROM [bilboard].[BilboardItem] AS [BI] (NOLOCK)
			INNER JOIN [bilboard].[Task] AS [T] (NOLOCK) ON [BI].[Task_Id] = [T].[Id]
			INNER JOIN [bilboard].[BilboardItemStatus] AS [ST] (NOLOCK) ON [T].[Status_Id] = [ST].[Id]
		  WHERE
			[BI].[IsActive] = 1
			AND [T].[IsActive] = 1
			AND [ST].[IsActive] = 1
			AND [BI].[Bilboard_Id] = [B].[Id]
			FOR JSON PATH
	  ) AS BilboardItemsJson
  FROM [bilboard].[Bilboard] AS [B] (NOLOCK)
  WHERE
  [B].[IsActive] = 1
  AND [B].[User_Id] = @UserId