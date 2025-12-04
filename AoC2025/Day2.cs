using System.Reflection;
using System.Text.RegularExpressions;

namespace AoC2025;

public class Day2
{
    [Theory]
    [InlineData("AoC2025.Input.d2ps.txt", 1227775554, false)]
    [InlineData("AoC2025.Input.d2p1.txt", 28844599675, false)]
    [InlineData("AoC2025.Input.d2ps.txt", 4174379265, true)]
    [InlineData("AoC2025.Input.d2p1.txt", 48778605167, true)]
    public void Puzzle1(string inputFile, long expected, bool multipleMatches)
    {
        // get input from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        Stream input = assembly.GetManifestResourceStream(inputFile);
        // use text reader
        using StreamReader reader = new StreamReader(input);
        string content = reader.ReadToEnd();

        var lines = content.Split(',', StringSplitOptions.RemoveEmptyEntries);
        List<string> matches = [];
        foreach (var line in lines)
        {
            var parts = line.Split('-');
            var first = long.Parse(parts[0]);
            var second = long.Parse(parts[1]);

            for (long i = first; i <= second; i++)
            {
                var str = i.ToString();
                var chars = str.ToCharArray();
                var repititions = chars.GroupBy(c => c).OrderBy(c => c.Count()).First().Count();

                for (var rep = repititions; rep > 1; rep--)
                {
                    if (str.Length % rep != 0 || (!multipleMatches && rep != 2)) continue;
                    var val = str[..(str.Length / rep)];
                    var test = new Regex(@$"^({val}){{{rep}}}$").Match(str);

                    if (test?.Success == true)
                    {
                        matches.Add(str);
                    }
                }
            }
        }

        Assert.Equal(expected, matches.Distinct().Select(long.Parse).Sum());
    }


}
