using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStream.Models.Models
{
    public class TwitterQueueConfiguration
    {

        public string TweetCountKey { get; set; }
        public string HashTagQueueKey { get; set; }
        public string SessionTweetCount { get; set; }
        public string SessionStartKey { get; set; }

    }
}
