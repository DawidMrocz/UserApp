UPDATE [bilboard].[Task]
SET 
	[ModifyUser_Id] = @UserId,
	[ModifyDate] = GETDATE(),
    [Title] = @Title,
    [Deadline] = @Deadline,              
    [IsImportant] = @IsImportant,
    [EstimatedHours] = @EstimatedHours   
WHERE 
    [External_Id] = @ExternalId; 