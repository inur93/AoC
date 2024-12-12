using System.Text.RegularExpressions;

namespace AoC2024;

public partial class Day03
{
    [Test]
    [Arguments("03S", 161)]
    [Arguments("03", 157621318)]

    public async Task Puzzle1(string inputFile, int expectedResult)
    {
        var input = await FileParser.GetText(inputFile);

        var sum = Puzzle1().Matches(input).Select(match =>
            {
                var a = int.Parse(match.Groups[2].Value);
                var b = int.Parse(match.Groups[3].Value);
                return a * b;
            }).Sum();

        await Assert.That(sum).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments("03BS", 48)]
    [Arguments("03B", 79845780)]

    public async Task Puzzle2(string inputFile, int expectedResult)
    {
        var input = await FileParser.GetText(inputFile);

        var sum = 0;
        var enabled = true;
        foreach (Match match in Puzzle2().Matches(input))
        {
            if (match.Groups[2].Value == "mul" && enabled)
            {
                var a = int.Parse(match.Groups[3].Value);
                var b = int.Parse(match.Groups[4].Value);
                sum += a * b;
            }
            else if (match.Groups[5].Success)
            {
                enabled = true;
            }
            else if (match.Groups[6].Success)
            {
                enabled = false;
            }
        }

        await Assert.That(sum).IsEqualTo(expectedResult);
    }

    [GeneratedRegex("(mul)\\((\\d+),(\\d+)\\)")]
    private static partial Regex Puzzle1();

    [GeneratedRegex("((mul)\\((\\d+),(\\d+)\\))|(do\\(\\))|(don't\\(\\))")]
    private static partial Regex Puzzle2();


}
