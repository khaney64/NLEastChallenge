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

		ResolveAllTies(groups, actual, scoringMode);

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

        var predictedRanks = playerTeams
            .Select((team, index) => new { team.Team, Rank = index })
            .ToDictionary(t => t.Team, t => t.Rank);

        for (var actualRank = 0; actualRank < actualTeams.Length; actualRank++)
        {
            var actualTeam = actualTeams[actualRank];
            if (!predictedRanks.TryGetValue(actualTeam.Team, out var predictedRank))
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
        var predictedRanks = playerTeams
            .Select((team, index) => new { team.Team, Rank = index })
            .ToDictionary(t => t.Team, t => t.Rank);

        var score = 0;
        for (var better = 0; better < actualTeams.Length; better++)
        {
            for (var worse = better + 1; worse < actualTeams.Length; worse++)
            {
                if (predictedRanks[actualTeams[better].Team] < predictedRanks[actualTeams[worse].Team])
                    score++;
            }
        }

        return score;
    }

    private static void ResolveAllTies(IEnumerable<IGrouping<int, DivisionData>> groups, DivisionData actual, ScoringMode scoringMode)
    {
	    foreach (var group in groups)
	    {
		    ResolveGroupTies(group, actual, scoringMode);
	    }
    }

    private static void ResolveGroupTies(IGrouping<int, DivisionData> group, DivisionData actual, ScoringMode scoringMode)
    {
	    var datas = group.Where(d => d.Teams is not null).ToList();
	    if (datas.Count < 2)
	    {
		    return;
	    }
	    for (var i = 0; i < datas.Count(); i++)
	    {
		    var data = datas[i];
            var topTeam = data.Teams?[0];
            var percent = GetActualPercent(actual, topTeam?.Team);
		    double pct = (double)(topTeam?.WinsGuess ?? 0) / 162;
            data.Teams![0].TieBreak = Math.Abs(percent - pct);
	    }
    }

    private static double GetActualPercent(DivisionData actual, string? team)
    {
        var actualTeam = actual.Teams?.FirstOrDefault(t => t.Team == team);
        if (actualTeam is null)
            return 0.0;

        var games = actualTeam.Wins + actualTeam.Losses;
        return games == 0 ? 0.0 : ((double)actualTeam.Wins / games);
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
