INSERT INTO [task].[TaskFile] 
(
    [CreateUser_Id], 
    [CreateDate], 
    [Task_Id], 
    [Guid], 
    [FileName],
    [User_Id],
    [RoleStrongName]
)
VALUES
(
    @UserId,
    GETDATE(),
    @TaskId,
    @Guid,
    @FileName,
    @UserId,
    @RoleStrongName
);

SELECT SCOPE_IDENTITY();