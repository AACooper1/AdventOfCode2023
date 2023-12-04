using Day3;

string filepath = "../../../input.txt";

string[] input = File.ReadAllLines(filepath);

(Dictionary<(int x, int y), int>, List<(int, char)>[]) partsAndSymbols = SchematicDecoder.findPartNumbers(input);

int sum = SchematicDecoder.partNumberSum(partsAndSymbols.Item1);

int q2 = SchematicDecoder.findGears(partsAndSymbols.Item1, partsAndSymbols.Item2);

Console.WriteLine("Question 1: " + sum);

Console.WriteLine("Question 2: " + q2);