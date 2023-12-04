using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    internal class Winnings_Calculator
    {
        public static HashSet<int> Get_Card_Winning_Numbers(string Card)
        {
            Card = Card.Split(":")[1];
            String[] Winning_Numbers_str = Card.Split("|")[0].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            String[] Chosen_Numbers_str = Card.Split("|")[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            HashSet<int> Winning_Numbers_int = new();
            HashSet<int> Chosen_Numbers_int = new();

            foreach(string number in Chosen_Numbers_str)
            {
                Chosen_Numbers_int.Add(Convert.ToInt32(number));
            }
            foreach(string number in Winning_Numbers_str)
            {
                Winning_Numbers_int.Add(Convert.ToInt32(number));
            }

            Winning_Numbers_int.IntersectWith(Chosen_Numbers_int);

            return Winning_Numbers_int;
        }

        public static int Calculate_Points(HashSet<int> Card)
        {
            if(Card.Count == 0)
            {
                return 0;
            }
            else
            {
                return (int)Math.Pow(2, (Card.Count - 1));
            }
        }

        public static List<int> Calculate_Final_Set(string[] input)
        {
            Dictionary<int, (int count_matches, int count_copies)> matches_counts = new();
            Dictionary<int, (int count_matches, int count_copies)> final_counts = new();
            bool finished = false;
            
            for(int i = 0; i < input.Length; i++)
            {
                matches_counts.Add(i, (Get_Card_Winning_Numbers(input[i]).Count, 1));
            }

            for(int i = 0; finished == false; i++)
            {
                finished = true;
                foreach (KeyValuePair<int, (int count_matches, int count_copies)> card in matches_counts)
                {
                    if (card.Value.count_copies < i)
                    {
                        final_counts.Add(card.Key, card.Value);
                        matches_counts.Remove(card.Key);
                        continue;
                    }
                    else if (card.Value.count_copies == i)
                    {
                        continue;
                    }
                    else
                    {
                        for (int j = 1; j <= card.Value.count_matches; j++)
                        {
                            if (card.Key + j < input.Length)
                            {
                                var copy = matches_counts[card.Key + j];
                                matches_counts[card.Key + j] = (copy.count_matches, copy.count_copies + 1);
                                finished = false;
                            }
                        }
                    }
                }
            }
            foreach(var card in matches_counts)
            {
                final_counts.Add(card.Key, card.Value);
            }

            return new List<int>(final_counts.Select(x => x.Value.count_copies));
        }
    }
}
