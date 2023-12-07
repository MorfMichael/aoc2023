using System.Xml.Linq;

string[] lines = File.ReadAllLines("input.txt");


Dictionary<char, int> weights = new Dictionary<char, int>
{
    { '2', 2 },
    { '3', 3 },
    { '4', 4 },
    { '5', 5 },
    { '6', 6 },
    { '7', 7 },
    { '8', 8 },
    { '9', 9 },
    { 'T', 10 },
    { 'J', 11 },
    { 'Q', 12 },
    { 'K', 13 },
    { 'A', 14 },
};

List<(string input, Dictionary<char, int> cards, int bid, string weight)> hands = new();

foreach (var line in lines)
{
    var split = line.Split();
    var cards = split[0].GroupBy(t => t).ToDictionary(t => t.Key, t => t.Count());
    var bid = int.Parse(split[1]);
    hands.Add((split[0], cards, bid, GetWeight(split[0])));
}

var ordered = hands.OrderBy(t => t.weight).ToList();

int sum = ordered.Select((t, i) => new { Bid = t.bid, Index = i + 1 }).Sum(x => x.Bid*x.Index);
Console.WriteLine(sum);


Dictionary<char, int> GetCards(string line)
{
    return line.GroupBy(t => t).ToDictionary(t => t.Key, t => t.Count());
}

string GetMapping(string input)
{
    string mapped = string.Join("", input.Select(t => weights[t].ToString("D2")));
    return mapped;
}

string GetWeight(string line)
{
    var cards = GetCards(line);

    if (cards.Any(t => t.Value == 5))
    {
        // five of a kind
        return 7+GetMapping(line);
    }
    else if (cards.Any(t => t.Value == 4))
    {
        // four of a kind
        return 6+GetMapping(line);
    }
    else if (cards.Any(t => t.Value == 2) && cards.Any(t => t.Value == 3))
    {
        // full house
        return 5+GetMapping(line);
    }
    else if (cards.Any(t => t.Value == 3))
    {
        // three of a kind
        return 4+GetMapping(line);
    }
    else if (cards.Count(t => t.Value == 2) >= 2)
    {
        // two pairs
        return 3+GetMapping(line);

    }
    else if (cards.Any(t => t.Value == 2))
    {
        // one pair
        return 2+GetMapping(line);
    }
    else
    {
        //highcard
        return 1+GetMapping(line);
    }
}