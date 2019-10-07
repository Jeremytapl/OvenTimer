using System.Collections.Generic;
using Newtonsoft.Json;

namespace OvenTimerAPI 
{
    public class TimeSetDefaults
    {   
        [JsonProperty(PropertyName = "id")]     
        public string Id { get; set; } = "default";

        [JsonProperty(PropertyName = "hour", Required = Required.Always)]
        public string Hour { get; set; }

        [JsonProperty(PropertyName = "minute", Required = Required.Always)]
        public string Minute { get; set; }

        [JsonProperty(PropertyName = "second", Required = Required.Always)]
        public string Second { get; set; }

        [JsonProperty(PropertyName = "timers", Required = Required.Always)]
        public int Timers { get;set; } = 10;
    }
}