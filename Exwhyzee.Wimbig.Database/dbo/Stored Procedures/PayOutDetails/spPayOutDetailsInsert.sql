--Create Category Entity

CREATE PROCEDURE [dbo].[spPayOutDetailsInsert]
		
		@percentage int,		
		@accountNumber nvarchar(max),
		@accountName nvarchar(max),
		@bankName nvarchar(max),
		@userId nvarchar(250)
		

		
AS
BEGIN
	INSERT INTO [dbo].[PayOutDetails] ([Percentage],[AccountNumber],[AccountName],[BankName],[UserId])
	output inserted.Id

	VALUES (@percentage,@accountNumber,@accountName,@bankName,@userId)
END
