namespace EurovisionApp.Common.Logic.Data.Models;

public class Round
{
    public string Name { get; set; }
    public string Date { get; set; }
    public IReadOnlyList<Performance> Performances { get; set; }
}
