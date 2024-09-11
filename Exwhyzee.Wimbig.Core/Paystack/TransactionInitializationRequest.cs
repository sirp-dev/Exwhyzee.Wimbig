using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Paystack
{
    public class CustomField
    {
        public CustomField(string displayName, string variableName, string value)
        {
            DisplayName = displayName;
            VariableName = variableName;
            Value = value;
        }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("variable_name")]
        public string VariableName { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public static CustomField From(string displayName, string variableName, string value)
            => new CustomField(displayName, variableName, value);
    }

    public class PaystackRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("metadata")]
        public Metadata MetaData { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("referrer")]
        public string Referrer { get; set; }

        [JsonProperty("custom-fields")]
        public IList<CustomField> CustomFields { get; set; }
    }
}
