using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEFF_Alban_Projet_Images_TD_K
{
    public class Pixel
    {
        /// <summary>
        /// Définition des attributs de la classe Pixel
        /// </summary>
        private int rouge;
        private int vert;
        private int bleu;

        /// <summary>
        /// Constructeur classe Pixel
        /// </summary>
        /// <param name="rouge"></param>
        /// <param name="vert"></param>
        /// <param name="bleu"></param>
        public Pixel(int rouge, int vert, int bleu)
        {
            this.vert = vert;
            this.bleu = bleu;
            this.rouge = rouge;
        }

        public int Vert
        {
            set { vert = value; }
            get { return vert; }
        }

        public int Rouge
        {
            get { return rouge; }
            set { rouge = value; }
        }

        public int Bleu
        {
            get { return bleu; }
            set { bleu = value; }
        }
        
        /// <summary>
        /// Permet de retourner les valeurs d'un pixel
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            return rouge + " \t" + vert + " \t" + bleu;
        }
    }
}
