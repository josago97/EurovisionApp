namespace EurovisionApp.Common.Logic.Data.Models.Eurovision;

public class Contestant : Models.Contestant
{
    public string Country { get; set; }
    public string Tone { get; set; }
    public int Bpm { get; set; } = -1;
    public string Broadcaster { get; set; }
    public string Spokesperson { get; set; }
    public IReadOnlyList<string> Commentators { get; set; }
}
