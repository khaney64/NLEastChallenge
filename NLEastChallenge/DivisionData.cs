using MlbApi;
using Flurl;
using Flurl.Http;

namespace NLEastChallenge;

public class DivisionData
{
    const int StatColumns = 2;

    public string Name { get; set; } = "";

    public TeamData[]? Teams { get; set; }

    internal static DivisionDataVm GetData(DivisionData[] configuredData)
    {
        if (configuredData is null)
            return new DivisionDataVm();

        var actual = FetchActual();
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

                if (aTeams[i].Team == cTeams[i].Team)
                {
                    cTeams[i].Value = 5 - i;
                }
            }
        }

        for (var i = 0; i < configuredData?.Length; i++)
        {
            divisionData[i + 1] = configuredData[i];
        };

        var result = new DivisionDataVm()
        {
            Headers = BuildHeaders(divisionData),
            Rows = BuildRows(divisionData),
            Footers = BuildFooter(divisionData)
        };

        return result;
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

    private static DivisionData? FetchActual()
    {
        var standings = "https://statsapi.mlb.com/api/v1/standings";
        var actual = standings
            .SetQueryParam("leagueId", "104")
            .SetQueryParam("season", "2022")
            .GetJsonAsync<Root>().Result;

        var nlEast = actual.Records.Where(r => r.Division.Id == 204).FirstOrDefault()?.TeamRecords;

        if (nlEast is null)
            return null;

        var result = new DivisionData()
        {
            Name = "Actual",
            Teams = new TeamData[5]
        };

        for (var t = 0; t < nlEast.Count; t++)
        {
            result.Teams[t] = new TeamData() { 
                Team = NameToNickname(nlEast[t].Team.Name ?? ""),
                Record = BuildRecord(nlEast[t].LeagueRecord),
                Streak = nlEast[t].Streak.StreakCode
            };
        }

        return result;
    }

    private static string BuildRecord(LeagueRecord leagueRecord)
    {
        return $"{leagueRecord.Wins}-{leagueRecord.Losses}";
    }

    private static string NameToNickname(string teamName)
    {
        switch (teamName)
        {
            case "New York Mets":
                return "NYM";

            case "Philadelphia Phillies":
                return "PHI";

            case "Atlanta Braves":
                return "ATL";

            case "Miami Marlins":
                return "MIA";

            case "Washington Nationals":
                return "WAS";

            default:
                return "???";
        }
    }
}