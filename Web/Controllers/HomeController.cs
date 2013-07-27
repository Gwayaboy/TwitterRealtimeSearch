using System;
using System.Web.Mvc;
using PusherServer;
using TweetSource.EventSource;
using TwitterRealtimeSearch.Web.App_Start;

namespace TwitterRealtimeSearch.Web.Controllers
{
    /// <summary>
    /// 
    /// <see cref="https://github.com/nkuln/tweetsource">tweet source manages oAuth needed to connect to Twitter's stream API</see>
    /// the library is event based so each new arrived tweet will trigger an event
    /// </summary>
    public class HomeController : Controller
    {
        private TweetEventSource _tweetEventSource;
        private readonly IPusher _pusher;

        public HomeController(TweetEventSource tweetEventSource, IPusher pusher)
        {
            _pusher = pusher;
            InitialiseTweetEventSource(tweetEventSource);
        }

        /// <summary>
        /// Poor man's IOC
        /// </summary>
        public HomeController() : this(TwitterOAuth.EventSource,
                                       new Pusher(Config.Pusher.AppId,
                                                  Config.Pusher.AppKey,
                                                  Config.Pusher.AppSecret))
        { }

        //
        // GET: /HomeController/
        public ActionResult Index(string search)
        {
            StartStreamingTweetsFor(search);
            return View();
        }


        private void InitialiseTweetEventSource(TweetEventSource tweetEventSource)
        {
            _tweetEventSource = tweetEventSource;
            _tweetEventSource.EventReceived += OnTweetReceived;
            _tweetEventSource.SourceDown += OnSourceDown;
            _tweetEventSource.Dispatch();
        }

        private void StartStreamingTweetsFor(string search)
        {
            _tweetEventSource.Stop();

            StreamingAPIParameters streamingApiParameters = null;
            if (!string.IsNullOrWhiteSpace(search))
            {
                streamingApiParameters = new StreamingAPIParameters {Track = search.Split(',')};
            }
            _tweetEventSource.Start(streamingApiParameters);
        }

        private void OnSourceDown(object sender, TweetEventArgs e)
        {
        }

        private void OnTweetReceived(object sender, TweetEventArgs tweetEventArg)
        {
            _pusher.Trigger(Config.Pusher.ChannelName, Config.Pusher.EventName, tweetEventArg.JsonText);
        }
     
    }
}
