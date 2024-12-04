namespace AoC2024;

internal static class FileParser
{
    public static async Task<IEnumerable<int[]>> ParseFile(string inputFile, string separator)
    {
        var rows = await File.ReadAllLinesAsync("Input/" + inputFile + ".txt");
        return rows.Select(row => row.Split(separator).Select(int.Parse).ToArray());
    }

    public static async Task<string> ParseFile(string inputFile)
    {
        return await File.ReadAllTextAsync("Input/" + inputFile + ".txt");
    }
}
