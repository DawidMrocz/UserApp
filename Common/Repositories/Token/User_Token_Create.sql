INSERT INTO [user].[Token] 
(
	[CreateUser_Id], 
	[CreateDate],
	[ModifyUser_Id],
	[ModifyDate],
	[IsActive], 
	[Value], 
	[ExpireDate],
	[User_Id]
)
VALUES 
(
	@UserId, 
	GETDATE(), 
	@UserId, 
	GETDATE(), 
	1, 
	@Value, 
	@ExpireDate, 
	@UserId
);

SELECT SCOPE_IDENTITY()