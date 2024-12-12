using System.Text;
using TUnit.Assertions.Extensions;

namespace AoC2024;

internal class Day10
{
    [Test]
    [Arguments("10", 709, false)]
    [Arguments("10S", 36, false)]
    [Arguments("10S1", 2, false)]
    [Arguments("10S2", 4, false)]
    [Arguments("10", 1326, true)]
    [Arguments("10S", 81, true)]
    public async Task Puzzle(string input, int result, bool sumRating)
    {
        var map = (await FileParser.GetCharacters(input))
            .Select(x =>
                x.Select(y => y == '.' ? -1 : int.Parse(y.ToString())).ToArray())
            .ToArray();

        var startingPoints = GetStartingPoints(map);

        var sum = 0;
        foreach (var point in startingPoints)
        {
            var trails = FindTrails(point, map, new Dictionary<Vector, int>());
            sum += sumRating ? trails.Values.Sum() : trails.Count;
        }
        await Assert.That(sum).IsEqualTo(result);
    }

    private Dictionary<Vector, int> FindTrails(Vector point, int[][] map, Dictionary<Vector, int> ratings, int height = 0)
    {
        if (point.Value(map) == 9)
        {
            ratings.TryAdd(point, 0);
            ratings[point] += 1;
            return ratings;
        }

        Vector[] neighbours = [
            point.Add(1, 0),
            point.Add(-1, 0),
            point.Add(0, 1),
            point.Add(0, -1)
            ];

        
        foreach (var neighbour in neighbours)
        {
            if (
                neighbour.InBounds(map) && 
                neighbour.Value(map) == height + 1)
            {
                FindTrails(neighbour, map, ratings, height + 1);
            }
        }

        return ratings;
    }


    private IEnumerable<Vector> GetStartingPoints(int[][] map)
    {
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 0)
                {
                    yield return new Vector(y, x);
                }
            }
        }
    }



    //[Test]
    //[Arguments("09", 6379677752410)]
    //[Arguments("09S", 2858)]
    public async Task Puzzle2(string input, long result)
    {

        //await Assert.That(sum).IsEqualTo(result);
    }

}
