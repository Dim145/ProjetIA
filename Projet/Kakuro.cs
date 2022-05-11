namespace Projet;

public class Kakuro
{
    private int?[,] TabValues { get; }
    private int?[,][] TabIndices { get; }
    
    public int NbCol { get; }
    public int NbLig { get; }

    public Kakuro(int nbLig, int nbCol)
    {
        TabIndices = new int?[nbLig, nbCol][];
        TabValues = new int?[nbLig, nbCol];

        NbCol = nbCol;
        NbLig = nbLig;
    }

    public void Initialize(int?[,][]? tabIndices = null, int?[,]? tabValues = null )
    {
        if (tabIndices is null || tabValues is null || tabIndices.Length != TabIndices.Length || tabValues.Length != TabValues.Length)
        {
            return;
        }

        for (int i = 0; i < NbLig; i++)
        {
            for (int j = 0; j < NbCol; j++)
            {
                if (tabIndices[i, j] == null)
                    TabValues[i, j] = tabValues[i, j];
                else
                    TabIndices[i, j] = tabIndices[i, j];
            }
        }
    }

    public int? GetValue(int lig, int col)
    {
        return TabValues[lig, col];
    }

    public int?[] GetIndices(int lig, int col)
    {
        return TabIndices[lig, col];
    }

    public object Get(int lig, int col)
    {
        return GetValue(lig, col) as object ?? GetIndices(lig, col);
    }

    public override string ToString()
    {
        var sRet = string.Empty;

        for (int i = 0; i < NbLig; i++)
        {
            for (int j = 0; j < NbCol; j++)
            {
                var tabCase = Get(i, j);
                
                sRet += tabCase switch
                {
                    int val => $"|  {val}  |",
                    int?[] indices => $"|{(indices[0] is null ? " □" : $"{indices[0]:D2}")}\\{(indices[1] is null ? "□ " : $"{indices[1]:D2}")}|",
                    _ => "|  □  |"
                };
            }

            sRet += "\n";
        }

        return sRet;
    }
}