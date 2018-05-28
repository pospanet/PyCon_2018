using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ImageGenerator
{
    public class Drawing
    {
        [JsonProperty("word")]
        [DataMember(Name = "word")]
        public string Word { get; set; }

        [JsonProperty("countrycode")]
        [DataMember(Name = "countrycode")]
        public string Countrycode { get; set; }

        [JsonProperty("timestamp")]
        [DataMember(Name = "timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("recognized")]
        [DataMember(Name = "recognized")]
        public bool Recognized { get; set; }

        [JsonProperty("key_id")]
        [DataMember(Name = "key_id")]
        public string KeyId { get; set; }

        [JsonProperty("drawing")]
        [DataMember(Name = "drawing")]
        public IEnumerable<int[][]> Lines { get; set; }
    }
}
