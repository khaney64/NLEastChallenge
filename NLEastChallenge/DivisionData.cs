using MlbApi;
using Flurl;
using Flurl.Http;

namespace NLEastChallenge;

public enum ScoringMode
{
    Normal,
    Horseshoes
}

public class DivisionData
{
    const int StatColumns = 2;

    public string Name { get; set; } = "";

    public TeamData[]? Teams { get; set; }

    internal static DivisionDataVm GetData(DivisionData[] configuredData, ILogger logger, ScoringMode scoringMode = ScoringMode.Normal)
    {
        var actual = FetchActual(logger);
        if (actual is null)
            return new DivisionDataVm();

        return GetData(configuredData, actual, scoringMode);
    }

    public static DivisionDataVm GetData(DivisionData[] configuredData, DivisionData actual, ScoringMode scoringMode = ScoringMode.Normal)
    {
        if (actual.Teams is null)
            return new DivisionDataVm();

        var players = CloneConfiguredData(configuredData);
        var divisionData = new DivisionData[players.Length + 1];

        divisionData[0] = actual;

        foreach (var player in players)
        {
            ScorePlayer(player, actual, scoringMode);
        }

        var groups = players.ToList()
	        .GroupBy(c => c.Teams!.Sum(t => t.Value));

		ResolveAllTies(groups, actual);

        // sort the configured data by total and team values
        var sorted = players.ToList()
            .OrderByDescending(c => c.Teams!.Sum(t => t.Value))
            .ThenBy(c => c.Teams![0].TieBreak)
            .ThenByDescending(c => c.Teams![0].Value)
            .ThenByDescending(c => c.Teams![1].Value)
            .ThenByDescending(c => c.Teams![2].Value)
            .ThenByDescending(c => c.Teams![3].Value)
            .ThenByDescending(c => c.Teams![4].Value)
            .ToList();

        for (var i = 0; i < sorted.Count; i++)
        {
            divisionData[i + 1] = sorted[i];
        };

        var result = new DivisionDataVm()
        {
            Headers = BuildHeaders(divisionData),
            Rows = BuildRows(divisionData),
            Footers = BuildFooter(divisionData)
        };

        return result;
    }

    private static DivisionData[] CloneConfiguredData(DivisionData[] configuredData)
    {
        return configuredData
            .Select(data => new DivisionData
            {
                Name = data.Name,
                Teams = data.Teams?
                    .Select(team => new TeamData
                    {
                        Team = team.Team,
                        WinsGuess = team.WinsGuess,
                        Value = team.Value,
                        RankDistanceValue = team.RankDistanceValue,
                        PairwiseBonusValue = team.PairwiseBonusValue,
                        Record = team.Record,
                        Streak = team.Streak,
                        Wins = team.Wins,
                        Losses = team.Losses
                    })
                    .ToArray()
            })
            .ToArray();
    }

    private static void ScorePlayer(DivisionData player, DivisionData actual, ScoringMode scoringMode)
    {
        if (scoringMode == ScoringMode.Horseshoes)
            ScoreHorseshoes(player, actual);
        else
            ScoreNormal(player, actual);
    }

    private static void ScoreNormal(DivisionData player, DivisionData actual)
    {
        var actualTeams = actual.Teams;
        var playerTeams = player.Teams;
        if (actualTeams is null || playerTeams is null)
            return;

        for (var i = 0; i < actualTeams.Length; i++)
        {
            if (actualTeams[i].Team == playerTeams[i].Team && actualTeams[i].Record != "0-0")
            {
                playerTeams[i].Value = 5 - i;
            }
        }
    }

    private static void ScoreHorseshoes(DivisionData player, DivisionData actual)
    {
        var actualTeams = actual.Teams;
        var playerTeams = player.Teams;
        if (actualTeams is null || playerTeams is null || actualTeams.All(t => t.Record == "0-0"))
            return;

        var predictedRanks = BuildPredictedRankMap(playerTeams);

        for (var actualRank = 0; actualRank < actualTeams.Length; actualRank++)
        {
            var actualTeam = actualTeams[actualRank];
            if (actualTeam.Team is null || !predictedRanks.TryGetValue(actualTeam.Team, out var predictedRank))
                continue;

            var slotValue = 5 - actualRank;
            var distance = Math.Abs(predictedRank - actualRank);
            var rankDistanceValue = Math.Max(0, slotValue - distance);
            playerTeams[predictedRank].RankDistanceValue += rankDistanceValue;
            playerTeams[predictedRank].Value += rankDistanceValue;
        }

        var pairwiseBonusValue = CountCorrectPairwiseOrders(playerTeams, actualTeams);
        playerTeams[0].PairwiseBonusValue += pairwiseBonusValue;
        playerTeams[0].Value += pairwiseBonusValue;
    }

