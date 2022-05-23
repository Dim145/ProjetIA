using Projet;

var kakuro = new Kakuro(8, 8);

var tabIndices = new[,]
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
};

kakuro.Initialize(tabIndices, tabValue);

var rand = new Random();

var resolvedKakuro = Algo.Recuit(kakuro,
    (kakuro1, f, arg3) =>
    {
        Console.WriteLine(kakuro1.NbIndiceInvalid());
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

        var indice = indices[rand.Next(0, indices.Count)];
        var tmp = indice.indice;
        var isForLig = tmp.Contains(null) ? Array.FindIndex(tmp, i => i != null) == 1 : rand.Next(0, tmp.Length) == 1;
        var indiceValue = tmp[isForLig ? 1 : 0];

        var decompositions = Algo.Decompositions(indiceValue ?? 0,
            newK.GetTabLengthForIndice(indice.lig, indice.col, isForLig) ?? 0 );

        var decomposition = decompositions
            .Where(vals => vals.All(i => i < 10))
            .RandomElement();
        
        for (int i = 0; i < decomposition.Length; i++)
        {
            newK.SetValue(isForLig ? indice.lig : indice.lig + 1 + i, isForLig ? indice.col + 1 + i : indice.col, decomposition[i]);
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

Console.WriteLine(resolvedKakuro);