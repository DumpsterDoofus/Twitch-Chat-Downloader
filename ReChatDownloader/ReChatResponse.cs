using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReChatDownloader
{

    public class ReChatResponse
    {
        public Datum[] data { get; set; }
        public Meta meta { get; set; }
    }

    public class Meta
    {
        public object next { get; set; }
    }

    public class Datum
    {
        public string type { get; set; }
        public string id { get; set; }
        public Attributes attributes { get; set; }
        public Links links { get; set; }
    }

    public class Attributes
    {
        public string command { get; set; }
        public string room { get; set; }
        public long timestamp { get; set; }
        public bool deleted { get; set; }
        public string message { get; set; }
        public string from { get; set; }
        public Tags tags { get; set; }
        public string color { get; set; }
    }

    public class Tags
    {
        public object badges { get; set; }
        public string color { get; set; }
        [JsonProperty(PropertyName = "display-name")]
        public string displayname { get; set; }
        // TODO: Figure out how to make emotes deserialization not stupid
        //public object emotes { get; set; }
        public string id { get; set; }
        public bool mod { get; set; }
        [JsonProperty(PropertyName = "room-id")]
        public string roomid { get; set; }
        public bool subscriber { get; set; }
        public bool turbo { get; set; }
        [JsonProperty(PropertyName = "user-id")]
        public string userid { get; set; }
        [JsonProperty(PropertyName = "user-type")]
        public object usertype { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }

}
