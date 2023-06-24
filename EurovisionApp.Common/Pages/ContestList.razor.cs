using EurovisionApp.Common.Logic.Data.Models;
namespace EurovisionApp.Common.Pages;

public partial class ContestList
{
    private string Title { get; set; }
    private IEnumerable<ContestData> AllContests { get; set; }
    private new IEnumerable<ContestData> Contests { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        switch (Page)
        {
            case PageType.Junior:
                Title = "Junior Eurovision";
                break;

            case PageType.Senior:
                Title = "Eurovision";
                break;
        }

        Contests = AllContests = GetContests(base.Contests);
    }

    private IEnumerable<ContestData> GetContests(IReadOnlyList<Contest> contests)
    {
        List<ContestData> result = new List<ContestData>();

        for (int i = 0; i < contests.Count; i++)
        {
            var contest = contests[i];
            string countryCode = contest.HostCountry;

            result.Add(new ContestData()
            {
                Id = i,
                CountryCode = countryCode,
                CountryName = Repository.Countries[countryCode],
                City = contest.City,
                Year = contest.Year
            });
        }

        return result;
    }

    private void Search(string query)
    {
        IEnumerable<ContestData> result = AllContests;

        if (!string.IsNullOrEmpty(query))
        {
            result = AllContests.Where(c =>
                c.CountryName.Contains(query, StringComparison.OrdinalIgnoreCase)
                || c.City.Contains(query, StringComparison.OrdinalIgnoreCase)
                || c.Year.ToString().Contains(query, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        Contests = result;
    }

    private class ContestData
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        public int Year { get; set; }
    }
}