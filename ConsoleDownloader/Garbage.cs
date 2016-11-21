using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDownloader
{
    class Garbage
    {
        public int _total { get; set; }
        public _Links _links { get; set; }
        public Video[] videos { get; set; }
    }

    public class _Links
    {
        public string self { get; set; }
        public string next { get; set; }
    }

    public class Video
    {
        public string title { get; set; }
        public object description { get; set; }
        public object description_html { get; set; }
        public long broadcast_id { get; set; }
        public string broadcast_type { get; set; }
        public string status { get; set; }
        public string language { get; set; }
        public string tag_list { get; set; }
        public int views { get; set; }
        public DateTime created_at { get; set; }
        public string url { get; set; }
        public DateTime published_at { get; set; }
        public string _id { get; set; }
        public DateTime recorded_at { get; set; }
        public string game { get; set; }
        public int length { get; set; }
        public string preview { get; set; }
        public string animated_preview { get; set; }
        public Thumbnail[] thumbnails { get; set; }
        public Fps fps { get; set; }
        public Resolutions resolutions { get; set; }
        public _Links1 _links { get; set; }
        public Channel channel { get; set; }
    }

    public class Fps
    {
        public int audio_only { get; set; }
        public float chunked { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float medium { get; set; }
        public float mobile { get; set; }
    }

    public class Resolutions
    {
        public string chunked { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string medium { get; set; }
        public string mobile { get; set; }
    }

    public class _Links1
    {
        public string self { get; set; }
        public string channel { get; set; }
    }

    public class Channel
    {
        public string name { get; set; }
        public string display_name { get; set; }
    }

    public class Thumbnail
    {
        public string url { get; set; }
        public string type { get; set; }
    }

}
