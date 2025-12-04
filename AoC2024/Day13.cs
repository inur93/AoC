using AoC2024.Models;

namespace AoC2024;

internal class Day13
{
    [Test]
    [Arguments("13S", 480, 2, 0)]
    [Arguments("13", 26299, 135, 0)]
    [Arguments("13S", 875318608908, 2, 10000000000000)]
    [Arguments("13", 107824497933339, 185, 10000000000000)]
    public async Task Puzzle1(string input, long expectedTotalCost, long expectedNumberOfPrizes, long addToPrize)
    {
        var lines = (await FileParser.GetLines(input)).GetEnumerator();
        long totalCost = 0;
        long numberOfPrizes = 0;
        do
        {
            var cost = GetTokenCost(lines, addToPrize);
            totalCost += cost;
            if (cost > 0)
            {
                numberOfPrizes++;
            }
        } while (lines.MoveNext());

        await Assert.That(totalCost).IsEqualTo(expectedTotalCost);
        await Assert.That(numberOfPrizes).IsEqualTo(expectedNumberOfPrizes);
    }

    private long GetTokenCost(IEnumerator<string> lines, long addToPrize)
    {
        var _a = GetButton(lines);
        var _b = GetButton(lines);
        var p = GetPrize(lines, addToPrize);

        // a * x1 + b * x2 = p_x
        // a * y1 + b * y2 = p_y
        // a = (p_x - b * x2) / x1
        // (p_x - b * x2) / x1 * y1 + b * y2 = p_y
        // (p_x - b * x2) / x1 * y1 - p_y  = - b * y2
        // b = (p_y - (p_x - b * x2) / x1 * y1) / y2

        //var b = (p.y - (p.x - _a.x) / _a.y * _b.y) / _b.y;
        var b = (p.y * _a.x - p.x * _a.y) / (_b.y * _a.x - _b.x* _a.y);
        var a = (p.x - b * _b.x) / _a.x;

        if (!(a * _a.x + b * _b.x != p.x || a * _a.y + b * _b.y != p.y))
        {
            return a * 3 + b;
        }
        return 0;
    }

    private Vector GetPrize(IEnumerator<string> lines, long addToPrize)
    {
        lines.MoveNext();
        var line = lines.Current;
        var parts = line.Split(": ");
        var coordinates = parts[1].Split(", ");

        var x = int.Parse(coordinates[0].Replace("X=", ""));
        var y = int.Parse(coordinates[1].Replace("Y=", ""));
        return new Vector(x + addToPrize, y + addToPrize);
    }

    private Button GetButton(IEnumerator<string> enumerator)
    {
        enumerator.MoveNext();
        var line = enumerator.Current;
        var parts = line.Split(": ");
        var name = parts[0].Split(' ')[1];

        var coordinates = parts[1].Split(", ");

        var x = int.Parse(coordinates[0].Replace("X+", ""));
        var y = int.Parse(coordinates[1].Replace("Y+", ""));
        return new Button(name, x, y);
    }
}

public record Button(string Name, int x, int y);
