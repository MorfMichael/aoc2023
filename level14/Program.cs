using System.Data;

string[] grid = File.ReadAllLines("input.txt");

string key = string.Join(",", grid);
HashSet<string> seen = new() { key } ;
List<string> check = new() { key };

int i = 0;
while (true)
{
    i++;
    grid = Cycle(grid);
    key = string.Join(",", grid);
    if (seen.Contains(key))
        break;
    seen.Add(key);
    check.Add(key);
}

int first = check.IndexOf(string.Join(",", grid));

int index = (1_000_000_000 - first) % (i - first) + first;
grid = check[index].Split(",");

long sum = grid.Select((x, i) => (x.Split("O").Length - 1) * (grid.Length - i)).Sum();
Console.WriteLine(sum);

string[] Cycle(string[] lines)
{
    for (int i = 0; i < 4; i++)
    {
        lines = GetColumns(lines);
        lines = lines.Select(t => string.Join("#", t.Split("#").Select(x => new string(x.OrderDescending().ToArray())))).ToArray();
        lines = lines.Select(t => new string(t.Reverse().ToArray())).ToArray();
    }

    return lines;
}


void Print(string[] columns)
{
    foreach (var c in columns)
    {
        Console.WriteLine(c);
    }
    Console.WriteLine();
    Console.ReadKey();
}

string[] GetColumns(string[] lines) => Enumerable.Range(0, lines[0].Length).Select(t => new string(lines.Select(l => l[t]).ToArray())).ToArray();
string[] GetLines(string[] columns) => Enumerable.Range(0, columns.Length).Select(t => new string(columns.Select(c => c[t]).ToArray())).ToArray();