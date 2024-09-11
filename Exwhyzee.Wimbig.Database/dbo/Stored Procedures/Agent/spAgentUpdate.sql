-- Update  entity

CREATE PROCEDURE [dbo].[spAgentUpdate]
	@Id bigint,
	@fullname nvarchar(Max),
	@emailAddress nvarchar(Max),
	@state nvarchar(Max),
	@lga nvarchar(Max),
	@areYouNewToWimbig nvarchar(Max),
	@contactAddress nvarchar(Max),
	@dateCreated datetime,
	@currentOccupation nvarchar(Max),
	@phoneNumber nvarchar(Max),
	@shopLocation nvarchar(Max),
	@gender nvarchar(10),
	@status int
		

	
AS
BEGIN
	UPDATE [dbo].[Agent] SET
	[Fullname] = @fullname,
	[EmailAddress] = @emailAddress,
	[State] = @state,
	[LGA] = @lga,
	[AreYouNewToWimbig] = @areYouNewToWimbig,
	[ContactAddress] = @contactAddress,
	[DateCreated] = @dateCreated,
	@currentOccupation = @currentOccupation,
	[PhoneNumber] = @phoneNumber,
	[ShopLocation] = @shopLocation,
	[Gender] = @gender,
	[Status] = @status

	WHERE [Id] = @Id
END