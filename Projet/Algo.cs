using System.Diagnostics.CodeAnalysis;

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

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.SZGenericArrayEnumerator`1[System.Int32]")]
    public static int[] DecompositionPlusRandom(int num, int subNum)
    {
        if (subNum == 1)
            return new[]{num};

        int startNum = num;
        int startSubNum = subNum;
        bool isGood = false;

        var res = new int[subNum];

        while (!isGood)
        {
            num = startNum;
            subNum = startSubNum;
            res = new int[subNum];

            int cumulSum = 0;
            int subSection = 0;

            int max_random_number = Math.Min(9, startNum);

            while (true)
            {
                int random_number = random.Next(1, max_random_number + 1);

                res[subSection] = random_number;
                cumulSum += random_number;
                num -= random_number;
                subSection++;

                if (cumulSum >= startNum)
                {
                    if (cumulSum > startNum)
                        res[subSection-1] = cumulSum - startNum - (res.Length - subSection);

                    for (int i = subSection; i < res.Length; i++)
                        res[i] = 1;
                    
                    break;
                }

                if (subSection == subNum - 1)
                {
                    random_number = num;
                    res[subSection] = random_number;
                    cumulSum += random_number;
                    break;
                }
            }

            isGood = res.All(i => i is > 0 and < 10);
        }

        //random.Shuffle(res);

        return res;
    }
    
    public static void Shuffle<T> (this Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}