using System.Diagnostics.CodeAnalysis;

namespace Projet;

public static class Algo
{
    private static float Tinit => 50f;

    /// <summary>
    /// Méthode générique de l'algo de recuit simulé.
    /// Simple traduction du pseudo code du cours en c#
    ///
    /// Toutes les zone plus spécifique doivent être précisé en paramètre de la méthode.
    /// </summary>
    /// <param name="x0">valeur initial</param>
    /// <param name="conditionArret">doit renvoyer true pour arret, false sinon</param>
    /// <param name="voisin">renvoie une valeur proche de celle envoyer. Dans le cas où "TYpe" est un objet, l'adresse mémoire ne doit pas être la même</param>
    /// <param name="f">fonction qui renvoie une valeur en float pour déterminé la valeur à maximisé ou minimisé</param>
    /// <param name="accept">renvoie true pour utilisé la nouvelle valeur, false sinon</param>
    /// <param name="decroissance">renvoie la nouvelle température en fonction de l'ancienne</param>
    /// <typeparam name="TYpe">Le type de l'objet qui est manipulé Kakuro par exemple</typeparam>
    /// <returns></returns>
    private static TYpe Recuit<TYpe>(TYpe x0, Func<TYpe, float, float, bool> conditionArret, Func<TYpe, TYpe> voisin, Func<TYpe, float> f, Func<float, float, bool> accept, Func<float, float> decroissance)
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

    /// <summary>
    /// Random initialisé au départ du programme et utilisé tous du long.
    ///
    /// Pendant un temps de le changeait en cours de route pour avoir une nouvelle graine, mais cela ne servait pas à grand choses
    /// </summary>
    private static Random random { get; } = new();

    /// <summary>
    /// Nom de méthode absolument parfait.
    ///
    /// Créer un tableau de la taille voulu dont l'adition des valeur = à la valeur total voulu.
    /// Les valeur du tableau sont random.
    /// ex: DecompositionPlusRandom(5, 3) peut renvoyer: [2, 1, 2] ou [1, 3, 1] etc...
    /// </summary>
    /// <param name="num">Valeur total voulu</param>
    /// <param name="subNum">Nombres de cases du tableau</param>
    /// <returns>Un tableau de taille Y dont la somme des valeurs est égale à X</returns>
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
    
    /// <summary>
    /// Plus utilisé, cela permettait de mélangé un tableau.
    /// Originalement utilisé dans la décomposition
    /// </summary>
    /// <param name="rng"></param>
    /// <param name="array"></param>
    /// <typeparam name="T"></typeparam>
    public static void Shuffle<T> (this Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            
            (array[n], array[k]) = (array[k], array[n]);
        }
    }

    /// <summary>
    /// Méthode créer pour éviter de polluer le fichier Program.cs.
    /// </summary>
    /// <param name="kakuro"></param>
    /// <returns></returns>
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static Kakuro RecuitKakuro(Kakuro kakuro)
    {
        var step = 0;
        
        return Recuit(kakuro,
  (kakuro1, f, arg3) =>
            {
                // Selon la taille du kakuro, il ne sert à rien de s'acharner plus que nécéssaire.
                if (f < 0.0001f && step > 250*(kakuro1.NbLig*kakuro1.NbCol))
                {
                    Console.WriteLine($"Fin avant d'avoir trouvé de solution. ({kakuro1.ValuesOfInvalidIndices()})");
                    return true;
                }
                
                step++;
                //Console.WriteLine(kakuro1.ValuesOfInvalidIndices());
                return !kakuro1.Contains(0) && kakuro1.IsValid();
            },
            oldK =>
            {
                int rLig;
                int rCol;

                // selection d'une case contenant une valeur
                int? tmpValue;
                do
                {
                    rLig = random.Next(0, oldK.NbLig);
                    rCol = random.Next(0, oldK.NbCol);

                    tmpValue = oldK.GetValue(rLig, rCol);
                } while (tmpValue is null);

                // récupération de ses indices
                var indices = oldK.GetIndiceOfValue(rLig, rCol);
                
                // clone de la valeur actuelle
                var newK = oldK.PseudoClone();
                
                var indiceLig = indices[1];
                
                // on calcule une décomposition de la ligne
                var decompositionLig = DecompositionPlusRandom(indiceLig.indice![1]!.Value, newK.GetTabLengthForIndice(indiceLig.lig, indiceLig.col, true)!.Value);

                // qui est directement ajouter sans vérification
                for (int i = 0; i < decompositionLig.Length; i++)
                    newK.SetValue(indiceLig.lig, indiceLig.col + 1 + i, decompositionLig[i]);
                
                var indiceCol = newK.GetIndiceOfValue(rLig, rCol - (rCol - indices[0].col))[0];

                // parcourt de chacunes des cases de la ligne
                // cette méthode est plus optimisé que celle fait de base qui consistait
                // à juste remplir la ligne et colonne de la case séléctionné
                for (int i = 0; i < decompositionLig.Length; i++)
                {
                    // cacule de la case qui croise la ligne sur le tableau de la colonne
                    var numLigLock = rLig - indiceCol.lig - 1;

                    // calcule de la décomposition de la colonne
                    var decompositionCol = DecompositionPlusRandom(
                        indiceCol.indice![0]!.Value, 
                        newK.GetTabLengthForIndice(indiceCol.lig, indiceCol.col, false)!.Value
                    );

                    // si les cases ont bien la même valeur, on modifie toute la colonne.
                    // de base d'autres vérification était faite comme est ce que cela ne casse pas les lignes de la case en cours
                    // mais cela réduisait drastiquement les performances sans pour autant aider pour les grand plateaux.
                    if (decompositionCol[numLigLock] == decompositionLig[i])
                    {
                        for (int j = 0; j < decompositionCol.Length; j++)
                            newK.SetValue(indiceCol.lig + 1 + j, indiceCol.col, decompositionCol[j]);
                    }

                    // Passage à la case suivante.
                    if (indiceCol.col + 1 < newK.NbCol && newK.GetValue(rLig, indiceCol.col + 1) != null)
                        indiceCol = newK.GetIndiceOfValue(rLig, indiceCol.col + 1)[0];
                }

                return newK;
            },
            kakuro1 => kakuro1.Count0Value() + kakuro1.ValuesOfInvalidIndices(), // compte le nombre de case valeur vde et la valeur du plateaux
            (diff, f) =>
            {
                // exactement la méthode présenté dans le cours, a la différence que le random n'est pas exécuté si
                // la valeur a <= 0 (inutile puisque nextDouble renvoi une valeur entre 0 et 1)
                if (diff > 0) // nouveau - ancien = 12 - 13 = -1
                {
                    var a = (float) Math.Exp( -diff/f );
                    if( a <= 0 || random.NextDouble() >= a ) 
                    {
                        return false;
                    }
                }

                return true;
            },
            decroissance: f =>
            {
                // la température diminue proportionellement à sa valeur actuelle.
                // ce n'est plus que surement pas la meilleur manière de faire.
                var value = f - (f / Tinit) * 2;

                return value;
            });
    }
}