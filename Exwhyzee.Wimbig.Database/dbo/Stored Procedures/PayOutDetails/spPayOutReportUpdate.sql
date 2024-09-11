-- Update Category entity

CREATE PROCEDURE [dbo].[spPayOutDetailsUpdate]
	@Id bigint,
		@percentage int,		
		@accountNumber nvarchar(max),
		@accountName nvarchar(max),
		@bankName nvarchar(max),
		@userId nvarchar(250)
AS
BEGIN
	UPDATE [dbo].[PayOutDetails] SET
	[Percentage] = @percentage,
	[AccountNumber] = @accountNumber,
	[AccountName] = @accountName,
	[BankName] = @bankName,
	[UserId] = @userId

	WHERE [Id] = @Id
END