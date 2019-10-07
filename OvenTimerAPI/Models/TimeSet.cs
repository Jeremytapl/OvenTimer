using Newtonsoft.Json;

namespace OvenTimerAPI
{
    public class TimeSet
    {
        public enum TimeStatus { Stopped, Started };

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "hour", Required = Required.Always)]
        public string Hour { get; set; }

        [JsonProperty(PropertyName = "minute", Required = Required.Always)]
        public string Minute { get; set; }

        [JsonProperty(PropertyName = "second", Required = Required.Always)]
        public string Second { get; set; }

        [JsonProperty(PropertyName = "status")]
        public TimeStatus Status { get; set; } = TimeStatus.Stopped;
    }
}