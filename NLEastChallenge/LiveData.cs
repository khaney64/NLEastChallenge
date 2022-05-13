using Flurl;
using Flurl.Http;
using MlbApi.Live;

namespace NLEastChallenge
{
    public class LiveData
    {
        public int Inning { get; set; }

        public string Indicator { get; set; } = String.Empty;

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public static LiveData? GetLiveData(ILogger logger, int gamePk)
        {
            var liveUrl = $"https://statsapi.mlb.com/api/v1.1/game/{gamePk}/feed/live";

            logger.LogTrace($"fetch live game {liveUrl}");

            var gameData = liveUrl
                .GetJsonAsync<LiveRoot>()
                .Result;

            logger.LogTrace($"fetched live game {liveUrl}");

            var currentPlay = gameData.LiveData.Plays.CurrentPlay;

            return new LiveData()
            {
                Inning = currentPlay.About.Inning,
                Indicator = currentPlay.About.HalfInning,
                HomeScore = currentPlay.Result.HomeScore,
                AwayScore = currentPlay.Result.AwayScore
            };
        }

        private static string GetGameDateEst(DateTime gameDateUtc, ILogger logger)
        {
            try
            {
                var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var gameDateEst = TimeZoneInfo.ConvertTimeFromUtc(gameDateUtc, easternZone);
                return gameDateEst.ToString("h:mm tt");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unexpected error getting game date");
                return "Scheduled";
            }
        }
    }
}
