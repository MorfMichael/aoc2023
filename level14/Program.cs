using System.Data;

string[] lines = File.ReadAllLines("input.txt");

//Print(lines);
string[] columns = GetColumns(lines);
//Print(columns);
columns = columns.Select(t => string.Join("#", t.Split("#").Select(x => new string(x.OrderDescending().ToArray())))).ToArray();
//Print(columns);

long sum = 0;
foreach (var c in columns)
{
    sum += c.Select((t, i) => t == 'O' ? c.Length - i : 0).Sum();
}

Console.WriteLine(sum);



void Print(string[] columns)
{
    foreach (var c in columns)
    {
        Console.WriteLine(c);
    }
    Console.WriteLine();
}

string[] GetColumns(string[] lines) => Enumerable.Range(0, lines[0].Length).Select(t => new string(lines.Select(l => l[t]).ToArray())).ToArray();