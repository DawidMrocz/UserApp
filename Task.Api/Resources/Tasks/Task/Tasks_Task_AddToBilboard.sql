UPDATE [task].[Task]
SET 
	[ModifyUser_Id] = @UserId,
	[ModifyDate] = GETDATE(),                
    [User_Id] = @UserId  
WHERE 
    [Id] = @TaskId; 