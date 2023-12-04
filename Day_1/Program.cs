using System;

namespace Day_1
{
    class Program
    {
        static void Main(string[] args)
        {
            double question_1 = Q1.Find_Sum(@"C:\Users\Peter\Documents\Advent of Code\2023\Day_1\input.txt");
            Console.WriteLine(question_1);

            double question_2 = Q2.Find_Sum(@"C:\Users\Peter\Documents\Advent of Code\2023\Day_1\input.txt");
            Console.WriteLine(question_2);
        }
    }
}
