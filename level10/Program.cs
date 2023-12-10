using System.Diagnostics;
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
(int X, int Y)? start = null;


for (int y = 0; y < lines.Length; y++)
{
    string line = lines[y];
    map[y] = line.ToCharArray();

    if (line.Contains('S'))
    {
        start = (line.IndexOf('S'), y);
    }
}

PriorityQueue<List<(int X, int Y)>, int> queue = new();
queue.Enqueue([start.Value], 0);

int count = 0;

while (queue.Count > 0)
{
    var current = queue.Dequeue();
    //Console.WriteLine(string.Join("", current.Select(t => map[t.Y][t.X])));

    if (current.Count > count) count = current.Count;
    else continue;

    var n = GetNeighbours(current);
    if (n.Count > 2)
    {
        Debugger.Break();
    }
    if (n.Any())
    {
        queue.EnqueueRange(n.Select(t => current.Append(t).ToList()), current.Count + 1);
    }
}

Console.WriteLine(count/2);

List<(int X, int Y)> GetNeighbours(List<(int X, int Y)> previous)
{
    var c = previous[^1];
    var ch = map[c.Y][c.X];
    List<(int X, int Y, Direction direction)> neighbours = new List<(int X, int Y, Direction direction)>();
    neighbours = ch switch
    {
        '-' => [(c.X - 1, c.Y, Direction.Left), (c.X + 1, c.Y, Direction.Right)],
        '|' => [(c.X, c.Y-1, Direction.Up), (c.X, c.Y+1, Direction.Down)],
        'F' => [(c.X + 1, c.Y, Direction.Right), (c.X, c.Y + 1, Direction.Down)],
        '7' => [(c.X - 1, c.Y, Direction.Left), (c.X, c.Y + 1, Direction.Down)],
        'L' => [(c.X + 1, c.Y, Direction.Right), (c.X, c.Y - 1, Direction.Up)],
        'J' => [(c.X - 1, c.Y, Direction.Left), (c.X, c.Y - 1, Direction.Up)],
        'S' => [(c.X, c.Y - 1, Direction.Up), (c.X + 1, c.Y, Direction.Right), (c.X, c.Y + 1, Direction.Down), (c.X - 1, c.Y, Direction.Left)],
    };

    neighbours = neighbours.Where(t => t.X >= 0 && t.X < map[0].Length && t.Y >= 0 && t.Y < map.Length && !previous.Any(x => x.X == t.X && x.Y == t.Y) && Possibles[t.direction].Contains(map[t.Y][t.X])).ToList();
    return neighbours.Select(t => (t.X, t.Y)).ToList();
}

void Print(int x1, int y1, bool read)
{
    Console.Clear();
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[y].Length; x++)
        {
            if (x == x1 && y == y1)
                Console.Write('X');
            else
                Console.Write(map[y][x]);
        }
        Console.WriteLine();
    }

    if (read) Console.ReadKey();
}

enum Direction
{
    Up,
    Right,
    Down,
    Left,
}