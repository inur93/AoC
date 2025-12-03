using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode2025;

public class Day3
{
    private class Cell
    {
        public int Value { get; set; }
        public bool IsOn { get; set; }
    }
    [Theory]
    [InlineData("AdventOfCode2025.Input.d3ps.txt", 357, 2)]
    [InlineData("AdventOfCode2025.Input.d3p1.txt", 17412, 2)]
    //[InlineData("AdventOfCode2025.Input.d3ps.txt", 4174379265, 12)]
    //[InlineData("AdventOfCode2025.Input.d3p1.txt", 48778605167, 12)]
    public void Puzzle1(string inputFile, long expected, int turnOnCount)
    {
        // get input from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        Stream input = assembly.GetManifestResourceStream(inputFile);
        // use text reader
        using StreamReader reader = new StreamReader(input);
        string content = reader.ReadToEnd();

        var lines = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        List<int> joltages = [];
        

        foreach (var bank in lines)
        {
            var countLeft = turnOnCount;
            var battery = bank.ToCharArray().Select(x => new Cell { Value = int.Parse(x.ToString()), IsOn = false }).ToArray();
            
            while(countLeft > 0)
            {
                //var cell = battery[0..(battery.Length-countLeft)].First(x => x.Value == ordered[i].Value && !x.IsOn);
                //cell.IsOn = true;
                var max = battery[0..(battery.Length - (countLeft - 1))].Where(x => x.IsOn == false).MaxBy(x => x.Value);
                max.IsOn = true;
                countLeft--;
                
            }

            //var firstMax = battery[0..(battery.Length - 1)].Max();
            //var first = battery.IndexOf(firstMax);
            //var secondMax = battery[(first + 1)..].Max();

            var joltage = string.Join("", battery.Where(x => x.IsOn).Select(x => x.Value));

            joltages.Add(int.Parse($"{joltage}"));
        }

        Assert.Equal(expected, joltages.Sum());
    }


}
