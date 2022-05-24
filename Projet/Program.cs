using Projet;

/*var tabIndices = new[,]
{
    {null, new int?[]{23, null}, new int?[]{30, null}, null, null, new int?[]{27, null}, new int?[]{12, null}, new int?[]{16, null} },
    {new int?[]{null, 16}, null, null, null, new int?[]{17, 24}, null, null, null },
    {new int?[]{null, 17}, null, null, new int?[]{15, 29}, null, null, null, null },
    {new int?[]{null, 35}, null, null, null, null, null, new int?[]{12, null}, null },
    {null, new int?[]{null, 7}, null, null, new int?[]{7, 8}, null, null, new int?[]{7, null} },
    {null, new int?[]{11, null}, new int?[]{10, 16}, null, null, null, null, null },
    {new int?[]{null, 21}, null, null, null, null, new int?[]{null, 5}, null, null },
    {new int?[]{null, 6}, null, null, null, null, new int?[]{null, 3}, null, null }
};

var tabValue = new int?[,]
{
    {null, null, null, null, null, null, null, null},
    {null, 0, 0, null, null, 0, 0, 0},
    {null, 0, 0, null, 0, 0, 0, 0},
    {null, 0, 0, 0, 0, 0, null, null},
    {null, null, 0, 0, null, 0, 0, null},
    {null, null, null, 0, 0, 0, 0, 0},
    {null, 0, 0, 0, 0, null, 0, 0},
    {null, 0, 0, 0, null, null, 0, 0}
};*/

var tabIndices = new[,]
{
    {null, new int?[]{4, null}, new int?[]{9, null}, null, null, null},
    {new int?[]{null, 3}, null, null, new int?[]{24, null}, null, null},
    {new int?[]{null, 17}, null, null, null, new int?[]{17, null}, null},
    {null, new int?[]{null, 18}, null, null, null, null},
    {null, null, new int?[]{null, 16}, null, null, null},
    {null, null, null, null, null, null}
};

var tabValue = new int?[,]
{
    {null, null, null, null, null, null},
    {null, 0, 0, null, null, null},
    {null, 0, 0, 0, null, null},
    {null, null, 0, 0, 0, null},
    {null, null, null, 0, 0, null},
    {null, null, null, null, null, null}
};

var kakuro = new Kakuro(tabIndices.GetLength(0), tabIndices.GetLength(1));

kakuro.Initialize(tabIndices, tabValue);

Console.WriteLine(kakuro);

var rand = new Random();

var start = DateTime.Now;

var resolvedKakuro = Algo.Recuit(kakuro,
    (kakuro1, f, arg3) =>
    {
        return !kakuro1.Contains(0) && kakuro1.IsValid();
    },
    oldK =>
    {
        var newK = oldK.Clone() as Kakuro;

        var rLig = rand.Next(0, newK.NbLig);
        var rCol = rand.Next(0, newK.NbCol);

        while (newK.GetValue(rLig, rCol) is null)
        {
            rLig = rand.Next(0, newK.NbLig);
            rCol = rand.Next(0, newK.NbCol);
        }

        var indices = newK.GetIndiceOfValue(rLig, rCol);
        var indiceLig = indices[1];
        
        var decompositionLig = Algo.DecompositionPlusRandom(indiceLig.indice[1]!.Value, newK.GetTabLengthForIndice(indiceLig.lig, indiceLig.col, true)!.Value);

        for (int i = 0; i < decompositionLig.Length; i++)
            newK.SetValue(indiceLig.lig, indiceLig.col + 1 + i, decompositionLig[i]);

        var indiceCol = newK.GetIndiceOfValue(rLig, rCol - (rCol - indices[0].col))[0];

        for (int i = 0; i < decompositionLig.Length; i++)
        {
            var decompositionCol = Array.Empty<int>();

            var nbTentative = 0;
            do
            {
                if (nbTentative++ > 100)
                    return newK;
                
                var numLigLock = rLig - indiceCol.lig - 1;

                var tmp = new Dictionary<int, int>();
                tmp.Add(numLigLock, decompositionLig[i]);
                decompositionCol = Algo.DecompositionPlusMoinsRandom(
                    indiceCol.indice[0]!.Value, 
                    newK.GetTabLengthForIndice(indiceCol.lig, indiceCol.col, false)!.Value,
                    tmp
                    );

                if (decompositionCol[numLigLock] == decompositionLig[i])
                {
                    for (int j = 0; j < decompositionCol.Length; j++)
                        newK.SetValue(indiceCol.lig + 1 + j, indiceCol.col, decompositionCol[j]);

                    if (indiceCol.col + 1 < newK.NbCol && newK.GetValue(rLig, indiceCol.col + 1) != null)
                    {
                        indiceCol = newK.GetIndiceOfValue(rLig, indiceCol.col + 1 + i)[0];
                    }
                    else
                    {
                        break;
                    }
                }
            } while (true);
        }

        return newK;
    },
    kakuro1 => kakuro1.Count0Value(),
    (i, f) =>
    {
        if (i > 0)
        {
            var A = (float) Math.Exp( -i/f );
            if( rand.NextDouble() >= A ) 
            {
                return false;
            }
        }

        return true;
    },
    decroissance: f =>
    {
        var value = (f - (f / (Algo.Tinit)) * 2);

        return value <= 0.05f ? 0 : value;
    });

var end = DateTime.Now;

Console.WriteLine(resolvedKakuro);
Console.WriteLine("temps: " + ((end - start).Ticks / TimeSpan.TicksPerMillisecond) + "ms");