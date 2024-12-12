


namespace AoC2024;

public record Vector(int x, int y)
{
    internal Vector Add(Vector other)
    {
        return new Vector(other.x + x, other.y + y);
    }

    internal Vector Add(int x, int y)
    {
        return new Vector(this.x + x, this.y + y);
    }

    internal Vector RotateRight()
    {
        return new Vector(-y, x);
    }

    internal Vector Sub(Vector vector)
    {
        return new Vector(x - vector.x, y - vector.y);
    }

    internal T Value<T>(T[][] map)
    {
        return map[x][y];
    }

    internal bool InBounds<T>(T[][] map)
    {
        return x >= 0 && x < map.Length && y >= 0 && y < map[0].Length;
    }
}
