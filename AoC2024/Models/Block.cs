namespace AoC2024.Models;

internal class Block
{
    public int Id { get; set; }

    public int Start { get; set; }

    public int End { get; set; }

    public bool IsFree => Id == -1;

    public int Length => End - Start + 1;

    public Block Left { get; set; }

    public Block Right { get; set; }
}
