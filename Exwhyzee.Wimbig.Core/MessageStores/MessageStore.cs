using System;
using System.ComponentModel;

namespace Exwhyzee.Wimbig.Core.MessageStores
{
    public class MessageStore
    {
        public MessageStore()
        {
            MessageStatus = MessageStatus.Not_Sent;
            DateCreated = DateTime.UtcNow.AddHours(1);
            AddressType = AddressType.Single;
            EmailAddress = null;
            PhoneNumber = null;
            DateSent = null;
        }
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


        public override string ToString()
        {
            if (string.IsNullOrEmpty(EmailAddress) && !string.IsNullOrEmpty(PhoneNumber))
            {
                return PhoneNumber;
            }
            else if (!string.IsNullOrEmpty(EmailAddress) && string.IsNullOrEmpty(PhoneNumber))
            {
                return EmailAddress;
            }
            else
            {
                throw new NotSupportedException("Address");
            }
        }
    }

    public enum MessageChannel
    {
        [Description("Email")]
        Email = 10,
        [Description("SMS")]
        SMS = 20
    }

    public enum MessageType
    {
        [Description("Acount Activation")]
        Activation = 10,
        [Description("Game Won")]
        Game_Won = 20,
        [Description("Game Played")]
        Game_Played = 30,
        [Description("Password Reset")]
        Password_Reset = 40
    }

    public enum MessageStatus
    {
        [Description("Sent")]
        Sent = 10,
        [Description("Not_Sent")]
        Not_Sent = 20
    }

    public enum AddressType
    {
        [Description("Single")]
        Single = 10,
        [Description("Bulk")]
        Bulk = 20
    }
}
