string[] lines = File.ReadAllLines("sample.txt");
int[][] map = lines.Select(t => t.Select(x => int.Parse(x.ToString())).ToArray()).ToArray();

var end = (map[0].Length - 1, map.Length - 1);

PriorityQueue<List<(int X, int Y, Direction Direction, int Cost)>, int> queue = new();

queue.Enqueue([(0, 0, Direction.Right, 0)], 0);
queue.Enqueue([(0, 0, Direction.Down, 0)], 0);

int min = int.MaxValue;
while (queue.TryDequeue(out var current, out var cost))
{
    if (current.Sum(x => x.Cost) > min)
        continue;
    //Print(current);
    var c = current[^1];

    if ((c.X,c.Y) == end)
    {
        int sum = current.Sum(x => x.Cost);
        if (sum < min)
        {
            min = sum;
            Console.WriteLine(sum);
        }
        //Console.ReadKey();
    }

    var next = GetNext(current);

    foreach (var n in next)
    {
        int n_cost = map[n.Y][n.X];
        int n_distance = GetDistance((n.X,n.Y), end);
        queue.Enqueue(current.Append((n.X, n.Y, n.Direction, n_cost)).ToList(), cost + n_distance + n_cost);
    }
}


int GetDistance((int X, int Y) first, (int X, int Y) second)
{
    return Math.Abs(second.X - first.X) + Math.Abs(second.Y - first.Y);
}

List<(int X, int Y, Direction Direction)> GetNext(List<(int X, int Y, Direction Direction, int Cost)> previous)
{
    var last = previous[^1];
    var last_directions = previous.TakeLast(3).Select(t => t.Direction).ToList();

    List<(int X, int Y, Direction Direction)> result = new();

    if (last.Direction == Direction.Up)
    {
        result.AddRange([
            (last.X, last.Y - 1, Direction.Up),
            (last.X - 1, last.Y, Direction.Left),
            (last.X + 1, last.Y, Direction.Right),
        ]);
    }
    else if (last.Direction == Direction.Right)
    {
        result.AddRange([
            (last.X + 1, last.Y, Direction.Right),
            (last.X, last.Y - 1, Direction.Up),
            (last.X, last.Y + 1, Direction.Down),
        ]);
    }
    else if (last.Direction == Direction.Down)
    {
        result.AddRange([
            (last.X, last.Y + 1, Direction.Down),
            (last.X - 1, last.Y, Direction.Left),
            (last.X + 1, last.Y, Direction.Right),
        ]);
    }
    else if (last.Direction == Direction.Left)
    {
        result.AddRange([
            (last.X - 1, last.Y, Direction.Left),
            (last.X, last.Y - 1, Direction.Up),
            (last.X, last.Y + 1, Direction.Down),
        ]);
    }

    var resultQry = result.Where(t => t.X >= 0 && t.X < map[0].Length && t.Y >= 0 && t.Y < map.Length && !previous.Any(p => p.X == t.X && p.Y == t.Y));

    if (last_directions.Count == 3 && last_directions.Distinct().Count() == 1)
        resultQry = resultQry.Where(t => t.Direction != last_directions.First());
    return resultQry.ToList();
}

void Print(List<(int X, int Y, Direction Direction, int Cost)> path)
{
    Console.Clear();
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[y].Length; x++)
        {
            var exists = path.FirstOrDefault(t => t.X == x && t.Y == y);
            if (exists != default)
            {
                Console.Write(exists.Direction switch
                {
                    Direction.Up => '^',
                    Direction.Right => '>',
                    Direction.Left => '<',
                    Direction.Down => 'v',
                });
            }
            else
            {
                Console.Write(map[y][x]);
            }
        }
        Console.WriteLine();
    }
    //Console.ReadKey();
}

enum Direction
{
    Up,
    Right,
    Down,
    Left,
}