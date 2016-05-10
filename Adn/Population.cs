using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticDraw
{
    class Population
    {
        public List<Adn> adn{ get; set; }
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public Population(int nbr)
        {
            adn = new List<Adn>();
            
            for(int i = 0; i < nbr; i++)
            {
                Adn newAdn = new Adn();    
                newAdn.setRandomGenes();
                adn.Add(newAdn);
            }
        }

        public Population evolution()
        {
            Population newPopulation = new Population(0);
            // on garde le meilleur ADN de notre population
            newPopulation.adn.Add(this.getMeilleurAdn());
            for (int i = 1; i < Constantes.SIZE_POPULATION / 2; i++)
            {
                Adn pere = tournoisAdn(this);
                Adn mere = tournoisAdn(this);
                Adn fils = Reproduction(pere, mere);
                newPopulation.adn.Add(fils);
            }
            for (int i = 0; i < Constantes.SIZE_POPULATION / 2; i++)
            {
                Adn fils = new Adn();
                fils.setRandomGenes();
                newPopulation.adn.Add(fils);
            }
            return newPopulation;
        }
        public Adn getMeilleurAdn()
        {
            return adn.OrderBy(x => x.score).First();
        }
        public Adn tournoisAdn(Population p)
        {
            Population popTournois = new Population(0);
            for (int i = 0; i < 5; i++)
            {
                int randAdn = RandomNumber(0, p.adn.Count);
                popTournois.adn.Add(p.adn[randAdn]);
            }
            Adn adn = popTournois.getMeilleurAdn();
            return adn;
        }
        public Adn Reproduction(Adn pere, Adn mere)
        {
            Adn fils = new Adn();

            for (int i = 0; i < pere.genes.Count; i++)
            {
                if (RandomNumberDouble() <= 0.5)
                    fils.genes.Add(pere.genes[i]);
                else
                    fils.genes.Add(mere.genes[i]);

                // TODO : mutatation 
                int mut = RandomNumber(0, Constantes.SIZE_MUTATION);

            }                     

            return fils;
        }
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { 
                return random.Next(min, max);
            }
        }
        public static double RandomNumberDouble()
        {
            lock (syncLock)
            { // synchronize
                return random.NextDouble();
            }
        }
    }
}
