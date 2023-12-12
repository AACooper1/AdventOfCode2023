using Day_5;

string test_filepath = "../../../testInput.txt";
MapDecoder decoder_test = new MapDecoder(test_filepath);

string filepath = "../../../input.txt";
MapDecoder decoder = new MapDecoder(filepath);

Console.WriteLine("Question 1:");
Console.WriteLine(" - Test output:" + decoder_test.seed_locations.Min(d => d.Value));
Console.WriteLine(" - Full output:" + decoder.seed_locations.Min(d => d.Value));

Console.WriteLine("Question 2:");
Console.WriteLine(" - Test output:" + decoder_test.seed_locations.Min(d => d.Value));