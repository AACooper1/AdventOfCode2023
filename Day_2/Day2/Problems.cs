using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2
{
    internal class Day_2
    {
        public static Dictionary<int, List<List<(int count, string color)>>> Build_Game_Dict(string[] all_games)
        {
            Dictionary<int, List<List<(int count, string color)>>> games_dict = new();

            // Create a dictionary of each game
            // The keys should be ints (game ID) and the values should be lists of lists of tuples (counts + colors from each round)
            foreach (string game in all_games)
            {
                string[] split_id_and_game = game.Split(':');
                int game_id = Int32.Parse(split_id_and_game[0].Substring(4));

                string[] rounds_strings = split_id_and_game[1].Split(";");

                List<List<(int, string)>> rounds_tuples_lists = new();

                foreach (string round in rounds_strings)
                {
                    string[] counts_colors_strings = round.Split(',');

                    List<(int, string)> counts_colors_tuples = new();

                    foreach (string count_color in counts_colors_strings)
                    {
                        string[] split_color_and_count = count_color.Split(" ");
                        int count = Int32.Parse(split_color_and_count[1]);
                        string color = split_color_and_count[2];

                        counts_colors_tuples.Add((count, color));
                    }

                    rounds_tuples_lists.Add(counts_colors_tuples);
                }

                games_dict.Add(game_id, rounds_tuples_lists);
            }
            return games_dict;
        }

        public static int Find_Games_And_Sum(Dictionary<int, List<List<(int, string)>>> games_dict, int[] max_count_rgb)
        {
            List<int> possible_games = new();
            string[] colors = { "red", "green", "blue" };

            foreach (KeyValuePair<int, List<List<(int count, string color)>>> game in games_dict)
            {
                bool can_add = true;
                IEnumerable<(int count, string color)> round = game.Value.SelectMany(i => i);

                for(int i = 0; i < colors.Length; i++)
                {
                    if (round.Where(c => c.Item2 == colors[i]).Any(cc => cc.Item1 > max_count_rgb[i]))
                    {
                        can_add = false;
                    }
                }
                if (can_add)
                {
                    possible_games.Add(game.Key);
                }
            }

            int sum = 0;

            foreach (int count in possible_games)
            {
                sum += count;
            }

            return sum;
        }

        public static int Find_Minima_And_Sum(Dictionary<int, List<List<(int, string)>>> games_dict)
        {
            Dictionary<int, Dictionary<string, int>> all_minima_dict = new();
            string[] colors = { "red", "green", "blue" };

            foreach (KeyValuePair<int, List<List<(int, string)>>> game in games_dict)
            {
                int gameId = game.Key;
                List<List<(int, string)>> rounds = game.Value;

                Dictionary<string, int> game_minima_dict = new();

                foreach (string color in colors)
                {
                    IEnumerable<(int, string)> count_color = rounds.SelectMany(g => g).Where(c => c.Item2 == color);
                    game_minima_dict.Add(color, count_color.Max(count_color => count_color.Item1));
                }

                all_minima_dict.Add(gameId, game_minima_dict);
            }

            int power_sum = 0;

            foreach(Dictionary<string, int> game in all_minima_dict.Values)
            {
                int current_product = 1;
                foreach(int minimum_count in game.Values)
                {
                    current_product *= minimum_count;
                }

                power_sum += current_product;
            }

            return power_sum;
        }
    }
}
