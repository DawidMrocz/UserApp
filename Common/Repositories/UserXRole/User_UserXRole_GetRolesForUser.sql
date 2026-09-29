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
    AND [UXR].[User_Id] = @UserId