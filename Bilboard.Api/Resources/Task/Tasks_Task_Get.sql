SELECT
       [T].[Id]
      ,[T].[CreateUser_Id] AS CreateUserId
      ,[T].[CreateDate]
      ,[T].[ModifyUser_Id] AS ModifyUserId
      ,[T].[ModifyDate]
      ,[T].[Title]
      ,[T].[Deadline]
      ,[T].[IsImportant]
      ,[T].[EstimatedHours]
	  ,[S].[Id] AS StatusId
  FROM 
	 [bilboard].[Task] AS [T] (NOLOCK)
	 INNER JOIN [bilboard].[BilboardItemStatus] AS [S] (NOLOCK) ON [T].[Status_Id] = [S].[Id]
  WHERE
	[T].[IsActive] = 1
	AND [S].[IsActive] = 1
	AND [T].[External_Id] = @ExternalTaskId