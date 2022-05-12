using Flurl;
using Flurl.Http;
using MlbApi;

namespace NLEastChallenge
{
    public class GameData
    {
        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public string GameTime { get; set; }

        public string Status { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public static List<GameData>? FetchTodaysGames(ILogger logger)
        {
            var games = "http://statsapi.mlb.com/api/v1/schedule/games";
            var todaysGames = games
                .SetQueryParam("sportId", "1")
                //.SetQueryParam("date", GetGameDate())
                .GetJsonAsync<GameRoot>()
                .Result;

            logger.LogTrace($"fetch standings {games} sportId 1");

            var latestDate = todaysGames.Dates.FirstOrDefault();
            if (latestDate is null || latestDate.TotalGames == 0)
                return null;

            var nlEastGames = latestDate.Games.Where(HasNlEastTeam);

            return nlEastGames.Select(game => new GameData()
                {
                    HomeTeam = game.Teams.Home.Team.Name,
                    AwayTeam = game.Teams.Away.Team.Name,
                    GameTime = GetGameDateEst(game.GameDate),
                    HomeScore = game.Teams.Home.Score,
                    AwayScore = game.Teams.Away.Score,
                    Status = game.Status.DetailedState
                })
                .ToList();
        }

        private static string GetGameDateEst(DateTime gameDateUtc)
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var gameDateEst = TimeZoneInfo.ConvertTimeFromUtc(gameDateUtc, easternZone);
            return gameDateEst.ToString("h:mm tt");
        }

        private static object GetGameDate()
        {
            // need to deal with Z time, and if after midnight (EST) return previous day
            return DateTime.Now.ToString("d");
        }

        private static bool HasNlEastTeam(Game game)
        {
            return TeamData.NameToNickname(game.Teams.Home.Team.Name) != "???" ||
                   TeamData.NameToNickname(game.Teams.Away.Team.Name) != "???";
        }
    }
}
