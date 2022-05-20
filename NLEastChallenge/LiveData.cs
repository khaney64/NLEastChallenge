using Flurl;
using Flurl.Http;
using MlbApi.Live;

namespace NLEastChallenge
{
    public class LiveData
    {
        public string Inning { get; set; } = String.Empty;

        public int Outs { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public static LiveData? GetLiveData(ILogger logger, int gamePk)
        {
            var liveUrl = $"https://statsapi.mlb.com/api/v1.1/game/{gamePk}/feed/live";

            logger.LogTrace($"fetch live game {liveUrl}");
            try
            {
                var gameData = liveUrl
                    .GetJsonAsync<LiveRoot>()
                    .Result;

                var currentPlay = gameData.LiveData.Plays.CurrentPlay;

                var inningsAndOuts = GetInningAndOuts(currentPlay.About, currentPlay.Count);
                return new LiveData()
                {
                    Inning = inningsAndOuts.inning,
                    Outs = inningsAndOuts.outs,
                    HomeScore = currentPlay.Result.HomeScore,
                    AwayScore = currentPlay.Result.AwayScore
                };
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error fetch game {gamePk}");
                throw;
            }
        }

        private static (string inning, int outs) GetInningAndOuts(About about, Count count)
        {
            var inning = about.HalfInning == "Top" ? "top" : "bot";
            var outs = count.Outs;

            if (count.Outs == 3)
            {
                inning = inning == "top" ? "mid" : "end";
                outs = 0;
            }

            return ($"{inning} {about.Inning}", outs);
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
