using Day_7;

string[] test_input = File.ReadAllLines("../../../testInput.txt");
string[] input = File.ReadAllLines("../../../Input.txt");

PokerWinningsCounter test_poker = new(test_input);

Console.WriteLine("TEST ANSWERS");
Console.WriteLine(" - Question 1: " + test_poker.answer_question_1());
Console.WriteLine(" - Question 2: " + test_poker.answer_question_2());

PokerWinningsCounter poker = new(input);

Console.WriteLine("REAL ANSWERS");
Console.WriteLine(" - Question 1: " + poker.answer_question_1());
Console.WriteLine(" - Question 2: " + poker.answer_question_2());