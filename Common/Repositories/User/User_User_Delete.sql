UPDATE [user].[User]
SET 
    [ModifyUser_Id] = @UserId,   -- Id u¿ytkownika, który dokonuje modyfikacji
    [ModifyDate] = GETDATE(),           -- Data modyfikacji
    [IsActive] = 0
WHERE 
    [User_Id] = @UserId;  