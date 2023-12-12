string[] test_input = File.ReadAllLines("../../../testInput.txt");
string[] input = File.ReadAllLines("../../../input.txt");

RaceWinner test_racewinner = new(test_input);
RaceWinner real_racewinner = new(input);

long test_result_1 = test_racewinner.solve_question_1();
Console.WriteLine("Question 1 (Test input): " + test_result_1);

long real_result_1 = real_racewinner.solve_question_1();
Console.WriteLine("Question 1 (Real input): " + real_result_1);

long test_result_2 = test_racewinner.solve_question_2();
Console.WriteLine("Question 2 (Test input): " + test_result_2);

long real_result_2 = real_racewinner.solve_question_2();
Console.WriteLine("Question 2 (Real input): " + real_result_2);

internal class RaceWinner
{
    List<(long, long)> records;
    List<(long, long)> fixed_records;
    public RaceWinner(string[] input)
    {
        records = ParseInput(input);
        fixed_records = new();
        oops_bad_kerning(records);
    }
    public List<(long, long)> ParseInput(string[] input)
    {
        List<(long, long)> races = new();

        string[] time = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries); ;
        string[] distance = input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < time.Length; i++)
        {
            races.Add((int.Parse(time[i]), int.Parse(distance[i])));
        }

        return races;
    }

    public void oops_bad_kerning(List<(long, long)> recs)
    {

        long fixed_times = 0;
        long fixed_distances = 0;

        for (int i = 0; i < records.Count; i++)
        {
            int exponent = recs.Count - i;
            fixed_times = long.Parse(fixed_times.ToString() + records[i].Item1);
            fixed_distances = long.Parse(fixed_distances.ToString() + records[i].Item2);
        }

        fixed_records.Add((fixed_times, fixed_distances));
    }

    private double[] SolveQuadratic(long a, long b, long c)
    {
        double discriminant = Math.Sqrt(Math.Pow(b, 2) - 4 * a * c);
        double x1 = ((-1 * b) - discriminant) / (2 * a);
        double x2 = ((-1 * b) + discriminant) / (2 * a);

        return x1 > x2 ? new double[] { x1, x2 } : new double[] { x2, x1 };
    }

    private List<long> calculate_winning_times(List<(long, long)> recs)
    {
        List<long> num_times = new();

        foreach ((long, long) race in recs)
        {
            double[] quadratic_solutions = SolveQuadratic(1, race.Item1, race.Item2);

            long possible_times = (long)(Math.Floor(quadratic_solutions[0]) - Math.Ceiling(quadratic_solutions[1]) + 1);

            possible_times -= quadratic_solutions.Where(x => x % 1 == 0).Count();

            num_times.Add(possible_times);
        }

        return num_times;
    }

    private long calculate_product(List<long> num_times)
    {
        long product = 1;
        foreach (long times in num_times)
        {
            product *= times;
        }

        return product;
    }

    public long solve_question_1()
    {
        return calculate_product(calculate_winning_times(records));
    }
    public long solve_question_2()
    {
        return calculate_product(calculate_winning_times(fixed_records));
    }
}