using System.Numerics;

namespace AoC2024;

internal class Day14
{
    private const int WindowWidth = 19;
    [Test]
    [Arguments("14S", 12, 100, 11, 7)]
    [Arguments("14", 230172768, 100, 101, 103)]
    public async Task Puzzle1(string input, int expected, int seconds, int mapWidth, int mapHeight)
    {
        var robots = (await FileParser.GetLines(input, " ")).Select(GetRobot).ToArray();

        var positions = robots.Select(r => r.GetPositionAfter(seconds, mapWidth, mapHeight)).ToArray();

        var center = new Vector2([mapWidth / 2, mapHeight / 2]);
        var q1 = positions.Count(p => p.X > center.X && p.Y > center.Y);
        var q2 = positions.Count(p => p.X < center.X && p.Y > center.Y);
        var q3 = positions.Count(p => p.X < center.X && p.Y < center.Y);
        var q4 = positions.Count(p => p.X > center.X && p.Y < center.Y);

        var result = q1 * q2 * q3 * q4;
        await Assert.That(result).IsEqualTo(expected);
    }

    [Skip("not working")]
    [Test]
    //[Arguments("14S", 12, 1, 6*16, 11, 7)]
    [Arguments("14S", 12, 1, 400, 11, 7)]
    //[Arguments("14", 230172768, 100, 101, 103)]
    public async Task Puzzle2(string input, int expected, int secondsFrom, int size, int mapWidth, int mapHeight)
    {
        var robots = (await FileParser.GetLines(input, " ")).Select(GetRobot).ToArray();
        using var file = new StreamWriter("output.txt");
        var output = Enumerable.Range(0, (mapHeight + 1) * (size+WindowWidth)/WindowWidth).Select(x => Enumerable.Range(0, (mapWidth + 1) * WindowWidth).Select(y => " ").ToArray()).ToArray();
        
        for (var i = 0; i < output.Length; i++)
        {
            for (var j = 0; j < output[i].Length; j++)
            {
                if (j > 1 && (j + 1) % (mapWidth + 1) == 0)
                    output[i][j] = "|";
                if (i > 1 && (i + 1) % (mapHeight + 1) == 0)
                    output[i][j] = "-";
            }
        }
        for (int i = 1; i < size; i++)
        {
            var seconds = i + secondsFrom;
            // i = 17
            // offsetHeight = 17;
            // i / ()
            var width = mapWidth + 1;
            var offsetWidth = ((i - 1) % WindowWidth) * width;
            var offsetHeight = GetHeightOffset(i - 1, mapHeight);
            var positions = robots.Select(r => r.GetPositionAfter(i, mapWidth, mapHeight)).ToArray();
            foreach (var p in positions)
            {
                var x = (int)p.X;
                var y = (int)p.Y;
                try
                {
                    output[offsetHeight + y][offsetWidth + x] = "#";
                }
                catch
                {

                }
            }
        }

        foreach (var row in output)
        {
            foreach (var col in row)
            {
                await file.WriteAsync(col);
            }
            await file.WriteLineAsync();
        }

        await Assert.That(0).IsEqualTo(expected);
    }

    private int GetHeightOffset(int i, int mapHeight)
    {
        var remainder = i - i % WindowWidth;

        return (remainder / WindowWidth) * (mapHeight + 1);
    }
    private Robot GetRobot(string[] robot)
    {

        var position = robot[0].Replace("p=", "").Split(',').Select(int.Parse).ToArray();
        var velocity = robot[1].Replace("v=", "").Split(',').Select(int.Parse).ToArray();

        return new Robot(new Vector2(position[0], position[1]), new Vector2(velocity[0], velocity[1]));

    }

    private record Robot(Vector2 Position, Vector2 Velocity)
    {
        public Vector2 GetPositionAfter(int seconds, int mapWidth, int mapHeight)
        {
            var newPosition = Position + Velocity * seconds;
            newPosition.X = newPosition.X % mapWidth;
            newPosition.Y = newPosition.Y % mapHeight;
            if (newPosition.X < 0)
                newPosition.X = mapWidth + newPosition.X;
            if (newPosition.Y < 0)
                newPosition.Y = mapHeight + newPosition.Y;

            return new Vector2([newPosition.X, newPosition.Y]);
        }
    }
}
