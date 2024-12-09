


namespace AoC2024;

internal record Vector(int x, int y)
{
    internal Vector Add(Vector other)
    {
        return new Vector(other.x + x, other.y + y);
    }

    internal Vector RotateRight()
    {
        return new Vector(-y, x);
    }

    internal Vector Sub(Vector vector)
    {
        return new Vector(x - vector.x, y - vector.y);
    }
}
