using System.Web.Mvc;
using PusherServer;
using TweetSource.EventSource;
using TwitterRealtimeSearch.Web.App_Start;

namespace TwitterRealtimeSearch.Web.Controllers
{
    public class RealTimeTweetSearch
    {
        public string Search { get; set; }
    }
    /// <summary>
    /// <see cref="https://github.com/nkuln/tweetsource">tweet source manages oAuth needed to connect to Twitter's stream API</see>
    /// the library is event based so each new arrived tweet will trigger an event
    /// </summary>
    public class HomeController : Controller
    {
        private TweetEventSource _tweetEventSource;
        private readonly IPusher _pusher;

        private bool IsActiveAndHasTweetEvents
        {
            get { return _tweetEventSource.Active && _tweetEventSource.NumberOfEventInQueue > 0; }
        }

        public HomeController(TweetEventSource tweetEventSource, IPusher pusher)
        {
            _pusher = pusher;
            InitialiseAndConfigureTweetEventSource(tweetEventSource);
        }

        /// <summary>
        /// Poor man's IOC
        /// </summary>
        public HomeController()
            : this(TwitterConfig.TweetEventSource,
                   new Pusher(Config.Pusher.AppId,Config.Pusher.AppKey,Config.Pusher.AppSecret))
        { }

        //
        // GET: /HomeController/Index/searchTerm1,searchTerm2
        [HttpGet]
        public ActionResult Index(string search)
        {
            StartStreamingTweetsFor(search);
            return View(new RealTimeTweetSearch{ Search = search});
        }

        //
        // Post: /HomeController/Index/searchTerm1,searchTerm2
        [HttpPost]
        public JsonResult Index(RealTimeTweetSearch searchViewModel)
        {
            StartStreamingTweetsFor(searchViewModel.Search);
            return Json(string.Empty);
        }
        
        private void InitialiseAndConfigureTweetEventSource(TweetEventSource tweetEventSource)
        {
            _tweetEventSource = tweetEventSource;
            _tweetEventSource.EventReceived += OnTweetReceived;
            _tweetEventSource.SourceDown += OnSourceDown;
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
            while (IsActiveAndHasTweetEvents)
            {
                _tweetEventSource.Dispatch(5000);
            }
        }

        private void OnSourceDown(object sender, TweetEventArgs tweetEventArg)
        {
            var result = _pusher.Trigger(Config.Pusher.ChannelName, Config.Pusher.StreamErrorTweetEventName,new {message = tweetEventArg.InfoText});
        }

        private void OnTweetReceived(object sender, TweetEventArgs tweetEventArg)
        {
            _pusher.Trigger(Config.Pusher.ChannelName, Config.Pusher.StreamErrorTweetEventName, tweetEventArg.JsonText);
        }

        ~HomeController()
        {
            _tweetEventSource.Stop();
        }
    }
}
