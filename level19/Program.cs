using System.Data;

string text = File.ReadAllText("input.txt");

string[] split = text.Split(Environment.NewLine + Environment.NewLine);
var instructions = split[0].Split(Environment.NewLine).Select(t => t.Split("{")).Select(t => new { Instruction = t[0], Rules = t[1][..^1].Split(",").ToList() }).ToDictionary(t => t.Instruction, t => t.Rules);

List<(long xmin, long xmax, long mmin, long mmax, long amin, long amax, long smin, long smax)> accepted = new();
Queue<(string instruction, (long xmin, long xmax, long mmin, long mmax, long amin, long amax, long smin, long smax) range)> queue = new();

queue.Enqueue(("in", (1, 4000, 1, 4000, 1, 4000, 1, 4000)));

while (queue.TryDequeue(out var current))
{
    if (current.instruction == "A")
    {
        accepted.Add(current.range);
        continue;
    }

    if (current.instruction == "R")
        continue;

    foreach (var rule in instructions[current.instruction])
    {
        var rsplit = rule.Split(":");
        if (rsplit.Length > 1)
        {
            char parameter = rsplit[0][0];
            char operation = rsplit[0][1];
            long value = long.Parse(rsplit[0][2..]);

            if (operation == '<')
            {
                switch (parameter)
                {
                    case 'x':
                        if (current.range.xmin >= value)
                        {
                            continue;
                        }
                        else if (current.range.xmax < value)
                        {
                            queue.Enqueue((rsplit[1], current.range));
                        }
                        else
                        {
                            queue.Enqueue((rsplit[1], current.range with { xmax = value - 1 }));
                            current.range = current.range with { xmin = value };
                        }
                        break;
                    case 'm':
                        if (current.range.mmin >= value)
                        {
                            continue;
                        }
                        else if (current.range.mmax < value)
                        {
                            queue.Enqueue((rsplit[1], current.range));
                        }
                        else
                        {
                            queue.Enqueue((rsplit[1], current.range with { mmax = value - 1 }));
                            current.range = current.range with { mmin = value };
                        }
                        break;
                    case 'a':
                        if (current.range.amin >= value)
                        {
                            continue;
                        }
                        else if (current.range.amax < value)
                        {
                            queue.Enqueue((rsplit[1], current.range));
                        }
                        else
                        {
                            queue.Enqueue((rsplit[1], current.range with { amax = value - 1 }));
                            current.range = current.range with { amin = value };
                        }
                        break;
                    case 's':
                        if (current.range.smin >= value)
                        {
                            continue;
                        }
                        else if (current.range.smax < value)
                        {
                            queue.Enqueue((rsplit[1], current.range));
                        }
                        else
                        {
                            queue.Enqueue((rsplit[1], current.range with { smax = value - 1 }));
                            current.range = current.range with { smin = value };
                        }
                        break;
                }
            }
            else if (operation == '>')
            {
                switch (parameter)
                {
                    case 'x':
                        if (current.range.xmax <= value)
                        {
                            continue;
                        }
                        else if (current.range.xmin > value)
                        {
                            queue.Enqueue((rsplit[1], current.range));
                        }
                        else
                        {
                            queue.Enqueue((rsplit[1], current.range with { xmin = value + 1 }));
                            current.range = current.range with { xmax = value };
                        }
                        break;
                    case 'm':
                        if (current.range.mmax <= value)
                        {
                            continue;
                        }
                        else if (current.range.mmin > value)
                        {
                            queue.Enqueue((rsplit[1], current.range));
                        }
                        else
                        {
                            queue.Enqueue((rsplit[1], current.range with { mmin = value + 1 }));
                            current.range = current.range with { mmax = value };
                        }
                        break;
                    case 'a':
                        if (current.range.amax <= value)
                        {
                            continue;
                        }
                        else if (current.range.amin > value)
                        {
                            queue.Enqueue((rsplit[1], current.range));
                        }
                        else
                        {
                            queue.Enqueue((rsplit[1], current.range with { amin = value + 1 }));
                            current.range = current.range with { amax = value };
                        }
                        break;
                    case 's':
                        if (current.range.smax <= value)
                        {
                            continue;
                        }
                        else if (current.range.smin > value)
                        {
                            queue.Enqueue((rsplit[1], current.range));
                        }
                        else
                        {
                            queue.Enqueue((rsplit[1], current.range with { smin = value + 1 }));
                            current.range = current.range with { smax = value };
                        }
                        break;
                }
            }
        }
        else
        {
            queue.Enqueue((rule, current.range));
        }
    }
}

long result = accepted.Sum(x => ((x.xmax - x.xmin)+1) * ((x.mmax - x.mmin)+1) * ((x.amax - x.amin)+1) * ((x.smax - x.smin)+1));

Console.WriteLine(result);