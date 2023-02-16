using Flurl;
using Flurl.Http;
using MlbApi;

namespace NLEastChallenge
{
    public class GameData
    {
        public string HomeTeam { get; set; } = String.Empty;

        public string AwayTeam { get; set; } = String.Empty;

        public string GameTime { get; set; } = String.Empty;

        public string Status { get; set; } = String.Empty;

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public string Inning { get; set; } = String.Empty;

        public int Outs { get; set; }

        public static List<GameData>? FetchTodaysGames(ILogger logger)
        {
            var gamesUrl = "http://statsapi.mlb.com/api/v1/schedule/games";

            logger.LogTrace($"fetch todays games {gamesUrl} sportId 1");

            try
            {
                var todaysGames = gamesUrl
                    .SetQueryParam("sportId", "1")
                    .SetQueryParam("date", GetScheduleDate(logger))
                    .GetJsonAsync<GameRoot>()
                    .Result;

                var latestDate = todaysGames.Dates.FirstOrDefault();
                if (latestDate is null || latestDate.TotalGames == 0)
                    return null;

                var nlEastGames = latestDate.Games.Where(HasNlEastTeam);

                return nlEastGames.Select(game =>
                    {
                        var inningAndOuts = InningAndOuts(game, logger);
                        return new GameData()
                        {
                            HomeTeam = game.Teams.Home.Team.Name,
                            AwayTeam = game.Teams.Away.Team.Name,
                            GameTime = GetGameDateEst(game.GameDate, logger),
                            HomeScore = game.Teams.Home.Score,
                            AwayScore = game.Teams.Away.Score,
                            Status = game.Status.DetailedState,
                            Inning = inningAndOuts.Inning,
                            Outs = inningAndOuts.Outs
                        };
                    })
                    .ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error getting GameData");
                throw;
            }
        }

        public static List<GameData>? FetchGamesDateRanges(ILogger logger, DateTime startDate, DateTime endDate)
        {
            var gamesUrl = "http://statsapi.mlb.com/api/v1/schedule/games";

            logger.LogTrace($"fetch upcoming games {startDate:MM/dd/yyyy} to {endDate:MM/dd/yyyy} {gamesUrl} sportId 1");

            try
            {
                var upcomingGames = gamesUrl
                    .SetQueryParam("sportId", "1")
                    .SetQueryParam("startDate", $"{startDate:MM/dd/yyyy}")
                    .SetQueryParam("endDate", $"{endDate:MM/dd/yyyy}")
                    .GetJsonAsync<GameRoot>()
                    .Result;

                var nlEastGames = GetLatestGameForEachNLEastTeam(logger, upcomingGames);

                return nlEastGames.Select(game =>
                    {
                        return new GameData()
                        {
                            HomeTeam = game.Teams.Home.Team.Name,
                            AwayTeam = game.Teams.Away.Team.Name,
                            GameTime = GetGameDateEst(game.GameDate, logger),
                            HomeScore = game.Teams.Home.Score,
                            AwayScore = game.Teams.Away.Score,
                            Status = game.Status.DetailedState,
                            Inning = "",
                            Outs = 0
                        };
                    })
                    .ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error getting GameData");
                throw;
            }
        }

        private static IEnumerable<Game> GetLatestGameForEachNLEastTeam(ILogger logger, GameRoot upcoming)
        {
            var teamGames = new Dictionary<string, Game?>()
            {
                { "Atlanta Braves", null },
                { "Miami Marlins", null },
                { "New York Mets", null },
                { "Philadelphia Phillies", null },
                { "Washington Nationals", null },
            };

            foreach (var date in upcoming.Dates)
            {
                var eastGamesOnDate = date.Games.Where(HasNlEastTeam);
                foreach (var eastGame in eastGamesOnDate)
                {
                    var gameDate = eastGame.GameDate;
                    foreach (var team in teamGames.Keys)
                    {
                        var teamNext = teamGames[team];
                        var home = eastGame.Teams.Home.Team;
                        var away = eastGame.Teams.Away.Team;
                        if (team == home.Name || team == away.Name)
                        {
                            if (teamNext is null || (gameDate.Date <=teamNext.GameDate.Date && gameDate.TimeOfDay < teamNext.GameDate.TimeOfDay))
                            {
                                var nextDateTime = gameDate.Date + gameDate.TimeOfDay;
                                var gameInfo = $" {nextDateTime:MM/dd/yyyy hh:mm} {away.Name} at {home.Name}";
                                if (teamNext is null)
                                    logger.LogTrace($"Adding for {team} - {gameInfo}");
                                else
                                    logger.LogTrace($"Updating for {team} - {gameInfo}");
                                teamGames[team] = eastGame;
                            }
                        }
                    }
                }
            }

            return teamGames.Values.Where(g => g is not null);
        }

        private static (string Inning, int Outs) InningAndOuts(Game game, ILogger logger)
        {
            if (game.Status.DetailedState != "In Progress")
                return ("",0);

            var liveData = LiveData.GetLiveData(logger, game.GamePk);

            if (liveData is null)
                return ("",0);
            
            return (liveData.Inning, liveData.Outs);
        }

        private static string GetGameDateEst(DateTime gameDateUtc, ILogger logger)
        {
            try
            {
                var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var gameDateEst = TimeZoneInfo.ConvertTimeFromUtc(gameDateUtc, easternZone);
                var estNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);
                if (gameDateEst.Date > estNow.Date)
                    return gameDateEst.ToString("M/d/yyyy h:mm tt");
                else
                    return gameDateEst.ToString("h:mm tt");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unexpected error getting game date");
                return "Scheduled";
            }
        }

        private static string GetScheduleDate(ILogger logger)
        {
            // need to deal with Z time, and if after midnight (EST) return previous day

            try
            {
                var utcNow = DateTime.UtcNow;
                var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var estNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, easternZone);
                var actual = estNow;
                if (estNow.Hour is >= 0 and <= 3)
                    actual = actual.AddDays(-1);

                return actual.ToString("MM/dd/yyy");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unexpected error getting schedule date");
                return "Scheduled";
            }
        }

        private static bool HasNlEastTeam(Game game)
        {
            return TeamData.NameToNickname(game.Teams.Home.Team.Name) != "???" ||
                   TeamData.NameToNickname(game.Teams.Away.Team.Name) != "???";
        }
    }
}
