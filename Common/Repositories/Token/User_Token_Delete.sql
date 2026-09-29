UPDATE [user].[Token]
SET 
[IsActive] = 0
WHERE [Id] = @TokenId; -- Zamieñ @Id na odpowiednie Id