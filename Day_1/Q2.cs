using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Q2
{
    public static double Find_Sum(string filepath)
    {
        string[] all_text = File.ReadAllLines(filepath);
        List<string> all_nums = new List<string>();

        string[] textnums = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

        // Set up cases for overlap
        Dictionary<string, string> combos = new Dictionary<string, string>();

        for(int i = 0; i < textnums.Length; i++)
        {
            string num1 = textnums[i];

            for(int j = 0; j < textnums.Length; j++)
            {
                string num2 = textnums[j];

                if(num1[num1.Length - 1] == num2[0])
                {
                    combos.Add(num1.Substring(0, num1.Length - 1) + num2, num1 + num2);
                }

            }
        }
        
        for(int i = 0; i < all_text.Length; i++)
        {
            
            // Replace overlap with non-overlapping forms
            foreach(var item in combos)
            {
                all_text[i] = all_text[i].Replace(item.Key, item.Value);
            }

            // Replace text with numerical forms
            foreach(var item in textnums)
            {
                all_text[i] = all_text[i].Replace(item, Array.IndexOf(textnums, item).ToString());
            }

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