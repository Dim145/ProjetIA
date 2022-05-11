namespace Projet;

public static class Algo
{
    private static int N => 300;
    public static float Tinit => 50f;
    private static float Tratio => (float) Math.Pow(10, -6);
    
    public static TYpe Recuit<TYpe>(TYpe x0, Func<TYpe, float, float, bool> conditionArret, Func<TYpe, TYpe> voisin, Func<TYpe, float> f, Func<float, float, bool> accept, Func<float, float> decroissance)
    {
        var x = x0;
        var t = Tinit;
        const float Nt = 100f;

        while (!conditionArret(x, t, Nt))
        {
            for (var m = 0; m < Nt; m++)
            {
                var y = voisin(x);
                var dF = f(y) - f(x);

                if (accept(dF, t))
                {
                    x = y;
                }
            }

            t = decroissance(t);
        }

        return x;
    }

    private static Random random { get; } = new();

    public static int[]? Decomposition(int numberToDecompose, int numberOfSubSection, int minRandom = 0)
    {
        var tab = new int[numberOfSubSection];
        var cumulative_sum_of_random_numbers = 0;
        var current_subsection = 1;
        var max_random_number = numberToDecompose / numberOfSubSection;

        if (minRandom > max_random_number)
        {
            Console.Error.WriteLine("ERROR: Cannot have min number as {0:D2} and split {1:D2} in {2:D2} subsections", minRandom, numberToDecompose, numberOfSubSection);
            return null;
        }

        while (true)
        {
            var random_number = random.Next(minRandom, max_random_number);
            tab[current_subsection] = random_number;
            cumulative_sum_of_random_numbers += random_number;
            numberToDecompose -= random_number;

            current_subsection++;

            if (current_subsection == numberOfSubSection)
            {
                random_number = numberToDecompose;
                tab[0] = random_number;

                cumulative_sum_of_random_numbers += random_number;
                break;
            }
        }

        return tab;
    }
    
    /*
     * def split_given_number_into_n_random_numbers(number, number_of_subsections, min_random_number_desired = 0):
    cumulative_sum_of_random_numbers = 0
    current_subsection = 1
    max_random_number = int(number/number_of_subsections)
    if min_random_number_desired > max_random_number:
        print("ERROR: Cannot have min number as {} and split {} in {} subsections".format(min_random_number_desired,
                                                                                          number, number_of_subsections))
        return False

    while (True):
        random_number = random.randint(min_random_number_desired, max_random_number)
        print("Random number {} = {}".format(current_subsection, random_number))
        cumulative_sum_of_random_numbers += random_number
        # print("Cumulative sum {}".format(sum_of_num))
        number -= random_number
        current_subsection += 1
        if current_subsection == number_of_subsections:
            random_number = number
            print("Random number {} = {}".format(current_subsection, random_number))
            cumulative_sum_of_random_numbers += random_number
            break

    print("Final cumulative sum of random numbers = {}".format(cumulative_sum_of_random_numbers))
    return True
     */
}