using System.Xml.Linq;

string[] lines = File.ReadAllLines("input.txt");


Dictionary<char, int> weights = new Dictionary<char, int>
{
    { 'J', 1 },
    { '2', 2 },
    { '3', 3 },
    { '4', 4 },
    { '5', 5 },
    { '6', 6 },
    { '7', 7 },
    { '8', 8 },
    { '9', 9 },
    { 'T', 10 },
    { 'Q', 12 },
    { 'K', 13 },
    { 'A', 14 },
};

List<(string input, int bid, string weight, Dictionary<char, int> cards)> hands = new();

foreach (var line in lines)
{
    var split = line.Split();
    var cards = split[0].GroupBy(t => t).ToDictionary(t => t.Key, t => t.Count());
    var bid = int.Parse(split[1]);
    hands.Add((split[0], bid, GetWeight(split[0]), cards));
}

var ordered = hands.OrderBy(t => t.weight).ToList();
var bla = ordered.GroupBy(t => t.weight).Any(t => t.Count() > 1);
Console.WriteLine(string.Join(Environment.NewLine, ordered.Where(t => true).Select(t => t.input + " - " + t.weight)));
int sum = ordered.Select((t, i) => new { Bid = t.bid, Index = i + 1 }).Sum(x => x.Bid * x.Index);
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
    var all_cards = GetCards(line);
    var joker = all_cards.ContainsKey('J') ? all_cards['J'] : 0;
    var cards = all_cards.Where(t => t.Key != 'J').ToDictionary();

    if (cards.Any(t => t.Value + joker == 5) || joker == 5)
    {
        return 7 + GetMapping(line);
    }
    else if (cards.Any(t => t.Value + joker == 4))
    {
        return 6 + GetMapping(line);
    }
    else if (CheckFullHouse(cards, joker))
    {
        return 5 + GetMapping(line);
    }
    else if (cards.Any(t => t.Value + joker == 3))
    {
        return 4 + GetMapping(line);
    }
    else if (CheckTwoParis(cards, joker))
    {
        return 3 + GetMapping(line);

    }
    else if (cards.Any(t => t.Value + joker == 2))
    {
        return 2 + GetMapping(line);
    }
    else
    {
        return 1 + GetMapping(line);
    }
}

bool CheckFullHouse(Dictionary<char, int> cards, int joker)
{
    var two = cards.FirstOrDefault(t => t.Value + joker == 2).Key;
    var three = cards.FirstOrDefault(t => t.Key != two && t.Value == 3).Key;
    if (two != default && three != default)
        return true;

    two = cards.FirstOrDefault(t => t.Value == 2).Key;
    three = cards.FirstOrDefault(t => t.Key != two && t.Value + joker == 3).Key;
    if (two != default && three != default)
        return true;

    return false;
}

bool CheckTwoParis(Dictionary<char, int> cards, int joker)
{
    var two = cards.FirstOrDefault(t => t.Value == 2).Key;
    var other = cards.FirstOrDefault(t => t.Key != two && t.Value == 2).Key;
    if (two != default && other != default)
        return true;

    two = cards.FirstOrDefault(t => t.Value + joker == 2).Key;
    other = cards.FirstOrDefault(t => t.Key != two &&  t.Value == 2).Key;
    return two != default && other != default;
}