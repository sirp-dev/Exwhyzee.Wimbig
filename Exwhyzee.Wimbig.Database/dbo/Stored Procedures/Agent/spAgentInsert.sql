--Create Entity

CREATE PROCEDURE [dbo].[spAgentInsert]
		
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
	INSERT INTO [dbo].[Agent] ([Fullname],[EmailAddress],[State],[LGA],[AreYouNewToWimbig],[ContactAddress],[DateCreated],[CurrentOccupation],[PhoneNumber],[ShopLocation],[Gender],[Status])
	output inserted.Id

	VALUES (@fullname,@emailAddress,@state,@lga,@areYouNewToWimbig,@contactAddress,@dateCreated,@currentOccupation,@phoneNumber,@shopLocation,@gender,@status)
END
