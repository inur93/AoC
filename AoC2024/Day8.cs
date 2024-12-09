namespace AoC2024;

public class Day8
{

    [Test]
    [Arguments("08S", 14, false)]
    [Arguments("08", 259, false)]
    [Arguments("08S1", 9, true)]
    [Arguments("08S", 34, true)]
    [Arguments("08", 927, true)]
    public async Task Puzzle1(string input, int result, bool infiniteDistance)
    {
        var data = (await FileParser.GetCharacters(input)).ToArray();

        var antennaPositions = new Dictionary<char, List<(int r, int c)>>();
        var validAntinodes = new List<(int r, int c)>();
        for (int i = 0; i < data.Length; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                var frequency = data[i][j];
                if (frequency == '.' || frequency == '#')
                {
                    continue;
                }

                if (!antennaPositions.ContainsKey(frequency))
                {
                    antennaPositions[frequency] = [(i, j)];
                }
                else
                {
                    antennaPositions[frequency].Add((i, j));
                }
            }
        }

        foreach (var antenna in antennaPositions)
        {
            foreach (var positions in antenna.Value)
            {
                validAntinodes.AddRange(FindValidAntinodes(
                    positions,
                    antenna.Value.Where(x => x != positions).ToList(),
                    data.Length,
                    data[0].Length,
                    infiniteDistance));
            }
        }

        var antinodes = validAntinodes.Distinct().ToList();
        await Assert.That(antinodes.Count).IsEqualTo(result);
    }

    private IEnumerable<(int r, int c)> FindValidAntinodes(
        (int r, int c) antenna,
        List<(int r, int c)> positions,
        int height,
        int width, bool infiniteDistance)
    {
        foreach (var position in positions)
        {
            if (infiniteDistance)
            {
                yield return antenna;
                yield return position;
            }
            (int r, int c) offset = (position.r - antenna.r, position.c - antenna.c);
            int step = 1;
            do
            {
                (int r, int c) antinode = (antenna.r - offset.r * step, antenna.c - offset.c * step);
                if (!IsValidAntinode(antinode, height, width))
                {
                    break;
                }
                yield return antinode;
                step++;
            } while (infiniteDistance);
        }
    }

    private bool IsValidAntinode((int r, int c) antinode, int height, int width)
    {
        return antinode.r >= 0 && antinode.r < height &&
                antinode.c >= 0 && antinode.c < width;
    }
}
