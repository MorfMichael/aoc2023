string input = File.ReadAllText("input.txt");
var split = input.Split(",");

long sum = 0;
foreach (var s in split)
{
    sum += GetHash(s);
}

Console.Write(sum);


long GetHash(string input)
{
    long result = 0;
    for (int i = 0; i < input.Length; i++)
    {
        Console.WriteLine((int)input[i]);
        result += (int)input[i];
        result *= 17;
        result %= 256;
    }

    return result;
}