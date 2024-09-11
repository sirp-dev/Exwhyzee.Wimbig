CREATE PROCEDURE [dbo].[spRaffleUpdate]
	@id bigint,
	@name nvarchar(250),	
	@hostedBy nvarchar(250),
	@description nvarchar(Max),
	@numberOfTickets int,
	@pricePerTicket decimal ,
	@deliveryType int,
	@startDate dateTime ,
	@endDate dateTime ,
	@status int,
	@dateCreated datetime,
	@totalSold int,
	@sortOrder int,
	@paidOut bit,
	@archieved bit,
	@location nvarchar(500),
	@areaInCity nvarchar(500)
	
AS
BEGIN

	UPDATE[dbo].[Raffle] SET [Name] =@name,	
	[HostedBy] = @hostedBy,
	[Description] = @description,
	[NumberOfTickets] = @numberOfTickets,
	[PricePerTicket] = @pricePerTicket,
	[DeliveryType] = @deliveryType,
	[StartDate] = @startDate,
	[EndDate] = @endDate,
	[Status] = @status,
	[DateCreated] = @dateCreated,
	[TotalSold] = @totalSold,
	[SortOrder] = @sortOrder,
	[PaidOut] = @paidOut,
	[Archived] = @archieved,
	[Location] = @location,
	[AreaInCity] = @areaInCity
                 
Where Id = @id
End
