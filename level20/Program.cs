
using System.Collections.Concurrent;

string[] lines = File.ReadAllLines("sample2.txt");

Dictionary<string, string[]> destinations = new();

foreach (var line in lines)
{
    var split = line.Split(" -> ");
    string key = "%&".Contains(split[0][0]) ? split[0][1..] : split[0];
    var D = split[1].Split(", ", StringSplitOptions.RemoveEmptyEntries);

    if (destinations.ContainsKey(key))
    {
        destinations[key] = new List<string>([.. destinations[key], .. D]).Distinct().ToArray();
    }
    else
    {
        destinations.Add(key, D);
    }
}

var distinct = destinations.SelectMany(t => t.Value).Distinct().Where(t => !destinations.ContainsKey(t)).ToList();
foreach (var d in distinct)
{
    destinations.Add(d, []);
}


BroadcasterModule broadcaster = new BroadcasterModule("broadcaster", destinations);
int low = 0, high = 0;
//for (int i = 0; i < 1000; i++)
{
    low++; // button
    var (clow, chigh) = broadcaster.HandleImpulse(false);
    low += clow;
    high += chigh;
}
Console.WriteLine(low * high);

abstract class Module(string name, Dictionary<string, string[]> destinations)
{
    public string Name { get; set; } = name;

    public List<Module> Destinations { get; set; } = destinations[name].Select(t => GetModule(t, destinations)).ToList();

    public (int low, int high) HandleImpulse(bool impulse)
    {
        Queue<(bool impulse, Module module)> queue = new();
        int low = 0, high = 0;
        impulse = ConvertImpulse(Name, impulse);
        high += impulse ? 1 : 0;
        low += impulse ? 0 : 1;
        //Destinations.ForEach(x => queue.Enqueue((impulse, x)));
        foreach (var m in Destinations)
        {
            Console.WriteLine(Name + " -> " + impulse + " -> " + m.Name);

            queue.Enqueue((impulse, m));

        }

        while (queue.TryDequeue(out var current))
        {
            (int mlow, int mhigh) = current.module.HandleImpulse(current.impulse);
            low += mlow;
            high += mhigh; 
        }

        return (low, high);
    }

    public abstract bool ConvertImpulse(string module, bool impulse);

    public static Module GetModule(string name, Dictionary<string, string[]> destinations)
    {
        if (name.StartsWith("%"))
            return new FlipFlopModule(name, destinations);
        else if (name.StartsWith("&"))
            return new ConjunctionModule(name, destinations);
        else if (name == "broadcaster")
            return new BroadcasterModule(name, destinations);
        else
            return new DummyModule(name, destinations);
        //throw new NotImplementedException();
    }
}


class FlipFlopModule(string name, Dictionary<string, string[]> destinations) : Module(name, destinations)
{
    public bool On { get; set; }

    public override bool ConvertImpulse(string module, bool impulse)
    {
        if (!impulse)
        {
            On = !On;
            return On;
        }

        return impulse;
    }
}

class ConjunctionModule(string name, Dictionary<string, string[]> destinations) : Module(name, destinations)
{
    ConcurrentDictionary<string, bool> Memory = new();

    public override bool ConvertImpulse(string module, bool impulse)
    {
        Memory.AddOrUpdate(module, false, (k,v) => impulse);
        return !Memory.All(t => t.Value);
    }
}

class BroadcasterModule(string name, Dictionary<string, string[]> destinations) : Module(name, destinations)
{
    public override bool ConvertImpulse(string module, bool impulse)
    {
        return impulse;
    }
}

class DummyModule(string name, Dictionary<string, string[]> destinations) : Module(name, destinations)
{
    public new void HandleImpulse(bool impulse)
    {
    }

    public override bool ConvertImpulse(string module, bool impulse)
    {
        return impulse;
    }
}