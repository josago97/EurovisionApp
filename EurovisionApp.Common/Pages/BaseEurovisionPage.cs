﻿using EurovisionApp.Common.Logic.Data;
using EurovisionApp.Common.Logic.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace EurovisionApp.Common.Pages;

public abstract class BaseEurovisionPage : ComponentBase, IDisposable
{
    protected enum PageType { Junior, Senior }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public IRepository Repository { get; set; }

    protected string BasePath { get; private set; }
    protected PageType Page { get; private set; }
    protected IReadOnlyList<Contest> Contests { get; private set; }
    private string PagePath { get; set; }
    private bool IsDisposed { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        if (!IsDisposed)
        {
            string pagePath = GetPagePath(GetBasePath());

            if (PagePath != pagePath)
            {
                OnParametersSet();
                StateHasChanged();
            }
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        string basePath = GetBasePath();
        string pagePath = GetPagePath(basePath);

        if (PagePath != pagePath)
        {
            BasePath = basePath;
            PagePath = pagePath;

            switch (pagePath)
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
    }

    private string GetBasePath()
    {
        return NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
    }

    private string GetPagePath(string basePath)
    {
        string result = basePath;
        int slashIndex = result.IndexOf('/');
        if (slashIndex > 0) result = result.Substring(0, slashIndex);

        return result;
    }

    protected Contest GetContest(int year)
    {
        int contestId = year - Contests[0].Year;

        return Contests[contestId];
    }

    public virtual void Dispose()
    {
        IsDisposed = true;
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}
