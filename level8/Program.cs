using System.Runtime.CompilerServices;

string[] lines = File.ReadAllLines("input.txt");

string instructions = lines[0];

Dictionary<string, (string left, string right)> data = lines.Skip(2).Select(t => t.Split(" = ")).Select(t => (pos: t[0], left: t[1].Split(",")[0][1..], right: t[1].Split(", ")[1][..^1])).ToDictionary(t => t.pos, t => (t.left, t.right));

int count = 0;
string[] current = data.Where(t => t.Key.EndsWith("A")).Select(t => t.Key).ToArray();
Dictionary<int,int> found = new();
while (true)
{
    for (int i = 0; i < instructions.Length; i++)
    {
        count++;
        for (int j = 0; j < current.Length; j++)
        {
            current[j] = instructions[i] == 'R' ? data[current[j]].right : data[current[j]].left;
            if (current[j].EndsWith("Z") && !found.ContainsKey(j))
            {
                Console.WriteLine(j + ": " + count);
                found.Add(j, count);
            }
        }

        if (found.Count == current.Length) goto result;
    }
}

result:
Console.WriteLine(found.Aggregate(1l, (a, b) => lcm(a, b.Value)));

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