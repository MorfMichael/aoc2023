using System.Collections.Concurrent;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

string[] lines = File.ReadAllLines("input.txt");

char[][] map = new char[lines.Length][];
List<Rock> rocks = new List<Rock>();

for (int y = 0; y < lines.Length; y++)
{
    map[y] = new char[lines[y].Length];
    for (int x = 0; x < map[y].Length; x++)
    {
        map[y][x] = lines[y][x];

        if (lines[y][x] == 'O')
        {
            map[y][x] = '.';
            rocks.Add(new Rock(x, y));
        }
    }
}

ConcurrentDictionary<int, List<Rock>> cache = new();
ConcurrentDictionary<string, int> cache2 = new();

//Print(map, rocks);

for (int i = 0; i <= 1_000_000_000; i++)
{
    if (i % 1_000 == 0) Console.WriteLine(i);

    int hash = rocks.GetSequenceHash();

    if (cache.ContainsKey(hash))
    {
        rocks = cache[hash];
    }
    else
    {
        rocks.OrderBy(r => r.Y).ThenBy(r => r.X).ToList().ForEach(x => x.Move(0, map, rocks));
        rocks.OrderBy(t => t.X).ThenBy(r => r.Y).ToList().ForEach(x => x.Move(1, map, rocks));
        rocks.OrderByDescending(t => t.Y).ThenBy(r => r.X).ToList().ForEach(x => x.Move(2, map, rocks));
        rocks.OrderByDescending(t => t.X).ThenBy(r => r.Y).ToList().ForEach(x => x.Move(3, map, rocks));
        cache.AddOrUpdate(hash, rocks, (k, v) => rocks);
    }
}

Console.WriteLine(rocks.Sum(x => x.GetLoad(map.Length)));

void Print(char[][] map, List<Rock> rocks)
{
    Console.Clear();
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[y].Length; x++)
        {
            if (rocks.Any(t => t.X == x && t.Y == y))
                Console.Write('O');
            else
                Console.Write(map[y][x]);
        }
        Console.WriteLine();
    }

    Console.ReadKey();
}

public class Rock
{
    public int X { get; set; }
    public int Y { get; set; }

    public Rock(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int GetLoad(int size)
    {
        return size - Y;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Rock rock)
        {
            return X == rock.X && Y == rock.Y;
        }

        return base.Equals(obj);
    }

    public override string ToString()
    {
        return (X, Y).ToString();
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }
    }

    public void Move(int direction, char[][] map, List<Rock> rocks)
    {
        if (direction == 0) // North
        {
            if (Y > 0)
            {

                while (Y - 1 >= 0 && map[Y - 1][X] == '.' && !rocks.Any(r => r.X == X && r.Y == Y - 1)) Y--;
            }
        }
        else if (direction == 1) // West
        {
            if (X > 0)
            {
                while (X - 1 >= 0 && map[Y][X - 1] == '.' && !rocks.Any(r => r.Y == Y && r.X == X - 1)) X--;
            }
        }
        else if (direction == 2)  // South
        {
            if (Y < map.Length)
            {
                while (Y + 1 < map.Length && map[Y + 1][X] == '.' && !rocks.Any(r => r.X == X && r.Y == Y + 1)) Y++;
            }
        }
        else if (direction == 3) // South
        {
            if (X < map[0].Length)
            {
                while (X + 1 < map[0].Length && map[Y][X + 1] == '.' && !rocks.Any(r => r.Y == Y && r.X == X + 1)) X++;
            }
        }
    }
}

public static class Extensions
{
    public static int GetSequenceHash(this IEnumerable<Rock> rocks)
    {
        unchecked
        {
            return rocks.Sum(x => x.GetHashCode());
        }
    }
}