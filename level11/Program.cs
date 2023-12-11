string[] lines = File.ReadAllLines("input.txt");
char[][] map = new char[lines.Length][];
Dictionary<int, (int X, int Y)> galaxies = new();

int gidx = 1;
for (int y = 0; y < lines.Length; y++)
{
    map[y] = lines[y].ToCharArray();
    for (int x = 0; x < lines[y].Length; x++)
    {
        if (lines[y][x] == '#')
            galaxies.Add(gidx++, (x, y));
    }
}

int[] y_offsets = lines.Select((t, i) => new { Expanding = !t.Contains('#'), Index = i }).Where(t => t.Expanding).Select(t => t.Index).ToArray();
int[] x_offsets = Enumerable.Range(0, lines[0].Length).Select(t => new { Expanding = !string.Join("", lines.Select(l => l[t])).Contains('#'), Index = t }).Where(t => t.Expanding).Select(t => t.Index).ToArray();

int sum = 0;
Dictionary<(int Start, int End), long> distances = new();

foreach (var start in galaxies)
{
    foreach (var end in galaxies.Where(t => t.Key != start.Key))
    {
        if (distances.ContainsKey((end.Key, start.Key)))
        {
            continue;
        }

        int y_steps = Math.Abs(end.Value.Y - start.Value.Y);
        int x_steps = Math.Abs(end.Value.X - start.Value.X);


        y_steps += y_offsets.Count(t => t >= Math.Min(start.Value.Y,end.Value.Y) && t <= Math.Max(start.Value.Y, end.Value.Y))* (1000000-1);
        x_steps += x_offsets.Count(t => t >= Math.Min(start.Value.X, end.Value.X) && t <= Math.Max(start.Value.X, end.Value.X)) * (1000000-1);

        int steps = x_steps + y_steps;

        Console.WriteLine(start.Key + " -> " + end.Key + " = " + steps);
        distances.Add((start.Key, end.Key), steps);
        sum += steps;
    }
}

Console.WriteLine(distances.Count);
Console.WriteLine(distances.Sum(x => x.Value));