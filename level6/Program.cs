using System.Security.Cryptography.X509Certificates;

string[] lines = File.ReadAllLines("input.txt");

long time = long.Parse(string.Join("", lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1)));
long distance = long.Parse(string.Join("", lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1)));


long sum = 1;
long count = 0;
for (int t = 0; t <= time; t++)
{
    long dist = t * (time - t);
    if (dist > distance)
        count++;
}

sum *= count;

Console.WriteLine(sum);