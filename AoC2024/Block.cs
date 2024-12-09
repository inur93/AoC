namespace AoC2024;

internal record Block(int Id, int Start, int End)
{
    public bool IsFree => Id == 0;
}
