UPDATE [bilboard].[Task]
SET 
	[ModifyUser_Id] = @UserId,
	[ModifyDate] = GETDATE(),
    [Status_Id] = (SELECT [Id] FROM [bilboard].[BilboardItemStatus] WHERE [IsActive] = 1 AND [StrongName] = @StatusStrongName)                
WHERE 
    [Id] = @TaskId; 