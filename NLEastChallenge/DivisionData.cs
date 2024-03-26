using MlbApi;
using Flurl;
using Flurl.Http;

namespace NLEastChallenge;

public class DivisionData
{
    const int StatColumns = 2;

    public string Name { get; set; } = "";

    public TeamData[]? Teams { get; set; }

    internal static DivisionDataVm GetData(DivisionData[] configuredData, ILogger logger)
    {
        var actual = FetchActual(logger);
        if (actual is null)
            return new DivisionDataVm();

        var divisionData = new DivisionData[configuredData.Length + 1];

        divisionData[0] = actual;

        for (var i = 0; i < 5; i++)
        {
            var aTeams = actual.Teams;
            if (aTeams is null)
                continue;

            for (var j = 0; j < configuredData?.Length; j++)
            {
                var cTeams = configuredData[j].Teams;
                if (cTeams is null)
                    continue;

                if (aTeams[i].Team == cTeams[i].Team  && aTeams[i].Record != "0-0")
                {
                    cTeams[i].Value = 5 - i;
                }
            }
        }

        var groups = configuredData.ToList()
	        .GroupBy(c => c.Teams.Sum(t => t.Value));

        var tiebreakTeam = actual.Teams.First(t => t.Team == "ATL");
        var games = tiebreakTeam.Wins + tiebreakTeam.Losses;
        double tiebreakPercent = games == 0 ? 0.0 : ((double)tiebreakTeam.Wins / games);

		ResolveAllTies(groups, tiebreakPercent);

        // sort the configured data by total and team values
        var sorted = configuredData?.ToList()
            .OrderByDescending(c => c.Teams.Sum(t => t.Value))
            .ThenBy(c => c.Teams[0].TieBreak)
            .ThenByDescending(c => c.Teams[0].Value)
            .ThenByDescending(c => c.Teams[1].Value)
            .ThenByDescending(c => c.Teams[2].Value)
            .ThenByDescending(c => c.Teams[3].Value)
            .ThenByDescending(c => c.Teams[4].Value)
            .ToList();

        for (var i = 0; i < sorted?.Count; i++)
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

    private static void ResolveAllTies(IEnumerable<IGrouping<int, DivisionData>> groups, double percent)
    {
	    foreach (var group in groups)
	    {
		    ResolveGroupTies(group, percent);
	    }
    }

    private static void ResolveGroupTies(IGrouping<int, DivisionData> group, double percent)
    {
	    var datas = group.Where(d => d.Teams is not null).ToList();
	    if (datas.Count < 2)
	    {
		    return;
	    }
	    for (var i = 0; i < datas.Count(); i++)
	    {
		    var data = datas[i];
		    double pct = (double)data.Teams?[0].WinsGuess / 162;
            data.Teams[0].TieBreak = Math.Abs(percent - pct);
	    }
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
            .SetQueryParam("season", "2024")
            .GetJsonAsync<StandingsRoot>()
            .Result;

        logger.LogTrace($"fetch standings {standings} leagueId 104 season 2023");

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