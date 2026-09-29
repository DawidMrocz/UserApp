UPDATE [user].[User]
SET 
    [ModifyUser_Id] = @UserId,
    [ModifyDate] = GETDATE(), 
    [Blocked] = 1,      
    [UnblockTime] = @UnblockTime
WHERE 
    [User_Id] = @UserId;  