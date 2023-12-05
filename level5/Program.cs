using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

string input = File.ReadAllText("input.txt");

var split = input.Split(Environment.NewLine + Environment.NewLine);
var seeds = split[0].Replace("seeds: ", "").Split().Select(long.Parse).Chunk(2).Select(t => (min: t[0], max: t[0] + t[1])).ToList();

Dictionary<long, long> bla = new();
var maps = split.Skip(1).Select((x,i) => new { Index = i, Map = x.Split(Environment.NewLine)[0], List = x.Split(Environment.NewLine).Skip(1).Select((t, i) => t.Split().Select(long.Parse).ToArray()).Select((t, i) => (dest: t[0], src: t[1], op: t[1] - t[0], length: t[2], map: i)).ToList() }).ToList();

var dest = maps[6].List.Where(t => t.dest != 0).Min(x => x.dest);
Console.WriteLine(dest);
//return;

//for (int i = 5; i >= 0; i--)
//{

//}

//foreach (var seed in seeds)
//{
//    for (long i = seed[0]; i < seed[1]; i++)
//    {
//        long src = i;

//        if (bla.ContainsKey(src))
//        {
//            continue;
//        }

//        foreach (var m in maps)
//        {
//            var m1 = m.FirstOrDefault(x => src >= x.src && src <= x.src + x.length);
//            if (m1 != default)
//            {
//                src = src - m1.op;
//            }
//        }

//        bla.Add(i, src);
//        lowest = bla[src] < lowest ? bla[src] : lowest;
//    }
//}

//Console.WriteLine(lowest);