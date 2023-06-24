using System.Text.Json;
using EurovisionApp.Common.Logic.Data.Models;

namespace EurovisionApp.Common.Logic.Data;

public abstract class BaseRepository : IRepository
{
    protected const string COUNTRIES_FILENAME = "countries.json";
    protected const string CONTESTS_FILENAME = "eurovision.json";
    protected const string JUNIOR_CONTESTS_FILENAME = "junior.json";

    private bool _isInitialized = false;

    public IDictionary<string, string> Countries { get; private set; }
    public IReadOnlyList<Contest> SeniorContests { get; private set; }
    public IReadOnlyList<Contest> JuniorContests { get; private set; }

    public async Task InitAsync()
    {
        if (!_isInitialized)
        {
            await Task.WhenAll(GetCountriesAsync(), GetSeniorContestsAsync(), GetJuniorContestsAsync());
            _isInitialized = true;
        }
        else
        {
            await Task.CompletedTask;
        }
    }

    private async Task GetCountriesAsync()
    {
        Countries = await GetAsync<Dictionary<string, string>>(COUNTRIES_FILENAME);
    }

    private async Task GetSeniorContestsAsync()
    {
        SeniorContests = await GetAsync<Contest[]>(CONTESTS_FILENAME);
    }

    private async Task GetJuniorContestsAsync()
    {
        JuniorContests = await GetAsync<Contest[]>(JUNIOR_CONTESTS_FILENAME);
    }

    private async Task<T> GetAsync<T>(string filename)
    {
        string json = await GetJsonDataAsync(Utils.GetStaticFileUrl("data/" + filename));

            JsonSerializerOptions _jsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        //return JsonSerializer.Generic.Utf16.Deserialize<T, IncludeNullsCamelCaseResolver<char>>(json);
    }

    protected abstract Task<string> GetJsonDataAsync(string filename);
}
