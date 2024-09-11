using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Raffles.Dto
{
    public class CreateRaffleDto
    {
        public CreateRaffleDto()
        {
            Status = EntityStatus.Active;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Number of Tickets")]
        public int NumberOfTickets { get; set; }

        [Required]
        [Display(Name = "Price Per Ticket")]
        public decimal PricePerTicket { get; set; }

        [Required]
        [Display(Name = "Hosted By")]
        public string HostedBy { get; set; }

        [Required]
        [Display(Name = "Delivery Type")]
        public DeliveryTypeEnum DeliveryType { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public EntityStatus Status { get; set; }

        [Display(Name="Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);

        public long? CategoryId { get; set; }

        [Display(Name="Sort Order")]
        public int SortOrder { get; set; }

        public string Location { get; set; }

        public string AreaInCity { get; set; }

    }
}
