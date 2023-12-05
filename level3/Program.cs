using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("input.txt");

Console.WriteLine(string.Join(",", lines.SelectMany(t => t.ToArray()).Distinct().Where(t => t != '.' && !char.IsNumber(t))));
//return;

int height = lines.Length;
int width = lines[0].Length;

var numbers = new List<(int x, int y, string number)>();
var stars = new List<(int x, int y, List<int> numbers)>();

char[][] map = new char[height][];

for (int i = 0; i < lines.Length ; i++)
{
    string line = lines[i];
    map[i] = line.ToArray();

    var matches = Regex.Matches(line, @"\d+");
    foreach (var match in matches.OfType<Match>())
    {
        numbers.Add((match.Index, i, match.Value));
    }

    var star_matches = Regex.Matches(line, @"\*");
    foreach (var star in star_matches.OfType<Match>())
    {
        stars.Add((star.Index, i, new()));
    }
}

int count = 0;
foreach (var n in numbers)
{
    var nes = GetNeighbours(n.x, n.y, n.number.Length, width, height);
    var bla = nes.Where(t => map[t.y][t.x] == '*');
    foreach (var b in bla)
    {
        var star = stars.FirstOrDefault(s => s.x == b.x && s.y == b.y);
        if (star != default)
        {
            star.numbers.Add(int.Parse(n.number));
        }
    }
}

foreach (var s in stars.Where(t => t.numbers.Count == 2))
{
    count += s.numbers[0] * s.numbers[1];
}

Console.WriteLine(count);


List<(int x, int y)> GetNeighbours(int x, int y, int length, int width, int height)
{
    var list = new List<(int x, int y)>();

    for (int i = 0; i < length; i++)
    {
        if (i == 0)
        {
            list.AddRange([
                (x+i-1, y-1),
                (x+i-1, y),
                (x+i-1, y+1),
            ]);
        }
        
        if (i == length - 1)
        {
            list.AddRange([
                (x+i+1,y-1),
                (x+i+1, y),
                (x+i+1, y+1),
            ]);
        }

        list.AddRange([
            (x+i, y - 1),
            (x+i, y + 1)
        ]);

    }

    return list.Distinct().Where(t => t.x >= 0 && t.x < width && t.y >= 0 && t.y < height).ToList();
}