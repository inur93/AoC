using AoC2024.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC2024;

public static class Vector2Extensions
{
    public static T Value<T>(this Vector2 vector, T[][] map)
    {
        return map[(int)vector.X][(int)vector.Y];
    }

    public static T Set<T>(this Vector2 vector, T[][] map, T value)
    {
        return map[(int)vector.X][(int)vector.Y] = value;
    }

    internal class Day15
    {
        [Test]
        //[Arguments("15S1", 2028, false)]
        //[Arguments("15S2", 10092, false)]
        //[Arguments("15", 1490942, false)]
        [Arguments("15S2", 9021, true)] // puzzle 2
        public async Task Puzzle1and2(string input, int expectedResult, bool extraWide)
        {
            var lines = (await FileParser.GetLines(input)).GetEnumerator();
            var map = GetMap(lines, extraWide);
            var movements = GetMovements(lines);
            var position = new Vector2(0, 0);
            for (var x = 0; x < map.Length; x++)
            {
                for (var y = 0; y < map[x].Length; y++)
                {
                    if (map[x][y] == '@')
                    {
                        position = new Vector2(x, y);
                        break;
                    }
                }
            }
            foreach (var movement in movements)
            {
                var dir = movement switch
                {
                    '^' => new Vector2(-1, 0),
                    'v' => new Vector2(1, 0),
                    '<' => new Vector2(0, -1),
                    '>' => new Vector2(0, 1),
                    _ => throw new InvalidOperationException()
                };
                if (Move(map, position, dir))
                {
                    position += dir;
                }
            }

            var result = GetSumOfBoxCoordinates(map);
            map.Print($"{input}-output.txt");
            await Assert.That(result).IsEqualTo(expectedResult);
        }

        private int GetSumOfBoxCoordinates(char[][] map)
        {
            var sum = 0;
            for (var x = 0; x < map.Length; x++)
            {
                for (var y = 0; y < map[x].Length; y++)
                {
                    if (map[x][y] == 'O')
                    {
                        sum += x * 100 + y;
                    }
                    if (map[x][y] == '[')
                    {
                        sum += x * 100 + y;
                    }
                }
            }
            return sum;
        }

        private bool Move(char[][] map, Vector2 position, Vector2 direction)
        {
            var positionType = position.Value(map);

            var nextPosition = position + direction;
            var nextPositionType = nextPosition.Value(map);
            if (nextPositionType == '#') // wall
            {
                return false;
            }

            if (nextPositionType == '.') // free space
            {
                nextPosition.Set(map, positionType);
                position.Set(map, '.');
                return true;
            }

            var rightBlock = position + new Vector2(0, 1);
            var leftBlock = position + new Vector2(0, -1);
            var isVertical = direction.X != 0;
            if (isVertical)
            {
                if (positionType == '[' && !CanMove(map, rightBlock, direction))
                {
                    return false;
                }

                if (positionType == ']' && !CanMove(map, leftBlock, direction))
                {
                    return false;
                }
            }

            if (CanMove(map, position, direction))
            {
                Move(map, nextPosition, direction);
                if (isVertical)
                {
                    if (positionType == '[')
                    {
                        Move(map, rightBlock + direction, direction);
                        (rightBlock + direction).Set(map, ']');
                        rightBlock.Set(map, '.');
                    }

                    if (positionType == ']')
                    {
                        Move(map, leftBlock + direction, direction);
                        (leftBlock + direction).Set(map, ']');
                        leftBlock.Set(map, '.');
                    }
                }
                nextPosition.Set(map, positionType);
                position.Set(map, '.');
                map.Print("debug.txt");
                return true;
            }

            return false;
        }

        private bool CanMove(char[][] map, Vector2 position, Vector2 direction)
        {
            var nextPosition = position + direction;
            var nextMoveType = nextPosition.Value(map);
            if (nextMoveType == '#') return false;
            if (nextMoveType == '.') return true;
            return CanMove(map, nextPosition, direction);
        }

        private char[][] GetMap(IEnumerator<string> lines, bool extraWide = false)
        {
            var rows = new List<char[]>();
            lines.MoveNext();
            do
            {
                var line = lines.Current;
                if (extraWide)
                {
                    line = string.Concat(line.Select<char, char[]>(x =>
                    {
                        return x switch
                        {
                            '.' => ['.', '.'],
                            '#' => ['#', '#'],
                            'O' => ['[', ']'],
                            '@' => ['@', '.'],
                            _ => ['.']
                        };
                    }).SelectMany(x => x));
                }

                rows.Add(line.ToCharArray());


                lines.MoveNext();
            } while (lines.Current != "");

            return [.. rows];
        }

        private char[] GetMovements(IEnumerator<string> lines)
        {
            lines.MoveNext();
            var movements = new List<char>();
            do
            {
                movements.AddRange(lines.Current.ToCharArray());
            } while (lines.MoveNext());
            return movements.ToArray();
        }

    }
}