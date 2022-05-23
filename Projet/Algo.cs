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

    public static List<int[]> Decompositions(int target, int nbSegments)
    {
        var list = new List<int[]>();
        
        Generate(target, 0, 0, 0, new int[nbSegments], list);

        return list;
    }
    
    private static void Generate(int target, int k, int last, int sum, int[] a, List<int[]> ints)
    {

        if (k == a.Length- 1)
        {

            a[k] = target - sum;
            ints.Add(a.ToArray());

        }
        else
        {

            for (int i = last; i < target - sum - i + 1; i++)
            {

                a[k] = i;
                Generate(target, k + 1, i, sum + i, a, ints);

            }

        }

    }
    
    public static T RandomElement<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.RandomElementUsing(new Random());
    }

    public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
    {
        int index = rand.Next(0, enumerable.Count());
        return enumerable.ElementAt(index);
    }
}