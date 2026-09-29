UPDATE [task].[Task]
SET 
	[ModifyUser_Id] = @UserId,
	[ModifyDate] = GETDATE(),
    [User_Id] = NULL ,
	[Status_Id] = 1
WHERE 
    [Id] = @TaskId;    