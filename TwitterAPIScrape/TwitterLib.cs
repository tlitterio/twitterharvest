using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TweetSharp;
using System.Xml.Linq;

namespace TwitterAPISearch
{
    public class Tweet
    {
        public string Id { get; set; }
        public DateTime Published { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Lang { get; set; }
        public Author Author { get; set; }
    }
    public class Author
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public string TwitterId
        {
            get
            {
                return s_idParser.Match(Name).Groups["twitterid"].Value;
            }
        }
        private static Regex s_idParser =
            new Regex(@"^(?<twitterid>.*)\s+\((?<displayname>.*)\)");
    }
    public class TweetStream
    {
        private string m_refreshUri;
        List<Tweet> m_tweets;
        public TweetStream(string queryUri)
        {
            m_refreshUri = queryUri;
            m_tweets = new List<Tweet>();
        }
        public List<Tweet> Tweets
        {
            get
            {
                return m_tweets;
            }
        }
        
        public void Refresh()
        {
            XDocument feed = XDocument.Load(m_refreshUri);
            XNamespace atomNS= "http://www.w3.org/2005/Atom";
            m_tweets = (from tweet in feed.Descendants(atomNS + "entry")
                        select new Tweet
                        {
                            Title = (string)tweet.Element(atomNS + "title"),
                            Published = DateTime.Parse((string)tweet.Element(atomNS + "published")),
                            Id = (string)tweet.Element(atomNS + "id"),
                            Lang = (string)tweet.Element(atomNS + "lang"),
                            Link = tweet.Elements(atomNS + "link")
                                .Where(link => (string)link.Attribute("rel") == "alternate")
                                .Select(link => (string)link.Attribute("href"))
                                .First(),
                            Author = (from author in tweet.Descendants(atomNS + "author")
                                    select new Author
                                    {
                                        Name = (string)author.Element(atomNS + "name"),
                                        Uri = (string)author.Element(atomNS + "uri"),
                                    }).First(),
                        }).ToList<Tweet>();
 
            m_refreshUri = feed.Descendants(atomNS + "link")
                .Where(link => link.Attribute("rel").Value == "refresh")
                .Select(link => link.Attribute("href").Value)
                .First();

        }
    }
}
