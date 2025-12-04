using System.Reflection;
using System.Text.RegularExpressions;

namespace AoC2025;

public class Day3
{
    //private class Cell
    //{
    //    public int Value { get; set; }
    //    public bool IsOn { get; set; }
    //}
    [Theory]
    [InlineData("AoC2025.Input.d3ps.txt", 357, 2)]
    [InlineData("AoC2025.Input.d3p1.txt", 17412, 2)]
    [InlineData("AoC2025.Input.d3ps.txt", 3121910778619, 12)]
    [InlineData("AoC2025.Input.d3p1.txt", 172681562473501, 12)]
    public void Puzzle(string inputFile, long expected, int turnOnCount)
    {
        // get input from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        Stream input = assembly.GetManifestResourceStream(inputFile);
        // use text reader
        using StreamReader reader = new StreamReader(input);
        string content = reader.ReadToEnd();

        var lines = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        List<long> joltages = [];


        foreach (var bank in lines)
        {
            var countLeft = turnOnCount;
            var battery = bank.ToCharArray(); // .Select(x => new Cell { Value = int.Parse(x.ToString()), IsOn = false }).ToArray();
            var start = 0;
            var selectedCells = new List<string>();
            while (countLeft > 0)
            {
                var section = battery[start..(battery.Length - (countLeft - 1))];
                var cell = section.Max();
                start += section.IndexOf(cell)+1;
                selectedCells.Add(cell.ToString());
                countLeft--;
            }

            //var firstMax = battery[0..(battery.Length - 1)].Max();
            //var first = battery.IndexOf(firstMax);
            //var secondMax = battery[(first + 1)..].Max();

            var joltage = string.Join("", selectedCells);

            joltages.Add(long.Parse($"{joltage}"));
        }

        Assert.Equal(expected, joltages.Sum());
    }


}
