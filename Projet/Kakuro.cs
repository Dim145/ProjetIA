using System.Diagnostics.CodeAnalysis;

namespace Projet;

public class Kakuro: ICloneable
{
    private int?[,] TabValues { get; set; }
    private int?[,][] TabIndices { get; }
    
    public int NbCol { get; }
    public int NbLig { get; }

    public Kakuro(int?[,][]? tabIndices)
    {
        NbLig = tabIndices.GetLength(0);
        NbCol = tabIndices.GetLength(1);

        TabIndices = tabIndices;
    }
    
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

    public bool SetValue(int lig, int col, int value)
    {
        if (value is 0 || GetValue(lig, col) is null)
            return false;

        if (GetIndiceOfValue(lig, col).All(i => IsValidIndice(i.lig, i.col) == 0))
            return false;

        TabValues[lig, col] = value;

        return true;
    }

    public int? GetValue(int lig, int col)
    {
        return TabValues[lig, col];
    }

    public int?[]? GetIndices(int lig, int col)
    {
        return TabIndices[lig, col];
    }

    public object Get(int lig, int col)
    {
        return GetValue(lig, col) as object ?? GetIndices(lig, col);
    }

    public bool Contains(int value)
    {
        return TabValues.Cast<int?>().Any(v => v == value);
    }

    public bool IsValid()
    {
        //todo: condition de victoire.
        return ValuesOfInvalidIndices() == 0;
    }

    public int ValuesOfInvalidIndices()
    {
        int nbInvalid = 0;
        
        for (int i = 0; i < NbLig; i++)
        {
            for (int j = 0; j < NbCol; j++)
            {
                nbInvalid += IsValidIndice(i, j) ?? 0;
            }
        }

        return nbInvalid;
    }
    
    public class GetIndiceStruct
    {
        public int lig {get; set;}
        public int col {get; set;}
        public int?[]? indice {get; set;}
    }

    public int? IsValidIndice(int lig, int col)
    {
        var indice = GetIndices(lig, col);
        
        if (indice is null) return null;
        
        int nb = 0;
        
        if (indice[0] is not null)
        {
            var totalCol = 0;

            int max = 1;
            for (int k = lig+1; k < NbLig; k++)
            {
                if(GetIndices(k, col) is not null)
                    break;

                if (k < max)
                    max = k;

                totalCol += GetValue(k, col) ?? 0;
            }

            if (totalCol != indice[0])
                nb += max;
        }

        if (indice[1] is not null)
        {
            var totalLig = 0;

            int max = 1;
            for (int k = col+1; k < NbCol; k++)
            {
                if(GetIndices(lig, k) is not null)
                    break;

                if (k < max)
                    max = k;

                totalLig += GetValue(lig, k) ?? 0;
            }

            if (totalLig != indice[1])
                nb += max;
        }

        return nb;
    }

    public List<GetIndiceStruct> GetIndiceOfValue(int lig, int col)
    {
        var list = new List<GetIndiceStruct>();
        
        if (GetValue(lig, col) is null)
            return list;

        for (int i = lig; i >= 0; i--)
        {
            if (GetIndices(i, col) is not null)
            {
                list.Add(new GetIndiceStruct{ col = col, lig = i, indice = GetIndices(i, col)});
                break;
            }
        }
        
        for (int i = col; i >= 0; i--)
        {
            if (GetIndices(lig, i) is not null)
            {
                list.Add(new GetIndiceStruct {col = i, lig = lig, indice = GetIndices(lig, i)});
                break;
            }
        }

        return list;
    }

    public int? GetTabLengthForIndice(int lig, int col, bool isForLig)
    {
        var indice = GetIndices(lig, col);

        if (indice == null)
            return null;

        int nb = 0;

        do
        {
            lig = isForLig ? lig : lig + 1;
            col = isForLig ? col + 1 : col;
            
            if(lig >= NbLig || col >= NbCol)
                break;
            
            var value = GetValue(lig, col);

            if (value != null)
                nb++;
            else
                break;
        } while (true);

        return nb;
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

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public int Count0Value()
    {
        return TabValues.Cast<int?>().Count(v => v == 0);
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Nullable`1[System.Int32][][,]")]
    public object Clone()
    {
        var newK = new Kakuro(TabIndices)
        {
            TabValues = (TabValues.Clone() as int?[,])!
        };

        return newK;
    }
}