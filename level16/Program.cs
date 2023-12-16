string[] lines = File.ReadAllLines("sample.txt");

char[][] map = lines.Select(t => t.ToCharArray()).ToArray();

HashSet<(int X, int Y)> seen = new();
Queue<(int X, int Y, Direction Direction)> queue = new();

queue.Enqueue((0, 0, Direction.Right));

while (queue.Count > 0)
{

}

List<(int X, int Y, Direction)>



enum Direction
{
    Up,
    Right,
    Down,
    Left,
}