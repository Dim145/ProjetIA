using System.Diagnostics.CodeAnalysis;

namespace Projet;

public static class Algo
{
    public static float Tinit => 50f;
    
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

    public static Kakuro RecuitKakuro(Kakuro kakuro)
    {
        return Recuit(kakuro,
    (kakuro1, f, arg3) =>
            {
                return !kakuro1.Contains(0) && kakuro1.IsValid();
            },
            oldK =>
            {
                var newK = oldK.Clone() as Kakuro;

                var rLig = random.Next(0, newK.NbLig);
                var rCol = random.Next(0, newK.NbCol);

                var tmpValue = newK.GetValue(rLig, rCol);
                while ( tmpValue is null || newK.Count0Value() > 0 && tmpValue != 0 )
                {
                    rLig = random.Next(0, newK.NbLig);
                    rCol = random.Next(0, newK.NbCol);
                    
                    tmpValue = newK.GetValue(rLig, rCol);
                }

                var indices = newK.GetIndiceOfValue(rLig, rCol);
                var indiceLig = indices[1];
                
                var decompositionLig = DecompositionPlusRandom(indiceLig.indice[1]!.Value, newK.GetTabLengthForIndice(indiceLig.lig, indiceLig.col, true)!.Value);

                for (int i = 0; i < decompositionLig.Length; i++)
                    newK.SetValue(indiceLig.lig, indiceLig.col + 1 + i, decompositionLig[i]);

                var indiceCol = newK.GetIndiceOfValue(rLig, rCol - (rCol - indices[0].col))[0];

                for (int i = 0; i < decompositionLig.Length; i++)
                {
                    int[] decompositionCol;

                    var numLigLock = rLig - indiceCol.lig - 1;

                    decompositionCol = Algo.DecompositionPlusRandom(
                        indiceCol.indice[0]!.Value, 
                        newK.GetTabLengthForIndice(indiceCol.lig, indiceCol.col, false)!.Value
                    );

                    if (decompositionCol[numLigLock] == decompositionLig[i])
                    {
                        for (int j = 0; j < decompositionCol.Length; j++)
                            newK.SetValue(indiceCol.lig + 1 + j, indiceCol.col, decompositionCol[j]);
                    }

                    if (indiceCol.col + 1 < newK.NbCol && newK.GetValue(rLig, indiceCol.col + 1) != null)
                        indiceCol = newK.GetIndiceOfValue(rLig, indiceCol.col + 1)[0];
                }

                return newK;
            },
            kakuro1 => kakuro1.Count0Value() + kakuro1.ValuesOfInvalidIndices(),
            (i, f) =>
            {
                if (i > 0)
                {
                    var A = (float) Math.Exp( -i/f );
                    if( random.NextDouble() >= A ) 
                    {
                        return false;
                    }
                }

                return true;
            },
            decroissance: f =>
            {
                var value = (f - (f / (Tinit)) * 2);

                return value <= 0.05f ? 0 : value;
            });
    }
}