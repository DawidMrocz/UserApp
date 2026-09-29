INSERT INTO [user].[User] (
    [CreateUser_Id],
    [CreateDate],
    [ModifyUser_Id],
    [ModifyDate],
    [IsActive],
    [Email],
    [FirstName],
    [LastName],
    [PasswordHash],
    [PasswordSalt],
    [Blocked],
    [Activated],
    [UnblockTime],
    [Street],
    [City],
    [Country_Id],
    [PostalCode],
    [Culture_Id]
)
VALUES (
    1,                          -- CreateUser_Id
    GETDATE(),                  -- CreateDate
    NULL,                       -- ModifyUser_Id
    NULL,                       -- ModifyDate
    1,                          -- IsActive
    @Email,
    @FirstName,
    @LastName,
    @PasswordHash,
    @PasswordSalt,
    0,
    0,
    NULL,
    @Street,              
    @City,              
    @CountryId,       
    @PostalCode,
    @CultureId 
);

SELECT SCOPE_IDENTITY();