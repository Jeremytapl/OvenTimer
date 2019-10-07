using System.Collections.Generic;
using Newtonsoft.Json;

namespace OvenTimerAPI 
{
    public class TimeSetDefaults
    {   
        [JsonProperty(PropertyName = "id")]     
        public string Id { get; set; } = "default";

        [JsonProperty(PropertyName = "defaultset")]
        public TimeSet DefaultTimeSet { get; set;}

        [JsonProperty(PropertyName = "timers", Required = Required.Always)]
        public int Timers { get;set; } = 10;
    }
}