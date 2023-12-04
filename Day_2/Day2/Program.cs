using System;
using System.IO;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath = @"../../../input.txt";
            string[] input = File.ReadAllLines(filepath);
            int[] max_count_rgb = { 12, 13, 14 };

            Dictionary<int, List<List<(int count, string color)>>> games_dict = Day_2.Build_Game_Dict(input);

            int Q1_sum = Day_2.Find_Games_And_Sum(games_dict, max_count_rgb);

            Console.WriteLine("Question 1: " + Q1_sum);

            int Q2_sum = Day_2.Find_Minima_And_Sum(games_dict);

            Console.WriteLine("Question 2: " + Q2_sum);
        }
    }
}