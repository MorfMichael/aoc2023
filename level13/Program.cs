using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.JavaScript;

string text = File.ReadAllText("input.txt");

var groups = text.Split(Environment.NewLine + Environment.NewLine);
long sum = 0;
for (int g = 0; g < groups.Length; g++)
{
    Console.WriteLine("group " + g);
    var group = groups[g];

    string[] lines = group.Split(Environment.NewLine);
    string[] columns = GetColumns(lines);

    int horizontal = Check(lines);
    sum += 100 * horizontal;
    int vertical = Check(columns);
    sum += vertical;
}

Console.WriteLine(sum);

string[] GetColumns(string[] lines)
{
    List<string> columns = new List<string>();

    for (int j = 0; j < lines[0].Length; j++)
    {
        string column = "";
        for (int i = 0; i < lines.Length; i++)
        {
            column += lines[i][j];
        }
        columns.Add(column);
    }
    return columns.ToArray();
}

int Check(string[] lines)
{
    for (int i = 1; i < lines.Length; i++)
    {
        var top = lines[..i].Reverse().ToArray();
        var bottom = lines[i..];

        top = top.Take(bottom.Length).ToArray();
        bottom = bottom.Take(top.Length).ToArray();

        if (Enumerable.SequenceEqual(top, bottom))
            return i;
    }

    return 0;
}