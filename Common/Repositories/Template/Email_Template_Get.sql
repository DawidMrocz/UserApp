SELECT 
Id, 
[Body], 
[Subject], 
[Name], 
[StrongName], 
[Description]
FROM [email].[Template]
WHERE 
[StrongName] = @StrongName
AND IsActive = 1;