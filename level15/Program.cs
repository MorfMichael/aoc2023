using System.Collections.Concurrent;

string input = File.ReadAllText("input.txt");
var split = input.Split(",");

long sum = 0;
ConcurrentDictionary<int, List<(string Lens, int Value)>> boxes = new();

foreach (var operation in split)
{
    if (operation[^1] == '-')
    {
        string label = operation[..^1];
        int box = GetHash(label);
        if (boxes.ContainsKey(box))
        {
            var exists = boxes[box].FirstOrDefault(x => x.Lens == label);
            boxes[box].Remove(exists);
        }
    }
    else
    {
        var bla = operation.Split("=");
        int focalLength = int.Parse(bla[1]);
        int box = GetHash(bla[0]);
        var content = boxes.AddOrUpdate(box, [(bla[0], focalLength)], (k, v) =>
        {
            var exists = v.FirstOrDefault(x => x.Lens == bla[0]);
            if (exists != default)
            {
                int idx = v.IndexOf(exists);
                v.Remove(exists);
                v.Insert(idx, (bla[0], focalLength));
            }
            else
            {
                v.Add((bla[0], focalLength));
            }
            return v;
        });
        
    }
}

foreach (var box in boxes)
{
    for (int i = 0; i < box.Value.Count; i++)
    {
        string lens = box.Value[i].Lens;
        int length = (box.Key + 1) * (i + 1) * box.Value.ElementAt(i).Value;
        Console.WriteLine(lens + ": " + length);
        sum += length;
    }
}

Console.Write(sum);


int GetHash(string input)
{
    int result = 0;
    for (int i = 0; i < input.Length; i++)
    {
        Console.WriteLine((int)input[i]);
        result += (int)input[i];
        result *= 17;
        result %= 256;
    }

    return result;
}