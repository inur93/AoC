using System.Reflection;
using System.Text.RegularExpressions;

namespace AoC2025;

public class Day5
{
    //private class Cell
    //{
    //    public int Value { get; set; }
    //    public bool IsOn { get; set; }
    //}
    [Theory]
    [InlineData("AoC2025.Input.d5ps.txt", 3, 14)]
    //[InlineData("AoC2025.Input.d5p1.txt", 509, 123)]
    //[InlineData("AoC2025.Input.d5ps.txt", 14, true)]
    //[InlineData("AoC2025.Input.d5p1.txt", 8758, true)]
    public void Puzzle(string inputFile, long expected, long extedTotalFreshIngredients)
    {
        // get input from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        Stream input = assembly.GetManifestResourceStream(inputFile);
        // use text reader
        using StreamReader reader = new StreamReader(input);
        string content = reader.ReadToEnd();

        var lines = content.Split("\r\n");
        var splitIndex = Array.IndexOf(lines, "");
        List<(long from, long to)> ranges = lines[0..splitIndex]
            .Select(x => x.Split('-'))
            .Select(x => (long.Parse(x[0]), long.Parse(x[1])))
            .ToList();

        var availableIngredients = lines[(splitIndex + 1)..].Select(long.Parse);
        var mergedRanges = new List<(long from, long to)>();
        foreach (var range in ranges)
        {
            var overlappingFromRanges = mergedRanges.Where(x => x.from <= range.from && x.from >= range.from).Select(x => x.to);
            var overlappingToRanges = mergedRanges.Where(x => x.from <= range.to && x.to >= range.to).Select(x => x.from);

            var newFrom = range.from;
            var newTo = range.to;
            if (overlappingFromRanges.Any())
            {
                newFrom = Math.Min(overlappingFromRanges.Max(), range.to) + 1;
            }
            if (overlappingToRanges.Any())
            {
                newTo = Math.Min(overlappingToRanges.Min(), range.from) - 1;
            }
            
            if(newFrom <= newTo)
            {
                mergedRanges.Add((newFrom, newTo));
            }

        }

        int freshIngredients = 0;
        foreach (var ingredient in availableIngredients)
        {
            if (mergedRanges.Any(r => ingredient >= r.from && ingredient <= r.to))
            {
                freshIngredients++;
            }
        }

        Assert.Equal(extedTotalFreshIngredients, mergedRanges.Sum(r => r.to - r.from + 1));
        Assert.Equal(expected, freshIngredients);
    }


}
