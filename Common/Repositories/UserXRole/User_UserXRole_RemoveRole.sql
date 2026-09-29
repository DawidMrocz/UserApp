UPDATE
	[user].[UserXRole]
SET 
	IsActive = 0
WHERE
	 [User_Id] = @UserId
	 AND [Role_Id] = @RoleId
	 AND [IsActive] = 1
