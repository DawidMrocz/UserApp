SET
	@Page = ISNULL(@Page, 1)
SET
	@PageSize = ISNULL(@PageSize, 10)
SET
	@OrderDir = ISNULL(@OrderDir, 1)
SET
	@OrderBy = ISNULL(@OrderBy, 'Name') DECLARE @OrderDesc VARCHAR(10) = CASE
		WHEN @OrderDir < 0 THEN 'DESC'
		WHEN @OrderDir >= 0 THEN 'ASC'
	END DECLARE @SortExpression VARCHAR(300) = CONCAT(@OrderBy, ' ', @OrderDesc);

SELECT
       [T].[Id]
      ,[T].[Title]
      ,[T].[Deadline]
      ,[T].[IsImportant]
	  ,[S].[Name] AS Status
	  ,COUNT(*) OVER() AS TotalRows
  FROM 
	 [task].[Task] AS [T] (NOLOCK)
	 INNER JOIN [task].[Status] AS [S] (NOLOCK) ON [T].[Status_Id] = [S].[Id]
  WHERE
	[T].[IsActive] = 1
	AND [S].[IsActive] = 1
	AND (@Title IS NULL OR [T].[Title] LIKE '%' + @Title + '%')
	AND (@StatusStrongName IS NULL OR [S].[StrongName] = @StatusStrongName)
	AND (@DateTo IS NULL OR [T].[Deadline] <= @DateTo)
	AND (@DateFrom IS NULL OR [T].[Deadline] >= @DateTo)
	AND [T].[User_Id] IS NULL
  ORDER BY
	CASE
		WHEN @SortExpression = 'Title ASC' THEN [T].[Title]
	END ASC,
	CASE
		WHEN @SortExpression = 'Title DESC' THEN [T].[Title]
	END DESC
	OFFSET (@Page -1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY