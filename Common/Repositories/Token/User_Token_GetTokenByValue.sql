SELECT TOP (1) 
	Id
	,Value
	,ExpierDate
	,User_Id AS UserId
FROM 
	[user].[Token]
WHERE 
	[Value] = @Value
	AND [IsActive] = 1
ORDER BY [ExpireDate] DESC