using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

string[] lines = File.ReadAllLines("input.txt");

//string s = "...";
//foreach (var perm in Permutations(s, [0, 1, 2]))
//    Console.WriteLine(perm);

//return;

int sum = 0;
foreach (var line in lines)
{
    var split = line.Split(' ');
    var springs = split[0];
    var groups = split[1].Split(",").Select(int.Parse).ToArray();


    int[] pos = springs.Select((t, i) => new { t = t, i = i }).Where(t => t.t == '?').Select(t => t.i).ToArray();
    var permutations = Permutations(springs.Replace("?", "."), pos);
    //Console.WriteLine(permutations.Count());
    permutations = permutations.Distinct();
    //Console.WriteLine(permutations.Count());
    //Console.WriteLine(string.Join(Environment.NewLine, permutations));
    int c = permutations.Count(t => Match(t.ToString(), groups));
    Console.WriteLine(line);
    Console.WriteLine(c);
    sum += c;
}

Console.WriteLine(sum);

IEnumerable<string> Permutations(string input, int[] positions, int depth = 0)
{
    if (depth == 0)
    {
        yield return input;
    }

    for (int i = 0; i < positions.Length; i++)
    {
        string s = input.Remove(positions[i], 1).Insert(positions[i], "#");
        yield return s;

        foreach (var perm in Permutations(s, positions[(i+1)..], depth++))
        {
            yield return perm;
        }
    }
}

bool Match(string springs, int[] groups)
{
    var grouped = springs.Split(".", StringSplitOptions.RemoveEmptyEntries).ToArray();
    //var tmp = groups.Select((t, i) => grouped[i].Length == t).ToList();
    return grouped.Count() == groups.Length && groups.Select((t, i) => grouped[i].Length == t).All(t => t);

}