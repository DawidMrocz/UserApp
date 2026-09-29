UPDATE [user].[User]
SET 
    [ModifyUser_Id] = @UserId,
    [ModifyDate] = GETDATE(),
    [PasswordHash] = @PasswordHash,
    [PasswordSalt] = @PasswordSalt
WHERE 
    [User_Id] = @UserId;  