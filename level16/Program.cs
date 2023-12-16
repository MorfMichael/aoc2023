using System.Collections.Concurrent;
using System.ComponentModel;
using System.Xml;

string[] lines = File.ReadAllLines("input.txt");

char[][] map = lines.Select(t => t.ToCharArray()).ToArray();

var left = Enumerable.Range(0, lines.Length).Select(y => (X: 0, Y: y, Direction: Direction.Right));
var right = Enumerable.Range(0, lines.Length).Select(y => (X: lines[0].Length-1, Y: y, Direction: Direction.Left));
var top = Enumerable.Range(0, lines[0].Length).Select(x => (X: x, Y: 0, Direction: Direction.Down));
var bottom = Enumerable.Range(0, lines[0].Length).Select(x => (X: x, Y: lines.Length-1, Direction: Direction.Up));

var entries = left.Concat(right).Concat(top).Concat(bottom).ToList();

int max = 0;
foreach (var entry in entries)
{
    int c = Check(entry.X, entry.Y, entry.Direction);
    max = c > max ? c : max;
}

Console.WriteLine(max);

int Check(int x, int y, Direction direction)
{
    ConcurrentDictionary<(int X, int Y, Direction direction), int> seen = new();
    Queue<(int X, int Y, Direction Direction)> queue = new();

    queue.Enqueue((x, y, direction));

    while (queue.Count > 0)
    {
        var c = queue.Dequeue();

        if (seen.ContainsKey(c))
            continue;

        seen.AddOrUpdate(c, 1, (k, v) => v + 1);

        GetNext(c.X, c.Y, c.Direction).ForEach(queue.Enqueue);
    }

    return seen.Keys.Select(t => (t.X, t.Y)).Distinct().Count();
}

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


//void Print()
//{
//    for (int y = 0; y < map.Length; y++)
//    {
//        for (int x = 0; x < map[y].Length; x++)
//        {
//            Console.Write(seen.Keys.Any(t => t.X == x && t.Y == y) ? "#" : map[y][x]);
//        }
//        Console.WriteLine();
//    }
//}


enum Direction
{
    Up,
    Right,
    Down,
    Left,
}