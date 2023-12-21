string[] lines = File.ReadAllLines("sample.txt");

Point current = (0, 0);
List<Point> edges = new() { current };
int sum = 0;
foreach (var line in lines)
{
    var split = line.Split();
    int steps = int.Parse(split[1]);
    sum += steps;
    var move = split[0] switch
    {
        "R" => (steps, 0),
        "D" => (0, steps),
        "L" => (-steps, 0),
        "U" => (0, -steps),
        _ => (0,0)
    };

    current += move;
    if (current != (0, 0))
    {
        edges.Add(current);
        Console.WriteLine(current);
    }
}

var area = Math.Abs(edges.Take(edges.Count - 1)
   .Select((p, i) => (edges[i + 1].X - p.X) * (edges[i + 1].Y + p.Y))
   .Sum() / 2);

Console.WriteLine(area);
Console.WriteLine(sum);


public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public static implicit operator (int X, int Y)(Point p)
    {
        return (p.X, p.Y);
    }

    public static implicit operator Point((int x, int y) value)
    {
        return new Point
        {
            X = value.x,
            Y = value.y
        };
    }


    public override string ToString()
    {
        return (X, Y).ToString();
    }

    public static Point operator +(Point left, Point right)
    {
        return (left.X + right.X, left.Y + right.Y);
    }

    public static Point operator -(Point left, Point right)
    {
        return (left.X - right.X, left.Y - right.Y);
    }

    public static bool operator ==(Point left, Point right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }
}