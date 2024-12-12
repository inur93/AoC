namespace AoC2024;

public class Day01
{
    public static async Task<(List<int> left, List<int> right)> GetInput(string file)
    {
        var input = await FileParser.GetNumbers(file, "   ");
        var right = input.Select(x => x[1]).Order().ToList();
        var left = input.Select(x => x[0]).Order().ToList();
        return (left, right);
    }

    [Test]
    [Arguments("01S", 11)]
    [Arguments("01", 2815556)]

    public async Task Puzzle1(string inputFile, int expectedResult)
    {
        var (left, right) = await GetInput(inputFile);
        var sum = left.Select((x, i) => Math.Abs(x - right[i])).Sum();
        await Assert.That(sum).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments("01S", 31)]
    [Arguments("01", 23927637)]
    public async Task Puzzle2(string inputFile, int expectedResult)
    {
        var (left, right) = await GetInput(inputFile);
        var score = left.Select(x => x * right.Count(y => y == x)).Sum();
        await Assert.That(score).IsEqualTo(expectedResult);
    }
}
