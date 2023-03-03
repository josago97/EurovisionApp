using System.Text;
using BlazorPro.BlazorSize;
using EurovisionApp.Common.Logic.Data.Models;
using Microsoft.AspNetCore.Components;
using Contest = EurovisionApp.Common.Logic.Data.Models.Eurovision.Contest;

namespace EurovisionApp.Common.Pages;

public partial class ContestDetails : IDisposable
{
    private const int MAX_WIDTH_PLACE_COLUMN_SMALL = 470; // px
    private const int MAX_WIDTH_POINTS_COLUMN_SMALL = 500; // px
    private const int MAX_WIDTH_RUNNING_COLUMN_SMALL = 550; // px

    [Inject]
    public IResizeListener ResizeListener { get; set; }
    [Parameter]
    public int Year { get; set; }
    private ContestData Contest { get; set; }
    private bool HasPlaceColumnSmall { get; set; }
    private bool HasPointsColumnSmall { get; set; }
    private bool HasRunningColumnSmall { get; set; }
    private bool IsCancelled { get; set; }
    private string CancelationMessage { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (Settings.CONTESTS_CANCELLED.TryGetValue(Year, out string message))
        {
            IsCancelled = true;
            CancelationMessage = message;
        }

        Contest = GetContestData(GetContest(Year));
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ResizeListener.OnResized += OnWindowResized;
    }

    public override void Dispose()
    {
        base.Dispose();

        ResizeListener.OnResized -= OnWindowResized;
    }

    private void OnWindowResized(object _, BrowserWindowSize window)
    {
        int width = window.Width;
        bool hasPlaceColumnSmall = false;
        bool hasPointsColumnSmall = false;
        bool hasRunningColumnSmall = false;

        if (width < MAX_WIDTH_PLACE_COLUMN_SMALL) hasPlaceColumnSmall = true;
        if (width < MAX_WIDTH_POINTS_COLUMN_SMALL) hasPointsColumnSmall = true;
        if (width < MAX_WIDTH_RUNNING_COLUMN_SMALL) hasRunningColumnSmall = true;

        bool hasChanges = HasPlaceColumnSmall != hasPlaceColumnSmall
            || HasPointsColumnSmall != hasPointsColumnSmall
            || HasRunningColumnSmall != hasRunningColumnSmall;

        if (hasChanges)
        {
            HasPlaceColumnSmall = hasPlaceColumnSmall;
            HasPointsColumnSmall = hasPointsColumnSmall;
            HasRunningColumnSmall = hasRunningColumnSmall;
            StateHasChanged();
        }
    }

    private ContestData GetContestData(Contest contest)
    {
        ContestData result = new ContestData()
        {
            Year = contest.Year,
            Date = GetDate(contest.Rounds),
            Country = contest.Country,
            City = contest.City,
            Location = GetLocation(contest),
            Slogan = contest.Slogan,
            LogoUrl = contest.LogoUrl,
            Participants = contest.Contestants.Count,
            Voting = contest.Voting,
            Rounds = IsCancelled ? GetRoundsDataCancelled(contest) : GetRoundsData(contest)
        };

        if (!contest.Broadcasters.IsNullOrEmpty())
            result.Broadcasters = string.Join(", ", contest.Broadcasters);

        if (!contest.Presenters.IsNullOrEmpty())
            result.Presenters = string.Join(", ", contest.Presenters);

        return result;
    }

    private string GetDate(IReadOnlyList<Round> rounds)
    {
        StringBuilder stringBuilder = new StringBuilder();
        rounds = rounds.OrderBy(r => r.Name.First())
            .ThenBy(r => r.Name.Last())
            .ToArray();

        for (int i = 0; i < rounds.Count; i++)
        {
            Round round = rounds[i];

            if (DateTime.TryParse(round.Date, out DateTime date))
            {
                if (i < rounds.Count - 1)
                    stringBuilder.Append(date.Day + ", ");
                else
                    stringBuilder.Append(date.ToString("d MMMM yyyy"));
            }
        }

        return stringBuilder.ToString();
    }

    private string GetLocation(Contest contest)
    {
        StringBuilder stringBuilder = new StringBuilder();

        if (string.IsNullOrEmpty(contest.Arena))
            stringBuilder.Append(contest.Arena + ", ");
        stringBuilder.Append(contest.City + ", ");
        stringBuilder.Append(Repository.Countries[contest.Country]);

        return stringBuilder.ToString();
    }

    private IReadOnlyList<RoundData> GetRoundsData(Contest contest)
    {
        List<RoundData> result = new List<RoundData>();

        foreach (var round in contest.Rounds)
        {
            result.Add(new RoundData()
            {
                Name = round.Name.ToLower() switch
                {
                    "final" => "Grand Final",
                    "semifinal" => "Semifinal",
                    "semifinal1" => "Semifinal 1",
                    _ => "Semifinal 2",
                },
                Contestants = GetContestantsData(contest, round.Performances)
            });
        }

        return result;
    }

    private IReadOnlyList<ContestantData> GetContestantsData(Contest contest, IReadOnlyList<Performance> performances)
    {
        List<ContestantData> result = new List<ContestantData>();

        foreach (var performance in performances)
        {
            var contestant = contest.Contestants[performance.ContestantId];

            result.Add(new ContestantData()
            {
                Id = contestant.Id,
                Place = performance.Place,
                CountryCode = contestant.Country,
                CountryName = Repository.Countries[contestant.Country],
                Song = contestant.Song,
                Artist = contestant.Artist,
                Running = performance.Running,
                Points = performance.Scores.Count > 0
                    ? performance.Scores.Sum(s => s.Points)
                    : null
            });
        }

        return result;
    }

    private IReadOnlyList<RoundData> GetRoundsDataCancelled(Contest contest)
    {
        return new RoundData[]
        {
            new RoundData()
            {
                Contestants = GetContestantsDataCancelled(contest)
            }
        };
    }

    private IReadOnlyList<ContestantData> GetContestantsDataCancelled(Contest contest)
    {
        List<ContestantData> result = new List<ContestantData>();

        foreach (var contestant in contest.Contestants)
        {
            result.Add(new ContestantData()
            {
                Id = contestant.Id,
                CountryCode = contestant.Country,
                CountryName = Repository.Countries[contestant.Country],
                Song = contestant.Song,
                Artist = contestant.Artist
            });
        }

        return result;
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
        public IReadOnlyList<RoundData> Rounds { get; set; }
    }

    private class RoundData
    {
        public string Name { get; set; }
        public IReadOnlyList<ContestantData> Contestants { get; set; }
    }

    private class ContestantData
    {
        public int Id { get; set; }
        public int? Place { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Song { get; set; }
        public string Artist { get; set; }
        public int? Running { get; set; }
        public int? Points { get; set; }
    }
}