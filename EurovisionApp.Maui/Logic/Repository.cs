using EurovisionApp.Common.Logic.Data;

namespace EurovisionApp.Maui.Logic;

public class Repository : BaseRepository
{
    private readonly IServiceProvider _serviceProvider;

    public Repository(IServiceProvider serviceProvider) : base()
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task<string> GetJsonDataAsync(string filename)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        HttpClient client = scope.ServiceProvider.GetRequiredService<HttpClient>();
        
        return await client.GetStringAsync(filename);
    }
}
