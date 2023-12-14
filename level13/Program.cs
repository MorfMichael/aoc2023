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

    int hcount = 0;
    int hreflection = 0;
    for (int i = 0; i < lines.Length; i++)
    {
        int rcount = 0;
        for (int j = 1; i - j >= 0 && i + j < lines.Length; j++)
        {
            if (lines[i - (j - 1)] == lines[i + j])
                rcount++;
        }

        if (rcount > hcount)
        {
            hcount = rcount;
            hreflection = i;
            Console.WriteLine("horizontal reflection found: " + hreflection + ": " + hcount);
        }
    }

    int vcount = 0;
    int vreflection = 0;
    for (int i = 0; i < columns.Length; i++)
    {
        int rcount = 0;
        for (int j = 1; i - j >= 0 && i + j < columns.Length; j++)
        {
            if (columns[i - (j - 1)] == columns[i + j])
                rcount++;
        }

        if (rcount > vcount)
        {
            vcount = rcount;
            vreflection = i;
            Console.WriteLine("vertical reflection found: " + vreflection + ": " + vcount);
        }
    }

    sum += hcount > vcount ? 100 * (hreflection + 1) : vreflection + 1;
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