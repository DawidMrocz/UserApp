UPDATE [user].[User]
SET 
    [ModifyUser_Id] = @UserId,
    [ModifyDate] = GETDATE(), 
    [Blocked] = 0
WHERE 
    [User_Id] = @UserId;  