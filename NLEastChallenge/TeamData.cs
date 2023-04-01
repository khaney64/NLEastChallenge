namespace NLEastChallenge;
public class TeamData
{
    public string Team { get; set; } = string.Empty;

    public string TeamName
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Team))
                return string.Empty;

            return Team;
        }
    }

    public int WinsGuess { get; set; } = 0;

    public int Value { get; set; }

    public string Record { get; set; } = String.Empty;

    public string Streak { get; set; } = String.Empty;

    public int Wins { get; set; }

    public int Losses { get; set; }

    public double TieBreak { get; set; }

    public static string NameToNickname(string teamName)
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