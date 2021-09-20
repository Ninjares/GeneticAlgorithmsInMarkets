using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgosForPortfolioManagement
{
    public enum Genome
    {
        Cash,
        RealEstate,
        Stocks,
        Crypto, 
        GoldSilver
    }
    public class Gene
    {
        public Genome[] gene;
        public double fitness;
    }
    class GenomeRepo
    {
        private Random rnd;
        public List<Gene> population;
        private int breedingPressure; //how much of the top population will be used for crosing
        private double MutationFactor;
        private int PopulationC;

        public GenomeRepo(int PopulationCount, double MutationFactor, int breedingPressure){
            this.breedingPressure = breedingPressure;
            this.MutationFactor = MutationFactor;
            population = new List<Gene>();
            rnd = new Random();
            PopulationC = PopulationCount;
            for(int i=0; i<PopulationCount; i++)
            {
                population.Add(new Gene { gene = GenerateRandomGene(), fitness = 100});
            }
        }

        public void RunSelection(double[] marketweeklyResult, int crossoverRange)
        {
            for(int i=0; i<population.Count; i++)
            {
                double percentage = EvaluateGene(population[i], marketweeklyResult);
                population[i].fitness = population[i].fitness + population[i].fitness*percentage/100;
            }
            population.Sort((a, b) => b.fitness.CompareTo(a.fitness));
            population = population.Take(breedingPressure).ToList();
            while (population.Count < PopulationC)
            {
                int a = rnd.Next(0, breedingPressure);
                int b = rnd.Next(0, breedingPressure);
                if (a != b)
                {
                    population.Add(MakeChild(population[a], population[b], crossoverRange));
                }
            }
            for(int i=0; i<population.Count; i++)
            {
                population[i].gene = MutateGene(population[i].gene);
            }
        }

        private double EvaluateGene(Gene gene, double[] marketResults) //percentages
        {
            double currentWeekPortfolioChangePercentage =
                gene.gene.Where(x => x == Genome.Cash).Count() * marketResults[0] / 100 +
                gene.gene.Where(x => x == Genome.RealEstate).Count() * marketResults[1] / 100 +
                gene.gene.Where(x => x == Genome.Stocks).Count() * marketResults[2] / 100 +
                gene.gene.Where(x => x == Genome.Crypto).Count() * marketResults[3] / 100 +
                gene.gene.Where(x => x == Genome.GoldSilver).Count() * marketResults[4] / 100;
            return currentWeekPortfolioChangePercentage;
        }

        private Genome[] GenerateRandomGene()
        {
            Genome[] gene = new Genome[100];
            for(int i=0; i<gene.Length; i++)
            {
                gene[i] = (Genome)rnd.Next(0, Enum.GetNames(typeof(Genome)).Length);
            }
            return gene;
        }

        private Genome[] MutateGene(Genome[] gene)
        {
            for(int i=0; i<gene.Length; i++)
            {
                if(rnd.Next(1, 1001) <= (int)(MutationFactor*10))
                {
                    gene[i] = (Genome)rnd.Next(0, Enum.GetNames(typeof(Genome)).Length);
                }
            }
            return gene;
        }
        
        private Gene MakeChild(Gene gene1, Gene gene2, int crossoverRange) //corssoverrange <= 100
        {
            crossoverRange /= 2;
            int crossoverPoint = 50 + rnd.Next(-crossoverRange, crossoverRange + 1);
            Gene child = new Gene
            {
                gene = gene1.gene.Take(crossoverPoint).Concat(gene2.gene.Skip(crossoverPoint)).ToArray(),
                fitness = (gene1.fitness + gene2.fitness) / 2
            };
            return child;
        }
    }
}
