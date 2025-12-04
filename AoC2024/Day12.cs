
using System.Runtime.CompilerServices;
using AoC2024.Models;

namespace AoC2024;

internal class Day12
{
    [Test]
    [Arguments("12S", 140)]
    [Arguments("12S1", 772)]
    [Arguments("12S2", 1930)]
    [Arguments("12", 1550156)]
    public async Task Puzzle1(string input, int result)
    {
        var regions = GetRegions(input);
        var totalPrize = 0;
        await foreach (var region in regions)
        {
            var sides = FindSides(region);
            var area = region.Count;
            var perimeter = region.Sum(x => x.Perimeters);
            var price = area * perimeter;
            totalPrize += price;
        }

        await Assert.That(totalPrize).IsEqualTo(result);
    }

    [Test]
    //[Arguments("12S", 80)]
    //[Arguments("12S2", 1206)]
    //[Arguments("12S3", 236)]
    [Arguments("12", 949516)] // too high
    public async Task Puzzle2(string input, int result)
    {
        var regions = GetRegions(input);
        var totalPrize = 0;
        await foreach (var region in regions)
        {
            var sides = FindSides(region);
            var area = region.Count;
            var price = area * sides.Count;
            totalPrize += price;
        }

        await Assert.That(totalPrize).IsEqualTo(result);
    }

    private async IAsyncEnumerable<List<Plot>> GetRegions(string input)
    {
        var map = (await FileParser.GetCharacters(input)).ToArray();

        var visited = new HashSet<Vector>();
        var totalPrize = 0;
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                var position = new Vector(i, j);
                if (visited.Contains(position))
                    continue;

                var region = FindRegion(map, position, position.Value(map), visited);
                yield return region;
            }
        }

    }

    private List<Side> FindSides(List<Plot> region)
    {
        var visited = new HashSet<(char side, Vector position)>();
        var sides = new List<Side>();
        foreach (var plot in region)
        {
            foreach (var side in plot.Sides)
            {
                if (visited.Contains((side, plot.Position)))
                    continue;

                var horizontal = side == 'T' || side == 'B';

                var option = new Side
                {
                    Value = side,
                    Horizontal = horizontal,
                    Position = horizontal ? plot.Position.x : plot.Position.y,
                    From = horizontal ? plot.Position.y : plot.Position.x,
                    To = horizontal ? plot.Position.y : plot.Position.x
                };

                var existing = sides.FirstOrDefault(x =>
                x.Value == side &&
                x.Horizontal == horizontal &&
                x.Position == option.Position &&
                (x.From - 1 == option.From || x.To + 1 == option.From));

                visited.Add((side, plot.Position));

                if (existing != default)
                {
                    existing.From = Math.Min(existing.From, option.From);
                    existing.To = Math.Max(existing.To, option.To);
                    continue;
                }

                sides.Add(option);
            }
        }
        return sides;
    }

    private List<Plot> FindRegion(char[][] map, Vector position, char region, HashSet<Vector> visited)
    {
        if (visited.Contains(position) || position.Value(map) != region)
            return new List<Plot>();

        var neighbours = new List<(char side, Vector position)>
        {
            ('B', position.Add(1, 0)),
            ('T', position.Add(-1, 0)),
            ('R', position.Add(0, 1)),
            ('L', position.Add(0, -1)),
        };
        
        var plot = new Plot
        {
            Position = position,
            Region = region,
            Sides = neighbours.Where(x => !x.position.InBounds(map) || x.position.Value(map) != region).Select(x => x.side).ToArray(),
            Perimeters = 4 - neighbours.Count(x => x.position.InBounds(map) && x.position.Value(map) == region)
        };

        visited.Add(position);

        var list = new List<Plot> { plot };
        list.AddRange(neighbours.Where(x => x.position.InBounds(map)).SelectMany(x => FindRegion(map, x.position, region, visited)));
        return list;
    }
}

public class Side
{
    public char Value { get; set; }

    public bool Horizontal { get; set; }

    public long From { get; set; }

    public long To { get; set; }
    public long Position { get; internal set; }
}
public class Plot
{
    public char[] Sides { get; set; }
    public Vector Position { get; set; }
    public char Region { get; set; }
    public int Perimeters { get; set; }
}