    private static int CountCorrectPairwiseOrders(TeamData[] playerTeams, TeamData[] actualTeams)
    {
        var predictedRanks = BuildPredictedRankMap(playerTeams);

        var score = 0;
        for (var better = 0; better < actualTeams.Length; better++)
        {
            for (var worse = better + 1; worse < actualTeams.Length; worse++)
            {
                var betterTeam = actualTeams[better].Team;
                var worseTeam = actualTeams[worse].Team;
                if (betterTeam is null || worseTeam is null)
                    continue;

                if (!predictedRanks.TryGetValue(betterTeam, out var betterRank) ||
                    !predictedRanks.TryGetValue(worseTeam, out var worseRank))
                    continue;

                if (betterRank < worseRank)
                    score++;
            }
        }

        return score;
    }

    private static Dictionary<string, int> BuildPredictedRankMap(TeamData[] teams)
    {
        var predictedRanks = new Dictionary<string, int>();
        for (var index = 0; index < teams.Length; index++)
        {
            var team = teams[index].Team;
            if (team is null || predictedRanks.ContainsKey(team))
                continue;

            predictedRanks.Add(team, index);
        }

        return predictedRanks;
    }

    private static void ResolveAllTies(IEnumerable<IGrouping<int, DivisionData>> groups, DivisionData actual)
    {
	    foreach (var group in groups)
	    {
		    ResolveGroupTies(group, actual);
	    }
    }

    private static void ResolveGroupTies(IGrouping<int, DivisionData> group, DivisionData actual)
    {
	    var datas = group.Where(d => d.Teams is not null).ToList();
	    if (datas.Count < 2)
	    {
		    return;
	    }

        var nextTieBreak = 0;
        AssignTieBreaks(datas, actual, 0, null, ref nextTieBreak);
    }

    private static void AssignTieBreaks(List<DivisionData> datas, DivisionData actual, int rank, int? underFallbackRank, ref int nextTieBreak)
    {
        var distanceGroups = datas
            .GroupBy(data => GetGuessDistance(data, actual, rank))
            .OrderBy(group => group.Key);

        foreach (var distanceGroup in distanceGroups)
        {
            var tiedDatas = distanceGroup.ToList();
            if (tiedDatas.Count == 1)
            {
                tiedDatas[0].Teams![0].TieBreak = nextTieBreak++;
                continue;
            }

            var fallbackRank = HasDifferentWinGuesses(tiedDatas, rank) ? rank : underFallbackRank;

            if (AllHaveWinGuess(tiedDatas, rank + 1))
            {
                AssignTieBreaks(tiedDatas, actual, rank + 1, fallbackRank, ref nextTieBreak);
                continue;
            }

            AssignUnderTieBreaks(tiedDatas, fallbackRank, ref nextTieBreak);
        }
    }

    private static void AssignUnderTieBreaks(List<DivisionData> datas, int? rank, ref int nextTieBreak)
    {
        if (rank is null)
        {
            foreach (var data in datas)
            {
                data.Teams![0].TieBreak = nextTieBreak;
            }

            nextTieBreak++;
            return;
        }

        foreach (var underGroup in datas.GroupBy(data => GetWinGuess(data, rank.Value)).OrderBy(group => group.Key))
        {
            foreach (var data in underGroup)
            {
                data.Teams![0].TieBreak = nextTieBreak;
            }

            nextTieBreak++;
        }
    }

    private static bool HasDifferentWinGuesses(IEnumerable<DivisionData> datas, int rank)
    {
        return datas.Select(data => GetWinGuess(data, rank)).Distinct().Skip(1).Any();
    }

    private static bool AllHaveWinGuess(IEnumerable<DivisionData> datas, int rank)
    {
        return datas.All(data => GetWinGuess(data, rank) != 0);
    }

    private static int GetGuessDistance(DivisionData data, DivisionData actual, int rank)
    {
        return Math.Abs(GetWinGuess(data, rank) - GetActualWins(actual, GetTeam(data, rank)));
    }

    private static int GetWinGuess(DivisionData data, int rank)
    {
        return data.Teams is not null && rank < data.Teams.Length ? data.Teams[rank].WinsGuess : 0;
    }

