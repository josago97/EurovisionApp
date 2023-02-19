using EurovisionApp.Common.Logic.Data;
using EurovisionApp.Common.Logic.Data.Models.Eurovision;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace EurovisionApp.Common.Pages;

public abstract class BaseEurovisionPage : ComponentBase, IDisposable
{
    protected enum PageType
    {
        Junior,
        Senior
    }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public IRepository Repository { get; set; }

    protected string Path { get; set; }
    protected PageType Page { get; set; }
    protected IReadOnlyList<Contest> Contests { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnNavigationLocationChanged;
    }

    private void OnNavigationLocationChanged(object sender, LocationChangedEventArgs e)
    {
        OnParametersSet();
        StateHasChanged();
    }

    protected override void OnParametersSet()
    {
        string relativePath = Path = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        int slashIndex = relativePath.IndexOf('/');
        if (slashIndex > 0) relativePath = relativePath.Substring(0, slashIndex);

        switch (relativePath)
        {
            case "junior":
                Page = PageType.Junior;
                Contests = Repository.JuniorContests;
                break;

            default:
                Page = PageType.Senior;
                Contests = Repository.SeniorContests;
                break;
        }
    }

    protected Contest GetContest(int year)
    {
        int contestId = year - Contests[0].Year;

        return Contests[contestId];
    }

    public virtual void Dispose()
    {
        NavigationManager.LocationChanged -= OnNavigationLocationChanged;
    }
}
