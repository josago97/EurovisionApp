using EurovisionApp.Common.Logic.Data;
using EurovisionApp.Common.Logic.Data.Models.Eurovision;
using Microsoft.AspNetCore.Components;

namespace EurovisionApp.Common.Pages;

public partial class BaseContestList
{
    [Inject]
    public IRepository Repository { get; set; }
    [Parameter]
    public string Title { get; set; }
    [Parameter]
    public IReadOnlyList<Contest> AllContests { get; set; }
    private IReadOnlyList<Contest> Contests { get; set; }

    protected override void OnInitialized()
    {
        Contests = AllContests;
    }

    private void Search(string query)
    {
        IReadOnlyList<Contest> result = AllContests;

        if (!string.IsNullOrEmpty(query))
        {
            result = AllContests.Where(c =>
                c.City.Contains(query, StringComparison.OrdinalIgnoreCase)
                || c.Year.ToString().Contains(query, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        Contests = result;
    }
}