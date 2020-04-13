using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace STEFF_Alban_Projet_Images_TD_K
{
    public class MyImage
    {
        /// Classe MyImage
        /// Définition des différents attributs

        private string fichier; // Nom du fichier
        private string type; // Contient le type de l'image (BM, ...)
        private int taillefichier; // Taille totale = image + métadonnées
        private int offset; // Taille du header (infos fichier + infos image)
        private int largeur; // Largeur image
        private int hauteur; // Hauteur image
        private int bitspercolor; // Une couleurRGB = 3 octets = 24 bits (à lire depuis le Header)
        private bool lecture; // Variable pour savoir si le fichier a bien été lu
        private Pixel[,] matriceRGB;

        /// CONSTRUCTEUR POUR LE TRAITEMENT D'IMAGE

        public MyImage(string fichier, int extension) // Extension contient le type de fichier (bitmap ou csv)
        {
            this.fichier = fichier;
            if (extension == 1) // Si c'est un fichier bitmap
            {
                try
                {
                    byte[] tab = File.ReadAllBytes(fichier);
                    type = Convertir_Ascii_To_String(Convert.ToInt32(tab[0])) + Convertir_Ascii_To_String(Convert.ToInt32(tab[1]));
                    taillefichier = Convertir_Endian_To_Int(tab, 2, 5);
                    offset = Convertir_Endian_To_Int(tab, 10, 13);
                    largeur = Convertir_Endian_To_Int(tab, 18, 21);
                    hauteur = Convertir_Endian_To_Int(tab, 22, 25);
                    bitspercolor = Convertir_Endian_To_Int(tab, 28, 29);
                    matriceRGB = new Pixel[hauteur, largeur];
                    int k = 0;
                    for (int i = 0; i < hauteur; i++)
                    {
                        for (int j = 0; j < largeur; j++)
                        {
                            matriceRGB[i, j] = new Pixel(Convert.ToInt32(tab[offset + k]), Convert.ToInt32(tab[offset + k + 1]), Convert.ToInt32(tab[offset + k + 2]));
                            k += 3;
                        }
                    }
                    lecture = true;
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    lecture = false;
                }
            }
            if (extension == 2)
            {
                int compteur = 0;
                int index = 0;
                byte[] reception = new byte[14];
                int tailletrouvee = 0;
                StreamReader flux = null;
                try
                {
                    flux = new StreamReader(fichier);
                    string line;
                    while ((line = flux.ReadLine()) != null && tailletrouvee < 1)
                    {
                        string[] t = Regex.Split(line, ";");
                        for (int j = 0; j < 14; j++)
                        {
                            reception[j] = Convert.ToByte(t[j]);
                        }
                        taillefichier = Convertir_Endian_To_Int(reception, 2, 5);
                        tailletrouvee = 1;
                    }
                    lecture = true;
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    lecture = false;
                }
                byte[] tab = new byte[taillefichier];
                int valeurlimite = (taillefichier - 54) / 5 + 2;
                StreamReader flux2 = null;
                try
                {
                    flux2 = new StreamReader(fichier);
                    string line;
                    while (((line = flux2.ReadLine()) != null) && compteur < valeurlimite)
                    {
                        if (compteur == 0)
                        {
                            string[] t = Regex.Split(line, ";");
                            for (int j = 0; j < 14; j++)
                            {
                                tab[j] = Convert.ToByte(t[j]);
                            }
                            index = 14;
                        }
                        if (compteur == 1)
                        {
                            string[] t = Regex.Split(line, ";");
                            for (int j = 0; j < 40; j++)
                            {
                                tab[j + index] = Convert.ToByte(t[j]);
                            }
                            index += 40;
                        }
                        if (compteur != 0 && compteur != 1)
                        {
                            string[] t = Regex.Split(line, ";");
                            for (int j = 0; j < 15; j++)
                            {
                                tab[j + index] = Convert.ToByte(t[j]);
                            }
                            index += 15;
                        }
                        compteur++;
                    }
                    type = Convertir_Ascii_To_String(Convert.ToInt32(tab[0])) + Convertir_Ascii_To_String(Convert.ToInt32(tab[1]));
                    taillefichier = Convertir_Endian_To_Int(tab, 2, 5);
                    offset = Convertir_Endian_To_Int(tab, 10, 13);
                    largeur = Convertir_Endian_To_Int(tab, 18, 21);
                    hauteur = Convertir_Endian_To_Int(tab, 22, 25);
                    bitspercolor = Convertir_Endian_To_Int(tab, 28, 29);
                    matriceRGB = new Pixel[hauteur, largeur];
                    int k = 0;
                    for (int i = 0; i < hauteur; i++)
                    {
                        for (int j = 0; j < largeur; j++)
                        {
                            matriceRGB[i, j] = new Pixel(Convert.ToInt32(tab[offset + k]), Convert.ToInt32(tab[offset + k + 1]), Convert.ToInt32(tab[offset + k + 2]));
                            k += 3;
                        }
                    }
                    lecture = true;
                }

                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    lecture = false;
                }
            }
        }

        /// CONSTRUCTEUR POUR LA CREATION DE FORME

        public MyImage(int largeur, int hauteur)
        {
            this.largeur = largeur;
            this.hauteur = hauteur;
            fichier = "";
            type = "BM";
            offset = 54;
            bitspercolor = 24;
            taillefichier = largeur * hauteur * 3 + 54;
            matriceRGB = new Pixel[hauteur, largeur];
        }

        /// PROPRIETES LECTURE / ECRITURE 

        public string Type
        {
            get { return type; }
        }

        public bool Lecture
        {
            get { return lecture; }
        }

        public int Taillefichier
        {
            get { return taillefichier; }
        }

        public int Offset
        {
            get { return offset; }
        }

        public int Largeur
        {
            get { return largeur; }
        }

        public int Hauteur
        {
            get { return hauteur; }
        }

        public int Bitspercolor
        {
            get { return bitspercolor; }
        }

        public Pixel[,] MatriceRGB
        {
            get { return matriceRGB; }
            set { matriceRGB = value; }
        }

        /// METHODES

        /// Fonction reprenant les valeurs des attributs de l'instance pour recréer un fichier .bmp ou .csv fonctionnel
        public void From_Image_To_File(string nomfichier, int format)
        {
            byte[] tofile = new byte[taillefichier];
            int compteur = 0;
            tofile[0] = Convert.ToByte(66);
            tofile[1] = Convert.ToByte(77);
            for (int i = 2; i < 6; i++)
            {
                tofile[i] = Convertir_Int_To_Endian(taillefichier)[compteur];
                compteur += 1;
            }
            for (int i = 6; i < 10; i++)
            {
                tofile[i] = Convert.ToByte(0);
            }
            compteur = 0;
            for (int i = 10; i < 14; i++)
            {
                tofile[i] = Convertir_Int_To_Endian(offset)[compteur];
                compteur += 1;
            }
            compteur = 0;
            for (int i = 14; i < 18; i++)
            {
                int offset1 = i;
                tofile[i] = Convertir_Int_To_Endian(offset - offset1)[compteur];
                compteur += 1;
            }
            compteur = 0;
            for (int i = 18; i < 22; i++)
            {
                tofile[i] = Convertir_Int_To_Endian(largeur)[compteur];
                compteur += 1;
            }
            compteur = 0;
            for (int i = 22; i < 26; i++)
            {
                tofile[i] = Convertir_Int_To_Endian(hauteur)[compteur];
                compteur += 1;
            }
            compteur = 0;
            for (int i = 26; i < 28; i++)
            {
                tofile[i] = Convertir_Int_To_Endian(1)[compteur];
                compteur += 1;
            }
            compteur = 0;
            for (int i = 28; i < 30; i++)
            {
                tofile[i] = Convertir_Int_To_Endian(bitspercolor)[compteur];
                compteur += 1;
            }
            for (int i = 30; i < 34; i++)
            {
                tofile[i] = Convert.ToByte(0);
            }
            compteur = 0;
            for (int i = 34; i < 38; i++)
            {
                tofile[i] = Convertir_Int_To_Endian(taillefichier - offset)[compteur];
                compteur += 1;
            }
            for (int i = 38; i < 54; i++)
            {
                tofile[i] = Convert.ToByte(0);
            }
            compteur = 54;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    tofile[compteur] = Convert.ToByte(matriceRGB[i, j].Rouge);
                    compteur++;
                    tofile[compteur] = Convert.ToByte(matriceRGB[i, j].Vert);
                    compteur++;
                    tofile[compteur] = Convert.ToByte(matriceRGB[i, j].Bleu);
                    compteur++;
                }
            }
            if (format == 1)
            {
                File.WriteAllBytes(nomfichier, tofile); // Ecriture du fichier modifié
                Process.Start(nomfichier); // Affichage du nouveau fichier
            }
            if (format == 2)
            {
                Console.WriteLine("/ ! | La création d'un fichier .csv est LONGUE. / ! | ");
                string headerinfo = "";
                for (int i = 0; i < 14; i++)
                {
                    headerinfo += Convert.ToString(tofile[i]) + ";";
                }
                string headerimage = "";
                for (int i = 14; i < 54; i++)
                {
                    headerimage += Convert.ToString(tofile[i]) + ";";
                }
                string image = "";
                int writeline = 0;
                for (int i = 54; i < tofile.Length; i++)
                {
                    image += Convert.ToString(tofile[i]) + ";";
                    writeline++;
                    if (writeline % 15 == 0) image += "\n";
                }
                string final = headerinfo + "\n" + headerimage + "\n" + image;
                File.WriteAllText(nomfichier, final);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Un nouveau fichier comportant votre image modifiée a bien été créé.\nIl porte le nom du traitement qu'il a subit.");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        /// <summary>
        /// Fonction affichant les valeurs de la matriceRGB
        /// </summary>
        public void AfficherMatriceRGB()
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Console.Write(matriceRGB[i, j].toString() + "\t");
                }
            }
        }

        /// <summary>
        /// Conversion de Endian à Int
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="debut"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        public int Convertir_Endian_To_Int(byte[] tab, int debut, int fin)
        {
            int valeur = 0;
            int puissance = 0;
            for (int i = debut; i <= fin; i++)
            {
                valeur += tab[i] * Puissance(2, puissance);
                puissance += 8;
            }
            return valeur;
        }

        /// <summary>
        /// Fonction puissance
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="puissance"></param>
        /// <returns></returns>
        public int Puissance(int nombre, int puissance)
        {
            int result = 1;
            for (int i = 0; i < puissance; i++)
            {
                result = result * nombre;
            }
            return result;

        }

        /// <summary>
        /// Conversion de Int à Endian
        /// </summary>
        /// <param name="valeur"></param>
        /// <returns></returns>
        public byte[] Convertir_Int_To_Endian(int valeur)
        {
            byte[] tab = new byte[4]; // Les informations sont codées sur 4 octets au maximum
            tab[3] = Convert.ToByte(valeur / Puissance(2, 24));
            valeur -= Convert.ToInt32(tab[3]) * Puissance(2, 24);
            tab[2] = Convert.ToByte(valeur / Puissance(2, 16));
            valeur -= Convert.ToInt32(tab[2]) * Puissance(2, 16);
            tab[1] = Convert.ToByte(valeur / Puissance(2, 8));
            valeur -= Convert.ToInt32(tab[1]) * Puissance(2, 8);
            tab[0] = Convert.ToByte(valeur);
            return tab;
        }

        /// <summary>
        /// Conversion d'ASCII à une chaîne de caractères
        /// </summary>
        /// <param name="valeur"></param>
        /// <returns></returns>
        public string Convertir_Ascii_To_String(int valeur)
        {
            return char.ConvertFromUtf32(valeur);
        }

        /// <summary>
        /// Niveaux de gris
        /// </summary>
        public void NiveauxDeGris()
        {
            for (int i = 0; i < MatriceRGB.GetLength(0); i++)
            {
                for (int j = 0; j < MatriceRGB.GetLength(1); j++)
                {
                    int tamp = (MatriceRGB[i, j].Bleu + MatriceRGB[i, j].Rouge + MatriceRGB[i, j].Vert) / 3;
                    MatriceRGB[i, j].Bleu = tamp;
                    MatriceRGB[i, j].Rouge = tamp;
                    MatriceRGB[i, j].Vert = tamp;
                }
            }
        }

        /// <summary>
        /// Rotation 90 degrés
        /// </summary>
        public void Rotation90()
        {
            Pixel[,] m = new Pixel[largeur, hauteur];
            for (int i = 0; i < MatriceRGB.GetLength(1); i++)
            {
                for (int j = 0; j < MatriceRGB.GetLength(0); j++)
                {
                    m[i, j] = MatriceRGB[j, largeur - 1 - i];
                }
            }
            int tamp = hauteur;
            hauteur = largeur;
            largeur = tamp;
            matriceRGB = m;
        }

        /// <summary>
        /// Rotation 180 degrés
        /// </summary>
        public void Rotation180()
        {
            Pixel[,] m = new Pixel[hauteur, largeur];
            for (int i = 0; i < MatriceRGB.GetLength(0); i++)
            {
                for (int j = 0; j < MatriceRGB.GetLength(1); j++)
                {
                    m[i, j] = MatriceRGB[Hauteur - 1 - i, Largeur - 1 - j];
                }
            }
            matriceRGB = m;
        }

        /// <summary>
        /// Rotation 270 degrés
        /// </summary>
        public void Rotation270()
        {
            Pixel[,] m = new Pixel[largeur, hauteur];
            for (int i = 0; i < MatriceRGB.GetLength(1); i++)
            {
                for (int j = 0; j < MatriceRGB.GetLength(0); j++)
                {
                    m[i, j] = matriceRGB[hauteur - 1 - j, i];
                }
            }
            int tamp = hauteur;
            hauteur = largeur;
            largeur = tamp;
            matriceRGB = m;
        }

        /// <summary>
        /// Noir et Blanc
        /// </summary>
        public void NoirEtBlanc()
        {
            int tamp = 0;
            for (int i = 0; i < MatriceRGB.GetLength(0); i++)
            {
                for (int j = 0; j < MatriceRGB.GetLength(1); j++)
                {
                    tamp = (MatriceRGB[i, j].Bleu + MatriceRGB[i, j].Rouge + MatriceRGB[i, j].Vert) / 3;
                    if (tamp > 127)
                    {
                        MatriceRGB[i, j].Rouge = 255;
                        MatriceRGB[i, j].Vert = 255;
                        MatriceRGB[i, j].Bleu = 255;
                    }
                    else
                    {
                        MatriceRGB[i, j].Rouge = 0;
                        MatriceRGB[i, j].Vert = 0;
                        MatriceRGB[i, j].Bleu = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Fonction de rétrécissement par 2
        /// </summary>
        public void Retrecir()
        {
            Pixel[,] m = new Pixel[hauteur / 2, largeur / 2];
            int k = 0;
            int n = 0;
            for (int i = 0; i < MatriceRGB.GetLength(0) / 2; i++)
            {
                for (int j = 0; j < MatriceRGB.GetLength(1) / 2; j++)
                {
                    m[i, j] = MatriceRGB[i + k, j + n];
                    n++;
                }
                k++;
                n = 0;
            }
            hauteur /= 2;
            largeur /= 2;
            taillefichier = hauteur * largeur * 3 + 54;
            MatriceRGB = m;
        }

        /// <summary>
        /// Fonction d'agrandissement par 2
        /// </summary>
        public void Agrandir()
        {
            Pixel[,] m = new Pixel[hauteur * 2, largeur * 2];
            int k = 0;
            int n = 0;
            hauteur *= 2;
            largeur *= 2;
            for (int i = 0; i < hauteur; i++)
            {
                if (i % 2 != 0) k++;
                for (int j = 0; j < largeur; j++)
                {
                    if (j % 2 != 0) n++;
                    m[i, j] = MatriceRGB[i - k, j - n];
                }
                n = 0;
            }
            taillefichier = hauteur * largeur * 3 + 54;
            MatriceRGB = m;
        }

        /// <summary>
        /// Fusion de 2 images
        /// </summary>
        /// <param name="superp"></param>
        public bool Fusion(MyImage superp)
        {
            bool done = false;
            if (hauteur == superp.Hauteur && largeur == superp.Largeur)
            {
                done = true;
                Pixel[,] m = new Pixel[hauteur, largeur];
                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        m[i, j] = new Pixel(0, 0, 0);
                        m[i, j].Bleu = (matriceRGB[i, j].Bleu + superp.matriceRGB[i, j].Bleu) / 2;
                        m[i, j].Rouge = (matriceRGB[i, j].Rouge + superp.matriceRGB[i, j].Rouge) / 2;
                        m[i, j].Vert = (matriceRGB[i, j].Vert + superp.matriceRGB[i, j].Vert) / 2;
                    }
                }
                matriceRGB = m;
            }
            if (hauteur > superp.Hauteur && largeur > superp.Largeur) // La première image est plus grande de partout (hauteur et largeur)
            {
                done = true;
                // On veut avoir l'image plus petite centrée
                int indexH = (hauteur - superp.Hauteur) / 2;
                int indexL = (largeur - superp.Largeur) / 2;
                for (int i = indexH; i < hauteur - indexH; i++)
                {
                    for (int j = indexL; j < largeur - indexL; j++)
                    {
                        matriceRGB[i, j].Bleu = (matriceRGB[i, j].Bleu + superp.matriceRGB[i - indexH, j - indexL].Bleu) / 2;
                        matriceRGB[i, j].Rouge = (matriceRGB[i, j].Rouge + superp.matriceRGB[i - indexH, j - indexL].Rouge) / 2;
                        matriceRGB[i, j].Vert = (matriceRGB[i, j].Vert + superp.matriceRGB[i - indexH, j - indexL].Vert) / 2;
                    }
                }
            }
            if ((hauteur < superp.Hauteur && largeur > superp.Largeur) || (hauteur > superp.Hauteur && largeur < superp.Largeur))
            {
                Console.WriteLine("Les dimensions de vos images étaient telles que la fusion n'était pas intéressante.\n"
                                 + "Essayez des images de mêmes taille ou une image qui englobe totalement l'autre.");
            }
            if (hauteur < superp.Hauteur && largeur < superp.Largeur) // Première image plus petite partout
            {
                done = true;
                int indexH = (superp.Hauteur - hauteur) / 2;
                int indexL = (superp.Largeur - largeur) / 2;
                for (int i = indexH; i < superp.Hauteur - indexH; i++)
                {
                    for (int j = indexL; j < superp.Largeur - indexL; j++)
                    {
                        superp.matriceRGB[i, j].Bleu = (superp.matriceRGB[i, j].Bleu + matriceRGB[i - indexH, j - indexL].Bleu) / 2;
                        superp.matriceRGB[i, j].Rouge = (superp.matriceRGB[i, j].Rouge + matriceRGB[i - indexH, j - indexL].Rouge) / 2;
                        superp.matriceRGB[i, j].Vert = (superp.matriceRGB[i, j].Vert + matriceRGB[i - indexH, j - indexL].Vert) / 2;
                    }
                }
                hauteur = superp.Hauteur;
                largeur = superp.Largeur;
                taillefichier = hauteur * largeur * 3 + 54;
                matriceRGB = superp.matriceRGB;
            }
            return done;
        }

        /// <summary>
        /// Superposition de deux images
        /// </summary>
        /// <param name="superp"></param>
        /// <returns></returns>
        public bool Superposition(MyImage superp)
        {
            bool done = false;
            if (hauteur == superp.Hauteur && largeur == superp.Largeur)
            {
                done = true;
                Pixel[,] m = new Pixel[hauteur, largeur];
                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        m[i, j] = new Pixel(0, 0, 0);
                        m[i, j].Bleu = superp.matriceRGB[i, j].Bleu;
                        m[i, j].Rouge = superp.matriceRGB[i, j].Rouge;
                        m[i, j].Vert = superp.matriceRGB[i, j].Vert;
                    }
                }
                matriceRGB = m;
            }
            if (hauteur > superp.Hauteur && largeur > superp.Largeur) // La première image est plus grande de partout (hauteur et largeur)
            {
                done = true;
                // On veut avoir l'image plus petite centrée
                int indexH = (hauteur - superp.Hauteur) / 2;
                int indexL = (largeur - superp.Largeur) / 2;
                for (int i = indexH; i < hauteur - indexH; i++)
                {
                    for (int j = indexL; j < largeur - indexL; j++)
                    {
                        matriceRGB[i, j].Bleu = superp.matriceRGB[i - indexH, j - indexL].Bleu;
                        matriceRGB[i, j].Rouge = superp.matriceRGB[i - indexH, j - indexL].Rouge;
                        matriceRGB[i, j].Vert = superp.matriceRGB[i - indexH, j - indexL].Vert;
                    }
                }
            }
            if ((hauteur < superp.Hauteur && largeur > superp.Largeur) || (hauteur > superp.Hauteur && largeur < superp.Largeur))
            {
                Console.WriteLine("Les dimensions de vos images étaient telles que la superposition n'était pas intéressante.\n"
                                 + "Essayez des images de mêmes taille ou une image qui englobe totalement l'autre.");
            }
            if (hauteur < superp.Hauteur && largeur < superp.Largeur) // Première image plus petite partout
            {
                done = true;
                int indexH = (superp.Hauteur - hauteur) / 2;
                int indexL = (superp.Largeur - largeur) / 2;
                for (int i = indexH; i < superp.Hauteur - indexH; i++)
                {
                    for (int j = indexL; j < superp.Largeur - indexL; j++)
                    {
                        superp.matriceRGB[i, j].Bleu = matriceRGB[i - indexH, j - indexL].Bleu;
                        superp.matriceRGB[i, j].Rouge = matriceRGB[i - indexH, j - indexL].Rouge;
                        superp.matriceRGB[i, j].Vert = matriceRGB[i - indexH, j - indexL].Vert;
                    }
                }
                hauteur = superp.Hauteur;
                largeur = superp.Largeur;
                taillefichier = hauteur * largeur * 3 + 54;
                matriceRGB = superp.matriceRGB;
            }
            return done;
        }

        /// <summary>
        /// Application d'un filtre sur l'image (flou, renforcement des bords, ...)
        /// </summary>
        /// <param name="convo"></param>
        /// <param name="diviseur"></param>
        public void Traitement(int[,] convo, int diviseur)
        {
            Pixel[,] m = new Pixel[hauteur, largeur];
            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    m[i, j] = matriceRGB[i, j];
                }
            }
            int sommerouge = 0;
            int sommebleu = 0;
            int sommevert = 0;
            for (int i = 0; i < largeur; i++) // On met des lignes noires sur les bords puisqu'elles ne font pas partie de l'algorithme
            {
                m[0, i] = new Pixel(0, 0, 0);
                m[hauteur - 1, i] = new Pixel(0, 0, 0);
            }
            for (int i = 0; i < hauteur; i++) // Pareil pour les colonnes aux extrémités
            {
                m[i, 0] = new Pixel(0, 0, 0);
                m[i, largeur - 1] = new Pixel(0, 0, 0);
            }
            for (int i = 1; i < hauteur - 1; i++) // Algorithme de traitement d'image
            {
                for (int j = 1; j < largeur - 1; j++)
                {
                    // On récupère la zone 3x3 autour du pixel modifié actuellement
                    Pixel[,] zone = { { matriceRGB[i - 1, j - 1], matriceRGB[i - 1, j], matriceRGB[i - 1, j + 1] },
                                    { matriceRGB[i, j - 1], matriceRGB[i, j], matriceRGB[i, j + 1] },
                                    { matriceRGB[i + 1, j - 1], matriceRGB[i + 1, j], matriceRGB[i + 1, j + 1] } };
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            // On fait la somme des produits pour chaque couleur
                            sommerouge += zone[k, l].Rouge * convo[k, l];
                            sommebleu += zone[k, l].Bleu * convo[k, l];
                            sommevert += zone[k, l].Vert * convo[k, l];
                        }
                    }
                    sommerouge /= diviseur;
                    sommebleu /= diviseur;
                    sommevert /= diviseur;
                    // Cas où les valeurs sont négatives
                    if (sommebleu < 0) sommebleu = 0;
                    if (sommerouge < 0) sommerouge = 0;
                    if (sommevert < 0) sommevert = 0;
                    // Cas où les valeurs sont > 255
                    if (sommebleu > 255) sommebleu = 255;
                    if (sommerouge > 255) sommerouge = 255;
                    if (sommevert > 255) sommevert = 255;
                    m[i, j] = new Pixel(0, 0, 0);
                    m[i, j].Rouge = sommerouge;
                    m[i, j].Vert = sommevert;
                    m[i, j].Bleu = sommebleu;
                    // On réinitialise la valeur des sommes à 0
                    sommevert = 0;
                    sommerouge = 0;
                    sommebleu = 0;
                }
            }
            // On modifie la matriceRGB
            MatriceRGB = m;
        }

        /// <summary>
        /// Création d'une forme géométrique
        /// </summary>
        /// <param name="forme"></param>
        public void CreationFormeGeometrique(int forme)
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    matriceRGB[i, j] = new Pixel(0, 0, 0);
                }
            }
            if (forme == 1)
            {
                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        matriceRGB[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }
            if (forme == 2)
            {
                int k = 0;
                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0 + k; j < largeur - k; j++)
                    {
                        matriceRGB[i, j] = new Pixel(255, 255, 255);
                    }
                    k++;
                }
            }
            if (forme == 3)
            {
                int k = 0;
                for (int i = hauteur / 2; i >= 0; i--)
                {
                    for (int j = 0 + k; j < largeur - k; j++)
                    {
                        matriceRGB[i, j] = new Pixel(255, 255, 255);
                    }
                    k++;
                }
                k = 0;
                for (int i = hauteur / 2; i < hauteur; i++)
                {
                    for (int j = 0 + k; j < largeur - k; j++)
                    {
                        matriceRGB[i, j] = new Pixel(255, 255, 255);
                    }
                    k++;
                }
            }
            if (forme == 4)
            {
                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        matriceRGB[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }

        }

        /// <summary>
        /// Création d'un cercle
        /// </summary>
        /// <param name="rayon"></param>
        public void CreationCercle(int rayon)
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if ((Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2)) <= Puissance(rayon, 2)) matriceRGB[i, j] = new Pixel(255, 255, 255);
                    else matriceRGB[i, j] = new Pixel(0, 0, 0);
                }
            }
        }

        /// <summary>
        /// Création d'un histogramme couleur (bleu, vert, rouge)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="couleur"></param>
        public void Histogramme(float[] t, int couleur)
        {
            int rouge = 0;
            int bleu = 0;
            int vert = 0;
            if (couleur == 1) rouge = 255;
            if (couleur == 2) vert = 255;
            if (couleur == 3) bleu = 255;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < t[255 - i]; j++)
                {
                    MatriceRGB[j, i] = new Pixel(bleu, vert, rouge);
                }
            }
        }

        /// <summary>
        /// Image comportant les 3 histogrammes à la fois
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        public void HistogrammeComplet(float[] t1, float[] t2, float[] t3)
        {
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < t1[255 - i]; j++) MatriceRGB[j, i] = new Pixel(0, 0, 255);
                for (int j = 0; j < t2[255 - i]; j++) MatriceRGB[j, i + 260] = new Pixel(0, 255, 0);
                for (int j = 0; j < t3[255 - i]; j++) MatriceRGB[j, i + 520] = new Pixel(255, 0, 0);
            }
        }

        /// <summary>
        /// Innovation : dégradation de l'intensité des couleurs selon des équations de cercles
        /// </summary>
        public void InnovationCercles()
        {
            int rayon1 = 0;
            int rayon2 = 0;
            int rayon3 = 0;
            int rayon4 = 0;
            if (hauteur >= largeur)
            {
                rayon1 = (largeur * 4) / 5;
                rayon2 = (largeur * 3) / 5;
                rayon3 = (largeur * 2) / 5;
                rayon4 = largeur / 5;
            }
            if (hauteur < largeur)
            {
                rayon1 = (hauteur * 4) / 5;
                rayon2 = (hauteur * 3) / 5;
                rayon3 = (hauteur * 2) / 5;
                rayon4 = hauteur / 5;
            }
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if ((Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2)) > Puissance(rayon1, 2))
                    {
                        matriceRGB[i, j].Bleu = (1 * matriceRGB[i, j].Bleu) / 10;
                        matriceRGB[i, j].Rouge = (1 * matriceRGB[i, j].Rouge) / 10;
                        matriceRGB[i, j].Vert = (1 * matriceRGB[i, j].Vert) / 10;
                    }
                    if ((Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2)) <= Puissance(rayon1, 2) &&
                    (Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2)) > Puissance(rayon2, 2))
                    {
                        matriceRGB[i, j].Bleu = matriceRGB[i, j].Bleu / 5;
                        matriceRGB[i, j].Rouge = matriceRGB[i, j].Rouge / 5;
                        matriceRGB[i, j].Vert = matriceRGB[i, j].Vert / 5;
                    }
                    if ((Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2)) <= Puissance(rayon2, 2) &&
                        (Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2)) > Puissance(rayon3, 2))
                    {
                        matriceRGB[i, j].Bleu = (2 * matriceRGB[i, j].Bleu) / 5;
                        matriceRGB[i, j].Rouge = (2 * matriceRGB[i, j].Rouge) / 5;
                        matriceRGB[i, j].Vert = (2 * matriceRGB[i, j].Vert) / 5;
                    }
                    if ((Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2)) <= Puissance(rayon3, 2) &&
                        (Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2)) > Puissance(rayon4, 2))
                    {
                        matriceRGB[i, j].Bleu = (3 * matriceRGB[i, j].Bleu) / 5;
                        matriceRGB[i, j].Rouge = (3 * matriceRGB[i, j].Rouge) / 5;
                        matriceRGB[i, j].Vert = (3 * matriceRGB[i, j].Vert) / 5;
                    }
                    if ((Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2)) <= Puissance(rayon4, 2))
                    {
                        matriceRGB[i, j].Bleu = (4 * matriceRGB[i, j].Bleu) / 5;
                        matriceRGB[i, j].Rouge = (4 * matriceRGB[i, j].Rouge) / 5;
                        matriceRGB[i, j].Vert = (4 * matriceRGB[i, j].Vert) / 5;
                    }
                }
            }
        }

        public void InnovationBeaucoupCercles()
        {
            int rayon1 = 0; //0.9
            int rayon2 = 0; //0.8
            int rayon3 = 0; //0.7
            int rayon4 = 0; //0.6
            int rayon5 = 0; //0.5
            int rayon6 = 0; //0.4
            int rayon7 = 0; //0.3
            int rayon8 = 0; //0.2
            int rayon9 = 0; //0.1
            if (hauteur >= largeur)
            {
                rayon1 = (largeur * 9) / 10;
                rayon2 = (largeur * 8) / 10;
                rayon3 = (largeur * 7) / 10;
                rayon4 = (largeur * 6) / 10;
                rayon5 = (largeur * 5) / 10;
                rayon6 = (largeur * 4) / 10;
                rayon7 = (largeur * 3) / 10;
                rayon8 = (largeur * 2) / 10;
                rayon9 = (largeur * 1) / 10;
            }
            if (hauteur < largeur)
            {
                rayon1 = (hauteur * 9) / 10;
                rayon2 = (hauteur * 8) / 10;
                rayon3 = (hauteur * 7) / 10;
                rayon4 = (hauteur * 6) / 10;
                rayon5 = (hauteur * 5) / 10;
                rayon6 = (hauteur * 4) / 10;
                rayon7 = (hauteur * 3) / 10;
                rayon8 = (hauteur * 2) / 10;
                rayon9 = (hauteur * 1) / 10;
            }
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    int a = Puissance(i - (hauteur / 2), 2) + Puissance(j - (largeur / 2), 2);
                    if (a > Puissance(rayon1, 2))
                    {
                        matriceRGB[i, j].Bleu = matriceRGB[i, j].Bleu / 10;
                        matriceRGB[i, j].Rouge = matriceRGB[i, j].Rouge / 10;
                        matriceRGB[i, j].Vert = matriceRGB[i, j].Vert / 10;
                    }
                    if ((a <= Puissance(rayon1, 2)) && (a > Puissance(rayon2, 2)))
                    {
                        matriceRGB[i, j].Bleu = (2 * matriceRGB[i, j].Bleu) / 10;
                        matriceRGB[i, j].Rouge = (2 * matriceRGB[i, j].Rouge) / 10;
                        matriceRGB[i, j].Vert = (2 * matriceRGB[i, j].Vert) / 10;
                    }
                    if ((a <= Puissance(rayon2, 2)) && (a > Puissance(rayon3, 2)))
                    {
                        matriceRGB[i, j].Bleu = (3 * matriceRGB[i, j].Bleu) / 10;
                        matriceRGB[i, j].Rouge = (3 * matriceRGB[i, j].Rouge) / 10;
                        matriceRGB[i, j].Vert = (3 * matriceRGB[i, j].Vert) / 10;
                    }
                    if ((a <= Puissance(rayon3, 2)) && (a > Puissance(rayon4, 2)))
                    {
                        matriceRGB[i, j].Bleu = (4 * matriceRGB[i, j].Bleu) / 10;
                        matriceRGB[i, j].Rouge = (4 * matriceRGB[i, j].Rouge) / 10;
                        matriceRGB[i, j].Vert = (4 * matriceRGB[i, j].Vert) / 10;
                    }
                    if ((a <= Puissance(rayon4, 2)) && (a > Puissance(rayon5, 2)))
                    {
                        matriceRGB[i, j].Bleu = (5 * matriceRGB[i, j].Bleu) / 10;
                        matriceRGB[i, j].Rouge = (5 * matriceRGB[i, j].Rouge) / 10;
                        matriceRGB[i, j].Vert = (5 * matriceRGB[i, j].Vert) / 10;
                    }
                    if ((a <= Puissance(rayon5, 2)) && (a > Puissance(rayon6, 2)))
                    {
                        matriceRGB[i, j].Bleu = (6 * matriceRGB[i, j].Bleu) / 10;
                        matriceRGB[i, j].Rouge = (6 * matriceRGB[i, j].Rouge) / 10;
                        matriceRGB[i, j].Vert = (6 * matriceRGB[i, j].Vert) / 10;
                    }
                    if ((a <= Puissance(rayon6, 2)) && (a > Puissance(rayon7, 2)))
                    {
                        matriceRGB[i, j].Bleu = (7 * matriceRGB[i, j].Bleu) / 10;
                        matriceRGB[i, j].Rouge = (7 * matriceRGB[i, j].Rouge) / 10;
                        matriceRGB[i, j].Vert = (7 * matriceRGB[i, j].Vert) / 10;
                    }
                    if ((a <= Puissance(rayon7, 2)) && (a > Puissance(rayon8, 2)))
                    {
                        matriceRGB[i, j].Bleu = (8 * matriceRGB[i, j].Bleu) / 10;
                        matriceRGB[i, j].Rouge = (8 * matriceRGB[i, j].Rouge) / 10;
                        matriceRGB[i, j].Vert = (8 * matriceRGB[i, j].Vert) / 10;
                    }
                    if ((a <= Puissance(rayon8, 2)) && (a > Puissance(rayon9, 2)))
                    {
                        matriceRGB[i, j].Bleu = (9 * matriceRGB[i, j].Bleu) / 10;
                        matriceRGB[i, j].Rouge = (9 * matriceRGB[i, j].Rouge) / 10;
                        matriceRGB[i, j].Vert = (9 * matriceRGB[i, j].Vert) / 10;
                    }
                }
            }
        }

        /// <summary>
        /// Innovation : création d'un Emoji souriant
        /// </summary>
        public void InnovationEmoji()
        {
            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (((Puissance(i - 150, 2) + Puissance(j - 200, 2)) <= Puissance(150, 2)))
                    {
                        matriceRGB[i, j] = new Pixel(0, 255, 255);
                    }
                    else matriceRGB[i, j] = new Pixel(0, 0, 0);
                }
            }
            for (int i = 150; i < 200; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    matriceRGB[i, j] = new Pixel(0, 0, 0);
                }
            }
            for (int i = 200; i < hauteur; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    matriceRGB[i, j] = new Pixel(0, 0, 0);
                }
                for (int j = 100; j < 150; j++)
                {
                    matriceRGB[i, j] = new Pixel(0, 255, 255);
                }
                for (int j = 150; j < 250; j++)
                {
                    matriceRGB[i, j] = new Pixel(0, 0, 0);
                }
                for (int j = 250; j < 300; j++)
                {
                    matriceRGB[i, j] = new Pixel(0, 255, 255);
                }
                for (int j = 300; j < 400; j++)
                {
                    matriceRGB[i, j] = new Pixel(0, 0, 0);
                }
            }
        }
    }
}
