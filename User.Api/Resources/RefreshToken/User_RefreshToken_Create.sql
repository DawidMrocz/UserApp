INSERT INTO [user].[RefreshToken] 
(
	[CreateUser_Id], 
	[CreateDate],
	[ModifyUser_Id],
	[ModifyDate],
	[IsActive], 
	[JwtTokenValue], 
	[RefreshTokenValue], 
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
	@JwtTokenValue, 
	@RefreshTokenValue, 
	@ExpireDate, 
	@UserId
);

SELECT SCOPE_IDENTITY()