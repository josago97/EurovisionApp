using EurovisionApp.Common.Logic.Data.Models.Eurovision;

namespace EurovisionApp.Common.Logic.Data;

public interface IRepository
{
    Dictionary<string, string> Countries { get; }
    IReadOnlyList<Contest> SeniorContests { get; }
    IReadOnlyList<Contest> JuniorContests { get; }

    Task InitAsync();
}
