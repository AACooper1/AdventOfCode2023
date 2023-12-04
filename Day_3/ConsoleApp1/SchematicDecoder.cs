using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Day3
{
    internal class SchematicDecoder
    {
        public static (Dictionary<(int x, int y), int>, List<(int, char)>[]) findPartNumbers(string[] schematic)
        {
            Dictionary<(int x, int y), int> partNumbers = new();
            List<(int, char)>[] symbolIndices = findSymbolIndices(schematic);

            string find_numbers_regex = @"[0-9]+";
            
            for(int i = 0; i < schematic.Length; i++)
            {
                MatchCollection all_numbers = Regex.Matches(schematic[i], find_numbers_regex);
                foreach(Match match in all_numbers)
                {
                    bool found_match = false;
                    for(int j = -1; j < 2; j++)
                    {
                        for (int k = match.Index; k < match.Index + match.Value.Length; k++)
                        {
                            if (i + j > 0 && i + j < symbolIndices.Length)
                            {
                                if (symbolIndices[i + j].Any(x => -1 <= k - x.Item1 && k - x.Item1 <= 1))
                                {
                                    found_match = true;
                                }
                            }
                        }
                    }

                    if(found_match)
                    {
                        partNumbers.Add((match.Index, i), int.Parse(match.Value));
                    }
                }
            }

            return (partNumbers, symbolIndices);
        }

        public static int partNumberSum(Dictionary<(int x, int y), int> parts_dict)
        {
            int sum = 0;
            foreach(int val in parts_dict.Values)
            {
                sum += val;
            }

            return sum;
        }

        public static int findGears(Dictionary<(int x, int y), int> parts_dict, List<(int, char)>[] symbolIndices)
        {
            Dictionary< int, (int index, int neighbors)>[] only_stars = new Dictionary<int, (int, int neighbors)>[symbolIndices.Length];
            int v = 1;

            for (int i = 0; i < symbolIndices.Length; i++)
            {

                foreach ((int, char) j in symbolIndices[i])
                {
                    if(j.Item2 == '*')
                    {
                        if (only_stars[i] == null)
                        {
                            only_stars[i] = new();
                        }
                        only_stars[i].Add(v, (j.Item1, 0));
                        v++;
                    }
                }
                
            }

            Dictionary<int, List<int>> all_neighbors = new();
            Dictionary<int,  List<int>> gear_neighbors = new();

            foreach (KeyValuePair<(int x, int y), int> part in parts_dict)
            {
                int y = part.Key.y;
                int x = part.Key.x;
                for (int j = -1; j < 2; j++)
                {
                    for (int k = x; k < x + part.Value.ToString().Length; k++)
                    {
                        if (y + j >= 0 && y + j < symbolIndices.Length)
                        {
                            if (only_stars[y + j] != null)
                            {
                                IEnumerable<KeyValuePair<int, (int index, int neighbors)>> nearby_gears = only_stars[y + j].Where(g => -1 <= k - g.Value.index && k - g.Value.index <= 1);
                                {
                                    foreach (KeyValuePair<int, (int index, int neighbors)> gear in nearby_gears)
                                    {
                                        int sadihasioduj = only_stars[y + j][gear.Key].neighbors + 1;

                                        if (!gear_neighbors.ContainsKey(gear.Key))
                                        {
                                            gear_neighbors.Add(gear.Key, new());
                                        }
                                        if (!gear_neighbors[gear.Key].Contains(part.Value))
                                        {
                                            gear_neighbors[gear.Key].Add(part.Value);
                                            only_stars[y + j][gear.Key] = (only_stars[y + j][gear.Key].index, sadihasioduj);
                                        }

                                        if (!all_neighbors.ContainsKey(gear.Key))
                                        {
                                            all_neighbors.Add(gear.Key, new());
                                        }
                                        all_neighbors[gear.Key].Add(part.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            List<int> gear_nums = new();
            int temp_mult = 1;
            foreach (Dictionary<int, (int, int neighbors)> gear in only_stars)
            {
                if(gear == null)
                {
                    continue;
                }
                foreach (KeyValuePair<int, (int, int neighbors)> neighbor_nums in gear)
                {
                    temp_mult = 1;
                    if (neighbor_nums.Value.neighbors == 2)
                    {
                        foreach(int neighbor in gear_neighbors[neighbor_nums.Key])
                        {
                            temp_mult *= neighbor;
                        }

                        gear_nums.Add(temp_mult);
                    }
                }
            }

            int ratio_sum = gear_nums.Sum();

            return ratio_sum;
        }

        private static List<(int, char)>[] findSymbolIndices(string[] schematic)
        {
            List<(int, char)>[] SymbolIndices = new List<(int, char)>[schematic.Length];

            for (int i = 0; i < schematic.Length; i++)
            {
                SymbolIndices[i] = new List<(int, char)>();
                for (int j = 0; j < schematic[i].Length; j++)
                {
                    if (Regex.Match(schematic[i][j].ToString(), @"[^0-9\.]").Success)
                    {
                        SymbolIndices[i].Add((j, schematic[i][j]));
                    }
                }
            }

            return SymbolIndices;
        }

    }

}
