using System.Security.Cryptography.X509Certificates;

string[] lines = File.ReadAllLines("input.txt");

int[] time = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
int[] distance = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();


int sum = 1;

for (int i = 0; i < time.Length; i++)
{
    int count = 0;
    for (int t = 0; t <= time[i]; t++)
    {
        int dist = t * (time[i] - t);
        if (dist > distance[i])
            count++;
    }

    sum *= count;
}

Console.WriteLine(sum );