using System.Reflection;
using System.Text.RegularExpressions;

namespace AoC2025;

public class Day4
{
    //private class Cell
    //{
    //    public int Value { get; set; }
    //    public bool IsOn { get; set; }
    //}
    [Theory]
    [InlineData("AoC2025.Input.d4ps.txt", 13, true)]
    [InlineData("AoC2025.Input.d4p1.txt", 1397, true)]
    [InlineData("AoC2025.Input.d4ps.txt", 43, false)]
    [InlineData("AoC2025.Input.d4p1.txt", 8758, false)]
    public void Puzzle(string inputFile, long expected, bool singleIteration)
    {
        // get input from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        Stream input = assembly.GetManifestResourceStream(inputFile);
        // use text reader
        using StreamReader reader = new StreamReader(input);
        string content = reader.ReadToEnd();

        var lines = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

        var map = lines.Select(x => x.ToCharArray()).ToArray();
        int totalRemovedRolls = 0;
        List<(int x, int y)> currentRemovedRolls = [];
        do
        {
            currentRemovedRolls = [];
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] != '@')
                    {
                        continue;
                    }
                    var count = new (int x, int y)[]
                    {
                    (x-1, y), // left
                    (x+1, y), // right
                    (x, y-1), // up
                    (x, y+1), // down
                    (x-1, y-1), // up-left
                    (x+1, y-1), // up-right
                    (x-1, y+1), // down-left
                    (x+1, y+1)  // down-right
                    }
                    .Where(p => p.x >= 0 && p.x < map[y].Length && p.y >= 0 && p.y < map.Length)
                    .Where(p => map[p.y][p.x] == '@')
                    .Count();
                    if (count < 4)
                    {
                        currentRemovedRolls.Add((x, y));
                    }
                }
            }
            totalRemovedRolls += currentRemovedRolls.Count;
            foreach (var roll in currentRemovedRolls)
            {
                map[roll.y][roll.x] = '.';
            }
        } while (!singleIteration && currentRemovedRolls.Count > 0);


        Assert.Equal(expected, totalRemovedRolls);
    }


}
