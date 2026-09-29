INSERT INTO 
[bilboard].[Bilboard]
    (
    [CreateUser_Id], 
    [CreateDate],
    [User_Id]
    )
VALUES
    (
    1, 
    GETDATE(), 
    @UserId
    );