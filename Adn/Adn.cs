using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticDraw
{
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point()
        {
            X = 0;
            Y = 0;
        }
        public Point(int posX, int posY)
        {
            X = posX;
            Y = posY;
        }
    }

    class Genes
    {
        public Point pos { get; set; }
        public Color couleur { get; set; }

        public Genes()
        {
            pos = new Point();
            couleur = new Color();
        }
        public void setGenes(Point p, Color c)
        {
            pos = p;
            couleur = c;
        }
        public override string ToString()
        {
            return "Position : (" + pos.X + ";" + pos.Y + ") Couleur : " +
                "(" + couleur.R + "," + couleur.G + "," + couleur.B + ")";
        }
    }
    class Adn 
    {
        public long score { get; set; }
        public List<Genes> genes { get; set; }
        public int size { get; set; }
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public Adn()
        {
            score = -1;
            size = 0;
            genes = new List<Genes>();

        }

        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        public void setRandomGenes()
        {
            
            int r, g, b;
            for(int i = 0; i < Constantes.SIZE_X / Constantes.SIZE_RECT; i++)
            {
                for(int j = 0; j < Constantes.SIZE_Y / Constantes.SIZE_RECT; j++)
                {
                    r = RandomNumber(0, Constantes.SIZE_COLOR + 1);
                    g = RandomNumber(0, Constantes.SIZE_COLOR + 1);
                    b = RandomNumber(0, Constantes.SIZE_COLOR + 1);
                    Genes gene = new Genes();
                    Color c = Color.FromArgb(r, g, b);
                   
                    gene.setGenes(new Point(i * Constantes.SIZE_RECT, j * Constantes.SIZE_RECT), c);
                    genes.Add(gene);
                }
            }
        }

        public void eval(byte[] data, int depth)
        {
            for (int i = 0; i < Constantes.SIZE_X / Constantes.SIZE_RECT; i++)
            {
                for (int j = 0; j < Constantes.SIZE_Y / Constantes.SIZE_RECT; j++)
                {
                    int indexX = i * Constantes.SIZE_RECT;
                    int indexY = j * Constantes.SIZE_RECT;

                    var offset = ((indexY * Constantes.SIZE_X) + indexX) * depth;
                    int rData = data[offset + 2];
                    int gData = data[offset + 1];
                    int bData = data[offset];


                    int diffR = Math.Abs(rData - getColorGenes(indexX, indexY).R);
                    int diffG = Math.Abs(gData - getColorGenes(indexX, indexY).G);
                    int diffB = Math.Abs(bData - getColorGenes(indexX, indexY).B);
                    score += diffR + diffG + diffB;
                }
            }
        }
        public Color getColorGenes(int x, int y)
        {

            foreach (Genes g in genes)
            {
                if (g.pos.X == x && g.pos.Y == y)
                {
                    return g.couleur;
                }

            }
            return new Color();
        }
        public override string ToString()
        {
            String info = "";

            foreach (Genes g in genes)
                info += (g + "\n");
            return info;
        }


    }
}
