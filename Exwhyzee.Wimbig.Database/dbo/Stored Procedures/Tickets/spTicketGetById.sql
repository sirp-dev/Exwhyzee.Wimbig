﻿-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spTicketGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[Ticket] WHERE [Id] = @id
END
