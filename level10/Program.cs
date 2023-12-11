using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Text;

string[] lines = File.ReadAllLines("input.txt");

Dictionary<Direction, string> Possibles = new()
{
    { Direction.Up, "|F7" },
    { Direction.Down, "|LJ" },
    { Direction.Left, "-LF" },
    { Direction.Right, "-7J" },
};

char[][] map = new char[lines.Length][];
List<(int X, int Y, char Value)> map2 = new List<(int X, int Y, char Value)>();
(int X, int Y)? start = null;

for (int y = 0; y < lines.Length; y++)
{
    string line = lines[y];
    map[y] = new char[line.Length];

    for (int x = 0; x < line.Length; x++)
    {
        map[y][x] = line[x];
        map2.Add((x, y, line[x]));

        if (line[x] == 'S')
        {
            start = (x, y);
        }
    }
}

PriorityQueue<List<(int X, int Y)>, int> queue = new();
queue.Enqueue([start.Value], 0);

HashSet<(int X, int Y)> loop = new();

while (queue.Count > 0)
{
    var current = queue.Dequeue();

    if (current.Count > loop.Count)
    {
        loop = current.ToHashSet();
    }
    else continue;

    var n = GetNeighbours(current);
    queue.EnqueueRange(n.Select(t => current.Append(t).ToList()), current.Count + 1);
}

for (int y = 0; y < map.Length; y++)
{
    for (int x = 0; x < map[y].Length; x++)
    {
        if (!loop.Contains((x, y)))
        {
            map[y][x] = '.';
        }
    }
}

var other = map2.Where(t => !loop.Contains((t.X, t.Y))).ToList();
int count = 0;

List<Direction> directions = [Direction.Up, Direction.Down, Direction.Left, Direction.Right];
foreach (var o in other)
{
    if (directions.All(t => Tryout(o.X, o.Y, t) % 2 != 0))
    {
        count++;
    }
}

Console.WriteLine(count);

int Tryout(int x, int y, Direction direction)
{
    if (direction == Direction.Up || direction == Direction.Down)
    {
        var entries = direction == Direction.Up ? Enumerable.Range(0, y - 1).Select(t => map[t][x]) : Enumerable.Range(y + 1, map.Length - (y + 1)).Select(t => map[t][x]);
        return entries.Count(t => t == '-') + (entries.Count(t => t == 'J' || t == '7') % 2) + (entries.Count(t => t == 'F' || t == 'L') % 2);
    }
    else
    {
        var entries = direction == Direction.Left ? Enumerable.Range(0, x - 1).Select(t => map[y][t]) : Enumerable.Range(x + 1, map[y].Length - (x + 1)).Select(t => map[y][t]);
        return entries.Count(t => t == '|') + (entries.Count(t => t == 'J' || t == 'L') % 2) + (entries.Count(t => t == 'F' || t == '7') % 2);
    }
}

List<(int X, int Y)> GetNeighbours(List<(int X, int Y)> previous)
{
    var c = previous[^1];
    var ch = map[c.Y][c.X];
    List<(int X, int Y, Direction direction)> neighbours = new List<(int X, int Y, Direction direction)>();
    neighbours = ch switch
    {
        '-' => [(c.X - 1, c.Y, Direction.Left), (c.X + 1, c.Y, Direction.Right)],
        '|' => [(c.X, c.Y - 1, Direction.Up), (c.X, c.Y + 1, Direction.Down)],
        'F' => [(c.X + 1, c.Y, Direction.Right), (c.X, c.Y + 1, Direction.Down)],
        '7' => [(c.X - 1, c.Y, Direction.Left), (c.X, c.Y + 1, Direction.Down)],
        'L' => [(c.X + 1, c.Y, Direction.Right), (c.X, c.Y - 1, Direction.Up)],
        'J' => [(c.X - 1, c.Y, Direction.Left), (c.X, c.Y - 1, Direction.Up)],
        'S' => [(c.X, c.Y - 1, Direction.Up), (c.X + 1, c.Y, Direction.Right), (c.X, c.Y + 1, Direction.Down), (c.X - 1, c.Y, Direction.Left)],
    };

    neighbours = neighbours.Where(t => t.X >= 0 && t.X < map[0].Length && t.Y >= 0 && t.Y < map.Length && !previous.Any(x => x.X == t.X && x.Y == t.Y) && Possibles[t.direction].Contains(map[t.Y][t.X])).ToList();
    if (ch == 'S')
    {
        if (neighbours.Count(t => t.direction == Direction.Up || t.direction == Direction.Down) == 2)
        {
            map[c.Y][c.X] = '|';
        }
        else if (neighbours.Count(t => t.direction == Direction.Left || t.direction == Direction.Right) == 2)
        {
            map[c.Y][c.X] = '-';
        }
        else if (neighbours.Count(t => t.direction == Direction.Right || t.direction == Direction.Down) == 2)
        {
            map[c.Y][c.X] = 'F';
        }
        else if (neighbours.Count(t => t.direction == Direction.Left || t.direction == Direction.Down) == 2)
        {
            map[c.Y][c.X] = '7';
        }
        else if (neighbours.Count(t => t.direction == Direction.Right || t.direction == Direction.Up) == 2)
        {
            map[c.Y][c.X] = 'L';
        }
        else if (neighbours.Count(t => t.direction == Direction.Left || t.direction == Direction.Up) == 2)
        {
            map[c.Y][c.X] = 'J';
        }

    }
    return neighbours.Select(t => (t.X, t.Y)).ToList();
}

enum Direction
{
    Up,
    Right,
    Down,
    Left,
}