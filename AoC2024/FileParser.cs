namespace AoC2024;

internal static class FileParser
{
    public static async Task<IEnumerable<int[]>> GetNumbers(string inputFile, string separator)
    {
        var rows = await File.ReadAllLinesAsync("Input/" + inputFile + ".txt");
        return rows.Select(row => row.Split(separator).Select(int.Parse).ToArray());
    }

    public static async Task<IEnumerable<string[]>> GetLines(string inputFile, string separator)
    {
        var rows = await File.ReadAllLinesAsync("Input/" + inputFile + ".txt");
        return rows.Select(row => row.Split(separator).ToArray());
    }

    public static async Task<IEnumerable<string>> GetLines(string inputFile)
    {
        var rows = await File.ReadAllLinesAsync("Input/" + inputFile + ".txt");
        return rows;
    }

    public static async Task<string> GetText(string inputFile)
    {
        return await File.ReadAllTextAsync("Input/" + inputFile + ".txt");
    }

    public static async Task<IEnumerable<char[]>> GetCharacters(string inputFile)
    {
        var lines = await File.ReadAllLinesAsync("Input/" + inputFile + ".txt");
        return lines.Select(x => x.ToCharArray());
    }
}
