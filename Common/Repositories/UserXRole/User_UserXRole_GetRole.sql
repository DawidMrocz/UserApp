SELECT
       [Id] AS RoleId
      ,[Name]
      ,[StrongName]
  FROM [user].[Role]
  WHERE 
  [IsActive] = 1
  AND [StrongName] = @StrongName