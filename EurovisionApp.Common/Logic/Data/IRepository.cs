using EurovisionApp.Common.Logic.Data.Models;

namespace EurovisionApp.Common.Logic.Data;

public interface IRepository
{
    IDictionary<string, string> Countries { get; }
    IReadOnlyList<Contest> SeniorContests { get; }
    IReadOnlyList<Contest> JuniorContests { get; }

    Task InitAsync();
}
