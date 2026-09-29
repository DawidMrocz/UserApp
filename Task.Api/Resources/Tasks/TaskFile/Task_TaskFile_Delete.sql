UPDATE [task].[TaskFile]
SET 
    [IsActive] = 0, 
    [ModifyUser_Id] = @UserId,
    [ModifyDate] = GETDATE() 
WHERE 
    [Guid] = @Guid;  