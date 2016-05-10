using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Runtime.InteropServices;


namespace GeneticDraw
{
    public partial class Form1 : Form
    {
        Bitmap image;
        Object locker = new Object();
        Population population;
        byte[] buffer;
        int depth;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            image = Bitmap.FromFile(Constantes.PATH_IMAGE) as Bitmap;
            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var data = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
            depth = Bitmap.GetPixelFormatSize(data.PixelFormat) / 8; 
            buffer = new byte[data.Width * data.Height * depth];

            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            image.UnlockBits(data);

            Control.CheckForIllegalCrossThreadCalls = false;
            Thread th = new Thread(generation);
            th.Start();
           
        }
        private void generation()
        {
            population = new Population(Constantes.SIZE_POPULATION);

            for (int gen = 0; gen < Constantes.SIZE_GENERATION; gen++)
            {            
               
                foreach (Adn adn in population.adn)
                    adn.eval(buffer, depth);
                
                label1.Text = "Generation : " + gen + ";" + population.getMeilleurAdn().score;
                population = population.evolution();
                drawGene(population.getMeilleurAdn());
            }
            
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void drawGene(Adn adn)
        {
            Graphics gr = this.CreateGraphics();
                foreach(Genes g in adn.genes)
                    gr.FillEllipse(new SolidBrush(g.couleur), new Rectangle(g.pos.X, g.pos.Y, Constantes.SIZE_RECT + 2, Constantes.SIZE_RECT + 2));

        }





    }
}
