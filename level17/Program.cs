string[] lines = File.ReadAllLines("input.txt");
int[][] map = lines.Select(t => t.Select(x => int.Parse(x.ToString())).ToArray()).ToArray();

var end = (map[0].Length - 1, map.Length - 1);


HashSet<(int X, int Y, Direction Direction, int DCount)> seen = new();
PriorityQueue<(int X, int Y, Direction Direction, int DirectionCount), int> queue = new();

queue.Enqueue((0, 0, Direction.Right, 0), 0);
queue.Enqueue((0, 0, Direction.Down, 0), 0);

while (queue.TryDequeue(out var c, out var cost))
{
    if (seen.Contains(c))
        continue;

    seen.Add(c);

    if ((c.X,c.Y) == end && c.DirectionCount >= 4)
    {
        Console.WriteLine(cost);
        break;
    }

    var next = GetNext(c.X, c.Y, c.Direction, c.DirectionCount);

    foreach (var n in next)
    {
        queue.Enqueue(n, cost + map[n.Y][n.X]);
    }
}

List<(int X, int Y, Direction Direction, int DirectionCount)> GetNext(int x, int y, Direction direction, int dcount)
{
    List<(int X, int Y, Direction Direction, int DirectionCount)> result = new();

    if (direction == Direction.Up)
    {
        result.AddRange([
            (x, y - 1, Direction.Up, dcount+1),
            (x - 1, y, Direction.Left, 1),
            (x + 1, y, Direction.Right, 1),
        ]);
    }
    else if (direction == Direction.Right)
    {
        result.AddRange([
            (x + 1, y, Direction.Right, dcount+1),
            (x, y - 1, Direction.Up, 1),
            (x, y + 1, Direction.Down, 1),
        ]);
    }
    else if (direction == Direction.Down)
    {
        result.AddRange([
            (x, y + 1, Direction.Down, dcount+1),
            (x - 1, y, Direction.Left, 1),
            (x + 1, y, Direction.Right, 1),
        ]);
    }
    else if (direction == Direction.Left)
    {
        result.AddRange([
            (x - 1, y, Direction.Left, dcount+1),
            (x, y - 1, Direction.Up, 1),
            (x, y + 1, Direction.Down, 1),
        ]);
    }

    var resultQry = result.Where(t => t.X >= 0 && t.X < map[0].Length && t.Y >= 0 && t.Y < map.Length && t.DirectionCount <= 10 && (dcount >= 4 ? true : t.Direction == direction));
    return resultQry.ToList();
}

enum Direction
{
    Up,
    Right,
    Down,
    Left,
}