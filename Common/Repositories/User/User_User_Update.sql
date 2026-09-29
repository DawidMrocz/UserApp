UPDATE [user].[User]
SET 
    [ModifyUser_Id] = @UserId,   -- Id u¿ytkownika, który dokonuje modyfikacji
    [ModifyDate] = GETDATE(),           -- Data modyfikacji
    [Email] = @Email,                   -- Nowy adres e-mail
    [FirstName] = @FirstName,           -- Nowe imiê
    [LastName] = @LastName,
    [Street] = @Street,                 -- Nowa ulica
    [City] = @City,                     -- Nowe miasto
    [Country_Id] = @CountryId,          -- Nowy kraj
    [PostalCode] = @PostalCode,         -- Nowy kod pocztowy
    [Culture_Id] = @CultureId           -- Nowa kultura
WHERE 
    [User_Id] = @UserId;  

SELECT SCOPE_IDENTITY();