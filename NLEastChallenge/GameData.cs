﻿using Flurl;
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
            var gamesUrl = "http://statsapi.mlb.com/api/v1/schedule/games";

            logger.LogTrace($"fetch standings {gamesUrl} sportId 1");

            var todaysGames = gamesUrl
                .SetQueryParam("sportId", "1")
                .SetQueryParam("date", GetScheduleDate(logger))
                .GetJsonAsync<GameRoot>()
                .Result;

            logger.LogTrace($"fetched standings {gamesUrl} sportId 1");

            var latestDate = todaysGames.Dates.FirstOrDefault();
            if (latestDate is null || latestDate.TotalGames == 0)
                return null;

            var nlEastGames = latestDate.Games.Where(HasNlEastTeam);

            return nlEastGames.Select(game => new GameData()
                {
                    HomeTeam = game.Teams.Home.Team.Name,
                    AwayTeam = game.Teams.Away.Team.Name,
                    GameTime = GetGameDateEst(game.GameDate, logger),
                    HomeScore = game.Teams.Home.Score,
                    AwayScore = game.Teams.Away.Score,
                    Status = game.Status.DetailedState
                })
                .ToList();
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