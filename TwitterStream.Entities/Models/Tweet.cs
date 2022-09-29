using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStream.Entities.Models
{
    public class Tweet
    {
        public Data data { get; set; }
        public Includes includes { get; set; }
    }

    public class Data
    {
        public string author_id { get; set; }
        public DateTime created_at { get; set; }
        public Entities entities { get; set; }
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Entities
    {
        public Hashtag[] hashtags { get; set; }
        public Mention[] mentions { get; set; }
        public Url[] urls { get; set; }
    }

    public class Hashtag
    {
        public int start { get; set; }
        public int end { get; set; }
        public string tag { get; set; }
    }

    public class Mention
    {
        public int start { get; set; }
        public int end { get; set; }
        public string username { get; set; }
        public string id { get; set; }
    }

    public class Url
    {
        public int start { get; set; }
        public int end { get; set; }
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public string media_key { get; set; }
    }

    public class Includes
    {
        public List<User> users { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
    }

}
