using System.Collections.Generic;
using Newtonsoft.Json;

namespace OvenTimerAPI 
{
    public class TimeSetViewModel
    {
        [JsonProperty(PropertyName = "defaults", Required = Required.Always)]
        public TimeSetDefaults Defaults { get; set; }
        
        [JsonProperty(PropertyName = "timesets", Required = Required.Always)]
        public List<TimeSet> TimeSets { get; set; } = new List<TimeSet>();
    }
}