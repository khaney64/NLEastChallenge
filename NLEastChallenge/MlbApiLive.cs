using Newtonsoft.Json;

#pragma warning disable CS8618

namespace MlbApi.Live
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class About
    {
        [JsonProperty("atBatIndex")]
        public int AtBatIndex { get; set; }

        [JsonProperty("halfInning")]
        public string HalfInning { get; set; }

        [JsonProperty("isTopInning")]
        public bool IsTopInning { get; set; }

        [JsonProperty("inning")]
        public int Inning { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }

        [JsonProperty("isScoringPlay")]
        public bool IsScoringPlay { get; set; }

        [JsonProperty("hasReview")]
        public bool HasReview { get; set; }

        [JsonProperty("hasOut")]
        public bool HasOut { get; set; }

        [JsonProperty("captivatingIndex")]
        public int CaptivatingIndex { get; set; }
    }

    public class AllPlay
    {
        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("about")]
        public About About { get; set; }

        [JsonProperty("count")]
        public Count Count { get; set; }

        [JsonProperty("matchup")]
        public Matchup Matchup { get; set; }

        [JsonProperty("pitchIndex")]
        public List<int> PitchIndex { get; set; }

        [JsonProperty("actionIndex")]
        public List<int> ActionIndex { get; set; }

        [JsonProperty("runnerIndex")]
        public List<int> RunnerIndex { get; set; }

        [JsonProperty("runners")]
        public List<Runner> Runners { get; set; }

        [JsonProperty("playEvents")]
        public List<PlayEvent> PlayEvents { get; set; }

        [JsonProperty("playEndTime")]
        public DateTime PlayEndTime { get; set; }

        [JsonProperty("atBatIndex")]
        public int AtBatIndex { get; set; }
    }

    public class AllPosition
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
    }

    public class Away
    {
        [JsonProperty("springLeague")]
        public SpringLeague SpringLeague { get; set; }

        [JsonProperty("allStarStatus")]
        public string AllStarStatus { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("season")]
        public int Season { get; set; }

        [JsonProperty("venue")]
        public Venue Venue { get; set; }

        [JsonProperty("springVenue")]
        public SpringVenue SpringVenue { get; set; }

        [JsonProperty("teamCode")]
        public string TeamCode { get; set; }

        [JsonProperty("fileCode")]
        public string FileCode { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("teamName")]
        public string TeamName { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("firstYearOfPlay")]
        public string FirstYearOfPlay { get; set; }

        [JsonProperty("league")]
        public League League { get; set; }

        [JsonProperty("division")]
        public Division Division { get; set; }

        [JsonProperty("sport")]
        public Sport Sport { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("record")]
        public Record Record { get; set; }

        [JsonProperty("franchiseName")]
        public string FranchiseName { get; set; }

        [JsonProperty("clubName")]
        public string ClubName { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("used")]
        public int Used { get; set; }

        [JsonProperty("remaining")]
        public int Remaining { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }

        [JsonProperty("inning")]
        public int Inning { get; set; }

        [JsonProperty("pitcher")]
        public Pitcher Pitcher { get; set; }

        [JsonProperty("batter")]
        public Batter Batter { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("hits")]
        public int Hits { get; set; }

        [JsonProperty("errors")]
        public int Errors { get; set; }

        [JsonProperty("leftOnBase")]
        public int LeftOnBase { get; set; }

        [JsonProperty("runs")]
        public int Runs { get; set; }

        [JsonProperty("teamStats")]
        public TeamStats TeamStats { get; set; }

        //[JsonProperty("players")]
        //public Players Players { get; set; }

        [JsonProperty("batters")]
        public List<int> Batters { get; set; }

        [JsonProperty("pitchers")]
        public List<int> Pitchers { get; set; }

        [JsonProperty("bench")]
        public List<int> Bench { get; set; }

        [JsonProperty("bullpen")]
        public List<int> Bullpen { get; set; }

        [JsonProperty("battingOrder")]
        public List<int> BattingOrder { get; set; }

        [JsonProperty("info")]
        public List<object> Info { get; set; }

        [JsonProperty("note")]
        public List<object> Note { get; set; }
    }

    public class BatSide
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Batter
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class BatterHotColdZone
    {
        [JsonProperty("zone")]
        public string Zone { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("temp")]
        public string Temp { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class BatterHotColdZoneStats
    {
        [JsonProperty("stats")]
        public List<Stat> Stats { get; set; }
    }

    public class Batting
    {
        [JsonProperty("flyOuts")]
        public int FlyOuts { get; set; }

        [JsonProperty("groundOuts")]
        public int GroundOuts { get; set; }

        [JsonProperty("runs")]
        public int Runs { get; set; }

        [JsonProperty("doubles")]
        public int Doubles { get; set; }

        [JsonProperty("triples")]
        public int Triples { get; set; }

        [JsonProperty("homeRuns")]
        public int HomeRuns { get; set; }

        [JsonProperty("strikeOuts")]
        public int StrikeOuts { get; set; }

        [JsonProperty("baseOnBalls")]
        public int BaseOnBalls { get; set; }

        [JsonProperty("intentionalWalks")]
        public int IntentionalWalks { get; set; }

        [JsonProperty("hits")]
        public int Hits { get; set; }

        [JsonProperty("hitByPitch")]
        public int HitByPitch { get; set; }

        [JsonProperty("avg")]
        public string Avg { get; set; }

        [JsonProperty("atBats")]
        public int AtBats { get; set; }

        [JsonProperty("obp")]
        public string Obp { get; set; }

        [JsonProperty("slg")]
        public string Slg { get; set; }

        [JsonProperty("ops")]
        public string Ops { get; set; }

        [JsonProperty("caughtStealing")]
        public int CaughtStealing { get; set; }

        [JsonProperty("stolenBases")]
        public int StolenBases { get; set; }

        [JsonProperty("stolenBasePercentage")]
        public string StolenBasePercentage { get; set; }

        [JsonProperty("groundIntoDoublePlay")]
        public int GroundIntoDoublePlay { get; set; }

        [JsonProperty("groundIntoTriplePlay")]
        public int GroundIntoTriplePlay { get; set; }

        [JsonProperty("plateAppearances")]
        public int PlateAppearances { get; set; }

        [JsonProperty("totalBases")]
        public int TotalBases { get; set; }

        [JsonProperty("rbi")]
        public int Rbi { get; set; }

        [JsonProperty("leftOnBase")]
        public int LeftOnBase { get; set; }

        [JsonProperty("sacBunts")]
        public int SacBunts { get; set; }

        [JsonProperty("sacFlies")]
        public int SacFlies { get; set; }

        [JsonProperty("catchersInterference")]
        public int CatchersInterference { get; set; }

        [JsonProperty("pickoffs")]
        public int Pickoffs { get; set; }

        [JsonProperty("atBatsPerHomeRun")]
        public string AtBatsPerHomeRun { get; set; }

        [JsonProperty("gamesPlayed")]
        public int GamesPlayed { get; set; }

        [JsonProperty("babip")]
        public string Babip { get; set; }
    }

    public class Boxscore
    {
        [JsonProperty("teams")]
        public Teams Teams { get; set; }

        [JsonProperty("officials")]
        public List<Official> Officials { get; set; }

        [JsonProperty("info")]
        public List<Info> Info { get; set; }

        [JsonProperty("pitchingNotes")]
        public List<object> PitchingNotes { get; set; }
    }

    public class Breaks
    {
        [JsonProperty("breakAngle")]
        public double BreakAngle { get; set; }

        [JsonProperty("spinRate")]
        public int SpinRate { get; set; }

        [JsonProperty("spinDirection")]
        public int SpinDirection { get; set; }
    }

    public class Call
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Catcher
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Center
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("aY")]
        public double AY { get; set; }

        [JsonProperty("aZ")]
        public double AZ { get; set; }

        [JsonProperty("pfxX")]
        public double PfxX { get; set; }

        [JsonProperty("pfxZ")]
        public double PfxZ { get; set; }

        [JsonProperty("pX")]
        public double PX { get; set; }

        [JsonProperty("pZ")]
        public double PZ { get; set; }

        [JsonProperty("vX0")]
        public double VX0 { get; set; }

        [JsonProperty("vY0")]
        public double VY0 { get; set; }

        [JsonProperty("vZ0")]
        public double VZ0 { get; set; }

        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("x0")]
        public double X0 { get; set; }

        [JsonProperty("y0")]
        public double Y0 { get; set; }

        [JsonProperty("z0")]
        public double Z0 { get; set; }

        [JsonProperty("aX")]
        public double AX { get; set; }

        [JsonProperty("coordX")]
        public double CoordX { get; set; }

        [JsonProperty("coordY")]
        public double CoordY { get; set; }
    }

    public class Count
    {
        [JsonProperty("balls")]
        public int Balls { get; set; }

        [JsonProperty("strikes")]
        public int Strikes { get; set; }

        [JsonProperty("outs")]
        public int Outs { get; set; }
    }

    public class Credit
    {
        [JsonProperty("player")]
        public Player Player { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("credit")]
        public string Credit_ { get; set; }
    }

    public class CurrentPlay
    {
        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("about")]
        public About About { get; set; }

        [JsonProperty("count")]
        public Count Count { get; set; }

        [JsonProperty("matchup")]
        public Matchup Matchup { get; set; }

        [JsonProperty("pitchIndex")]
        public List<int> PitchIndex { get; set; }

        [JsonProperty("actionIndex")]
        public List<object> ActionIndex { get; set; }

        [JsonProperty("runnerIndex")]
        public List<object> RunnerIndex { get; set; }

        [JsonProperty("runners")]
        public List<object> Runners { get; set; }

        [JsonProperty("playEvents")]
        public List<PlayEvent> PlayEvents { get; set; }

        [JsonProperty("playEndTime")]
        public DateTime PlayEndTime { get; set; }

        [JsonProperty("atBatIndex")]
        public int AtBatIndex { get; set; }
    }

    public class Datetime
    {
        [JsonProperty("dateTime")]
        public DateTime DateTime { get; set; }

        [JsonProperty("originalDate")]
        public string OriginalDate { get; set; }

        [JsonProperty("officialDate")]
        public string OfficialDate { get; set; }

        [JsonProperty("dayNight")]
        public string DayNight { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("ampm")]
        public string Ampm { get; set; }
    }

    public class DefaultCoordinates
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }

    public class Defense
    {
        [JsonProperty("pitcher")]
        public Pitcher Pitcher { get; set; }

        [JsonProperty("catcher")]
        public Catcher Catcher { get; set; }

        [JsonProperty("first")]
        public First First { get; set; }

        [JsonProperty("second")]
        public Second Second { get; set; }

        [JsonProperty("third")]
        public Third Third { get; set; }

        [JsonProperty("shortstop")]
        public Shortstop Shortstop { get; set; }

        [JsonProperty("left")]
        public Left Left { get; set; }

        [JsonProperty("center")]
        public Center Center { get; set; }

        [JsonProperty("right")]
        public Right Right { get; set; }

        [JsonProperty("batter")]
        public Batter Batter { get; set; }

        [JsonProperty("onDeck")]
        public OnDeck OnDeck { get; set; }

        [JsonProperty("inHole")]
        public InHole InHole { get; set; }

        [JsonProperty("battingOrder")]
        public int BattingOrder { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }
    }

    public class Details
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("movementReason")]
        public object MovementReason { get; set; }

        [JsonProperty("runner")]
        public Runner Runner { get; set; }

        [JsonProperty("responsiblePitcher")]
        public object ResponsiblePitcher { get; set; }

        [JsonProperty("isScoringEvent")]
        public bool IsScoringEvent { get; set; }

        [JsonProperty("rbi")]
        public bool Rbi { get; set; }

        [JsonProperty("earned")]
        public bool Earned { get; set; }

        [JsonProperty("teamUnearned")]
        public bool TeamUnearned { get; set; }

        [JsonProperty("playIndex")]
        public int PlayIndex { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("awayScore")]
        public int AwayScore { get; set; }

        [JsonProperty("homeScore")]
        public int HomeScore { get; set; }

        [JsonProperty("isScoringPlay")]
        public bool IsScoringPlay { get; set; }

        [JsonProperty("hasReview")]
        public bool HasReview { get; set; }

        [JsonProperty("call")]
        public Call Call { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("ballColor")]
        public string BallColor { get; set; }

        [JsonProperty("trailColor")]
        public string TrailColor { get; set; }

        [JsonProperty("isInPlay")]
        public bool? IsInPlay { get; set; }

        [JsonProperty("isStrike")]
        public bool? IsStrike { get; set; }

        [JsonProperty("isBall")]
        public bool? IsBall { get; set; }

        [JsonProperty("type")]
        public Type Type { get; set; }
    }

    public class Division
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class FieldInfo
    {
        [JsonProperty("capacity")]
        public int Capacity { get; set; }

        [JsonProperty("turfType")]
        public string TurfType { get; set; }

        [JsonProperty("roofType")]
        public string RoofType { get; set; }

        [JsonProperty("leftLine")]
        public int LeftLine { get; set; }

        [JsonProperty("left")]
        public int Left { get; set; }

        [JsonProperty("leftCenter")]
        public int LeftCenter { get; set; }

        [JsonProperty("center")]
        public int Center { get; set; }

        [JsonProperty("rightCenter")]
        public int RightCenter { get; set; }

        [JsonProperty("rightLine")]
        public int RightLine { get; set; }
    }

    public class Fielding
    {
        [JsonProperty("assists")]
        public int Assists { get; set; }

        [JsonProperty("putOuts")]
        public int PutOuts { get; set; }

        [JsonProperty("errors")]
        public int Errors { get; set; }

        [JsonProperty("chances")]
        public int Chances { get; set; }

        [JsonProperty("caughtStealing")]
        public int CaughtStealing { get; set; }

        [JsonProperty("passedBall")]
        public int PassedBall { get; set; }

        [JsonProperty("stolenBases")]
        public int StolenBases { get; set; }

        [JsonProperty("stolenBasePercentage")]
        public string StolenBasePercentage { get; set; }

        [JsonProperty("pickoffs")]
        public int Pickoffs { get; set; }

        [JsonProperty("fielding")]
        public string Fielding_ { get; set; }

        [JsonProperty("gamesStarted")]
        public int GamesStarted { get; set; }
    }

    public class First
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Flags
    {
        [JsonProperty("noHitter")]
        public bool NoHitter { get; set; }

        [JsonProperty("perfectGame")]
        public bool PerfectGame { get; set; }

        [JsonProperty("awayTeamNoHitter")]
        public bool AwayTeamNoHitter { get; set; }

        [JsonProperty("awayTeamPerfectGame")]
        public bool AwayTeamPerfectGame { get; set; }

        [JsonProperty("homeTeamNoHitter")]
        public bool HomeTeamNoHitter { get; set; }

        [JsonProperty("homeTeamPerfectGame")]
        public bool HomeTeamPerfectGame { get; set; }
    }

    public class Game
    {
        [JsonProperty("pk")]
        public int Pk { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("doubleHeader")]
        public string DoubleHeader { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("gamedayType")]
        public string GamedayType { get; set; }

        [JsonProperty("tiebreaker")]
        public string Tiebreaker { get; set; }

        [JsonProperty("gameNumber")]
        public int GameNumber { get; set; }

        [JsonProperty("calendarEventID")]
        public string CalendarEventID { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("seasonDisplay")]
        public string SeasonDisplay { get; set; }
    }

    public class GameData
    {
        [JsonProperty("game")]
        public Game Game { get; set; }

        [JsonProperty("datetime")]
        public Datetime Datetime { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("teams")]
        public Teams Teams { get; set; }

        //[JsonProperty("players")]
        //public Players Players { get; set; }

        [JsonProperty("venue")]
        public Venue Venue { get; set; }

        [JsonProperty("officialVenue")]
        public OfficialVenue OfficialVenue { get; set; }

        [JsonProperty("weather")]
        public Weather Weather { get; set; }

        [JsonProperty("gameInfo")]
        public GameInfo GameInfo { get; set; }

        [JsonProperty("review")]
        public Review Review { get; set; }

        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("alerts")]
        public List<object> Alerts { get; set; }

        [JsonProperty("probablePitchers")]
        public ProbablePitchers ProbablePitchers { get; set; }

        [JsonProperty("officialScorer")]
        public OfficialScorer OfficialScorer { get; set; }

        [JsonProperty("primaryDatacaster")]
        public PrimaryDatacaster PrimaryDatacaster { get; set; }
    }

    public class GameInfo
    {
        [JsonProperty("firstPitch")]
        public DateTime FirstPitch { get; set; }
    }

    public class GameStatus
    {
        [JsonProperty("isCurrentBatter")]
        public bool IsCurrentBatter { get; set; }

        [JsonProperty("isCurrentPitcher")]
        public bool IsCurrentPitcher { get; set; }

        [JsonProperty("isOnBench")]
        public bool IsOnBench { get; set; }

        [JsonProperty("isSubstitute")]
        public bool IsSubstitute { get; set; }
    }

    public class Group
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }

    public class HitData
    {
        [JsonProperty("launchSpeed")]
        public double LaunchSpeed { get; set; }

        [JsonProperty("launchAngle")]
        public double LaunchAngle { get; set; }

        [JsonProperty("totalDistance")]
        public double TotalDistance { get; set; }

        [JsonProperty("trajectory")]
        public string Trajectory { get; set; }

        [JsonProperty("hardness")]
        public string Hardness { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }
    }

    public class HitDistance
    {
    }

    public class Hits
    {
        [JsonProperty("away")]
        public List<Away> Away { get; set; }

        [JsonProperty("home")]
        public List<object> Home { get; set; }
    }

    public class HitSpeed
    {
    }

    public class Home
    {
        [JsonProperty("springLeague")]
        public SpringLeague SpringLeague { get; set; }

        [JsonProperty("allStarStatus")]
        public string AllStarStatus { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("season")]
        public int Season { get; set; }

        [JsonProperty("venue")]
        public Venue Venue { get; set; }

        [JsonProperty("springVenue")]
        public SpringVenue SpringVenue { get; set; }

        [JsonProperty("teamCode")]
        public string TeamCode { get; set; }

        [JsonProperty("fileCode")]
        public string FileCode { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("teamName")]
        public string TeamName { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("firstYearOfPlay")]
        public string FirstYearOfPlay { get; set; }

        [JsonProperty("league")]
        public League League { get; set; }

        [JsonProperty("division")]
        public Division Division { get; set; }

        [JsonProperty("sport")]
        public Sport Sport { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("record")]
        public Record Record { get; set; }

        [JsonProperty("franchiseName")]
        public string FranchiseName { get; set; }

        [JsonProperty("clubName")]
        public string ClubName { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("used")]
        public int Used { get; set; }

        [JsonProperty("remaining")]
        public int Remaining { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("hits")]
        public int Hits { get; set; }

        [JsonProperty("errors")]
        public int Errors { get; set; }

        [JsonProperty("leftOnBase")]
        public int LeftOnBase { get; set; }

        [JsonProperty("runs")]
        public int Runs { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }

        [JsonProperty("teamStats")]
        public TeamStats TeamStats { get; set; }

        //[JsonProperty("players")]
        //public Players Players { get; set; }

        [JsonProperty("batters")]
        public List<int> Batters { get; set; }

        [JsonProperty("pitchers")]
        public List<int> Pitchers { get; set; }

        [JsonProperty("bench")]
        public List<int> Bench { get; set; }

        [JsonProperty("bullpen")]
        public List<int> Bullpen { get; set; }

        [JsonProperty("battingOrder")]
        public List<int> BattingOrder { get; set; }

        [JsonProperty("info")]
        public List<object> Info { get; set; }

        [JsonProperty("note")]
        public List<object> Note { get; set; }
    }

    public class Info
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class InHole
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Inning
    {
        [JsonProperty("num")]
        public int Num { get; set; }

        [JsonProperty("ordinalNum")]
        public string OrdinalNum { get; set; }

        [JsonProperty("home")]
        public Home Home { get; set; }

        [JsonProperty("away")]
        public Away Away { get; set; }
    }

    public class Leaders
    {
        [JsonProperty("hitDistance")]
        public HitDistance HitDistance { get; set; }

        [JsonProperty("hitSpeed")]
        public HitSpeed HitSpeed { get; set; }

        [JsonProperty("pitchSpeed")]
        public PitchSpeed PitchSpeed { get; set; }
    }

    public class League
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
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

    public class Left
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Linescore
    {
        [JsonProperty("currentInning")]
        public int CurrentInning { get; set; }

        [JsonProperty("currentInningOrdinal")]
        public string CurrentInningOrdinal { get; set; }

        [JsonProperty("inningState")]
        public string InningState { get; set; }

        [JsonProperty("inningHalf")]
        public string InningHalf { get; set; }

        [JsonProperty("isTopInning")]
        public bool IsTopInning { get; set; }

        [JsonProperty("scheduledInnings")]
        public int ScheduledInnings { get; set; }

        [JsonProperty("innings")]
        public List<Inning> Innings { get; set; }

        [JsonProperty("teams")]
        public Teams Teams { get; set; }

        [JsonProperty("defense")]
        public Defense Defense { get; set; }

        [JsonProperty("offense")]
        public Offense Offense { get; set; }

        [JsonProperty("balls")]
        public int Balls { get; set; }

        [JsonProperty("strikes")]
        public int Strikes { get; set; }

        [JsonProperty("outs")]
        public int Outs { get; set; }
    }

    public class LiveData
    {
        [JsonProperty("plays")]
        public Plays Plays { get; set; }

        //[JsonProperty("linescore")]
        //public Linescore Linescore { get; set; }

        //[JsonProperty("boxscore")]
        //public Boxscore Boxscore { get; set; }

        //[JsonProperty("leaders")]
        //public Leaders Leaders { get; set; }
    }

    public class Location
    {
        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("stateAbbrev")]
        public string StateAbbrev { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("defaultCoordinates")]
        public DefaultCoordinates DefaultCoordinates { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }

    public class Matchup
    {
        [JsonProperty("batter")]
        public Batter Batter { get; set; }

        [JsonProperty("batSide")]
        public BatSide BatSide { get; set; }

        [JsonProperty("pitcher")]
        public Pitcher Pitcher { get; set; }

        [JsonProperty("pitchHand")]
        public PitchHand PitchHand { get; set; }

        [JsonProperty("batterHotColdZones")]
        public List<BatterHotColdZone> BatterHotColdZones { get; set; }

        [JsonProperty("pitcherHotColdZones")]
        public List<object> PitcherHotColdZones { get; set; }

        [JsonProperty("splits")]
        public Split Splits { get; set; }

        [JsonProperty("batterHotColdZoneStats")]
        public BatterHotColdZoneStats BatterHotColdZoneStats { get; set; }
    }

    public class MetaData
    {
        [JsonProperty("wait")]
        public int Wait { get; set; }

        [JsonProperty("timeStamp")]
        public string TimeStamp { get; set; }

        [JsonProperty("gameEvents")]
        public List<string> GameEvents { get; set; }

        [JsonProperty("logicalEvents")]
        public List<string> LogicalEvents { get; set; }
    }

    public class Movement
    {
        [JsonProperty("originBase")]
        public object OriginBase { get; set; }

        [JsonProperty("start")]
        public object Start { get; set; }

        [JsonProperty("end")]
        public object End { get; set; }

        [JsonProperty("outBase")]
        public string OutBase { get; set; }

        [JsonProperty("isOut")]
        public bool IsOut { get; set; }

        [JsonProperty("outNumber")]
        public int OutNumber { get; set; }
    }

    public class Offense
    {
        [JsonProperty("batter")]
        public Batter Batter { get; set; }

        [JsonProperty("onDeck")]
        public OnDeck OnDeck { get; set; }

        [JsonProperty("inHole")]
        public InHole InHole { get; set; }

        [JsonProperty("pitcher")]
        public Pitcher Pitcher { get; set; }

        [JsonProperty("battingOrder")]
        public int BattingOrder { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }
    }

    public class Official
    {
        [JsonProperty("official")]
        public Official Official_ { get; set; }

        [JsonProperty("officialType")]
        public string OfficialType { get; set; }
    }

    public class Official2
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class OfficialScorer
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class OfficialVenue
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class OnDeck
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Person
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class PitchData
    {
        [JsonProperty("startSpeed")]
        public double StartSpeed { get; set; }

        [JsonProperty("endSpeed")]
        public double EndSpeed { get; set; }

        [JsonProperty("strikeZoneTop")]
        public double StrikeZoneTop { get; set; }

        [JsonProperty("strikeZoneBottom")]
        public double StrikeZoneBottom { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty("breaks")]
        public Breaks Breaks { get; set; }

        [JsonProperty("zone")]
        public int Zone { get; set; }

        [JsonProperty("plateTime")]
        public double PlateTime { get; set; }

        [JsonProperty("extension")]
        public double Extension { get; set; }
    }

    public class Pitcher
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class PitchHand
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Pitching
    {
        [JsonProperty("groundOuts")]
        public int GroundOuts { get; set; }

        [JsonProperty("airOuts")]
        public int AirOuts { get; set; }

        [JsonProperty("runs")]
        public int Runs { get; set; }

        [JsonProperty("doubles")]
        public int Doubles { get; set; }

        [JsonProperty("triples")]
        public int Triples { get; set; }

        [JsonProperty("homeRuns")]
        public int HomeRuns { get; set; }

        [JsonProperty("strikeOuts")]
        public int StrikeOuts { get; set; }

        [JsonProperty("baseOnBalls")]
        public int BaseOnBalls { get; set; }

        [JsonProperty("intentionalWalks")]
        public int IntentionalWalks { get; set; }

        [JsonProperty("hits")]
        public int Hits { get; set; }

        [JsonProperty("hitByPitch")]
        public int HitByPitch { get; set; }

        [JsonProperty("atBats")]
        public int AtBats { get; set; }

        [JsonProperty("obp")]
        public string Obp { get; set; }

        [JsonProperty("caughtStealing")]
        public int CaughtStealing { get; set; }

        [JsonProperty("stolenBases")]
        public int StolenBases { get; set; }

        [JsonProperty("stolenBasePercentage")]
        public string StolenBasePercentage { get; set; }

        [JsonProperty("numberOfPitches")]
        public int NumberOfPitches { get; set; }

        [JsonProperty("era")]
        public string Era { get; set; }

        [JsonProperty("inningsPitched")]
        public string InningsPitched { get; set; }

        [JsonProperty("saveOpportunities")]
        public int SaveOpportunities { get; set; }

        [JsonProperty("earnedRuns")]
        public int EarnedRuns { get; set; }

        [JsonProperty("whip")]
        public string Whip { get; set; }

        [JsonProperty("battersFaced")]
        public int BattersFaced { get; set; }

        [JsonProperty("outs")]
        public int Outs { get; set; }

        [JsonProperty("completeGames")]
        public int CompleteGames { get; set; }

        [JsonProperty("shutouts")]
        public int Shutouts { get; set; }

        [JsonProperty("strikes")]
        public int Strikes { get; set; }

        [JsonProperty("strikePercentage")]
        public string StrikePercentage { get; set; }

        [JsonProperty("hitBatsmen")]
        public int HitBatsmen { get; set; }

        [JsonProperty("balks")]
        public int Balks { get; set; }

        [JsonProperty("wildPitches")]
        public int WildPitches { get; set; }

        [JsonProperty("pickoffs")]
        public int Pickoffs { get; set; }

        [JsonProperty("groundOutsToAirouts")]
        public string GroundOutsToAirouts { get; set; }

        [JsonProperty("rbi")]
        public int Rbi { get; set; }

        [JsonProperty("pitchesPerInning")]
        public string PitchesPerInning { get; set; }

        [JsonProperty("runsScoredPer9")]
        public string RunsScoredPer9 { get; set; }

        [JsonProperty("homeRunsPer9")]
        public string HomeRunsPer9 { get; set; }

        [JsonProperty("inheritedRunners")]
        public int InheritedRunners { get; set; }

        [JsonProperty("inheritedRunnersScored")]
        public int InheritedRunnersScored { get; set; }

        [JsonProperty("catchersInterference")]
        public int CatchersInterference { get; set; }

        [JsonProperty("sacBunts")]
        public int SacBunts { get; set; }

        [JsonProperty("sacFlies")]
        public int SacFlies { get; set; }

        [JsonProperty("gamesPlayed")]
        public int GamesPlayed { get; set; }

        [JsonProperty("gamesStarted")]
        public int GamesStarted { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("saves")]
        public int Saves { get; set; }

        [JsonProperty("holds")]
        public int Holds { get; set; }

        [JsonProperty("blownSaves")]
        public int BlownSaves { get; set; }

        [JsonProperty("gamesPitched")]
        public int GamesPitched { get; set; }

        [JsonProperty("winPercentage")]
        public string WinPercentage { get; set; }

        [JsonProperty("gamesFinished")]
        public int GamesFinished { get; set; }

        [JsonProperty("strikeoutWalkRatio")]
        public string StrikeoutWalkRatio { get; set; }

        [JsonProperty("strikeoutsPer9Inn")]
        public string StrikeoutsPer9Inn { get; set; }

        [JsonProperty("walksPer9Inn")]
        public string WalksPer9Inn { get; set; }

        [JsonProperty("hitsPer9Inn")]
        public string HitsPer9Inn { get; set; }

        [JsonProperty("pitchesThrown")]
        public int PitchesThrown { get; set; }

        [JsonProperty("balls")]
        public int Balls { get; set; }
    }

    public class PitchSpeed
    {
    }

    public class Player
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class PlayEvent
    {
        [JsonProperty("details")]
        public Details Details { get; set; }

        [JsonProperty("count")]
        public Count Count { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("isPitch")]
        public bool IsPitch { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("player")]
        public Player Player { get; set; }

        [JsonProperty("pitchData")]
        public PitchData PitchData { get; set; }

        [JsonProperty("playId")]
        public string PlayId { get; set; }

        [JsonProperty("pitchNumber")]
        public int? PitchNumber { get; set; }

        [JsonProperty("hitData")]
        public HitData HitData { get; set; }
    }

    public class Plays
    {
        //[JsonProperty("allPlays")]
        //public List<AllPlay> AllPlays { get; set; }

        [JsonProperty("currentPlay")]
        public CurrentPlay CurrentPlay { get; set; }

        //[JsonProperty("scoringPlays")]
        //public List<object> ScoringPlays { get; set; }

        //[JsonProperty("playsByInning")]
        //public List<PlaysByInning> PlaysByInning { get; set; }
    }

    public class PlaysByInning
    {
        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("endIndex")]
        public int EndIndex { get; set; }

        [JsonProperty("top")]
        public List<int> Top { get; set; }

        [JsonProperty("bottom")]
        public List<object> Bottom { get; set; }

        [JsonProperty("hits")]
        public Hits Hits { get; set; }
    }

    public class Position
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
    }

    public class PrimaryDatacaster
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class PrimaryPosition
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
    }

    public class ProbablePitchers
    {
        [JsonProperty("away")]
        public Away Away { get; set; }

        [JsonProperty("home")]
        public Home Home { get; set; }
    }

    public class Record
    {
        [JsonProperty("gamesPlayed")]
        public int GamesPlayed { get; set; }

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

        [JsonProperty("records")]
        public Records Records { get; set; }

        [JsonProperty("divisionLeader")]
        public bool DivisionLeader { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("winningPercentage")]
        public string WinningPercentage { get; set; }
    }

    public class Records
    {
    }

    public class Result
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("rbi")]
        public int Rbi { get; set; }

        [JsonProperty("awayScore")]
        public int AwayScore { get; set; }

        [JsonProperty("homeScore")]
        public int HomeScore { get; set; }
    }

    public class Review
    {
        [JsonProperty("hasChallenges")]
        public bool HasChallenges { get; set; }

        [JsonProperty("away")]
        public Away Away { get; set; }

        [JsonProperty("home")]
        public Home Home { get; set; }
    }

    public class Right
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class LiveRoot
    {
        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("gamePk")]
        public int GamePk { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("metaData")]
        public MetaData MetaData { get; set; }

        //[JsonProperty("gameData")]
        //public GameData GameData { get; set; }

        [JsonProperty("liveData")]
        public LiveData LiveData { get; set; }
    }

    public class Runner
    {
        [JsonProperty("movement")]
        public Movement Movement { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; }

        [JsonProperty("credits")]
        public List<Credit> Credits { get; set; }
    }

    public class Runner2
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class SeasonStats
    {
        [JsonProperty("batting")]
        public Batting Batting { get; set; }

        [JsonProperty("pitching")]
        public Pitching Pitching { get; set; }

        [JsonProperty("fielding")]
        public Fielding Fielding { get; set; }
    }

    public class Second
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Shortstop
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Split
    {
        [JsonProperty("stat")]
        public Stat Stat { get; set; }

        [JsonProperty("batter")]
        public string Batter { get; set; }

        [JsonProperty("pitcher")]
        public string Pitcher { get; set; }

        [JsonProperty("menOnBase")]
        public string MenOnBase { get; set; }
    }

    public class Sport
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class SpringLeague
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
    }

    public class SpringVenue
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Stat
    {
        [JsonProperty("type")]
        public Type Type { get; set; }

        [JsonProperty("group")]
        public Group Group { get; set; }

        [JsonProperty("exemptions")]
        public List<object> Exemptions { get; set; }

        [JsonProperty("splits")]
        public List<Split> Splits { get; set; }

        [JsonProperty("batting")]
        public Batting Batting { get; set; }

        [JsonProperty("pitching")]
        public Pitching Pitching { get; set; }

        [JsonProperty("fielding")]
        public Fielding Fielding { get; set; }
    }

    public class Stat2
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("zones")]
        public List<Zone> Zones { get; set; }
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

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Team
    {
        [JsonProperty("springLeague")]
        public SpringLeague SpringLeague { get; set; }

        [JsonProperty("allStarStatus")]
        public string AllStarStatus { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class Teams
    {
        [JsonProperty("away")]
        public Away Away { get; set; }

        [JsonProperty("home")]
        public Home Home { get; set; }
    }

    public class TeamStats
    {
        [JsonProperty("batting")]
        public Batting Batting { get; set; }

        [JsonProperty("pitching")]
        public Pitching Pitching { get; set; }

        [JsonProperty("fielding")]
        public Fielding Fielding { get; set; }
    }

    public class Third
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class TimeZone
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("tz")]
        public string Tz { get; set; }
    }

    public class Type
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Venue
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("timeZone")]
        public TimeZone TimeZone { get; set; }

        [JsonProperty("fieldInfo")]
        public FieldInfo FieldInfo { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }

    public class Weather
    {
        [JsonProperty("condition")]
        public string Condition { get; set; }

        [JsonProperty("temp")]
        public string Temp { get; set; }

        [JsonProperty("wind")]
        public string Wind { get; set; }
    }

    public class Zone
    {
        [JsonProperty("zone")]
        public string Zone_ { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("temp")]
        public string Temp { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }


}
