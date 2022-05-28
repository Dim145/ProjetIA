using System.Diagnostics.CodeAnalysis;

namespace Projet;

/// <summary>
/// Classe qui représente le tableau du jeu.
/// Pour cela 2 tableaux sont utilisée en réalité. Un qui contient les indices (tableau de 2 cases) et l'autre les valeur (case ou on joue)
/// Cela était plus simple à modélisé et à utilisé.
/// Chanque valeur peut être null. Voici les différent cas (exemple avec la case 0/0 :
/// - 1 les deux tableau sont null à cette case: case inutilisé (case bleu sur le site)
/// - 2 un des deux tableau contient une valeur: soit indice soit valeur
/// - 3 les deux tableau contiennent une valeur. Ce n'est pas sencé arrivé. Mais si c'est ça, c'est l'indice qui domine.
///
/// Dans tous les indices, chaque cases contient un tableau de 2 cases. L'indice 0 représente l'indice de la colonne et le 1 celui de la ligne.
/// </summary>
public class Kakuro: ICloneable
{
    private int?[,]? TabValues { get; set; }
    private int?[,][] TabIndices { get; }
    
    public int NbCol { get; }
    public int NbLig { get; }

    /// <summary>
    /// Constructeur spécial utilisé dans le clone. Il permet d'utilisé par référence un tableau d'indice
    /// </summary>
    /// <param name="tabIndices"></param>
    private Kakuro(int?[,][]? tabIndices)
    {
        NbLig = tabIndices!.GetLength(0);
        NbCol = tabIndices.GetLength(1);

        TabIndices = tabIndices;
    }
    
    /// <summary>
    /// Initialise un plateau de jeu avec des valeurs null partout.
    /// </summary>
    /// <param name="nbLig"></param>
    /// <param name="nbCol"></param>
    public Kakuro(int nbLig, int nbCol)
    {
        TabIndices = new int?[nbLig, nbCol][];
        TabValues = new int?[nbLig, nbCol];

        NbCol = nbCol;
        NbLig = nbLig;
    }

