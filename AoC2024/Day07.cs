
namespace AoC2024;

public partial class Day07
{

    [Test]
    [Arguments("07S", 3749, new string[] { "+", "*" })]
    [Arguments("07", 1708857123053, new string[] { "+", "*" })]
    [Arguments("07S", 11387, new string[] { "+", "*", "||" })]
    [Arguments("07", 189207836795655, new string[] { "+", "*", "||" })]

    public async Task Puzzle1(string input, long expectedResult, string[] operators)
    {
        var data = (await FileParser.GetLines(input)).ToArray();

        long sum = 0;
        foreach (var row in data)
        {
            var parts = row.Split(":");
            var result = long.Parse(parts[0]);
            var numbers = parts[1].Trim().Split(" ").Select(long.Parse).ToArray();
            if (CanBeTrue(result, numbers[0], numbers.Skip(1).ToArray(), operators))
            {
                sum += result;
            }
        }

        await Assert.That(sum).IsEqualTo(expectedResult);
    }

    private bool CanBeTrue(long result, long sum, long[] numbers, string[] operators)
    {
        if (numbers.Length == 0)
        {
            return sum == result;
        }

        for (int i = 0; (i) < operators.Length; i++)
        {
            var op = operators[i];
            switch (op)
            {
                case "+":
                    if (CanBeTrue(result, sum + numbers[0], numbers.Skip(1).ToArray(), operators))
                    {
                        return true;
                    }
                    break;
                case "*":
                    if (CanBeTrue(result, sum * numbers[0], numbers.Skip(1).ToArray(), operators))
                    {
                        return true;
                    }
                    break;
                case "||":
                    if (CanBeTrue(result, long.Parse($"{sum}{numbers[0]}"), numbers.Skip(1).ToArray(), operators))
                    {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }
}
