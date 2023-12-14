using System.Data;
using System.Net;
using System.Runtime.InteropServices;

string[] lines = File.ReadAllLines("input.txt");

char[][] map = new char[lines.Length][];

for (int y = 0; y < lines.Length; y++)
{
    map[y] = lines[y].ToCharArray();
}


Print(map);

int count = 0;
var rocks = GetRocks(map).Where(t => CanMove(t.X,t.Y,map));
while (rocks.Any())
{
    Console.WriteLine(count++);
    foreach (var rock in rocks)
    {
        Move(rock.X, rock.Y, map);
    }

    //Print(map);
    //Console.ReadKey();
    rocks = rocks.Where(t => CanMove(t.X, t.Y, map));
}

Console.WriteLine(GetRocks(map).Sum(x => GetLoad(x.Y, map)));

void Move(int x, int y, char[][] map)
{
    char upper = map[y - 1][x];
    map[y - 1][x] = map[y][x];
    map[y][x] = upper;
}

int GetLoad(int y, char[][] map)
{
    return map.Length - y;
}

bool CanMove(int x, int y, char[][] map)
{
    return y > 0 && map[y - 1][x] == '.';
}

IEnumerable<(int X, int Y)> GetRocks(char[][] map)
{
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[y].Length; x++)
        {
            if (map[y][x] == 'O')
                yield return (x, y);
        }
    }
}

void Print(char[][] map)
{
    Console.Clear();
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[y].Length; x++)
        {
            Console.Write(map[y][x]);
        }
        Console.WriteLine();
    }
}