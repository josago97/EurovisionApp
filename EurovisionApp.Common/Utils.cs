using System.Text;
using EurovisionApp.Common.Logic.Data.Models;

namespace EurovisionApp.Common;

public static class Utils
{
    private static readonly string AssemblyName = typeof(Utils).Assembly.GetName().Name;

    public static string GetStaticFileUrl(string wwwrootPath)
    {
        return $"_content/{AssemblyName}/{wwwrootPath}";
    }

    public static string GetDisplayRoundName(string roundName)
    {
        return roundName.ToLower() switch
        {
            "final" => "Grand Final",
            "semifinal" => "Semifinal",
            "semifinal1" => "Semifinal 1",
            _ => "Semifinal 2",
        };
    }
}
