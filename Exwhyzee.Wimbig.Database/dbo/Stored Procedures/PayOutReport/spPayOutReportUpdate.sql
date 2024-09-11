-- Update Category entity

CREATE PROCEDURE [dbo].[spPayOutReportUpdate]
	@Id bigint,
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
	UPDATE [dbo].[PayOutReport] SET
	[Date]= @date,
	[Amount] = @amount,
	[PercentageAmount] = @percentageAmount,
	[Percentage] = @percentage,
	[Reference] = @reference,
	[Status] = @status,
	[Note] = @note,
	[StartDate] = @startDate,
	[EndDate] = @endDate,
	[UserId] = @userId

	WHERE [Id] = @Id
END