using System.Diagnostics.CodeAnalysis;

string[] lines = File.ReadAllLines("input.txt");

Point current = (0, 0);
List<Point> edges = new() { current };
int sum = 0;
foreach (var line in lines)
{
    var split = line.Split();
    int steps = int.Parse(split[1]);
    sum += steps;
    Point move = split[0] switch
    {
        "R" => (steps, 0),
        "D" => (0, steps),
        "L" => (-steps, 0),
        "U" => (0, -steps),
        _ => (0, 0)
    };

    current += move;
    edges.Add(current);
}

// shoelace algorithm
var area = Math.Abs(edges.Take(edges.Count - 1)
   .Select((p, i) => (edges[i + 1].X - p.X) * (edges[i + 1].Y + p.Y))
   .Sum() / 2);

//pick's theorem
var tmp = area - Math.Floor(sum / 2d) + 1;

Console.WriteLine(sum + tmp);


public struct Point(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public static implicit operator (int X, int Y)(Point p)
    {
        return (p.X, p.Y);
    }

    public static implicit operator Point((int x, int y) value)
    {
        return new Point(value.x, value.y);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Point p) 
            return p == this;

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return (X, Y).GetHashCode();
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