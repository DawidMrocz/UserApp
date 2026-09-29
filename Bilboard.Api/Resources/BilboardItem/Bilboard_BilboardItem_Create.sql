INSERT INTO [bilboard].[BilboardItem] 
(
    [CreateUser_Id], 
    [CreateDate], 
    [IsActive], 
    [Bilboard_Id], 
    [Hours], 
    [Task_Id]
)
VALUES
(
    @UserId,               -- U¿ytkownik tworz¹cy
    GETDATE(),                   -- Data utworzenia (aktualna data i godzina)
    1,                           -- Aktywnoœæ (1 = aktywna, 0 = nieaktywna)
    @BilboardId,                 -- ID billboardu
    @Hours,                      -- Liczba godzin (mo¿e byæ NULL, jeœli nie dotyczy)
    @TaskId                      -- ID zadania
);

SELECT SCOPE_IDENTITY();