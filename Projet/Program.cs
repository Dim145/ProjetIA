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

Console.WriteLine(kakuro);