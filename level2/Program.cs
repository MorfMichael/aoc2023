using System.Collections.Concurrent;

string[] lines = File.ReadAllLines("input.txt");

var games = new List<(int Game, int Red, int Green, int Blue)>();

int count = 0;
foreach (var line in lines)
{
    var values = new ConcurrentDictionary<string, int>();
    var split = line.Split(": ");
    int game = int.Parse(split[0].Replace("Game ", ""));

    var sets = split[1].Split("; ");

    foreach (var set in sets)
    {
        var cubes = set.Split(", ").Select(t => t.Split()).Select(t => new { Value = int.Parse(t[0]), Key = t[1] }).ToList();
        foreach (var cube in cubes)
        {
            values.AddOrUpdate(cube.Key, cube.Value, (key, value) => cube.Value > value ? cube.Value : value);
        }
    }
    
    int product = values.Aggregate(1, (a, b) => a * b.Value);
    Console.WriteLine(string.Join(",", values.Select(t => t.Value)) + " = " + product);
    count += product;
}

Console.WriteLine(count);