using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Q1
{
    public static double Find_Sum(string filepath)
    {
        string[] all_text = File.ReadAllLines(filepath);
        List<string> all_nums = new List<string>();
        
        for(int i = 0; i < all_text.Length; i++)
        {
            string curr = all_text[i];
            all_nums.Add(new string(curr.Where(c => char.IsDigit(c)).ToArray()));
        }

        List<(double, double)> first_last = new List<(double, double)>();
        
        for(int i = 0; i < all_nums.Count; i++)
        {
            string curr = all_nums[i];
            first_last.Add((char.GetNumericValue(curr[0]), char.GetNumericValue(curr[curr.Length - 1])));
        }

        List<double> sums = new List<double>();

        for(int i = 0; i < first_last.Count; i++)
        {
            (double, double) curr = first_last[i];
            sums.Add((curr.Item1 * 10) + curr.Item2);
        }

        double result = sums.Sum();
        return result;
    }
}