using System.Drawing;

namespace AoC2024;

public class Day06
{
    [Test]
    [Arguments("06", 5212)]
    [Arguments("06S", 41)]
    public async Task Puzzle1(string input, int result)
    {
        var map = (await FileParser.GetCharacters(input)).ToArray();

        var path = GetPath(map, out var _);

        var unique = path.Distinct().ToList();
        await Assert.That(unique.Count).IsEqualTo(result);
    }

    [Skip("not working")]
    [Test]
    //[Arguments("06", 5212)]
    [Arguments("06S", 6)]
    public async Task Puzzle2(string input, int result)
    {
        var map = (await FileParser.GetCharacters(input)).ToArray();

        var loopCount = 0;
        var path = GetPath(map, out var _);
        map[path[0].y][path[0].x] = '.';
        for (var i = 1; i < path.Count; i++)
        {
            var guard = path[i - 1];
            var obstacle = path[i];
            var dir = obstacle.Sub(guard);
            dir = new Vector(dir.y, dir.x);
            map[obstacle.y][obstacle.x] = '#';
             if (TestLoop(map, guard, obstacle, dir))
            {
                loopCount++;
            }
            map[obstacle.y][obstacle.x] = '.';
        }
        await Assert.That(loopCount).IsEqualTo(result);
    }

    private bool TestLoop(char[][] map, Vector guard, Vector obstacle, Vector direction)
    {
        var visited = new List<Vector> { guard };
        var turns = 0;
        while (InBounds(guard, map))
        {
            if (turns > 4)
            {
                return false;
            }
            visited.Add(guard);
            var newPosition = guard.Add(direction);
            while (IsWall(newPosition, map))
            {
                direction = direction.RotateRight();
                newPosition = guard.Add(direction);
                turns++;
                if (turns == 4 && visited.Contains(newPosition))
                {
                    return true;
                }
            }
            guard = newPosition;
        }
        return false;
    }

    private List<Vector> GetPath(char[][] map, out bool isLooping)
    {
        isLooping = false;
        var guard = map.Select((r, i) => new Vector(r.ToList().IndexOf('^'), i)).First(v => v.x > 0);
        var visited = new List<Vector>();
        var direction = new Vector(0, -1);
        var loopTurns = 0;
        var turnIncrement = 0;
        while (InBounds(guard, map))
        {
            if (visited.Contains(guard))
            {
                loopTurns += turnIncrement;
            }
            //if (loopTurns > 4)
            //{
            //    isLooping = true;
            //    return visited;
            //}
            turnIncrement = 0;
            visited.Add(guard);
            var newPosition = guard.Add(direction);
            while (IsWall(newPosition, map))
            {
                direction = direction.RotateRight();
                newPosition = guard.Add(direction);
                turnIncrement = 1;
            }
            guard = newPosition;
        }
        return visited;
    }

    private bool IsWall(Vector v, char[][] map)
    {
        try
        {
            return map[v.y][v.x] == '#';
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private bool InBounds(Vector v, char[][] map)
    {
        return v.x >= 0 && v.x < map[0].Length && v.y >= 0 && v.y < map.Length;
    }
}
