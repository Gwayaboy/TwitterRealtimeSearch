using System;
using TweetSource.EventSource;

namespace TwitterRealtimeSearch.Web.App_Start
{
    public class TwitterOAuth
    {
        private static readonly TweetEventSource TweetEventEventSource = TweetEventSource.CreateFilterStream();
        public static TweetEventSource EventSource { get { return TweetEventEventSource; }}

        public static void Configure()
        {
            var config = EventSource.AuthConfig;
            config.ConsumerKey = Config.Twitter.ConsumerKey;
            config.ConsumerSecret = Config.Twitter.ConsumerSecret;
            config.Token = Config.Twitter.Token;
            config.TokenSecret = Config.Twitter.TokenSecret;
        }

        
    }
}