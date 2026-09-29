UPDATE [user].[RefreshToken]
SET 
[IsActive] = 0
WHERE [Id] = @RefreshTokenId; -- Zamieñ @Id na odpowiednie Id