using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Serialization;
using System.Text;

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
    for (int i = 0; i < lines.Length-1; i++)
    {
        int failure = 0;
        for (int j = 1; i - (j - 1) >= 0 && i + j < lines.Length; j++)
        {
            failure += Compare(lines[i - (j - 1)], lines[i+j]);
        }

        if (failure==1)
        {
            Console.WriteLine(i+1);
            return i+1;
        }
    }

    return 0;
}

int Compare(string a, string b)
{
    int count = 0;
    for (int i = 0; i < a.Length; i++)
    {
        if (a[i] != b[i]) count++;
    }

    return count;
}

/*
37359
37360
32932
24466
26223
9131
26277
32754
26223
34202*
*/