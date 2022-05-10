namespace NetCoreReact;

public class DivisionDataVm
{
    public string[] Headers { get; set; }

    public TeamData[][] Rows { get; set; }

    public string[] Footers { get; set; }

    public DivisionDataVm() :
        this(Array.Empty<string>(), Array.Empty<TeamData[]>(), Array.Empty<string>())
    { }

    public DivisionDataVm(IEnumerable<string> headers, IEnumerable<IEnumerable<TeamData>> rows, IEnumerable<string> footers)
    {
        Headers = headers.ToArray();
        var buildRows = new TeamData[rows.Count()][];
        var rowArray = rows.ToArray();
        for (var row = 0; row < rows.Count(); row++)
        {
            buildRows[row] = rowArray[row].ToArray();
        }
        Rows = buildRows;
        Footers = footers.ToArray();
    }
}