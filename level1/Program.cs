using System.Runtime.InteropServices.Marshalling;

string[] lines = File.ReadAllLines("input.txt");

var map = new List<(string Key, char Value)>
{
    ("one", '1' ),
    ( "two", '2' ),
    ( "three", '3' ),
    ( "four", '4' ),
    ( "five", '5' ),
    ( "six", '6' ),
    ( "seven", '7' ),
    ( "eight", '8' ),
    ( "nine", '9' ),
};

int count = 0;
foreach (var line in lines)
{
    char? first = null, second = null;
    for (int i = 0; i < line.Length; i++)
    {
        if (char.IsNumber(line[i]))
        {
            first = first ?? line[i];
            second = line[i];
        } 
        else
        {
            var entry = map.FirstOrDefault(x => line.Substring(i).StartsWith(x.Key));
            if (entry != default)
            {
                first = first ?? entry.Value;
                second = entry.Value;
            }
        }
    }

    count += int.Parse($"{first}{second}");
}

Console.WriteLine(count);