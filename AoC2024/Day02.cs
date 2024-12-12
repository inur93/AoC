namespace AoC2024;

public class Day02
{
    public static async Task<IEnumerable<int[]>> GetInput(string file)
    {
        return await FileParser.GetNumbers(file, " ");
    }

    public static bool IsValidReport(int[] report)
    {
        var previous = report.First();
        if (!(report.Order().SequenceEqual(report) || report.OrderDescending().SequenceEqual(report)))
        {
            return false;
        }
        foreach (var level in report.Skip(1))
        {
            var diff = Math.Abs(level - previous);
            if (diff > 3 || diff < 1)
            {
                return false;
            }
            previous = level;
        }
        return true;
    }

    [Test]
    [Arguments("02S", 2)]
    [Arguments("02", 252)]

    public async Task Puzzle1(string inputFile, int expectedResult)
    {
        var input = await GetInput(inputFile);
        var count = input.Where(IsValidReport).Count();
        await Assert.That(count).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments("02S", 4)]
    [Arguments("02", 324)]
    public async Task Puzzle2(string inputFile, int expectedResult)
    {
        var input = await GetInput(inputFile);
        var count = input.Where(report =>
        {
            return report.Select((level, index) => report.Where((_, i) => i != index).ToArray()).Any(IsValidReport);
        }).Count();

        await Assert.That(count).IsEqualTo(expectedResult);
    }
}
