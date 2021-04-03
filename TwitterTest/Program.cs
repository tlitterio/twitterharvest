using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> listOfTweets = new List<string>();

            TwitterService service = new TwitterService();
            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnPublicTimeline();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\temp\tweets.csv", append: true))
                foreach (var tweet in tweets)
                {
                    //file.WriteLine("{0},"+"'{1}'"+","+"{2}", tweet.User.ScreenName, tweet.Text, tweet.User.Language);
                    //listOfTweets.Add(tweet.User.Name);
                    listOfTweets.Add(tweet.Text);
                    //listOfTweets.Add(tweet.User.Language);
                }

        }
    }
}
