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

    protected PageType Page { get; set; }

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
        string relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        int slashIndex = relativePath.IndexOf('/');
        if (slashIndex > 0) relativePath = relativePath.Substring(0, slashIndex);

        Page = relativePath switch
        {
            "junior" => PageType.Junior,
            _ => PageType.Senior
        };
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnNavigationLocationChanged;
    }
}
