using System.Text.RegularExpressions;

namespace AoC2024;

public partial class Day04
{
    [Test]
    [Arguments("04S", 18)]
    [Arguments("04", 2654)]

    public async Task Puzzle1(string inputFile, int expectedResult)
    {
        var input = (await FileParser.GetCharacters(inputFile)).ToArray();
        var sum = 0;
        for (var x = 0; x < input.Length; x++)
        {
            for (var y = 0; y < input[x].Length; y++)
            {
                var ch = input[x][y];
                if (ch == 'X')
                {
                    sum += FindXmas(x, y, input);
                }
            }
        }

        await Assert.That(sum).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments("04S", 9)]
    [Arguments("04", 1990)]

    public async Task Puzzle2(string inputFile, int expectedResult)
    {
        var input = (await FileParser.GetCharacters(inputFile)).ToArray();
        var sum = 0;
        for (var x = 0; x < input.Length; x++)
        {
            for (var y = 0; y < input[x].Length; y++)
            {
                var ch = input[x][y];
                if (ch == 'A')
                {
                    sum += FindMas(x, y, input);
                }
            }
        }

        await Assert.That(sum).IsEqualTo(expectedResult);
    }


    public int FindMas(int x, int y, char[][] input)
    {
        char[] characters = ['M', 'A', 'S'];

        if ((FindSequence(input, characters, x - 1, y - 1, 1, 1) || FindSequence(input, characters, x + 1, y + 1, -1, -1)) &&
            (FindSequence(input, characters, x - 1, y + 1, 1, -1) || FindSequence(input, characters, x + 1, y - 1, -1, 1)))
        {
            return 1;
        }
        return 0;

    }

    public int FindXmas(int x, int y, char[][] input)
    {
        char[] characters = ['X', 'M', 'A', 'S'];

        return new List<bool>
        {
            FindSequence(input, characters, x, y, 1, 0), // right
            FindSequence(input, characters, x, y, 0, 1), // down
            FindSequence(input, characters, x, y, 1, 1), // down right
            FindSequence(input, characters, x, y, 1, -1), // down left
            FindSequence(input, characters, x, y, -1, 0), // left
            FindSequence(input, characters, x, y, 0, -1), // up
            FindSequence(input, characters, x, y, -1, -1), // up left
            FindSequence(input, characters, x, y, -1, 1), // up right
        }.Where(x => x).Count();
    }

    private static bool FindSequence(char[][] input, char[] lookFor, int x, int y, int dir_x, int dir_y)
    {
        if (lookFor.Length == 0)
        {
            return true;
        }

        if (x < 0 || x >= input.Length || y < 0 || y >= input[x].Length)
        {
            return false;
        }

        if (input[x][y] != lookFor[0])
        {
            return false;
        }

        return FindSequence(input, lookFor[1..], x + dir_x, y + dir_y, dir_x, dir_y);
    }

}
