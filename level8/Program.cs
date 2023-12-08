using System.Runtime.CompilerServices;

string[] lines = File.ReadAllLines("input.txt");

string instructions = lines[0];

Dictionary<string, (string left, string right)> data = lines.Skip(2).Select(t => t.Split(" = ")).Select(t => (pos: t[0], left: t[1].Split(",")[0][1..], right: t[1].Split(", ")[1][..^1])).ToDictionary(t => t.pos, t => (t.left,t.right));

int count = 0;
string curr = "AAA";
bool found = false;

while (true)
{
    for (int i = 0; i < instructions.Length; i++)
    {
        count++;
        curr = instructions[i] == 'R' ? data[curr].right : data[curr].left;
        if (curr == "ZZZ")
        {
            Console.WriteLine(count);
            return;
        }
    }
}