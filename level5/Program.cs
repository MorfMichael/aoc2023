using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

string input = File.ReadAllText("input.txt");

var split = input.Split(Environment.NewLine + Environment.NewLine);
var seeds = split[0].Replace("seeds: ", "").Split().Select(long.Parse).ToList();

Dictionary<long, long> bla = new();
var maps = split.Skip(1).Select(x => x.Split(Environment.NewLine).Skip(1).Select((t, i) => t.Split().Select(long.Parse).ToArray()).Select((t, i) => (dest: t[0], src: t[1], op: t[1] - t[0], length: t[2], map: i)).ToList()).ToList();

long lowest = long.MaxValue;

foreach (var seed in seeds)
{
    long src = seed;

    foreach (var m in maps)
    {
        var m1 = m.FirstOrDefault(x => src >= x.src && src <= x.src + x.length);
        if (m1 != default)
        {
            src = src - m1.op;
        }
    }

    lowest = src < lowest ? src : lowest;
}

Console.WriteLine(lowest);