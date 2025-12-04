

namespace AoC2024;


public class Day06
{
    private record Vector(int x, int y)
    {
        internal Vector Add(Vector other)
        {
            return new Vector(other.x + x, other.y + y);
        }

        internal Vector Add(int x, int y)
        {
            return new Vector(this.x + x, this.y + y);
        }

        internal Vector RotateRight()
        {
            return new Vector(-y, x);
        }

        internal Vector Sub(Vector vector)
        {
            return new Vector(x - vector.x, y - vector.y);
        }

        internal T Value<T>(T[][] map)
        {
            return map[x][y];
        }

        internal bool InBounds<T>(T[][] map)
        {
            return x >= 0 && x < map.Length && y >= 0 && y < map[0].Length;
        }
    }
    [Skip("while debugging")]
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

    [Skip("too slow")]
    [Test]
    [Arguments("06", 5212)]
    //[Arguments("06S", 6)]
    public async Task Puzzle2(string input, int result)
    {
        var map = (await FileParser.GetCharacters(input)).ToArray();

        
        var path = GetPath(map, out var _); // .Distinct().ToList();

        var tasks = path.Chunk(500).Select(chunk =>
        {
            return Task.Run(() =>
            {
                var loopObstacles = new HashSet<Vector>();
                // clone map
                var mapCopy = map.Select(r => r.ToArray()).ToArray();

                for (var i = 1; i < path.Count; i++)
                {
                    var obstacle = path[i];
                    if (mapCopy[obstacle.y][obstacle.x] == '^') // obstacle is in guard position
                    {
                        continue;
                    }
                    mapCopy[obstacle.y][obstacle.x] = '#'; // add obstacle
                    GetPath(mapCopy, out var isLooping);

                    if (isLooping)
                    {
                        loopObstacles.Add(obstacle);
                    }
                    mapCopy[obstacle.y][obstacle.x] = '.'; // remove obstacle
                }
                return loopObstacles;
            });
        });

        var results = await Task.WhenAll(tasks);
        var total = new HashSet<Vector>();
        foreach(var r in results)
        {
            total = total.Union(r).ToHashSet();
        }
        ////Vector? previousGuardPosition = null;
        ////map[path[0].y][path[0].x] = '.'; // remove guard
        //for (var i = 1; i < path.Count; i++)
        //{
        //    //var guard = path[i - 1];
        //    var obstacle = path[i];
        //    //var dir = obstacle.Sub(guard);
        //    //dir = new Vector(dir.y, dir.x);
        //    if(map[obstacle.y][obstacle.x] == '^') // obstacle is in guard position
        //    {
        //        continue;
        //    }
        //    map[obstacle.y][obstacle.x] = '#'; // add obstacle
        //    //if(previousGuardPosition != null)
        //    //{
        //    //    map[guard.y][guard.x] = '^'; // move guard
        //    //    map[previousGuardPosition.y][previousGuardPosition.x] = '.'; // remove guard
        //    //}
        //    //previousGuardPosition = guard;

        //    GetPath(map, out var isLooping);

        //     if (isLooping)
        //    { 
        //        loopObstacles.Add(obstacle);
        //    }
        //    map[obstacle.y][obstacle.x] = '.'; // remove obstacle
        //}
        await Assert.That(total.Count).IsEqualTo(result);
    }

    private List<Vector> GetPath(char[][] map, out bool isLooping, Vector? startDir = null)
    {
        isLooping = false;
        var guard = map.Select((r, i) => new Vector(r.ToList().IndexOf('^'), i)).First(v => v.x > 0);
        var visited = new HashSet<(long px, long py, long dx, long dy)>();
        var path = new List<Vector>();
        var direction = startDir ?? new Vector(0, -1);
        while (InBounds(guard, map))
        {
            if (path.Count > 6000) break; // failsafe ?
            var newPosition = guard.Add(direction);
            while (IsWall(newPosition, map))
            {
                direction = direction.RotateRight();
                newPosition = guard.Add(direction);
            }
            path.Add(guard);
            if (visited.Contains((newPosition.x, newPosition.y, direction.x, direction.y)))
            {
                isLooping = true;
                break;
            }
            visited.Add((guard.x, guard.y, direction.x, direction.y));
            guard = newPosition;
        }
        return path;
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
