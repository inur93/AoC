using System.Reflection;

namespace AdventOfCode2025;

public class Day1
{
    [Theory]
    [InlineData("AdventOfCode2025.Input.d1ps.txt", 3, false)]
    [InlineData("AdventOfCode2025.Input.d1p1.txt", 1023, false)]
    [InlineData("AdventOfCode2025.Input.d1ps.txt", 6, true)]
    [InlineData("AdventOfCode2025.Input.d1p1.txt", 5899, true)]
    public void Puzzle1(string inputFile, int expected, bool countRoundTrips)
    {
        // get input from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        Stream input = assembly.GetManifestResourceStream(inputFile);
        // use text reader
        using StreamReader reader = new StreamReader(input);
        string content = reader.ReadToEnd();

        var lines = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

        var zeroCounts = 0;
        var dial = 50;
        foreach (var line in lines)
        {
            var direction = line[0] == 'L' ? -1 : 1;

            int number = int.Parse(line[1..]) * direction;

            int prevDial = dial;
            dial += number;
            var roundTrips = Math.Abs(dial / 100);


            if (countRoundTrips)
            {
                if (prevDial > 0 && dial < 0) zeroCounts++;
                if (roundTrips == 0 && dial == 0) zeroCounts++;
                zeroCounts += roundTrips;
            }

            dial %= 100;
            if (dial < 0) dial = 100 + dial;

            if (!countRoundTrips && dial == 0) zeroCounts++;
        }


        Assert.Equal(expected, zeroCounts);
    }


}
