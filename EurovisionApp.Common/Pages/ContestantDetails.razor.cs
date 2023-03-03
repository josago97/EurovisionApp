using EurovisionApp.Common.Components;
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
            Lyrics = GetLyrics(contestant),
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

    private IReadOnlyList<Lyrics> GetLyrics(Contestant contestant)
    {
        List<Lyrics> result = new List<Lyrics>();

        foreach (Lyrics lyrics in contestant.Lyrics)
        {
            if (!string.IsNullOrEmpty(lyrics.Content))
            {
                if (string.IsNullOrEmpty(lyrics.Title)) lyrics.Title = contestant.Song;
                result.Add(lyrics);
            }
        }

        return result;
    }

    private RenderFragment CreateArmorMusicSheet() => builder =>
    {
        string[] toneAndScaleName = Contestant.Tone.Split();
        string toneName = toneAndScaleName[0].Replace("b", "Flat").Replace("#", "Sharp");
        ArmorMusicSheet.Notes tone = Enum.Parse<ArmorMusicSheet.Notes>(toneName, true);
        ArmorMusicSheet.Scales scale = toneAndScaleName[1].Equals("minor", StringComparison.OrdinalIgnoreCase) 
            ? ArmorMusicSheet.Scales.Minor 
            : ArmorMusicSheet.Scales.Major;

        builder.OpenComponent(0, typeof(ArmorMusicSheet));
        builder.AddAttribute(1, nameof(ArmorMusicSheet.Tempo), Contestant.Bpm);
        builder.AddAttribute(2, nameof(ArmorMusicSheet.Tone), tone);
        builder.AddAttribute(3, nameof(ArmorMusicSheet.Scale), scale);
        builder.CloseComponent();
    };

    // Columns / Paragraph / Line
    private string[][][] GetLyricsColumns()
    {
        string qa = Contestant.Lyrics[LyricsSelectedIndex].Content;
        string[] w = qa.Split('\r');
        string[][][] result = new string[2][][];
        string[][] paragraphs = Contestant.Lyrics[LyricsSelectedIndex].Content
            .Split('\r')
            .Select(p => p.Split("\n").Where(l => !string.IsNullOrEmpty(l)).ToArray())
            .Where(p => p.Length > 0)
            .ToArray();
        int totalLines = paragraphs.Sum(p => p.Length + 1); // Lines break (+1)
        int middleLines = (int)Math.Ceiling(totalLines / 2.0);
        int middle = 0;

        while (middleLines > 0)
        {
            int lines = paragraphs[middle].Length + 1; // Line break (+1)

            // Si lo que me queda es menor que la mitad del párrafo,
            // entonces lo añado mejor a la otra mitad, ya que está
            // más cerca de la segunda mitad que de la primera
            if (middleLines < lines / 2)
                middleLines = 0;
            else
            {
                middleLines -= lines;
                middle++;
            }
        }

        List<string[]> columnParagraphs = new List<string[]>();
        int start, end;

        for (int i = 0; i < result.Length; i++)
        {
            start = i == 0 ? 0 : middle;
            end = i == 0 ? middle : paragraphs.Length;

            for (int j = start; j < end; j++)
                columnParagraphs.Add(paragraphs[j]);

            result[i] = columnParagraphs.ToArray();
            columnParagraphs.Clear();
        }

        return result;
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
        public int? Place { get; set; }
        public int? Points { get; set; }
        public int? Running { get; set; }
        public string Song { get; set; }
        public string Spokesperson { get; set; }
        public string StageDirector { get; set; }
        public string Tone { get; set; }
        public IReadOnlyList<string> Videos { get; set; }
        public string Writers { get; set; }
    }
}