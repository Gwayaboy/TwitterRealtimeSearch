using System;
using TweetSource.EventSource;

namespace TwitterRealtimeSearch.Web.App_Start
{
    public class TwitterConfig
    {
        private static readonly Lazy<TweetEventSource> LazyTweetEventSource =
            new Lazy<TweetEventSource>(CreateFilterStream);
        
        public static TweetEventSource TweetEventSource
        {
            get { return LazyTweetEventSource.Value; }
        }

        public static void RegisterOAuth()  
        {
            var config = TweetEventSource.AuthConfig;
            config.ConsumerKey = Config.Twitter.ConsumerKey;
            config.ConsumerSecret = Config.Twitter.ConsumerSecret;
            config.Token = Config.Twitter.Token;
            config.TokenSecret = Config.Twitter.TokenSecret;   
        }

        private static TweetEventSource CreateFilterStream()
        {
            return TweetEventSource.CreateFilterStream();
        }
    }
}