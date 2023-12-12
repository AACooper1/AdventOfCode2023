using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_7
{
    internal class PokerWinningsCounter
    {
        private struct hand_struct
        {
            public List<int> list_form;
            public Dictionary<int, int> dict_form;
            public int bid;
            public int type;

            public hand_struct()
            {
                list_form = new();
                dict_form = new();
                bid = 0;
                type = 0;
            }
        }

        List<hand_struct> all_hands = new();
        List<hand_struct> all_hands_sorted = new();

        public PokerWinningsCounter(string[] input)
        {
            ParseInput(input);
            for (int i = 0; i < all_hands.Count; i++)
            {
                hand_struct hand = all_hands[i];
                calculate_hand_type(ref hand, false);
                all_hands[i] = hand;
                all_hands_sorted = all_hands;
            }
            sort_bids(false);
        }

        private void ParseInput(string[] input)
        {
            foreach (string line in input)
            {
                string[] line_split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                hand_struct hand = new();   

                for (int i = 0; i <= 14; i++)
                {
                    hand.dict_form.Add(i, 0);
                }

                foreach(char c in line_split[0])
                {
                    if (char.IsDigit(c))
                    {
                        hand.dict_form[(int)char.GetNumericValue(c)]++;
                        hand.list_form.Add((int)char.GetNumericValue(c));
                    }
                    else
                    {
                        switch (c)
                        {
                            case 'T': hand.dict_form[10]++; hand.list_form.Add(10); break;
                            case 'J': hand.dict_form[11]++; hand.list_form.Add(11); break;
                            case 'Q': hand.dict_form[12]++; hand.list_form.Add(12); break;
                            case 'K': hand.dict_form[13]++; hand.list_form.Add(13); break;
                            case 'A': hand.dict_form[14]++; hand.list_form.Add(14); break;
                            default: throw new Exception("Invalid input: Got card " + c);
                        }
                    }
                }

                hand.bid = int.Parse(line_split[1]);

                all_hands.Add(hand);
            }
        }

        private void calculate_hand_type(ref hand_struct hand, bool jack_or_joker)
        {
            Dictionary<int, int> hand_dict = hand.dict_form;

            if (jack_or_joker)
            {
                hand_dict = jacks_to_jokers(hand_dict);
            }

            switch (hand_dict.Values.Max())
            {
                case 5: hand.type = 7; break;
                case 4: hand.type = 6; break;
                case 3:
                {
                        if (hand_dict.ContainsValue(2))
                        {
                            hand.type = 5;
                            break;
                        }
                        else
                        {
                            hand.type = 4;
                            break;
                        }
                }
                case 2:
                    {
                        if (hand_dict.Count(c => c.Value == 2) == 2)
                        {
                            hand.type = 3;
                            break;
                        }
                        else
                        {
                            hand.type = 2;
                            break;
                        }
                    }
                case 1: hand.type = 1; break;
                default: throw new Exception("Somehow got an empty hand");
            }
        }

        private void sort_bids(bool jack_or_joker)
        {
            // Radix sort :D
            List<hand_struct>[] helper_array = new List<hand_struct>[14];


            for (int i = 5; i > 0; i--)
            {
                helper_array = new List<hand_struct>[14];
                for(int j = 0; j < all_hands_sorted.Count; j++)
                {
                    int bucket = all_hands_sorted[j].list_form[i - 1] - 1;

                    if (bucket == 10)
                    {
                        bucket = 0;
                    }

                    helper_array[bucket] = helper_array[bucket] ?? new();
                    helper_array[bucket].Add(all_hands_sorted[j]);
                }
                all_hands_sorted = helper_array.Where(h => h != null).SelectMany(h => h).ToList();
                
            }

            helper_array = new List<hand_struct>[14];

            for (int j = 0; j < all_hands.Count; j++)
            {
                int bucket = all_hands_sorted[j].type;
                
                if (bucket == 10)
                {
                    bucket = 0;
                }

                helper_array[bucket] = helper_array[bucket] ?? new();
                helper_array[bucket].Add(all_hands_sorted[j]);
            }
            all_hands_sorted = helper_array.Where(h => h != null).SelectMany(h => h).ToList();
        }

        private Dictionary<int, int> jacks_to_jokers(Dictionary<int, int> hand)
        {
            int summand = hand[11];
            if (summand == 0)
            {
                return hand;
            }
            hand[11] = 0;

            int highest_count = hand.Max(c => c.Value);
            int best_card = hand.Where(c => c.Value == highest_count).Max(c => c.Key);
            hand[best_card] += summand;

            return hand;
        }

        public long answer_question_1()
        {
            long sum = 0;
            for(int i = 0; i < all_hands_sorted.Count; i++)
            {
                int summand = all_hands_sorted[i].bid * (i + 1);
                sum += summand;
            }

            return sum;
        }

        public long answer_question_2()
        {
            for (int i = 0; i < all_hands.Count; i++)
            {
                hand_struct hand = all_hands[i];
                calculate_hand_type(ref hand, true);
                all_hands[i] = hand;
                all_hands_sorted = all_hands;
            }
            sort_bids(true);

            long sum = 0;
            for (int i = 0; i < all_hands_sorted.Count; i++)
            {
                int summand = all_hands_sorted[i].bid * (i + 1);
                sum += summand;
            }

            return sum;
        }
    }
}
