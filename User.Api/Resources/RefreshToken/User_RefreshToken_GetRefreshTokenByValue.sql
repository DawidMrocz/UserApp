SELECT TOP (1) 
	 Id
	,RefreshTokenValue
	,JwtTokenValue
	,ExpireDate
	,User_Id AS UserId
FROM 
	[user].[RefreshToken]
WHERE 
	[RefreshTokenValue] = @RefreshTokenValue
	AND [IsActive] = 1
ORDER BY [ExpireDate] DESC