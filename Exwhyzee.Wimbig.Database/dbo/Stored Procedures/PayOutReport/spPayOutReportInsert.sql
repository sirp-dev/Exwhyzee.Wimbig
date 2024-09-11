--Create Category Entity

CREATE PROCEDURE [dbo].[spPayOutReportInsert]
		@date datetime,
		@amount decimal,
		@percentageAmount decimal,
		@percentage int,
		@reference int,
		@status int,
		@note nvarchar(max),
		@startDate datetime,
		@endDate datetime,
		@userId nvarchar(250)

		
AS
BEGIN
	INSERT INTO [dbo].[PayOutReport] ([Date],[Amount],[PercentageAmount],[Percentage],[Reference],[Status],[Note],[StartDate],[EndDate],[UserId])
	output inserted.Id

	VALUES (@date,@amount,@percentageAmount,@percentage,@reference,@status,@note,@startDate,@endDate,@userId)
END
