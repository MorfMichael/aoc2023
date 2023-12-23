using System.Numerics;

string[] lines = File.ReadAllLines("input.txt");

Dictionary<string, Module> modules = new();
Dictionary<string, string[]> destinations = new();

Queue<(string source, string[] destinations, string? previous, bool impulse)> queue = new();

foreach (var line in lines)
{
    var split = line.Split(" -> ");
    string key = "%&".Contains(split[0][0]) ? split[0][1..] : split[0];
    var D = split[1].Split(", ", StringSplitOptions.RemoveEmptyEntries);

    if (!modules.ContainsKey(key))
        modules.Add(key, Module.GetModule(split[0]));

    if (!destinations.ContainsKey(key))
        destinations.Add(key, D);
}

var conjunctions = modules.Select(t => (t.Key, Module: t.Value as ConjunctionModule)).Where(t => t.Module != null).ToList();
foreach (var c in conjunctions)
{
    var inputs = destinations.Where(t => t.Value.Contains(c.Key)).Select(t => t.Key).ToArray();
    c.Module.UpdateMemory(inputs);
}


long high = 0;
long low = 0;

var before_rx = destinations.Single(t => t.Value.Contains("rx")).Key;

Dictionary<string,int> seen = destinations.Where(t => t.Value.Contains(before_rx)).ToDictionary(x => x.Key, x => 0);
Dictionary<string, int> cycles = new();

int i = 0;
while (true)
{
    i++;
    queue.Enqueue(("button", ["broadcaster"], null, false));
    while (queue.TryDequeue(out var q))
    {
        bool? impulse = q.impulse;
        if (modules.ContainsKey(q.source))
        {
            impulse = modules[q.source].ConvertImpulse(q.previous, impulse.Value);
        }

        if (!impulse.HasValue)
            continue;

        foreach (var d in q.destinations)
        {
            if (d == before_rx && impulse.Value)
            {
                seen[q.source]++;

                if (!cycles.ContainsKey(q.source))
                    cycles.Add(q.source, i);
                else
                    if (i != seen[q.source] * cycles[q.source]) throw new Exception("assertion");

                if (seen.All(x => x.Value > 100))
                {
                    Console.WriteLine(cycles.Aggregate(1l, (a, b) => lcm(a, b.Value)));
                    return;
                }
            }
            low += impulse.Value ? 0 : 1;
            high += impulse.Value ? 1 : 0;
            //Console.WriteLine(q.source + " -" + (impulse.Value ? "high" : "low") + "-> " + d);
            if (destinations.ContainsKey(d))
                queue.Enqueue((d, destinations[d], q.source, impulse.Value));
        }
    }
}

Console.WriteLine(low);
Console.WriteLine(high);
Console.WriteLine(low * high);



static long gcf(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

static long lcm(long a, long b)
{
    return (a / gcf(a, b)) * b;
}

class Module(string name)
{
    public string Name { get; set; } = name;

    public virtual bool? ConvertImpulse(string module, bool impulse)
    {
        return impulse;
    }

    public static Module GetModule(string name)
    {
        if (name.StartsWith("%"))
            return new FlipFlopModule(name[1..]);
        else if (name.StartsWith("&"))
            return new ConjunctionModule(name[1..]);

        return new Module(name);
        //throw new NotImplementedException();
    }
}


class FlipFlopModule(string name) : Module(name)
{
    public bool On { get; set; }

    public override bool? ConvertImpulse(string module, bool impulse)
    {
        if (!impulse)
        {
            On = !On;
            return On;
        }

        return null;
    }
}

class ConjunctionModule(string name) : Module(name)
{
    private Dictionary<string, bool> _memory = new();

    public void UpdateMemory(string[] inputs)
    {
        foreach (var key in inputs.Where(t => !_memory.ContainsKey(t)))
        {
            _memory.Add(key, false);
        }
    }

    public override bool? ConvertImpulse(string module, bool impulse)
    {
        _memory[module] = impulse;

        return !_memory.All(t => t.Value);
    }
}