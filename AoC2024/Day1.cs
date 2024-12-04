namespace AoC2024;

public class Day1
{
    public static async Task<(List<int> left, List<int> right)> GetInput(string file)
    {
        var input = await FileParser.ParseFile(file, "   ");
        var right = input.Select(x => x[1]).Order().ToList();
        var left = input.Select(x => x[0]).Order().ToList();
        return (left, right);
    }

    [Test]
    [Arguments("day1-test", 11)]
    [Arguments("day1", 2815556)]

    public async Task Puzzle1(string inputFile, int expectedResult)
    {
        var (left, right) = await GetInput(inputFile);
        var sum = left.Select((x, i) => Math.Abs(x - right[i])).Sum();
        await Assert.That(sum).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments("day1-test", 31)]
    [Arguments("day1", 23927637)]
    public async Task Puzzle2(string inputFile, int expectedResult)
    {
        var (left, right) = await GetInput(inputFile);
        var score = left.Select(x => x * right.Count(y => y == x)).Sum();
        await Assert.That(score).IsEqualTo(expectedResult);
    }
}
