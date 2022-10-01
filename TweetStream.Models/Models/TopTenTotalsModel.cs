using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStream.Models.Models
{
    public class TopTenTotalsModel
    {
        public IEnumerable<string> TopTenHashTags { get; set; }
        public int TotalTweets { get; set; }
        public double TweetsPerSecond { get; set; }

        public TopTenTotalsModel()
        {
            TopTenHashTags = new List<string>();
        }
    }
}
