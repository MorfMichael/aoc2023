using System.Collections.Concurrent;

string[] lines = File.ReadAllLines("input.txt");

double count = 0;

ConcurrentDictionary<int, int> c_count = new ConcurrentDictionary<int, int>();

foreach (var line in lines)
{
    var split = line.Split('|');

    var first_split = split[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var wining = first_split[2..].Select(int.Parse).ToArray();
    var numbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    int card = int.Parse(first_split[1][..^1]);
    int c = wining.Count(x => numbers.Contains(x));

    var next = Enumerable.Range(card + 1, c).ToList();
    //Console.WriteLine("Card " + card + ": " + string.Join(",", next));
    int amount = c_count.GetOrAdd(card, 1);
    for (int i = 0; i < amount; i++)
    {
        foreach (var n in next)
        {
            c_count.AddOrUpdate(n, 2, (key, current) => current + 1);
        }
    }
}

//Console.WriteLine(string.Join(Environment.NewLine, c_count.Select(t => t.Key + ": " + t.Value)));

Console.WriteLine(c_count.Sum(x => x.Value));