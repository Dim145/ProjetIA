﻿using Projet;

// création de quelques tests (il n'y as pas de 15x15 mais le 14x14 ne passe pas toujours, donc bon c'est pas utile de tester plus...)
var exemples = new object[]
{
    new object[]
    {
        "6x6: Cas très simple (starter du site)\t<90ms en moyenne",
        new[,]
        {
            {null, new int?[]{4, null}, new int?[]{9, null}, null, null, null},
            {new int?[]{null, 3}, null, null, new int?[]{24, null}, null, null},
            {new int?[]{null, 17}, null, null, null, new int?[]{17, null}, null},
            {null, new int?[]{null, 18}, null, null, null, null},
            {null, null, new int?[]{null, 16}, null, null, null},
            {null, null, null, null, null, null}
        },
        new int?[,]
        {
            {null, null, null, null, null, null},
            {null, 0, 0, null, null, null},
            {null, 0, 0, 0, null, null},
            {null, null, 0, 0, 0, null},
            {null, null, null, 0, 0, null},
            {null, null, null, null, null, null}
        }
    },
    new object[]{
        "8x8: exemple du sujet (= wiki)\t<300ms en moyenne",
        new[,]{
        {null, new int?[]{23, null}, new int?[]{30, null}, null, null, new int?[]{27, null}, new int?[]{12, null}, new int?[]{16, null} },
        {new int?[]{null, 16}, null, null, null, new int?[]{17, 24}, null, null, null },
        {new int?[]{null, 17}, null, null, new int?[]{15, 29}, null, null, null, null },
        {new int?[]{null, 35}, null, null, null, null, null, new int?[]{12, null}, null },
        {null, new int?[]{null, 7}, null, null, new int?[]{7, 8}, null, null, new int?[]{7, null} },
        {null, new int?[]{11, null}, new int?[]{10, 16}, null, null, null, null, null },
        {new int?[]{null, 21}, null, null, null, null, new int?[]{null, 5}, null, null },
        {new int?[]{null, 6}, null, null, null, null, new int?[]{null, 3}, null, null }
    },
        new int?[,]{
            {null, null, null, null, null, null, null, null},
            {null, 0, 0, null, null, 0, 0, 0},
            {null, 0, 0, null, 0, 0, 0, 0},
            {null, 0, 0, 0, 0, 0, null, null},
            {null, null, 0, 0, null, 0, 0, null},
            {null, null, null, 0, 0, 0, 0, 0},
            {null, 0, 0, 0, 0, null, 0, 0},
            {null, 0, 0, 0, null, null, 0, 0}
        }
    },
    new object[]
    {
        "10x10: niveau médium du site\t<4000ms en moyenne",
        new[,]
        {
            {null, null, new int?[]{31, null}, new int?[]{17, null}, new int?[]{6, null}, new int?[]{18, null}, null, new int?[]{21, null}, new int?[]{15, null}, null},
            {null, new int?[]{22, 18}, null, null, null, null, new int?[]{null, 9}, null, null, new int?[]{3, null}},
            {new int?[]{null, 35}, null, null, null, null, null, new int?[]{17, 19}, null, null, null},
            {new int?[]{null, 8}, null, null, new int?[]{4, null}, new int?[]{null, 17}, null, null, null, null, null},
            {new int?[]{null, 18}, null, null, null, new int?[]{16, 13}, null, null, new int?[]{14, null}, new int?[]{18, null}, null},
            {null, new int?[]{null, 18}, null, null, null, new int?[]{12, 12}, null, null, null, new int?[]{6, null}},
            {null, new int?[]{17, null}, new int?[]{12, null}, new int?[]{19, 6}, null, null, new int?[]{null, 19}, null, null, null},
            {new int?[]{null, 19}, null, null, null, null, null, new int?[]{13, null}, new int?[]{14, 3}, null, null},
            {new int?[]{null, 21}, null, null, null, new int?[]{null, 18}, null, null, null, null, null},
            {null, new int?[]{null, 16}, null, null, new int?[]{null, 26}, null, null, null, null, null}
        },
        new int?[,]
        {
            {null, null, null, null, null, null, null, null, null, null},
            {null, null, 0, 0, 0, 0, null, 0, 0, null},
            {null, 0, 0, 0, 0, 0, null, 0, 0, 0},
            {null, 0, 0, null, null, 0, 0, 0, 0, 0},
            {null, 0, 0, 0, null, 0, 0, null, null, null},
            {null, null, 0, 0, 0, null, 0, 0, 0, null},
            {null, null, null, null, 0, 0, null, 0, 0, 0},
            {null, 0, 0, 0, 0, 0, null, null, 0, 0},
            {null, 0, 0, 0, null, 0, 0, 0, 0, 0},
            {null, null, 0, 0, null, 0, 0, 0, 0, null}
        }
    },
    new object[]
    {
        "12x12: niveau hard du site\t<80s en moyenne (réussi 1fois sur 3)", //#H23341
        new[,]
        {
            {null, null, new int?[]{25, null}, new int?[]{4, null}, null, new int?[]{16, null}, new int?[]{17, null}, new int?[]{22, null}, new int?[]{37, null}, new int?[]{10, null}, new int?[]{29, null}, null},
            {null, new int?[]{null, 5}, null, null, new int?[]{12, 37}, null, null, null, null, null, null, null},
            {null, new int?[]{21, 45}, null, null, null, null, null, null, null, null, null, new int?[]{9, null}},
            {new int?[]{null, 6}, null, null, new int?[]{null, 14}, null, null, new int?[]{null, 35}, null, null, null, null, null},
            {new int?[]{null, 13}, null, null, new int?[]{8, 3}, null, null, new int?[]{23, 8}, null, null, new int?[]{22, 3}, null, null},
            {new int?[]{null, 16}, null, null, null, new int?[]{37, null}, new int?[]{35, 31}, null, null, null, null, null, null},
            {null, new int?[]{10, null}, new int?[]{30, 28}, null, null, null, null, null, null, null, new int?[]{31, null}, new int?[]{10, null}},
            {new int?[]{null, 37}, null, null, null, null, null, null, new int?[]{14, null}, new int?[]{18, 12}, null, null, null},
            {new int?[]{null, 11}, null, null, new int?[]{9, 5}, null, null, new int?[]{null, 16}, null, null, new int?[]{null, 16}, null, null},
            {new int?[]{null, 35}, null, null, null, null, null, new int?[]{11, 7}, null, null, new int?[]{9, 5}, null, null},
            {null, new int?[]{null, 45}, null, null, null, null, null, null, null, null, null, null},
            {null, new int?[]{null, 24}, null, null, null, null, null, null, new int?[]{null, 12}, null, null, null}
        },
        new int?[,]
        {
            {null, null, null, null, null, null, null, null, null, null, null, null},
            {null, null, 0, 0, null, 0, 0, 0, 0, 0, 0, null},
            {null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, null},
            {null, 0, 0, null, 0, 0, null, 0, 0, 0, 0, 0},
            {null, 0, 0, null, 0, 0, null, 0, 0, null, 0, 0},
            {null, 0, 0, 0, null, null, 0, 0, 0, 0, 0, 0},
            {null, null, null, 0, 0, 0, 0, 0, 0, 0, null, null},
            {null, 0, 0, 0, 0, 0, 0, null, null, 0, 0, 0},
            {null, 0, 0, null, 0, 0, null, 0, 0, null, 0, 0},
            {null, 0, 0, 0, 0, 0, null, 0, 0, null, 0, 0},
            {null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, null},
            {null, null, 0, 0, 0, 0, 0, 0, null, 0, 0, null}
        }
    },
    new object[]
    {
        "14x14: niveau hard du site\t< 130s (réussi 1 fois sur 3)", // #H13162
        new[,]
        {
            {null, new int?[]{7, null}, new int?[]{37, null}, new int?[]{11, null}, new int?[]{44, null}, new int?[]{17, null}, new int?[]{17, null}, null, new int?[]{19, null}, new int?[]{11, null}, new int?[]{11, null}, null, new int?[]{29, null}, new int?[]{11, null}},
            {new int?[]{null, 39}, null, null, null, null, null, null, new int?[]{null, 20}, null, null, null, new int?[]{16, 17}, null, null},
            {new int?[]{null, 32}, null, null, null, null, null, null, new int?[]{45, 21}, null, null, null, null, null, null},
            {new int?[]{null, 11}, null, null, null, null, new int?[]{9, 10}, null, null, null, new int?[]{23, 6}, null, null, null, new int?[]{9, null}},
            {null, new int?[]{14, 25}, null, null, null, null, null, null, new int?[]{21, 30}, null, null, null, null, null},
            {new int?[]{null, 13}, null, null, new int?[]{15, 30}, null, null, null, null, null, null, new int?[]{42, null}, new int?[]{14, 15}, null, null},
            {new int?[]{null, 15}, null, null, null, null, null, new int?[]{16, 16}, null, null, null, null, null, new int?[]{29, null}, null},
            {null, new int?[]{null, 22}, null, null, null, new int?[]{20, 24}, null, null, null, new int?[]{7, 6}, null, null, null, new int?[]{4, null}},
            {null, new int?[]{5, null}, new int?[]{34, 32}, null, null, null, null, null, new int?[]{35, 26}, null, null, null, null, null},
            {new int?[]{null, 10}, null, null, new int?[]{10, null}, new int?[]{17, 22}, null, null, null, null, null, null, new int?[]{26, 9}, null, null},
            {new int?[]{null, 31}, null, null, null, null, null, new int?[]{10, 39}, null, null, null, null, null, null, new int?[]{11, null}},
            {null, new int?[]{9, 8}, null, null, null, new int?[]{14, 8}, null, null, null, new int?[]{6, 29}, null, null, null, null},
            {new int?[]{null, 21}, null, null, null, null, null, null, new int?[]{null, 21}, null, null, null, null, null, null},
            {new int?[]{null, 16}, null, null, new int?[]{null, 20}, null, null, null, new int?[]{null, 31}, null, null, null, null, null, null}
        },
        new int?[,]
        {
            {null, null, null, null, null, null, null, null, null, null, null, null, null, null},
            {null, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, null, 0, 0},
            {null, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0},
            {null, 0, 0, 0, 0, null, 0, 0, 0, null, 0, 0, 0, null},
            {null, null, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0},
            {null, 0, 0, null, 0, 0, 0, 0, 0, 0, null, null, 0, 0},
            {null, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, null, null},
            {null, null, 0, 0, 0, null, 0, 0, 0, null, 0, 0, 0, null},
            {null, null, null, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0},
            {null, 0, 0, null, null, 0, 0, 0, 0, 0, 0, null, 0, 0},
            {null, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, null},
            {null, null, 0, 0, 0, null, 0, 0, 0, null, 0, 0, 0, 0},
            {null, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0},
            {null, 0, 0, null, 0, 0, 0, null, 0, 0, 0, 0, 0, 0}
        }
    }
};

