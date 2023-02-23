using EurovisionApp.Common.Logic.Data.Models;
using Microsoft.AspNetCore.Components;
using Contest = EurovisionApp.Common.Logic.Data.Models.Eurovision.Contest;
using Contestant = EurovisionApp.Common.Logic.Data.Models.Eurovision.Contestant;

namespace EurovisionApp.Common.Pages;

public partial class ContestantDetails
{
    [Parameter]
    public int Year { get; set; }
    [Parameter]
    public int ContestantId { get; set; }
    private ContestData Contest { get; set; }
    private ContestantData Contestant { get; set; }
    private int LyricsSelectedIndex { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        Contest contest = GetContest(Year);
        Contest = GetContestData(contest);
        Contestant = GetContestantData(contest, ContestantId);
    }

    private ContestData GetContestData(Contest contest)
    {
        ContestData result = new ContestData()
        {
            Year = contest.Year,
            Country = contest.Country
        };

        return result;
    }

    private ContestantData GetContestantData(Contest contest, int contestantId)
    {
        Contestant contestant = contest.Contestants[contestantId];

        ContestantData result = new ContestantData()
        {
            Artist = contestant.Artist,
            Bpm = contestant.Bpm >= 0 ? contestant.Bpm : null,
            Broadcaster = contestant.Broadcaster,
            Conductor = contestant.Conductor,
            CountryCode = contestant.Country,
            CountryName = Repository.Countries[contestant.Country],
            Lyrics = contestant.Lyrics,
            Song = contestant.Song,
            Spokesperson = contestant.Spokesperson,
            StageDirector = contestant.StageDirector,
            Tone = contestant.Tone,
            Videos = contestant.VideoUrls,
        };

        if (!contestant.Backings.IsNullOrEmpty())
            result.Backings = string.Join(", ", contestant.Backings);

        if (!contestant.Commentators.IsNullOrEmpty())
            result.Commentators = string.Join(", ", contestant.Commentators);

        if (!contestant.Composers.IsNullOrEmpty())
            result.Composers = string.Join(", ", contestant.Composers);

        if (!contestant.Dancers.IsNullOrEmpty())
            result.Dancers = string.Join(", ", contestant.Dancers);

        if (!contestant.Writers.IsNullOrEmpty())
            result.Writers = string.Join(", ", contestant.Writers);

        return result;
    }

    private IEnumerable<RoundData> GetRoundsData(Contest contest)
    {
        List<RoundData> result = new List<RoundData>();

        foreach (var round in contest.Rounds)
        {
            result.Add(new RoundData()
            {
                Name = round.Name.ToTitleCase(),
                Contestants = GetContestantsData(contest, round.Performances)
            });
        }

        return result;
    }

    private ContestantData[] GetContestantsData(Contest contest, IEnumerable<Performance> performances)
    {
        List<ContestantData> result = new List<ContestantData>();

        foreach (var performance in performances)
        {
            var contestant = contest.Contestants[performance.ContestantId];

            result.Add(new ContestantData()
            {
                CountryCode = contestant.Country,
                CountryName = Repository.Countries[contestant.Country],
                Song = contestant.Song,
                Artist = contestant.Artist,
                Videos = contestant.VideoUrls,
                Lyrics = contestant.Lyrics
            });
        }

        return result.ToArray();
    }

    private class ContestData
    {
        public int Year { get; set; }
        public string Country { get; set; }
        public string CountryName { get; set; }
    }

    private class RoundData
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public IReadOnlyList<ContestantData> Contestants { get; set; }
    }

    private class ContestantData
    {
        public string Artist { get; set; }
        public string Backings { get; set; }
        public int? Bpm { get; set; }
        public string Broadcaster { get; set; }
        public string Commentators { get; set; }
        public string Composers { get; set; }
        public string Conductor { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Dancers { get; set; }
        public IReadOnlyList<Lyrics> Lyrics { get; set; }
        public string Song { get; set; }
        public string Spokesperson { get; set; }
        public string StageDirector { get; set; }
        public string Tone { get; set; }
        public IReadOnlyList<string> Videos { get; set; }
        public string Writers { get; set; }
    }
}