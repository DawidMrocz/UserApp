UPDATE [task].[Task]
SET 
	[ModifyUser_Id] = @UserId,
	[ModifyDate] = GETDATE(),
    [Status_Id] = (SELECT [Id] FROM [task].[Status] WHERE [IsActive] = 1 AND [StrongName] = @StatusStrongName)                
WHERE 
    [Id] = @TaskId
	AND [IsActive] = 1; 