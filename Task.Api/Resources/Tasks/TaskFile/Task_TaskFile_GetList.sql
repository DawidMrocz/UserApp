SELECT 
    [Id],
    [Task_Id] AS TaskId, 
    [Guid], 
    [FileName],
    [User_Id] AS UserId,
    [RoleStrongName]
FROM 
    [task].[TaskFile]
WHERE 
    [IsActive] = 1 
    AND [Task_Id] = @TaskId;