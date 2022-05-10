using Newtonsoft.Json;

#pragma warning disable CS8618

namespace MlbApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class League
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Division
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Sport
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Team
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Streak
    {
        [JsonProperty("streakType")]
        public string StreakType { get; set; }

        [JsonProperty("streakNumber")]
        public int StreakNumber { get; set; }

        [JsonProperty("streakCode")]
        public string StreakCode { get; set; }
    }

    public class LeagueRecord
    {
        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("ties")]
        public int Ties { get; set; }

        [JsonProperty("pct")]
        public string Pct { get; set; }
    }

    public class SplitRecord
    {
        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("pct")]
        public string Pct { get; set; }
    }

    public class DivisionRecord
    {
        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("pct")]
        public string Pct { get; set; }

        [JsonProperty("division")]
        public Division Division { get; set; }
    }

    public class OverallRecord
    {
        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("pct")]
        public string Pct { get; set; }
    }

    public class LeagueRecord2
    {
        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("pct")]
        public string Pct { get; set; }

        [JsonProperty("league")]
        public League League { get; set; }
    }

    public class ExpectedRecord
    {
        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("pct")]
        public string Pct { get; set; }
    }

    public class Records
    {
        [JsonProperty("splitRecords")]
        public List<SplitRecord> SplitRecords { get; set; }

        [JsonProperty("divisionRecords")]
        public List<DivisionRecord> DivisionRecords { get; set; }

        [JsonProperty("overallRecords")]
        public List<OverallRecord> OverallRecords { get; set; }

        [JsonProperty("leagueRecords")]
        public List<LeagueRecord> LeagueRecords { get; set; }

        [JsonProperty("expectedRecords")]
        public List<ExpectedRecord> ExpectedRecords { get; set; }

        [JsonProperty("standingsType")]
        public string StandingsType { get; set; }

        [JsonProperty("league")]
        public League League { get; set; }

        [JsonProperty("division")]
        public Division Division { get; set; }

        [JsonProperty("sport")]
        public Sport Sport { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("teamRecords")]
        public List<TeamRecord> TeamRecords { get; set; }
    }

    public class TeamRecord
    {
        [JsonProperty("team")]
        public Team Team { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("streak")]
        public Streak Streak { get; set; }

        [JsonProperty("divisionRank")]
        public string DivisionRank { get; set; }

        [JsonProperty("leagueRank")]
        public string LeagueRank { get; set; }

        [JsonProperty("sportRank")]
        public string SportRank { get; set; }

        [JsonProperty("gamesPlayed")]
        public int GamesPlayed { get; set; }

        [JsonProperty("gamesBack")]
        public string GamesBack { get; set; }

        [JsonProperty("wildCardGamesBack")]
        public string WildCardGamesBack { get; set; }

        [JsonProperty("leagueGamesBack")]
        public string LeagueGamesBack { get; set; }

        [JsonProperty("springLeagueGamesBack")]
        public string SpringLeagueGamesBack { get; set; }

        [JsonProperty("sportGamesBack")]
        public string SportGamesBack { get; set; }

        [JsonProperty("divisionGamesBack")]
        public string DivisionGamesBack { get; set; }

        [JsonProperty("conferenceGamesBack")]
        public string ConferenceGamesBack { get; set; }

        [JsonProperty("leagueRecord")]
        public LeagueRecord LeagueRecord { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("records")]
        public Records Records { get; set; }

        [JsonProperty("runsAllowed")]
        public int RunsAllowed { get; set; }

        [JsonProperty("runsScored")]
        public int RunsScored { get; set; }

        [JsonProperty("divisionChamp")]
        public bool DivisionChamp { get; set; }

        [JsonProperty("divisionLeader")]
        public bool DivisionLeader { get; set; }

        [JsonProperty("hasWildcard")]
        public bool HasWildcard { get; set; }

        [JsonProperty("clinched")]
        public bool Clinched { get; set; }

        [JsonProperty("eliminationNumber")]
        public string EliminationNumber { get; set; }

        [JsonProperty("wildCardEliminationNumber")]
        public string WildCardEliminationNumber { get; set; }

        [JsonProperty("magicNumber")]
        public string MagicNumber { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("runDifferential")]
        public int RunDifferential { get; set; }

        [JsonProperty("winningPercentage")]
        public string WinningPercentage { get; set; }

        [JsonProperty("wildCardRank")]
        public string WildCardRank { get; set; }

        [JsonProperty("wildCardLeader")]
        public bool? WildCardLeader { get; set; }
    }

    public class Root
    {
        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("records")]
        public List<Records> Records { get; set; }
    }


}