    private static string? GetTeam(DivisionData data, int rank)
    {
        return data.Teams is not null && rank < data.Teams.Length ? data.Teams[rank].Team : null;
    }

    private static int GetActualWins(DivisionData actual, string? team)
    {
        return actual.Teams?.FirstOrDefault(t => t.Team == team)?.Wins ?? 0;
    }

    private static string[] BuildHeaders(DivisionData[] divisionData)
    {
        var headers = new string[divisionData.Length+StatColumns];

        for (var t = 0; t < divisionData.Length; t++)
        {
            if (t == 0)
            {
                headers[0] = divisionData[t].Name;
                headers[1] = "Record";
                headers[2] = "Streak";
            }
            else
            {
                headers[t+StatColumns] = divisionData[t].Name;
            }
        }

        return headers;
    }

    private static TeamData[][] BuildRows(DivisionData[] divisionData)
    {
        var row = new TeamData[5][];

        for (var team = 0; team < 5; team++)
        {
            row[team] = new TeamData[divisionData.Length+StatColumns];
            for (var user = 0; user < divisionData.Length; user++)
            {
                var teams = divisionData[user].Teams;
                if (teams is null)
                    row[team][user] = new TeamData();
                else
                {
                    if (user == 0)
                    {
                        row[team][0] = teams[team];
                        // change the team to the header (team really should be column header or something like that)
                        // I'll need to distinquish when I render the data row
                        row[team][1] = new TeamData()
                        {
                            Team = "Record",
                            Record = teams[team].Record
                        };
                        row[team][2] = new TeamData()
                        {
                            Team = "Streak",
                            Streak = teams[team].Streak
                        };
                    }
                    else
                    {
                        row[team][user+StatColumns] = teams[team];
                    }
                }
            }
        }
        return row;
    }

    private static string[] BuildFooter(DivisionData[] divisionData)
    {
        var footers = new string[divisionData.Length+StatColumns];

        for (var t = 0; t < divisionData.Length; t++)
        {
            if (t == 0)
            {
                footers[0] = "";
                footers[1] = "";
                footers[2] = "";
            }
            else
            {
                var teams = divisionData[t].Teams;
                if (teams is null)
                    footers[t] = "";
                else
                    footers[t+StatColumns] = teams.Sum(t => t.Value).ToString();
            }
        }
        return footers;
    }

    private static DivisionData? FetchActual(ILogger logger)
    {
        var standings = "https://statsapi.mlb.com/api/v1/standings";
        var actual = standings
            .SetQueryParam("leagueId", "104")
            .SetQueryParam("season", "2026")
            .GetJsonAsync<StandingsRoot>()
            .Result;

        logger.LogTrace($"fetch standings {standings} leagueId 104 season 2026");

        var nlEast = actual.Records.FirstOrDefault(r => r.Division.Id == 204)?.TeamRecords;

        if (nlEast is null)
            return null;

        var result = new DivisionData()
        {
            Name = "Actual",
            Teams = new TeamData[5]
        };

        for (var t = 0; t < nlEast.Count; t++)
        {
	        var wAndL = WinsAndLosses(nlEast[t]);
            result.Teams[t] = new TeamData() { 
                Team = TeamData.NameToNickname(nlEast[t].Team.Name ?? ""),
                Record = BuildRecord(nlEast[t]),
                Streak = nlEast[t]?.Streak?.StreakCode ?? "",
                Wins = wAndL.wins,
                Losses = wAndL.losses
            };
        }

        return result;
    }

    private static string GetWildcardGamesBack(TeamRecord teamRecord)
    {
        if (teamRecord.GamesPlayed < 130)
            return "";

        if (teamRecord.DivisionLeader)
            return "";

        if (teamRecord.WildCardEliminationNumber == "E")
            return "x";

        return teamRecord.WildCardGamesBack;
    }

    private static string BuildRecord(TeamRecord teamRecord)
    {
        var leagueRecord = teamRecord.LeagueRecord;
        var wcgb = GetWildcardGamesBack(teamRecord);
        var wcgbValue = String.IsNullOrEmpty(wcgb) ? "" : $" ({wcgb})";
        return $"{leagueRecord.Wins}-{leagueRecord.Losses}{wcgbValue}";
    }

    private static (int wins, int losses) WinsAndLosses(TeamRecord teamRecord)
    {
	    return (teamRecord.LeagueRecord.Wins, teamRecord.LeagueRecord.Losses);
    }
}
