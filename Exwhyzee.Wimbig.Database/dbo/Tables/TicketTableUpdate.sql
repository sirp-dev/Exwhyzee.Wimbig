ALTER TABLE [dbo].[Ticket]
  ADD CONSTRAINT uq_ticket UNIQUE([TicketNumber], [RaffleId]);