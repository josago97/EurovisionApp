namespace EurovisionApp.Common.Logic.Data.Models;

public class Performance
{
    public int ContestantId { get; set; }
    public int Running { get; set; } = -1;
    public int Place { get; set; } = -1;
    public IReadOnlyList<Score> Scores { get; set; }
}
