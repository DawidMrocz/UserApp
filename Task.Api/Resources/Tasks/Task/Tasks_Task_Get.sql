SELECT
       [T].[Id]
      ,[T].[CreateDate]
      ,[T].[ModifyDate]
      ,[T].[Title]
      ,[T].[Deadline]
      ,[T].[IsImportant]
      ,[T].[EstimatedHours]
	  ,[S].[Name] AS Status
      ,(
		SELECT
			 [Id]		
			,[Guid]
			,[FileName] AS Name
		FROM [task].[TaskFile]
		WHERE
			[IsActive] = 1
			AND [Task_Id] = @TaskId
		FOR JSON PATH
      ) AS FileJson
  FROM 
	 [task].[Task] AS [T] (NOLOCK)
	 INNER JOIN [task].[Status] AS [S] (NOLOCK) ON [T].[Status_Id] = [S].[Id]
  WHERE
	[T].[IsActive] = 1
	AND [S].[IsActive] = 1
	AND [T].[Id] = @TaskId