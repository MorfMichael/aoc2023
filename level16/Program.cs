using System.Collections.Concurrent;
using System.Xml;

string[] lines = File.ReadAllLines("input.txt");

char[][] map = lines.Select(t => t.ToCharArray()).ToArray();

ConcurrentDictionary<(int X, int Y), int> seen = new();
Queue<(Guid Id, int X, int Y, Direction Direction)> queue = new();

queue.Enqueue((Guid.NewGuid(), 0, 0, Direction.Right));

int i = 0;
int seen_c = 0;
int last_i = 0;
while (queue.Count > 0)
{
    i++;

    if (i == last_i + 10_000_000)
    {
        last_i = i;
        if (seen.Count == seen_c)
        {
            break;
        }
        else
            seen_c = seen.Count;
    }

    var c = queue.Dequeue();

    seen.AddOrUpdate((c.X, c.Y), 1, (k,v) => v+1);

    GetNext(c.Id, c.X, c.Y, c.Direction).ForEach(queue.Enqueue);
}

Print();

Console.WriteLine(seen.Count);

List<(Guid Id, int X, int Y, Direction Direction)> GetNext(Guid id, int x, int y, Direction direction)
{
    List<(Guid Id, int X, int Y, Direction Direction)> result = new();
    if (map[y][x] == '.')
    {
        result = direction switch
        {
            Direction.Up => [(id, x, y - 1, Direction.Up)],
            Direction.Down => [(id, x, y + 1, Direction.Down)],
            Direction.Left => [(id, x - 1, y, Direction.Left)],
            Direction.Right => [(id, x + 1, y, Direction.Right)],
        };
    }
    else if (map[y][x] == '-')
    {
        result = direction switch
        {
            Direction.Up => [(id, x - 1, y, Direction.Left), (Guid.NewGuid(), x + 1, y, Direction.Right)],
            Direction.Down => [(id, x - 1, y, Direction.Left), (Guid.NewGuid(), x + 1, y, Direction.Right)],
            Direction.Left => [(id, x - 1, y, Direction.Left)],
            Direction.Right => [(id, x + 1, y, Direction.Right)],
        };
    }
    else if (map[y][x] == '|')
    {
        result = direction switch
        {
            Direction.Up => [(id, x,y-1,Direction.Up)],
            Direction.Down => [(id,x,y+1,Direction.Down)],
            Direction.Left => [(id, x, y-1, Direction.Up), (Guid.NewGuid(), x, y+1, Direction.Down)],
            Direction.Right => [(id, x, y - 1, Direction.Up), (Guid.NewGuid(), x, y + 1, Direction.Down)],
        };
    }
    else if (map[y][x] == '\\')
    {
        result = direction switch
        {
            Direction.Up => [(id, x-1, y, Direction.Left)],
            Direction.Down => [(id, x + 1, y, Direction.Right)],
            Direction.Left => [(id, x, y - 1, Direction.Up)],
            Direction.Right => [(id, x, y + 1, Direction.Down)],
        };
    }
    else if (map[y][x] == '/')
    {
        result = direction switch
        {
            Direction.Up => [(id, x + 1, y, Direction.Right)],
            Direction.Down => [(id, x - 1, y, Direction.Left)],
            Direction.Left => [(id, x, y + 1, Direction.Down)],
            Direction.Right => [(id, x, y - 1, Direction.Up)],
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
            Console.Write(seen.ContainsKey((x,y)) ? "#" : map[y][x]);
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