using System.Configuration;

namespace TwitterRealtimeSearch.Web
{
    public static class Config
    {
        public static class Twitter
        {
            public static string ConsumerKey
            {
                get { return GetSettings("ConsumerKey   "); }
            }

            public static string ConsumerSecret
            {
                get { return GetSettings("ConsumerSecret"); }
            }

            public static string Token
            {
                get { return GetSettings("Token"); }
            }

            public static string TokenSecret
            {
                get { return GetSettings("TokenSecret"); }
            }
        }

        public static class Pusher
        {
            public static string AppId
            {
                get { return GetSettings("Pusher_AppId"); }
            }

            public static string AppKey
            {
                get { return GetSettings("Pusher_AppKey"); }
            }

            public static string AppSecret
            {
                get { return GetSettings("Pusher_AppSecret"); }
            }

            public static string ChannelName
            {
                get { return GetSettings("Pusher_ChannelName"); }
            }

            public static string EventName
            {
                get { return GetSettings("Pusher_EventName"); }
            }
        }
        

        private static string GetSettings(string key, string defaultValue = "")
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                value = defaultValue;
            }
            return value;
        }
    }
}