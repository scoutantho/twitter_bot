using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TweetSharp;

namespace Test_twitter
{
    class Program
    {

        private static string consumer_key = "0I3mtD9kuwvglAFZTQ1sgdCsg";
        private static string consumer_key_secret = "pZtHkjD5j7DdJaPJpBU7P0CIirFnupcWKqSxVTB3M91iviKm2l";
        private static string access_token = "976944504-sEFnvI7Jzvb9JWrrGzxGLbxHrduscmD3cUDxHECg";
        private static string access_token_secret = "bEDBY7U9AFIcaZSBRbR4aPZVH4ojruIHNBSSBwRM05CXL";
        private static string ScreenName = "LaPetiteLisa_";
        private static List<TwitterStatus> tweetDay = new List<TwitterStatus>();

        private static TwitterService service = new TwitterService(consumer_key, consumer_key_secret, access_token, access_token_secret);
       

        static void Main(string[] args)
        {
            Console.WriteLine($"<{DateTime.Now}> - Bot Started");
            GetTweet();
           
            GetTweetAfter(DateTime.Today);

            getLatestTweet();

            Console.ReadKey();
        }

        private static void SendReply(string _status, long _statusID)
        {
            service.SendTweet(new SendTweetOptions { Status = _status+" #tweetsharpScoutantho", InReplyToStatusId = _statusID }, (tweet, response) =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"<{DateTime.Now}> - reply sent !");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"<ERROR> "+response.Error.Message);
                        Console.ResetColor();
                    }
                });
        }
        private static void SendTweet(string _status)
        {
            service.SendTweet(new SendTweetOptions { Status = _status + " #tweetsharpScoutantho" }, (tweet, response) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"<{DateTime.Now}> - tweet sent !");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"<ERROR> " + response.Error.Message);
                    Console.ResetColor();
                }
            });
        }
        private static void SendTweetAt(string _status, string screenName)
        {
            string text = "@" + screenName + " ";
            service.SendTweet(new SendTweetOptions { Status = text+_status + " #tweetsharpScoutantho" }, (tweet, response) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"<{DateTime.Now}> - tweet sent !");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"<ERROR> " + response.Error.Message);
                    Console.ResetColor();
                }
            });
        }

        //get a ienumerable of all tweets
        private static IEnumerable<TwitterStatus> GetTweet() 
        {
            var currentTweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions
            {
                ScreenName = "LaPetiteLisa_",
                Count = 1000,
                ExcludeReplies = true,
                IncludeRts = false,
                
            });
            
            return currentTweets;
        }
       private static void GetTweetAfter(DateTime date)
        {
            
            foreach(var tweet in GetTweet())
            {
                if (tweet.CreatedDate.Day == date.Day) //get onlly tweet after one date
                { tweetDay.Add(tweet); }
            }
        }
        //get nb of tweet for a particular day 
        private static int GetNbOfTweet(DateTime day)
        {
            int count = 0;
            foreach(var tweet in GetTweet())
            {
                if (tweet.CreatedDate.Day == day.Day) { count++; }
            }
            return count;
        }

        private static void getLatestTweet()
        {
            Console.WriteLine( tweetDay.First().Text); //get latest tweet
        }



    }
}
