string text = File.ReadAllText("input.txt");

string[] split = text.Split(Environment.NewLine + Environment.NewLine);
var instructions = split[0].Split(Environment.NewLine).Select(t => t.Split("{")).Select(t => new { Instruction = t[0], Rules = t[1][..^1].Split(",").ToList() }).ToDictionary(t => t.Instruction, t => t.Rules);
string[] values = split[1].Split(Environment.NewLine);

int A = 0;
int R = 0;

foreach (var value in values)
{
    //{x=4,m=211,a=430,s=167}
    var data = value[1..^1].Split(",").Select(t => (Key: t[0], Value: int.Parse(t[2..]))).ToDictionary(t => t.Key, t => t.Value);

    FitCondition(instructions["in"], data, instructions);
}

Console.WriteLine(A);

void FitCondition(List<string> rules, Dictionary<char, int> data, Dictionary<string, List<string>> instructions)
{
    foreach (var rule in rules)
    {
        var split = rule.Split(":");
        if (split.Length > 1)
        {
            char parameter = split[0][0];
            char operation = split[0][1];
            int value = int.Parse(split[0][2..]);

            if (operation == '<' && data[parameter] < value)
            {
                if (split[1] == "A")
                {
                    A += data.Sum(t => t.Value);
                    break;
                }
                else if (split[1] == "R")
                {
                    R += data.Sum(t => t.Value);
                    break;
                }
                else
                {
                    FitCondition(instructions[split[1]], data, instructions);
                    break;
                }
            }
            else if (operation == '>' && data[parameter] > value)
            {
                if (split[1] == "A")
                {
                    A += data.Sum(t => t.Value);
                    break;
                }
                else if (split[1] == "R")
                {
                    R += data.Sum(t => t.Value);
                    break;
                }
                else
                {
                    FitCondition(instructions[split[1]], data, instructions);
                    break;
                }
            }
        }
        else
        {
            if (split[0] == "A")
            {
                A += data.Sum(t => t.Value);
                break;
            }
            else if (split[0] == "R")
            {
                R += data.Sum(t => t.Value);
                break;
            }
            else
            {
                FitCondition(instructions[split[0]], data, instructions);
                break;
            }
        }
    }
}

