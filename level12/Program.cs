using System.Collections.Concurrent;

string[] lines = File.ReadAllLines("input.txt");

long sum = 0;
Dictionary<string, List<string>> history = new Dictionary<string, List<string>>();
ConcurrentDictionary<string, long> cache = new();

Parallel.ForEach(lines, line =>
{
    var split = line.Split(' ');
    //string springs = split[0];
    //int[] groups = split[1].Split(",").Select(int.Parse).ToArray();
    var springs = string.Join("?", Enumerable.Range(0, 5).Select(t => split[0]));
    string g = string.Join(",", Enumerable.Range(0, 5).Select(t => split[1]));
    var groups = g.Split(",").Select(int.Parse).ToArray();
    long count = Count(springs, groups);
    Console.WriteLine(line);
    Console.WriteLine(count);
    sum += count;
});

Console.WriteLine(sum);


long Count(string springs, int[] groups)
{
    if (springs.Length == 0)
    {
        return groups.Length == 0 ? 1 : 0;
    }

    if (groups.Length == 0)
    {
        return springs.Contains("#") ? 0 : 1;
    }

    string key = $"{springs};{string.Join(",", groups)}";
    if (cache.ContainsKey(key)) return cache[key];

    long result = 0;
    if (".?".Contains(springs[0]))
    {
        result += Count(springs[1..], groups);
    }

    if ("#?".Contains(springs[0]))
    {
        if (springs.Length >= groups[0] && !springs[..groups[0]].Contains(".") && (springs.Length == groups[0] || springs[groups[0]] != '#'))
            result += Count(springs.Substring(Math.Min(groups[0]+1, springs.Length)), groups[1..]);
    }
    cache.AddOrUpdate(key,result, (key,value) => result);
    return result;
}