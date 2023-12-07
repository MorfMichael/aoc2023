using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Transactions;
using System.Xml.Serialization;

string input = File.ReadAllText("input.txt");

var split = input.Split(Environment.NewLine + Environment.NewLine);
var seeds = split[0].Replace("seeds: ", "").Split().Select(long.Parse).Chunk(2).Select(t => new { Min = t[0], Max = t[0] + t[1] - 1 }).ToList();

Dictionary<long, long> bla = new();
var maps = split.Skip(1).Select(x => x.Split(Environment.NewLine).Skip(1).Select((t, i) => t.Split().Select(long.Parse).ToArray()).Select((t, i) => (dest: t[0], src: t[1], op: t[1] - t[0], length: t[2], map: i)).ToList()).ToList();

ulong lowest = ulong.MaxValue;

var a = new Range(10, 20);
var b = new Range(12, 16);

Console.WriteLine(a);
Console.WriteLine(b);

var res = a.Intersection(b);
Console.WriteLine(res.inside);
Console.WriteLine(res.outside);

Console.WriteLine("inside: " + res.inside.Start + "-" + res.inside.End);
Console.WriteLine("outside: " + res.outside.Start + "-" + res.outside.End);

foreach (var seed in seeds)
{

}

Console.WriteLine(lowest);



class Range
{
    public int Start { get; set; }
    public int End { get; set; }

    public Range(int start, int end)
    {
        Start = start;
        End = end;
    }

    public (Range? inside, Range? outside) Intersection(Range other)
    {
        if (End >= other.Start && Start <= other.End) // intersection
        {
            if (Start >= other.Start && End <= other.End)
            {
                return (new Range(Start, End), null);
            }
            else
            {
                int start = Math.Max(Start, other.Start);
                int end = Math.Min(End, other.End);
                Range including = new Range(start, end);

                if (Start <= other.Start)
                {
                    int s_e = Start;
                    int e_e = other.Start - 1;
                    return (including, new Range(s_e, e_e));
                }
                else
                {
                    int s_e = other.End + 1;
                    int e_e = End;
                    return (including, new Range(s_e, e_e));
                }

            }
        }
        else
        {
            return (null, null);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(' ', Start-1);
        sb.Append('|', 1);
        sb.Append('-', End - Start - 1);
        sb.Append('|', 1);
        return sb.ToString();
    }
}