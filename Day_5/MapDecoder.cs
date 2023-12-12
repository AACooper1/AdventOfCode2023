using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Day_5
{
    internal class MapDecoder
    {
        private List<string>[] almanac;
        private List<long> seeds;
        private SortedDictionary<int, List<(long destination_start, long source_start, long length)>> ranges;
        public SortedDictionary<long, long> seed_locations;
        public SortedDictionary<(long, long), long> seed_minima;

        public List<SortedDictionary<long, long>> sum_ranges_dict_list;
        private SortedDictionary<long, bool> seed_ranges_dict;

        public MapDecoder(string input)
        {
            string[] split_input = open_and_split(input, "\r\n\r\n");

            almanac = new List<string>[split_input.Length];

            for(int i = 0; i < split_input.Length; i++)
            {
                string s = split_input[i];
                almanac[i] = s.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            seeds = new();
            seed_locations = new();
            
            ranges = almanac_to_ranges(almanac);
            
            seed_locations = seeds_to_locations(ranges);
            seed_ranges_dict = new();
            
            sum_ranges_dict_list = new List<SortedDictionary<long, long>>();
            build_sum_ranges();

            seeds_to_ranges();
            seed_minima = ranges_to_locations();
        }

        private void build_sum_ranges()
        {
            foreach (var map in ranges)
            {
                var temp_sum_ranges_dict = new SortedDictionary<long, long>();
                temp_sum_ranges_dict.Add(0, 0);

                foreach (var region in map.Value)
                {
                    var what_to_concat = new SortedDictionary<long, long>();

                    long region_start = region.source_start;
                    long summand = region.destination_start - region.source_start;
                    long region_end = region.source_start + region.length;

                    temp_sum_ranges_dict[region_start] = summand;

                    if (!temp_sum_ranges_dict.ContainsKey(region_end))
                    {
                        temp_sum_ranges_dict[region_end] = 0;
                    }

                    temp_sum_ranges_dict = new SortedDictionary<long, long>(temp_sum_ranges_dict.Concat(what_to_concat)
                    .GroupBy(sum_ranges => sum_ranges.Key).ToDictionary(group => group.Key, group => group.Max(pair => pair.Value)));
                }

                sum_ranges_dict_list.Add(temp_sum_ranges_dict);
            }
            return;
        }

        private void seeds_to_ranges()
        {
            seed_ranges_dict[0] = false;

            for (int j = 0; j < seeds.Count; j += 2)
            {
                long start = seeds[j];
                long summand = seeds[j + 1];

                seed_ranges_dict.Add(start, true);
                seed_ranges_dict.Add(start + summand, false);
            }

            return;
        }

        private SortedDictionary<(long, long), long> ranges_to_locations()
        {
            var seed_ranges_list = seed_ranges_dict.OrderBy(s => s.Key).ToList();

            int testing_82 = 82;

            foreach (var current_x_to_y_dict in sum_ranges_dict_list)
            {
                seed_ranges_list = seed_ranges_dict.OrderBy(s => s.Key).ToList();
                Console.WriteLine("Testing 82: " + testing_82);

                for (int i = 0; i < seed_ranges_list.Count; i++)
                {

                    KeyValuePair<long, bool> current_seed_range = seed_ranges_list[i];
                    if (current_seed_range.Value == false)
                    {
                        continue;
                    }
                    KeyValuePair<long, bool> next_seed_range = i + 1 < seed_ranges_list.Count ? seed_ranges_list[i + 1] : new(-1, false);
                    var current_x_to_y_list = current_x_to_y_dict.OrderBy(s => s.Key).ToList();

                    for (int j = 0; j < current_x_to_y_list.Count; j++)
                    {

                        var current_sum_range = new KeyValuePair<long, long>(current_x_to_y_list[j].Key, current_x_to_y_list[j].Value);
                        KeyValuePair<long, long> next_sum_range = j + 1 < current_x_to_y_list.Count ? current_x_to_y_list[j + 1] : new(-1, 0);

                        if (next_sum_range.Key <= testing_82 || current_sum_range.Key > testing_82)
                        {
                            continue;
                        }

                        testing_82 += (int)current_sum_range.Value;

                        if (next_sum_range.Key <= current_seed_range.Key || current_sum_range.Key > next_seed_range.Key)
                        {
                            continue;
                        }

                        long jump_range_start = current_sum_range.Key + current_sum_range.Value;
                        seed_ranges_dict[jump_range_start] = true;

                        long jump_range_end = Math.Min(next_seed_range.Key, next_sum_range.Key) + 1;
                        seed_ranges_dict[jump_range_end] = false;

                        seed_ranges_dict[current_sum_range.Key] = false;
                        seed_ranges_dict[next_sum_range.Key + 1] = true;
                    }
                }
            }

            return new();
        }

        private SortedDictionary<long, long> seeds_to_locations(SortedDictionary<int, List<(long destination_start, long source_start, long length)>> all_ranges_dict)
        {

            var temp_seed_locations = new SortedDictionary<long, long>();

            foreach (long seed in seeds)
            {
                long temp_location = seed;

                for(int i = 1; i <= ranges.Count; i++)
                {
                    List<(long destination_start, long source_start, long length)> current_ranges = all_ranges_dict[i];
                    for (int j = 0; j < current_ranges.Count; j++)
                    {
                        long range_end = current_ranges[j].source_start + current_ranges[j].length;
                        if (temp_location >= current_ranges[j].source_start && temp_location <= range_end)
                        {
                           long dist_from_range_start = temp_location - current_ranges[j].source_start;
                            temp_location = current_ranges[j].destination_start + dist_from_range_start;
                            break;
                        }
                    }
                }

                temp_seed_locations.Add(seed, temp_location);
            }
            return temp_seed_locations;
        }

        private SortedDictionary<int, List<(long destination_start, long source_start, long length)>> almanac_to_ranges(List<string>[] almanac)
        {

            var ranges_dict = new SortedDictionary<int, List<(long destination_start, long source_start, long length)>>();

            string[] seeds_str = almanac[0][0].Split(' ');

            for(int i = 1; i < seeds_str.Length; i++)
            {
                long temp_seed_parsed = long.Parse(seeds_str[i]);
                seeds.Add(temp_seed_parsed);
            }

            for (int i = 1; i < almanac.Length; i++)
            {
                List<string> entry = almanac[i];

                int temp_key = i;

                List<(long destination_start, long source_start, long length)> temp_value = new();
                for (int j = 1; j < entry.Count; j++)
                {
                    List<long> range = entry[j].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(new Converter<string, long>(long.Parse));

                    long temp_d_start = range[0], temp_s_start = range[1], temp_length = range[2];

                    temp_value.Add((temp_d_start, temp_s_start, temp_length));
                }

                ranges_dict.Add(temp_key, temp_value);
            }

            return ranges_dict;
        }

        // Put this in util like d9 has because that's a pretty cool idea
        public string[] open_and_split(string filepath, string delimiter)
        {
            string raw_input = File.ReadAllText(filepath);
            string[] input_split = Regex.Split(raw_input, delimiter);

            return input_split;
        }
    }
}
