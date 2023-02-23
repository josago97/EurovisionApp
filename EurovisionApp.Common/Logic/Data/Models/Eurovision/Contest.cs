﻿namespace EurovisionApp.Common.Logic.Data.Models.Eurovision;

public class Contest : Models.Contest
{
    public string Arena { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Slogan { get; set; }
    public string LogoUrl { get; set; }
    public string Voting { get; set; }
    public IReadOnlyList<string> Presenters { get; set; }
    public IReadOnlyList<string> Broadcasters { get; set; }
    public IReadOnlyList<Contestant> Contestants { get; set; }
    public IReadOnlyList<Round> Rounds { get; set; }
}