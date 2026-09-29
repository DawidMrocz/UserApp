INSERT INTO [task].[Task] (
    [CreateUser_Id],
    [CreateDate],
    [Title],
    [Deadline],
    [Status_Id],
    [IsImportant],
	[EstimatedHours]
)
VALUES (  
    @UserId,
    GETDATE(),
    @Title,  
    @Deadline,  
    @StatusId,                                                      
    @IsImportant,                         
	@EstimatedHours
);

SELECT SCOPE_IDENTITY();