UPDATE [bilboard].[Task]
SET 
	[ModifyUser_Id] = @UserId,
	[ModifyDate] = GETDATE(),
    [IsActive] = 0  
WHERE 
    [External_Id] = @ExternalId;    