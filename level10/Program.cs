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
    //Console.WriteLine(string.Join("", current.Select(t => map[t.Y][t.X])));

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

Print();

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
    int count = 0;
    if (direction == Direction.Up)
    {
        bool left = false, right = false;
        for (int i = y-1; i >= 0; i--)
        {
            if (map[i][x] == '-') count++;
            if (map[i][x] == '|') continue;
            if (map[i][x] == 'J' || map[i][x] == '7')
            {
                if (left && !right)
                    left = false;
                else if (!left && right)
                {
                    right = false;
                    count++;
                }
                else
                {
                    left = true;
                }
            }
            if (map[i][x] == 'F' || map[i][x] == 'L')
            {
                if (right && !left)
                {
                    right = false;
                }
                else if (!right && left)
                {
                    left = false;
                    count++;
                }
                else
                {
                    right = true;
                }
            }
        }
    }
    else if (direction == Direction.Down)
    {
        bool left = false, right = false;
        for (int i = y+1; i < map.Length; i++)
        {
            if (map[i][x] == '-') count++;
            if (map[i][x] == '|') continue;
            if (map[i][x] == 'J' || map[i][x] == '7')
            {
                if (left && !right)
                    left = false;
                else if (!left && right)
                {
                    right = false;
                    count++;
                }
                else
                {
                    left = true;
                }
            }
            if (map[i][x] == 'F' || map[i][x] == 'L')
            {
                if (right && !left)
                {
                    right = false;
                }
                else if (!right && left)
                {
                    left = false;
                    count++;
                }
                else
                {
                    right = true;
                }
            }
        }
    }
    else if (direction == Direction.Left)
    {
        bool up = false, down = false;
        for (int i = x-1; i >= 0; i--)
        {
            if (map[y][i] == '|') count++;
            if (map[y][i] == '-') continue;
            if (map[y][i] == 'J' || map[y][i] == 'L')
            {
                if (up && !down)
                    up = false;
                else if (!up && down)
                {
                    down = false;
                    count++;
                }
                else
                {
                    up = true;
                }
            }
            if (map[y][i] == 'F' || map[y][i] == '7')
            {
                if (down && !up)
                {
                    down = false;
                }
                else if (!down && up)
                {
                    up = false;
                    count++;
                }
                else
                {
                    down = true;
                }
            }
        }
    }
    else if (direction == Direction.Right)
    {
        bool up = false, down = false;
        for (int i = x+1; i < map[0].Length; i++)
        {
            if (map[y][i] == '|') count++;
            if (map[y][i] == '-') continue;
            if (map[y][i] == 'J' || map[y][i] == 'L')
            {
                if (up && !down)
                    up = false;
                else if (!up && down)
                {
                    down = false;
                    count++;
                }
                else
                {
                    up = true;
                }
            }
            if (map[y][i] == 'F' || map[y][i] == '7')
            {
                if (down && !up)
                {
                    down = false;
                }
                else if (!down && up)
                {
                    up = false;
                    count++;
                }
                else
                {
                    down = true;
                }
            }
        }
    }

    return count;
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

//List<(int X, int Y)> GetInsideout(int X, int Y, bool insideout, HashSet<(int X, int Y)> visited)
//{
//    List<(int X, int Y)> result = new List<(int X, int Y)>();
//    char ch = map[Y][X];

//    result = ch switch
//    {
//        'F' => insideout ? [(X - 1, Y - 1), (X, Y - 1), (X + 1, Y - 1), (X - 1, Y), (X - 1, Y + 1)] : [(X + 1, Y + 1)],
//        '7' => insideout ? [(X - 1, Y - 1), (X, Y - 1), (X + 1, Y - 1), (X + 1, Y), (X + 1, Y + 1)] : [(X - 1, Y + 1)],
//        'L' => insideout ? [(X - 1, Y - 1), (X - 1, Y), (X - 1, Y + 1), (X, Y + 1), (X + 1, Y + 1)] : [(X + 1, Y - 1)],
//        'J' => insideout ? [(X + 1, Y - 1), (X + 1, Y), (X - 1, Y + 1), (X, Y + 1), (X + 1, Y + 1)] : [(X - 1, Y - 1)],
//        '|' => insideout ? [(X - 1, Y - 1), (X - 1, Y), (X - 1, Y + 1)] : [(X + 1, Y - 1), (X + 1, Y), (X + 1, Y + 1)],
//        '-' => insideout ? [(X - 1, Y - 1), (X, Y - 1), (X + 1, Y - 1)] : [(X - 1, Y + 1), (X, Y + 1), (X + 1, Y + 1)],
//        _ => [],
//    };

//    return result.Where(t => t.X >= 0 && t.X < map[0].Length && t.Y >= 0 && t.Y < map.Length && !visited.Contains(t)).ToList();
//}

//List<(int X, int Y)> GetSurrounding(int X, int Y, HashSet<(int X, int Y)> visited)
//{
//    List<(int X, int Y)> result = [(X - 1, Y - 1), (X, Y - 1), (X + 1, Y - 1), (X - 1, Y), (X + 1, Y), (X - 1, Y + 1), (X, Y + 1), (X + 1, Y + 1)];
//    return result.Where(t => t.X >= 0 && t.X < map[0].Length && t.Y >= 0 && t.Y < map.Length && !visited.Contains(t)).ToList();
//}


void Print()
{
    Console.Clear();
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[y].Length; x++)
        {
            bool contains = loop.Contains((x, y));
            Console.ForegroundColor = contains ? ConsoleColor.Green : ConsoleColor.White;
            if (map[y][x] == 'S') Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(contains ? Replace(map[y][x]) : '#');
        }
        Console.WriteLine();
    }

    //if (read) Console.ReadKey();
}

char Replace(char input)
{
    return input switch
    {
        '-' => '─',
        '|' => '│',
        'F' => '┌',
        '7' => '┐',
        'L' => '└',
        'J' => '┘',
        _ => input
    };
}

enum Direction
{
    Up,
    Right,
    Down,
    Left,
}