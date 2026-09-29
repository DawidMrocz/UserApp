UPDATE [task].[Task]
SET 
	[ModifyUser_Id] = @UserId,
	[ModifyDate] = GETDATE(),
    [IsActive] = 0  
WHERE 
    [Id] = @TaskId;    