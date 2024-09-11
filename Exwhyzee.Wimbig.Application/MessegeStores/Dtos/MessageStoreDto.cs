using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.MessageStores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.MessageStores.Dto
{
    public class MessageStoreDto
    {
        public long Id { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public MessageStatus MessageStatus { get; set; }
        public MessageChannel MessageChannel { get; set; }
        public MessageType MessageType { get; set; }
        /// <summary>
        /// This values could be comma delimited
        /// </summary>
        public string Message { get; set; }
        public int Retries { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateSent { get; set; }
        public AddressType AddressType { get; set; }
        public string ImageUrl { get; set; }

        public string Response { get; set; }
    }
}
