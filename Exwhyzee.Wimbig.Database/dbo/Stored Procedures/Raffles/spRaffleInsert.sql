-- Create a new Raffle

CREATE PROCEDURE [dbo].[spRaffleInsert]
	@name nvarchar(250),
	@description nvarchar(Max),
	@hostedBy nvarchar(250),
	@numberOfTickets int,
	@pricePerTicket decimal,
	@deliveryType int,
	@startDate dateTime,
	@endDate dateTime,
	@status int,
	@dateCreated datetime,
	@totalSold int = 0,
	@sortOrder int,
	@location nvarchar(500),
	@areaInCity nvarchar(500)

AS
Begin
	  INSERT INTO[dbo].[Raffle] (
	  [Name],[Description],[HostedBy],
	  [NumberOfTickets],[PricePerTicket],
	  [DeliveryType],[StartDate],
	  [EndDate],[Status],[DateCreated],[TotalSold], [SortOrder],[Location],[AreaInCity])
                                    
									output inserted.Id 
                                    VALUES 
	(@name, @description, @hostedBy,
	@numberOfTickets, @pricePerTicket,
	@deliveryType, @startDate,
	@endDate, @status, @dateCreated, @totalSold, @sortOrder, @location, @areaInCity) 
End
