namespace AoC2024.Extensions;

public static class ArrayExtensions
{

    public static void Print<T>(this T[][] map, string filename)
    {
        using var file = new StreamWriter(filename);
        for (var x = 0; x < map.Length; x++)
        {
            for (var y = 0; y < map[x].Length; y++)
            {
                file.Write(map[x][y]);
            }
            file.WriteLine();
        }
    }
}
