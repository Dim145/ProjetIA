namespace Projet;

public static class Algo
{
    private static int N => 300;
    private static float Tinit => 50f;
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
}