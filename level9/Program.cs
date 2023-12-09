string[] lines = File.ReadAllLines("input.txt");

var histories = lines.Select(t => t.Split().Select(long.Parse).ToList()).ToList();

foreach (var history in histories)
{
    var current = history;
    List<List<long>> differences = new() { current.ToList() };
    while (!current.All(x => x == 0))
    {
        List<long> diff = new();
        for (int i = 0; i < current.Count-1; i++)
        {
            diff.Add(current[i + 1] - current[i]);
        }
        differences.Add(diff);
        current = diff.ToList();
    }

    for (int i = differences.Count-1; i >= 0; i--)
    {
        if (i == differences.Count - 1)
        {
            differences[i].Insert(0, differences[i][0]);
        }
        else
        {
            differences[i].Insert(0, differences[i][0] - differences[i + 1][0]);
        }
    }

    history.Insert(0, differences[0][0]);

    Console.WriteLine(string.Join(Environment.NewLine, differences.Select(t => string.Join(" ", t))));
}

Console.WriteLine(string.Join(" + ", histories.Select(t => t[0])) + " = " + histories.Sum(t => t[0]));