// -------------------------
// --- Séléction du test ---
// -------------------------
var index = -1;
do
{
    Console.WriteLine("Veuillez séléctionnez un cas parmis les suivants:");
    for (int i = 0; i < exemples.Length; i++)
    {
        var tabExemple = exemples[i] as object[];
        Console.WriteLine($"{i} = {tabExemple![0]}");
    }

    Console.Write("exemple séléctionné: ");
    try
    {
        index = int.Parse(Console.ReadLine()!);
    }
    catch (Exception)
    {
        Console.WriteLine("Valeur invalide, recommencez\n");
    }
} while (index < 0 || index >= exemples.Length);

var tabIndices = ((exemples[index] as object[])![1] as int?[]?[,])!;
var tabValue = ((exemples[index] as object[])![2] as int?[,])!;


// --------------------------
// --- Création du kakuro ---
// --------------------------

var kakuro = new Kakuro(tabIndices.GetLength(0), tabIndices.GetLength(1));

kakuro.Initialize(tabIndices!, tabValue);

kakuro.PrintColorKakuro();

// enregistrement de l'heure de départ
var start = DateTime.Now;

var resolvedKakuro = Algo.RecuitKakuro(kakuro);

// enregistrement de l'heure de fin
var end = DateTime.Now;

Console.Write('\n');
resolvedKakuro.PrintColorKakuro();
// affichage du temps en millisecondes
Console.WriteLine("temps: " + ((end - start).Ticks / TimeSpan.TicksPerMillisecond) + "ms");