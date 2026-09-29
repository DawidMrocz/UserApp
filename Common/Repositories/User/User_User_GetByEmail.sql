SELECT
       [U].[Id]
      ,[U].[Email]
      ,[U].[FirstName]
      ,[U].[LastName]
      ,[U].[PasswordHash]
      ,[U].[PasswordSalt]
      ,[U].[Blocked]
      ,[U].[Activated]
      ,[U].[UnblockTime]
      ,[U].[Street]
      ,[U].[City]
      ,[U].[PostalCode]
	  ,[CL].[Name] AS Culture
      ,[CL].[Code] AS CultureCode
	  ,[CT].[Name] AS Country
      ,[CT].[Code] AS CountryCode
	  ,(
		  SELECT 
			   [R].[Id] AS RoleId
			  ,[R].[Name]
			  ,[R].[StrongName]
		  FROM
			  [user].[UserXRole] AS [UXR] (NOLOCK)
			  INNER JOIN [user].[Role] AS [R] (NOLOCK) ON [UXR].[Role_Id] = [R].[Id]
		  WHERE
			  [UXR].[IsActive] = 1
			  AND [R].[IsActive] = 1
			  AND [UXR].[User_Id] = [U].[Id]
		  FOR JSON PATH
	  ) AS RolesJson
  FROM [user].[User] AS [U] (NOLOCK)
  LEFT JOIN [crm].[Culture] AS [CL] (NOLOCK) ON [U].[Culture_Id] = [CL].[Id] AND [CL].[IsActive] = 1
  LEFT JOIN [crm].[Country] AS [CT] (NOLOCK) ON [U].[Country_Id] = [CT].[Id] AND [CT].[IsActive] = 1
  WHERE
  [U].[IsActive] = 1
  AND [U].[Email] = @Email