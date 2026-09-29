UPDATE [user].[User]
SET 
    [ModifyUser_Id] = @UserId,
    [ModifyDate] = GETDATE(), 
    [Activated] = 1
WHERE 
    [User_Id] = @UserId;  