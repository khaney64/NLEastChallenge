using Newtonsoft.Json;

#pragma warning disable CS8618

namespace MlbApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<StandingRoot>(myJsonResponse);
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

    public class StandingsRoot
    {
        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("records")]
        public List<Records> Records { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Away
    {
        [JsonProperty("leagueRecord")]
        public LeagueRecord LeagueRecord { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }

        [JsonProperty("isWinner")]
        public bool IsWinner { get; set; }

        [JsonProperty("splitSquad")]
        public bool SplitSquad { get; set; }

        [JsonProperty("seriesNumber")]
        public int SeriesNumber { get; set; }
    }

    public class Content
    {
        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class GameDate
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

        [JsonProperty("totalEvents")]
        public int TotalEvents { get; set; }

        [JsonProperty("totalGames")]
        public int TotalGames { get; set; }

        [JsonProperty("totalGamesInProgress")]
        public int TotalGamesInProgress { get; set; }

        [JsonProperty("games")]
        public List<Game> Games { get; set; }

        [JsonProperty("events")]
        public List<object> Events { get; set; }
    }

    public class Game
    {
        [JsonProperty("gamePk")]
        public int GamePk { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("gameType")]
        public string GameType { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("gameDate")]
        public DateTime GameDate { get; set; }

        [JsonProperty("officialDate")]
        public string OfficialDate { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("teams")]
        public Teams Teams { get; set; }

        [JsonProperty("venue")]
        public Venue Venue { get; set; }

        [JsonProperty("content")]
        public Content Content { get; set; }

        [JsonProperty("isTie")]
        public bool IsTie { get; set; }

        [JsonProperty("gameNumber")]
        public int GameNumber { get; set; }

        [JsonProperty("publicFacing")]
        public bool PublicFacing { get; set; }

        [JsonProperty("doubleHeader")]
        public string DoubleHeader { get; set; }

        [JsonProperty("gamedayType")]
        public string GamedayType { get; set; }

        [JsonProperty("tiebreaker")]
        public string Tiebreaker { get; set; }

        [JsonProperty("calendarEventID")]
        public string CalendarEventID { get; set; }

        [JsonProperty("seasonDisplay")]
        public string SeasonDisplay { get; set; }

        [JsonProperty("dayNight")]
        public string DayNight { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("scheduledInnings")]
        public int ScheduledInnings { get; set; }

        [JsonProperty("reverseHomeAwayStatus")]
        public bool ReverseHomeAwayStatus { get; set; }

        [JsonProperty("inningBreakLength")]
        public int InningBreakLength { get; set; }

        [JsonProperty("gamesInSeries")]
        public int GamesInSeries { get; set; }

        [JsonProperty("seriesGameNumber")]
        public int SeriesGameNumber { get; set; }

        [JsonProperty("seriesDescription")]
        public string SeriesDescription { get; set; }

        [JsonProperty("recordSource")]
        public string RecordSource { get; set; }

        [JsonProperty("ifNecessary")]
        public string IfNecessary { get; set; }

        [JsonProperty("ifNecessaryDescription")]
        public string IfNecessaryDescription { get; set; }
    }

    public class Home
    {
        [JsonProperty("leagueRecord")]
        public LeagueRecord LeagueRecord { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }

        [JsonProperty("isWinner")]
        public bool IsWinner { get; set; }

        [JsonProperty("splitSquad")]
        public bool SplitSquad { get; set; }

        [JsonProperty("seriesNumber")]
        public int SeriesNumber { get; set; }
    }

    public class GameRoot
    {
        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

        [JsonProperty("totalEvents")]
        public int TotalEvents { get; set; }

        [JsonProperty("totalGames")]
        public int TotalGames { get; set; }

        [JsonProperty("totalGamesInProgress")]
        public int TotalGamesInProgress { get; set; }

        [JsonProperty("dates")]
        public List<GameDate> Dates { get; set; }
    }

    public class Status
    {
        [JsonProperty("abstractGameState")]
        public string AbstractGameState { get; set; }

        [JsonProperty("codedGameState")]
        public string CodedGameState { get; set; }

        [JsonProperty("detailedState")]
        public string DetailedState { get; set; }

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("startTimeTBD")]
        public bool StartTimeTBD { get; set; }

        [JsonProperty("abstractGameCode")]
        public string AbstractGameCode { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public class Teams
    {
        [JsonProperty("away")]
        public Away Away { get; set; }

        [JsonProperty("home")]
        public Home Home { get; set; }
    }

    public class Venue
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
