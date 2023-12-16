using System.Collections.Concurrent;
using System.Xml;

string[] lines = File.ReadAllLines("input.txt");

char[][] map = lines.Select(t => t.ToCharArray()).ToArray();

ConcurrentDictionary<(int X, int Y, Direction direction), int> seen = new();
Queue<List<(int X, int Y, Direction Direction)>> queue = new();

queue.Enqueue([(0, 0, Direction.Right)]);

while (queue.Count > 0)
{
    var c = queue.Dequeue();
    var cp = c[^1];

    if (seen.ContainsKey(cp))
        continue;

    seen.AddOrUpdate(cp, 1, (k,v) => v+1);

    var next = GetNext(cp.X, cp.Y, cp.Direction);
    foreach (var n in next)
    {
        if (!c.Contains(n))
        queue.Enqueue(c.Append(n).ToList());
    }
}

Print();

Console.WriteLine(seen.Keys.Select(t => (t.X,t.Y)).Distinct().Count());

List<(int X, int Y, Direction Direction)> GetNext(int x, int y, Direction direction)
{
    List<(int X, int Y, Direction Direction)> result = new();
    if (map[y][x] == '.')
    {
        result = direction switch
        {
            Direction.Up => [(x, y - 1, Direction.Up)],
            Direction.Down => [(x, y + 1, Direction.Down)],
            Direction.Left => [(x - 1, y, Direction.Left)],
            Direction.Right => [(x + 1, y, Direction.Right)],
        };
    }
    else if (map[y][x] == '-')
    {
        result = direction switch
        {
            Direction.Up => [(x - 1, y, Direction.Left), (x + 1, y, Direction.Right)],
            Direction.Down => [(x - 1, y, Direction.Left), (x + 1, y, Direction.Right)],
            Direction.Left => [(x - 1, y, Direction.Left)],
            Direction.Right => [(x + 1, y, Direction.Right)],
        };
    }
    else if (map[y][x] == '|')
    {
        result = direction switch
        {
            Direction.Up => [(x,y-1,Direction.Up)],
            Direction.Down => [(x,y+1,Direction.Down)],
            Direction.Left => [(x, y-1, Direction.Up), (x, y+1, Direction.Down)],
            Direction.Right => [(x, y - 1, Direction.Up), (x, y + 1, Direction.Down)],
        };
    }
    else if (map[y][x] == '\\')
    {
        result = direction switch
        {
            Direction.Up => [(x-1, y, Direction.Left)],
            Direction.Down => [(x + 1, y, Direction.Right)],
            Direction.Left => [(x, y - 1, Direction.Up)],
            Direction.Right => [(x, y + 1, Direction.Down)],
        };
    }
    else if (map[y][x] == '/')
    {
        result = direction switch
        {
            Direction.Up => [(x + 1, y, Direction.Right)],
            Direction.Down => [(x - 1, y, Direction.Left)],
            Direction.Left => [(x, y + 1, Direction.Down)],
            Direction.Right => [(x, y - 1, Direction.Up)],
        };
    }

    return result.Where(t => t.X >= 0 && t.X < map[0].Length && t.Y >= 0 && t.Y < map.Length).ToList();
}


void Print()
{
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[y].Length; x++)
        {
            Console.Write(seen.Keys.Any(t => t.X == x && t.Y == y) ? "#" : map[y][x]);
        }
        Console.WriteLine();
    }
}


enum Direction
{
    Up,
    Right,
    Down,
    Left,
}