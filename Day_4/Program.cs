using Day4;

string filepath = "../../../input.txt";
string[] input = File.ReadAllLines(filepath);

HashSet<int>[] matches = new HashSet<int>[input.Length];

for(int i = 0; i < input.Length; i++)
{
    matches[i] = (Winnings_Calculator.Get_Card_Winning_Numbers(input[i]));
}

int[] winnings = new int[matches.Length];

for(int i = 0; i < matches.Length;i++)
{
    winnings[i] = Winnings_Calculator.Calculate_Points(matches[i]);
}

Console.WriteLine("Question 1: " + winnings.Sum());

List<int> REAL_winnings = Winnings_Calculator.Calculate_Final_Set(input);
Console.WriteLine("Question 1: " + REAL_winnings.Sum());