    /// <summary>
    /// Initialise les deux tableau en fonction des deux tableau passé en paramètres.
    /// </summary>
    /// <param name="tabIndices"></param>
    /// <param name="tabValues"></param>
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
                if (tabIndices?[i, j] == null)
                    TabValues[i, j] = tabValues[i, j];
                else
                    TabIndices[i, j] = tabIndices[i, j];
            }
        }
    }

    /// <summary>
    /// Permet d'ajouter une valeur à la case souhaiter.
    /// Au début des vérifications d'usage était faites (est-c bien une case valeur, la ligne/col est-elle déjà valide etc...
    /// Mais par soucis d'optimisation, cela à été enlevé.
    /// </summary>
    /// <param name="lig"></param>
    /// <param name="col"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool SetValue(int lig, int col, int value)
    {
        /*if (GetIndiceOfValue(lig, col).All(i => IsValidIndice(i.lig, i.col) == 0))
            return false;*/

        TabValues![lig, col] = value;

        return true;
    }

    public int? GetValue(int lig, int col)
    {
        return TabValues![lig, col];
    }

    public int?[]? GetIndices(int lig, int col)
    {
        return TabIndices[lig, col];
    }

    /// <summary>
    /// retourne soit un indice soit une valeur selon ce qui s'y trouve.
    /// </summary>
    /// <param name="lig"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    private object? Get(int lig, int col)
    {
        return GetValue(lig, col) as object ?? GetIndices(lig, col);
    }

    /// <summary>
    /// Permet de vérifier si une valeur est contenue dans le tableau.
    /// Un parcourt manuel est effectuer car celui-ci est plus rapide que les méthodes de programmation fonctionnel.
    /// En effet la méthode .Cast creer une nouvelle liste et pour chaque cases du tableau transforme le type en celui demander.
    /// Cela est très couteux en performance.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(int value)
    {
        //return TabValues!.Cast<int?>().Any(v => v == value);
        
        for (var i = 0; i < NbLig; i++)
        {
            for (var j = 0; j < NbCol; j++)
            {
                if (GetValue(i, j) == value)
                    return true;
            }
        }

        return false;
    }

    public bool IsValid()
    {
        return ValuesOfInvalidIndices() == 0;
    }

    /// <summary>
    /// permet de récupéré une valeur montrant l'avancement d'un plateau.
    /// Plus celle-ci est petite plus le plateau est proche d'être résolue.
    /// </summary>
    /// <returns></returns>
    public float ValuesOfInvalidIndices()
    {
        float nbInvalid = 0;
        
        for (int i = 0; i < NbLig; i++)
        {
            for (int j = 0; j < NbCol; j++)
            {
                nbInvalid += ValueOfIndice(i, j);
            }
        }

        return nbInvalid;
    }
    
    /// <summary>
    /// Sous class utilisé pour représenter un indice de façon temporaire
    /// </summary>
    public class GetIndiceStruct
    {
        public int lig {get; set;}
        public int col {get; set;}
        public int?[]? indice {get; set;}

        public override string ToString()
        {
            return $"{lig}:{col} = {indice?[0]}\\{indice?[1]}";
        }
    }

    /// <summary>
    /// Cette méthode récupère les deux indices d'une case et cacule leurs valeurs.
    ///
    /// La valeur est de 0 si l'indice est valide (valide = colonne == indice pour le premier, et pareil avec la ligne pour le second)
    /// sinon la valeur est calculer de la manière suivante:
    /// - on ajoute 5 pour chacun des deux indice si il est invalide et on ajoute la différence absolue sur 100 à ce chiffre.
    /// par exemple: si la valeur est de 5.08, cela indique qu'un des deux indice est faut avec un différentiel de 8.
    ///
    /// J'ai pensé à différencier les lignes et colonnes au niveau de la valeur, par exemple colonne = 5 et ligne = 10.
    /// Cela fait que l'algorithme privilégie l'un des indice en fonction de celui qui ajoute la plus grande valeur.
    /// par exemple si la colonne ajoute 5 et la ligne 10, c'est la ligne qui seras résolue en priorité.
    /// Mais au final cela ralentissait plus qu'autre choses les algos et ne permettait pas de mieux résoudre les grand plateaux.
    /// Donc j'ai laissé de coté cette différenciation.
    ///
    /// La raison pour laquel j'incrémente de 5 est toute bête: et pourquoi pas ?
    /// Toutes les valeur donnait le même résultats. Il faut juste que l'incrémentation soit d'au moins 1 pour différencier
    /// la partie comptant les indices de celle contant les différences.
    /// J'avais fait un test avec 0.5 d'incréent (s'écrivant .5f en c#). j'ai donc juste retiré le point...
    ///
    /// Enfin, j'utilise ces deux paramètres (nombre et différence) afin que chacun joue un role en particulier:
    /// - le nombre est le plus important, il faut que la différence soit plus nette pour que l'algo le privilégie si il y as plus d'indice résolue
    /// - la différence sert dan le cas ou aucuns nouvel indice est résolue. Si la différence est plus petite, l'algo
    /// considère tous de même la solution comme meilleur
    /// </summary>
    /// <param name="lig"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    private float ValueOfIndice(int lig, int col)
    {
        var indice = GetIndices(lig, col);

        float nb = 0;
        
        if (indice?[0] is not null)
        {
            var totalCol = 0;

            for (int k = lig+1; k < NbLig; k++)
            {
                var tmp = GetValue(k, col);
                
                if(tmp is null)
                    break;

                totalCol += tmp.Value;
            }

            if (totalCol != indice[0])
                nb += 5f + Math.Abs(indice[0]!.Value - totalCol)/100f;
        }

        if (indice?[1] is not null)
        {
            var totalLig = 0;
            
            for (int k = col+1; k < NbCol; k++)
            {
                var tmp = GetValue(lig, k);
                
                if(tmp is null)
                    break;

                totalLig += tmp.Value;
            }

            if (totalLig != indice[1])
                nb += 5f + Math.Abs(indice[1]!.Value - totalLig)/100f;
        }

        return nb;
    }

    /// <summary>
    /// récupère les deux indices d'une case valeur en parcourant colonne puis ligne en ligne droite.
    /// </summary>
    /// <param name="lig"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public GetIndiceStruct[] GetIndiceOfValue(int lig, int col)
    {
        var list = new GetIndiceStruct[2];

        for (int i = lig; i >= 0; i--)
        {
            var tmp = GetIndices(i, col);
            
            if (tmp is not null)
            {
                list[0] = new GetIndiceStruct{ col = col, lig = i, indice = tmp};
                break;
            }
        }
        
        for (int i = col; i >= 0; i--)
        {
            var tmp = GetIndices(lig, i);
            if (tmp is not null)
            {
                list[1] = new GetIndiceStruct {col = i, lig = lig, indice = tmp};
                break;
            }
        }

        return list;
    }

    /// <summary>
    /// permet de récupéré la taille d'une ligne ou colonne d'un indice.
    /// </summary>
    /// <param name="lig"></param>
    /// <param name="col"></param>
    /// <param name="isForLig">true pour la taille de la ligne, false pour celle de la colonne</param>
    /// <returns></returns>
    public int? GetTabLengthForIndice(int lig, int col, bool isForLig)
    {
        lig = isForLig ? lig : lig + 1;
        col = isForLig ? col + 1 : col;

        int nb = 0;
        while (lig < NbLig && col < NbCol && GetValue(lig, col) != null)
        {
            nb++;
            
            lig = isForLig ? lig : lig + 1;
            col = isForLig ? col + 1 : col;
        }

        return nb;
    }

    public override string ToString()
    {
        return ToString(false);
    }

    /// <summary>
    /// Ajoute un identifiant spécial pour les coueleurs
    /// </summary>
    /// <param name="addColor"></param>
    /// <returns></returns>
    private string ToString(bool addColor)
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
                    //int?[] indices => $"|{(addColor ? IsValidIndice(i, j) is > 5f and < 10f or > 15f ? "R" : "W" : "")}{(indices[0] is null ? " □" : $"{indices[0]:D2}")}{(addColor ? "W" : "")}\\{(addColor ? IsValidIndice(i, j) > 10f ? "R" : "W" : "")}{(indices[1] is null ? "□ " : $"{indices[1]:D2}")}{(addColor ? "W" : "")}|",
                    int?[] indices => $"{(addColor ? ValueOfIndice(i, j) == 0 ? "W" : "R" : "")}|{(indices[0] is null ? " □" : $"{indices[0]:D2}")}\\{(indices[1] is null ? "□ " : $"{indices[1]:D2}")}|{(addColor ? "W" : "")}",
                    _ => "|  □  |"
                };
            }

            sRet += "\n";
        }

        return sRet;
    }

    /// <summary>
    /// ecrit le plateau sur la console avec les couleurs
    /// </summary>
    public void PrintColorKakuro()
    {
        string str = this.ToString(true);

        foreach (var c in str)
        {
            if (c == 'W')
                Console.ForegroundColor = ConsoleColor.White;
            else if (c == 'R')
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.Write(c);
        }

        Console.ForegroundColor = ConsoleColor.White;
    }

    /// <summary>
    /// Compte rapidement le nombre de valeur à 0.
    /// Celà est utilisé pour accéléré le remplissage des valeur au début de l'algo.
    ///
    /// Avant cela permettait de restreindre le random qui sélectionnait une case au début de voisin.
    /// Mais celâ faisait perdre légèrement en performance.
    /// </summary>
    /// <returns></returns>
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public int Count0Value()
    {
        var nb = 0;

        for (var i = 0; i < NbLig; i++)
        {
            for (var j = 0; j < NbCol; j++)
            {
                if (TabValues?[i, j] == 0)
                    nb++;
            }
        }
        
        return nb;
    }

    /// <summary>
    /// Pour optimisé les temps, on créer un nouveau kakuro en ne clonant que le tableau de valeur.
    /// Le tableau d'indice ne changeant pas et étant en lecture seul.
    /// Cela permet de créer beaucoup moins d'objet et donc de prendre moins de ram en plus dene pas perdre de temps à les créer.
    /// </summary>
    /// <returns></returns>
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Nullable`1[System.Int32][][,]")]
    public Kakuro PseudoClone()
    {
        return new Kakuro(TabIndices)
        {
            TabValues = (TabValues.Clone() as int?[,])!
        };
    }

    public object Clone()
    {
        var newK = new Kakuro(NbLig, NbCol);
        
        newK.Initialize(TabIndices, TabValues);

        return newK;
    }
}