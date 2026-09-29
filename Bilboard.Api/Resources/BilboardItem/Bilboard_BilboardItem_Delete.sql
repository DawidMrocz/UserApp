UPDATE [bilboard].[BilboardItem]
SET
    [ModifyUser_Id] = @UserId,       -- Ustaw u¿ytkownika modyfikuj¹cego rekord
    [ModifyDate] = GETDATE(),             -- Zaktualizuj datê modyfikacji
    [IsActive] = 0                    -- Zaktualizuj identyfikator zadania
WHERE 
    [Id] = @BilboardItemId; 