using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace STEFF_Alban_Projet_Images_TD_K
{
    class Program
    {
        /// <summary>
        /// Méthode permettant de demander un entier positif à l'utilisateur
        /// </summary>
        /// <returns></returns>
        static int SaisieNombrePositif()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result) || result < 0)
            {
                Console.WriteLine("Veuillez entrer un entier");
            }
            return result;
        }

        /// <summary>
        /// Méthode pour savoir si le fichier à traiter est un .csv ou .bmp
        /// </summary>
        /// <returns></returns>
        static int DemandeTypeFichier()
        {
            int type = 0;
            Console.WriteLine("Veuillez entrer 1 si vous voulez traiter un fichier .bmp ou 2 pour entrer un fichier .csv");
            type = SaisieNombrePositif();
            if (type < 1 || type > 2)
            {
                while (type < 1 || type > 2)
                {
                    Console.WriteLine("Votre numéro n'existe pas, veuillez entrer 1 si vous voulez traiter un fichier .bmp"
                                     + " ou 2 pour entrer un fichier .csv");
                    type = SaisieNombrePositif();
                }
            }
            return type;
        }

        /// <summary>
        /// Méthode pour connaître le type du fichier de sortie
        /// </summary>
        /// <returns></returns>
        static int DemandeFichierSortie()
        {
            int type = 0;
            Console.WriteLine("Veuillez entrer 1 si vous voulez obtenir un fichier .bmp ou 2 pour obtenir un fichier .csv");
            type = SaisieNombrePositif();
            if (type < 1 || type > 2)
            {
                while (type < 1 || type > 2)
                {
                    Console.WriteLine("Votre numéro n'existe pas, veuillez entrer 1 si vous voulez obtenir un fichier .bmp"
                                     + " ou 2 pour obtenir un fichier .csv");
                    type = SaisieNombrePositif();
                }
            }
            return type;
        }

        /// <summary>
        /// Programme principal
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            Console.WindowHeight = 40;
            Console.WindowWidth = 180;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            int objectif = 0; // Permet de récupérer l'objectif de l'utilisateur (agrandir, pivoter, ...)
            int type = 0;

            /// Accueil de l'utilisateur, demande du nom du fichier à traiter

            Console.WriteLine("-----------------       Projet de traitement d'images       -----------------\n\n");
            type = DemandeTypeFichier();
            Console.WriteLine("\nLes fichiers suivants sont à votre disposition :\n"
                             + "tajmahal.bmp\nlac_en_montagne.bmp\ncoco.bmp\nworm.bmp\nImage.csv"
                             + "\n\n-------->    Veuillez entrer le nom de votre fichier avec l'extension (.csv ou .bmp)");
            string fichier = Console.ReadLine();
            MyImage image = new MyImage(fichier, type);
            if (!image.Lecture)
            {
                Console.WriteLine("\n\n------------------- Votre fichier n'a pas été lu, vérifiez la syntaxe au prochain essai. Merci de relancer.");
                Console.ReadKey();
            }

            /// Si le fichier existe, alors on continue et on propose le menu

            if (image.Lecture)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Menu :\n"
                                     // Propose les différentes méthodes
                                     + "Possibilité 1 : Rétrécissement par 2 des lignes et colonnes\n"
                                     + "Possibilité 2 : Agrandissement par 2 des lignes et colonnes\n"
                                     + "Possibilité 3 : Rotation 90°\n"
                                     + "Possibilité 4 : Rotation 180°\n"
                                     + "Possibilité 5 : Rotation 270°\n"
                                     + "Possibilité 6 : Nuances de gris \n"
                                     + "Possibilité 7 : Superposition de 2 images\n"
                                     + "Possibilité 8 : Noir et Blanc\n"
                                     + "Possibilité 9 : Traitements sur l'image :\n- Flou\n- Détection contours\n- Renforcement des bords\n- Repoussage\n"
                                     + "Possibilité 10 : Création d'une image décrivant une forme géométrique\n"
                                     + "Possibilité 11 : Histogrammes d'une image\n"
                                     + "Possibilité 12 : Innovation : Fusion de deux images\n"
                                     + "Possibilité 13 : Innovation : Assombrissement d'image selon des équations de cercles\n"
                                     + "Possibilité 14 : Innovation : Création d'un emoji souriant\n"
                                     + "\n\n----------> Saisissez le numéro souhaité <----------\n");
                    // Demande quoi exécuter
                    objectif = SaisieNombrePositif();
                    if (objectif < 1 || objectif > 14)
                    {
                        while (objectif < 1 || objectif > 14)
                        {
                            Console.WriteLine("Votre numéro n'existe pas, veuillez entrer un nouveau");
                            objectif = SaisieNombrePositif();
                        }
                    }
                    switch (objectif) // Lance ce qui est souhaité par l'utilisateur
                    {
                        #region Rétrécissement
                        case 1:
                            Console.Clear();
                            MyImage image1 = new MyImage(fichier, type);
                            image1.Retrecir();
                            type = DemandeFichierSortie();
                            if (type == 1) image1.From_Image_To_File("Rétrécissement.bmp", 1);
                            if (type == 2) image1.From_Image_To_File("Rétrécissement.csv", 2);
                            break;
                        #endregion
                        
                        #region Agrandissement
                        case 2:
                            Console.Clear();
                            MyImage imagee2 = new MyImage(fichier, type);
                            imagee2.Agrandir();
                            type = DemandeFichierSortie();
                            if (type == 1) imagee2.From_Image_To_File("Agrandissement.bmp", 1);
                            if (type == 2) imagee2.From_Image_To_File("Agrandissement.csv", 2);
                            break;
                        #endregion

                        #region Rotation 90°
                        case 3:
                            Console.Clear();
                            MyImage image3 = new MyImage(fichier, type);
                            image3.Rotation90();
                            type = DemandeFichierSortie();
                            if (type == 1) image3.From_Image_To_File("Rotation90.bmp", 1);
                            if (type == 2) image3.From_Image_To_File("Rotation90.csv", 2);
                            break;
                        #endregion

                        #region Rotation 180°
                        case 4:
                            Console.Clear();
                            MyImage image4 = new MyImage(fichier, type);
                            image4.Rotation180();
                            type = DemandeFichierSortie();
                            if (type == 1) image4.From_Image_To_File("Rotation180.bmp", 1);
                            if (type == 2) image4.From_Image_To_File("Rotation180.csv", 2);
                            break;
                        #endregion

                        #region Rotation 270°
                        case 5:
                            Console.Clear();
                            MyImage image5 = new MyImage(fichier, type);
                            image5.Rotation270();
                            type = DemandeFichierSortie();
                            if (type == 1) image5.From_Image_To_File("Rotation270.bmp", 1);
                            if (type == 2) image5.From_Image_To_File("Rotation270.csv", 2);
                            break;
                        #endregion

                        #region Niveaux de gris
                        case 6:
                            Console.Clear();
                            MyImage image6 = new MyImage(fichier, type);
                            image6.NiveauxDeGris();
                            type = DemandeFichierSortie();
                            if (type == 1) image6.From_Image_To_File("Niveauxdegris.bmp", 1);
                            if (type == 2) image6.From_Image_To_File("Niveauxdegris.csv", 2);
                            break;
                        #endregion

                        #region Superposition
                        case 7:
                            Console.Clear();
                            MyImage image7 = new MyImage(fichier, type);
                            Console.WriteLine("Veuillez entrer le nom du deuxième fichier .bmp que vous souhaitez traiter");
                            string image2 = Console.ReadLine();
                            MyImage test2 = new MyImage(image2, 1);
                            if (image7.Superposition(test2))
                            {
                                type = DemandeFichierSortie();
                                if (type == 1) image7.From_Image_To_File("Superposition.bmp", 1);
                                if (type == 2) image7.From_Image_To_File("Superposition.csv", 2);
                            }
                            break;
                        #endregion

                        #region Noir Et Blanc
                        case 8:
                            Console.Clear();
                            MyImage image8 = new MyImage(fichier, type);
                            image8.NoirEtBlanc();
                            type = DemandeFichierSortie();
                            if (type == 1) image8.From_Image_To_File("CocoN&B.bmp", 1);
                            if (type == 2) image8.From_Image_To_File("CocoN&B.csv", 2);
                            break;
                        #endregion

                        #region Application d'un filtre
                        case 9:
                            Console.Clear();
                            Console.WriteLine("Tapez 1 pour une image plus floue.");
                            Console.WriteLine("Tapez 2 pour renforcement des bords.");
                            Console.WriteLine("Tapez 3 pour détection de contours.");
                            Console.WriteLine("Tapez 4 pour repoussage.");
                            int choice = SaisieNombrePositif();
                            if (choice < 1 || choice > 4)
                            {
                                while (choice < 1 || choice > 4)
                                {
                                    Console.WriteLine("Votre numéro n'existe pas, veuillez entrer un nouveau");
                                    choice = SaisieNombrePositif();
                                }
                            }
                            if (choice == 1)
                            {
                                MyImage image9 = new MyImage(fichier, type);
                                int[,] m = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                                image9.Traitement(m, 9);
                                type = DemandeFichierSortie();
                                if (type == 1) image9.From_Image_To_File("ImageFloue.bmp", 1);
                                if (type == 2) image9.From_Image_To_File("ImageFloue.csv", 2);
                            }
                            if (choice == 2)
                            {
                                MyImage image10 = new MyImage(fichier, type);
                                int[,] m = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
                                image10.Traitement(m, 2);
                                type = DemandeFichierSortie();
                                if (type == 1) image10.From_Image_To_File("RenforcementBords.bmp", 1);
                                if (type == 2) image10.From_Image_To_File("RenforcementBords.csv", 2);
                            }
                            if (choice == 3)
                            {
                                MyImage image11 = new MyImage(fichier, type);
                                int[,] m = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
                                image11.Traitement(m, 1);
                                type = DemandeFichierSortie();
                                if (type == 1) image11.From_Image_To_File("DétectionDesBords.bmp", 1);
                                if (type == 2) image11.From_Image_To_File("DétectionDesBords.csv", 2);
                            }
                            if (choice == 4)
                            {
                                MyImage image12 = new MyImage(fichier, type);
                                int[,] m = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
                                image12.Traitement(m, 1);
                                type = DemandeFichierSortie();
                                if (type == 1) image12.From_Image_To_File("Repoussage.bmp", 1);
                                if (type == 2) image12.From_Image_To_File("Repoussage.csv", 2);
                            }
                            break;
                        #endregion

                        #region Création de formes
                        case 10:
                            Console.Clear();
                            Console.WriteLine("Quelle forme voulez-vous créer ?\n\n"
                                              + "Tapez 1, 2, 3 ou 4 :\n"
                                              + "Carré : 1\nTriangle : 2\nLosange: 3\nRectangle : 4\nCercle : 5");
                            int forme = SaisieNombrePositif();
                            if (forme < 1 || forme > 5)
                            {
                                while (forme < 1 || forme > 5)
                                {
                                    Console.WriteLine("Votre numéro n'existe pas, veuillez entrer un nouveau");
                                    forme = SaisieNombrePositif();
                                }
                            }
                            if (forme == 1)
                            {
                                Console.WriteLine("Quelle est la taille en pixels du côté de votre carré ?");
                                int cote = SaisieNombrePositif();
                                if ((cote * cote) % 4 == 0)
                                {
                                    MyImage carre = new MyImage(cote, cote);
                                    carre.CreationFormeGeometrique(1);
                                    type = DemandeFichierSortie();
                                    if (type == 1) carre.From_Image_To_File("Carré.bmp", 1);
                                    if (type == 2) carre.From_Image_To_File("Carré.csv", 2);
                                }
                                else Console.WriteLine("Votre image n'a pas été créée car elle n'est pas de taille multiple de 4.");
                            }
                            if (forme == 2)
                            {
                                Console.WriteLine("Quelle est la taille en pixels de la hauteur de votre triangle ?");
                                int hauteur = SaisieNombrePositif();
                                if ((hauteur * hauteur) % 4 == 0)
                                {
                                    MyImage triangle = new MyImage(hauteur, hauteur);
                                    triangle.CreationFormeGeometrique(2);
                                    type = DemandeFichierSortie();
                                    if (type == 1) triangle.From_Image_To_File("Triangle.bmp", 1);
                                    if (type == 2) triangle.From_Image_To_File("Triangle.csv", 2);
                                }
                                else Console.WriteLine("Votre image n'a pas été créée car elle n'est pas de taille multiple de 4.");
                            }
                            if (forme == 3)
                            {
                                Console.WriteLine("Quelle est la taille en pixels de la diagonale du losange ?");
                                int diag = SaisieNombrePositif();
                                if ((diag * diag) % 4 == 0)
                                {
                                    MyImage losange = new MyImage(diag, diag);
                                    losange.CreationFormeGeometrique(3);
                                    type = DemandeFichierSortie();
                                    if (type == 1) losange.From_Image_To_File("Losange.bmp", 1);
                                    if (type == 2) losange.From_Image_To_File("Losange.csv", 2);
                                }
                                else Console.WriteLine("Votre image n'a pas été créée car elle n'est pas de taille multiple de 4.");
                            }
                            if (forme == 4)
                            {
                                Console.WriteLine("Quelle est la taille en pixels de la hauteur du rectangle ?");
                                int hauteur = SaisieNombrePositif();
                                Console.WriteLine("Quelle est la taille en pixels de la largeur du rectangle ?");
                                int largeur = SaisieNombrePositif();
                                if ((hauteur * largeur) % 4 == 0)
                                {
                                    MyImage rect = new MyImage(largeur, hauteur);
                                    rect.CreationFormeGeometrique(1);
                                    type = DemandeFichierSortie();
                                    if (type == 1) rect.From_Image_To_File("Rectangle.bmp", 1);
                                    if (type == 2) rect.From_Image_To_File("Rectangle.csv", 2);
                                }
                                else Console.WriteLine("Votre image n'a pas été créée car elle n'est pas de taille multiple de 4.");
                            }
                            if (forme == 5)
                            {
                                Console.WriteLine("Quelle est la taille en pixels du rayon de votre cercle ?");
                                int rayon = SaisieNombrePositif();
                                int val = rayon * 2;
                                while (val % 4 != 0) val++;
                                MyImage cercle = new MyImage(val, val);
                                cercle.CreationCercle(rayon);
                                type = DemandeFichierSortie();
                                if (type == 1) cercle.From_Image_To_File("Cercle.bmp", 1);
                                if (type == 2) cercle.From_Image_To_File("Cercle.csv", 2);
                            }
                            break;
                        #endregion

                        #region Histogrammes
                        case 11:
                            Console.Clear();
                            MyImage image13 = new MyImage(fichier, type);
                            float[] tab1 = new float[256];
                            float[] tab2 = new float[256];
                            float[] tab3 = new float[256];
                            float max1 = 0;
                            float max2 = 0;
                            float max3 = 0;
                            for (int k = 0; k < tab1.Length; k++) tab1[k] = 0;
                            for (int k = 0; k < tab2.Length; k++) tab2[k] = 0;
                            for (int k = 0; k < tab3.Length; k++) tab3[k] = 0;
                            Console.WriteLine("Quel histogramme voulez-vous ?\n1 = Rouge\n2 = Vert\n3 = Bleu\n4 = Les 3 en même temps");
                            int couleur = SaisieNombrePositif();
                            if (couleur < 1 || couleur > 4)
                            {
                                while (couleur < 1 || couleur > 4)
                                {
                                    Console.WriteLine("Votre numéro n'existe pas, veuillez entrer un nouveau");
                                    couleur = SaisieNombrePositif();
                                }
                            }
                            if (couleur == 1 || couleur == 4)
                            {
                                for (int i = 0; i < image13.Hauteur; i++)
                                {
                                    for (int j = 0; j < image13.Largeur; j++)
                                    {
                                        tab1[image13.MatriceRGB[i, j].Rouge] += 1;
                                    }
                                }
                                for (int l = 0; l < tab1.Length; l++)
                                {
                                    if (tab1[l] > max1) max1 = tab1[l];
                                }
                                for (int l = 0; l < tab1.Length; l++)
                                {
                                    tab1[l] = (tab1[l] / max1) * 500;
                                }
                            }
                            if (couleur == 2 || couleur == 4)
                            {
                                for (int i = 0; i < image13.Hauteur; i++)
                                {
                                    for (int j = 0; j < image13.Largeur; j++)
                                    {
                                        tab2[image13.MatriceRGB[i, j].Vert] += 1;
                                    }
                                }
                                for (int l = 0; l < tab2.Length; l++)
                                {
                                    if (tab2[l] > max2) max2 = tab2[l];
                                }
                                for (int l = 0; l < tab2.Length; l++)
                                {
                                    tab2[l] = (tab2[l] / max2) * 500;
                                }
                            }
                            if (couleur == 3 || couleur == 4)
                            {
                                for (int i = 0; i < image13.Hauteur; i++)
                                {
                                    for (int j = 0; j < image13.Largeur; j++)
                                    {
                                        tab3[image13.MatriceRGB[i, j].Bleu] += 1;
                                    }
                                }
                                for (int l = 0; l < tab3.Length; l++)
                                {
                                    if (tab3[l] > max3) max3 = tab3[l];
                                }
                                for (int l = 0; l < tab3.Length; l++)
                                {
                                    tab3[l] = (tab3[l] / max3) * 500;
                                }
                            }
                            if (couleur == 1)
                            {
                                MyImage histo = new MyImage(260, 500);
                                histo.CreationFormeGeometrique(4);
                                histo.Histogramme(tab1, couleur);
                                type = DemandeFichierSortie();
                                if (type == 1) histo.From_Image_To_File("HistogrammeR.bmp", 1);
                                if (type == 2) histo.From_Image_To_File("HistogrammeR.csv", 2);
                            }
                            if (couleur == 2)
                            {
                                MyImage histo = new MyImage(260, 500);
                                histo.CreationFormeGeometrique(4);
                                histo.Histogramme(tab2, couleur);
                                type = DemandeFichierSortie();
                                if (type == 1) histo.From_Image_To_File("HistogrammeV.bmp", 1);
                                if (type == 2) histo.From_Image_To_File("HistogrammeV.csv", 2);
                            }
                            if (couleur == 3)
                            {
                                MyImage histo = new MyImage(260, 500);
                                histo.CreationFormeGeometrique(4);
                                histo.Histogramme(tab3, couleur);
                                type = DemandeFichierSortie();
                                if (type == 1) histo.From_Image_To_File("HistogrammeB.bmp", 1);
                                if (type == 2) histo.From_Image_To_File("HistogrammeB.csv", 2);
                            }
                            if (couleur == 4)
                            {
                                MyImage histo = new MyImage(260 * 3, 500);
                                histo.CreationFormeGeometrique(4);
                                histo.HistogrammeComplet(tab1, tab2, tab3);
                                type = DemandeFichierSortie();
                                if (type == 1) histo.From_Image_To_File("HistogrammeComplet.bmp", 1);
                                if (type == 2) histo.From_Image_To_File("HistogrammeB.csv", 2);
                            }
                            break;
                        #endregion

                        #region Fusion d'images (Innovation 1)
                        case 12:
                            Console.Clear();
                            MyImage image14 = new MyImage(fichier, type);
                            Console.WriteLine("Veuillez entrer le nom du deuxième fichier .bmp que vous souhaitez traiter");
                            string imagee3 = Console.ReadLine();
                            MyImage test3 = new MyImage(imagee3, 1);
                            if (image14.Fusion(test3))
                            {
                                type = DemandeFichierSortie();
                                if (type == 1) image14.From_Image_To_File("Fusion.bmp", 1);
                                if (type == 2) image14.From_Image_To_File("Fusion.csv", 2);
                            }
                            break;
                        #endregion

                        #region Assombrissement selon des équations de cercle (Innovation 2)
                        case 13:
                            Console.Clear();
                            Console.WriteLine("Cette section permet d'assombrir l'image selon des couches circulaires"
                                             + "\nVoulez-vous assombrir rapidement (tapez 1) ou progressivement l'image (tapez 2) ?");
                            int choix = SaisieNombrePositif();
                            if (choix < 1 || choix > 2)
                            {
                                while (choix < 1 || choix > 2)
                                {
                                    Console.WriteLine("Votre numéro n'existe pas, veuillez entrer un nouveau");
                                    choix = SaisieNombrePositif();
                                }
                            }
                            if (choix == 1)
                            {
                                MyImage image15 = new MyImage(fichier, type);
                                image15.InnovationCercles();
                                type = DemandeFichierSortie();
                                if (type == 1) image15.From_Image_To_File("InnovationRapide.bmp", 1);
                                if (type == 2) image15.From_Image_To_File("InnovationRapide.csv", 2);
                            }
                            if (choix == 2)
                            {
                                MyImage image16 = new MyImage(fichier, type);
                                image16.InnovationBeaucoupCercles();
                                type = DemandeFichierSortie();
                                if (type == 1) image16.From_Image_To_File("InnovationLente.bmp", 1);
                                if (type == 2) image16.From_Image_To_File("InnovationLente.csv", 2);
                            }
                            break;
                        #endregion

                        #region Innovation Emoji
                        case 14:
                            Console.Clear();
                            MyImage image17 = new MyImage(400, 400);
                            image17.InnovationEmoji();
                            type = DemandeFichierSortie();
                            if (type == 1) image17.From_Image_To_File("InnovationEmoji.bmp", 1);
                            if (type == 2) image17.From_Image_To_File("InnovationEmoji.csv", 2);
                            break;
                        #endregion
                    }
                    Console.WriteLine("\n\n\nAppuyez sur ECHAP pour quitter ou sur une autre touche pour continuer");
                    cki = Console.ReadKey();
                } while (cki.Key != ConsoleKey.Escape); // Tant qu'on ne presse pas ECHAP
            }
        }
    }
}
