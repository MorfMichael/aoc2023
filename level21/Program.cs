using System.Runtime.Serialization;

string[] lines = File.ReadAllLines("input.txt");
char[][] map = lines.Select(t => t.ToCharArray()).ToArray();

int steps = 64;

(int x, int y) start = map.Select((t, i) => (x: Array.IndexOf(t, 'S'), y: i)).FirstOrDefault(p => p.x != -1);

HashSet<(int x, int y)> seen = new();
HashSet<(int x, int y)> check = new();
Queue<(int x, int y, int count)> queue = new();
queue.Enqueue((start.x, start.y, 0));

while (queue.TryDequeue(out var cur))
{
    if (seen.Contains((cur.x, cur.y)))
        continue;

    seen.Add((cur.x, cur.y));

    //Print();
    if (cur.count == steps || cur.count % 2 == (steps % 2))
    {
        check.Add((cur.x, cur.y));
    }

    var next = Next(cur.x, cur.y, cur.count, steps);
    foreach (var n in next)
    {
        if (map[n.y][n.x] != '.') continue;

        queue.Enqueue((n.x, n.y, n.count));
    }
}

//Print();
Console.WriteLine(check.Count);


List<(int x, int y, int count)> Next(int x, int y, int count, int max_steps)
{
    if (count > max_steps) return [];

    List<(int x, int y, int count)> next = [(x - 1, y, count+1), (x + 1, y, count + 1), (x, y - 1,count+1),(x, y + 1,count+1)];
    return next.Where(t => t.x >= 0 && t.x < map[0].Length && t.y >= 0 && t.y < map.Length).ToList();
}

void Print()
{
    Console.Clear();
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[y].Length; x++)
        {
            if (check.Contains((x, y)))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write('O');
                Console.ResetColor();
            }
            else
                Console.Write(map[y][x]);
        }
        Console.WriteLine();
    }
    //Console.ReadKey();
}