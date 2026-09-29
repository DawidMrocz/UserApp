INSERT INTO [bilboard].[Task] (
    [CreateUser_Id],
    [CreateDate],
    [Title],
    [Deadline],
    [Status_Id],
    [IsImportant],
	[EstimatedHours],
    [External_Id]
)
VALUES ( 
@UserId,
GETDATE(),
    @Title,  
    @Deadline,  
    (
            SELECT
	        [Id]
          FROM 
	        [bilboard].[BilboardItemStatus]
          WHERE
	        [ExternalId] = @StatusExternalId
            AND [IsActive] = 1
    ),                                                      
    @IsImportant,                       
	@EstimatedHours,
    @ExternalId
);

SELECT SCOPE_IDENTITY();