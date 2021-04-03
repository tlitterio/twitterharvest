using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using TweetSharp;
using System.Xml.Linq;

namespace TwitterAPISearch
{
    class Program
    {
        static void Main(string[] args)
        {
            MySqlConnection mySql = new MySqlConnection();
            SparkSend mySparkSend = new SparkSend();
            mySparkSend.sparkconnect();
            int totalTweets = 0;
            TweetStream[] tweetStreams = new TweetStream[] {
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+alpha&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+bodemeister&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22creative%20cause%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22daddy%20long%20legs%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22daddy%20nose%20best%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22done%20talking%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+dullahan&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22el%20padrino%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+gemologist&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+hansen&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22i'll%20have%20another%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+liaison&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22mark%20valeski%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+optimizer&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+prospective&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22rousing%20sermon%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+sabercat&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22take%20charge%20indy%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+trinniberg&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22union%20rags%22&lang=en&rpp=100"),
                new TweetStream("http://search.twitter.com/search.atom?q=kentucky+derby+%22went%20the%20day%20well%22&lang=en&rpp=100"),
            };
            while (true)
            {
                mySql.CreateConn();
                
                int currentTweets = 0;
                bool newTweets = false;
                int currStream = 0;
                foreach (TweetStream stream in tweetStreams)
                {
                    try
                    {
                        stream.Refresh();
                        foreach (Tweet tweet in stream.Tweets)
                        {
                           
                            string sn = tweet.Author.TwitterId;
                            string tw = tweet.Title;
                            string se = tweet.Id;

                            DateTime pu = tweet.Published;

                            if (pu < DateTime.Now.AddMinutes(-157))
                            {
                                //Console.WriteLine("Ignoring Tweet from : " + sn + " at time: " + pu);
                                continue;
                            }

                            Console.WriteLine();
                            Console.WriteLine("Found Recent Tweet from stream: " + currStream + " Tweet: " + sn + " at time: " + pu);


                            string to = sn + ": " + tw + " - " + pu;
                            //string la = tweet.Lang;
                            //mySparkSend.sparksend(to, mySparkSend.getsparkusers());
                            mySql.InsertOneItem(sn, tw, pu, currStream, se);
                            Console.WriteLine("{0}: {1} - {2}",
                                tweet.Author.TwitterId,
                                tweet.Title,
                                tweet.Published);
                            newTweets = true;
                            totalTweets++;
                            currentTweets++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0} Exception caught.", ex);
                    }

                    currStream++;

                }
                if (newTweets)
                {
                    Console.WriteLine("Loaded {0} more tweets", currentTweets);
                    Console.WriteLine("Loaded {0} total tweets", totalTweets);
                }
                else
                {
                    Console.WriteLine("No new tweets.");
                }
                mySql.CloseConn();
                DateTime nextCheck = DateTime.Now.AddSeconds(120);
                Console.WriteLine("Will check again at {0}", nextCheck.ToLongTimeString());
                Thread.Sleep(TimeSpan.FromSeconds(120));
            }
        }
    }
}

