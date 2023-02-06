using System.Globalization;
using System.Reflection.Metadata;
using System.Text;
using EurovisionApp.Common.Logic.Data;
using EurovisionApp.Common.Logic.Data.Models;
using Microsoft.AspNetCore.Components;
using Contest = EurovisionApp.Common.Logic.Data.Models.Eurovision.Contest;

namespace EurovisionApp.Common.Pages;

public partial class ContestDetails
{
    [Parameter]
    public int ContestId { get; set; }
    [Inject]
    public IRepository Repository { get; set; }
    private ContestData Contest { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        IReadOnlyList<Contest> contests = Page switch
        {
            PageType.Junior => Repository.JuniorContests,
            _ => Repository.SeniorContests
        };

        Contest = GetContest(contests[ContestId]);
    }

    private ContestData GetContest(Contest contest)
    {
        ContestData result = new ContestData()
        {
            Year = contest.Year,
            Country = contest.Country,
            City = contest.City,
            Slogan = contest.Slogan,
            LogoUrl = contest.LogoUrl,
            Participants = contest.Contestants.Count,
            Voting = contest.Voting,
            Rounds = GetRounds(contest)
        };

        StringBuilder dateBuilder = new StringBuilder();
        for (int i = 0; i < contest.Rounds.Count; i++)
        {
            Round round = contest.Rounds[i];

            if (DateTime.TryParse(round.Date, out DateTime date))
            {
                if (i < contest.Rounds.Count - 1)
                    dateBuilder.Append(date.Day + ", ");
                else
                    dateBuilder.Append(date.ToString("d MMMM yyyy"));
            }
        }
        result.Date = dateBuilder.ToString();

        StringBuilder locationBuilder = new StringBuilder();
        if (string.IsNullOrEmpty(contest.Arena)) locationBuilder.Append(contest.Arena + ", ");
        locationBuilder.Append(contest.City + ", ");
        locationBuilder.Append(Repository.Countries[contest.Country]);
        result.Location = locationBuilder.ToString();

        if (!contest.Broadcasters.IsNullOrEmpty())
            result.Broadcasters = string.Join(", ", contest.Broadcasters);

        if (!contest.Presenters.IsNullOrEmpty())
            result.Presenters = string.Join(", ", contest.Presenters);

        return result;
    }

    private IEnumerable<RoundData> GetRounds(Contest contest)
    {
        List<RoundData> result = new List<RoundData>();

        foreach (var round in contest.Rounds)
        {
            result.Add(new RoundData()
            {
                Name = round.Name,
                Contestants = GetContestants(contest, round.Performances)
            });
        }

        return result;
    }

    private ContestantData[] GetContestants(Contest contest, IEnumerable<Performance> performances)
    {
        List<ContestantData> result = new List<ContestantData>();

        foreach (var performance in performances)
        {
            var contestant = contest.Contestants[performance.ContestantId];

            result.Add(new ContestantData()
            {
                Place = performance.Place,
                CountryCode = contestant.Country,
                CountryName = Repository.Countries[contestant.Country],
                Song = contestant.Song,
                Artist = contestant.Artist,
                Running = performance.Running,
                Points = performance.Scores.Sum(s => s.Points)
            });
        }

        return result.ToArray();
    }

    private class ContestData
    {
        public int Year { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string Slogan { get; set; }
        public string LogoUrl { get; set; }
        public int Participants { get; set; }
        public string Voting { get; set; }
        public string Presenters { get; set; }
        public string Broadcasters { get; set; }
        public IEnumerable<RoundData> Rounds { get; set; }
    }

    private class RoundData
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public IReadOnlyList<ContestantData> Contestants { get; set; }
    }

    private class ContestantData
    {
        public int Place { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Song { get; set; }
        public string Artist { get; set; }
        public int Running { get; set; }
        public int Points { get; set; }
    }
}