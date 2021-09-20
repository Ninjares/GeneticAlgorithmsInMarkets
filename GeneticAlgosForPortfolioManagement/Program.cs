using System;
using System.Linq;

namespace GeneticAlgosForPortfolioManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int totalGenerations = 1000;
                GenomeRepo repo = new GenomeRepo(100, 2, 20);
                Markets markets = new Markets(totalGenerations, true, true) ;
                for (int i = 0; i < totalGenerations; i++)
                {
                    var marketweek = markets.GetWeeklyResults();
                    repo.RunSelection(marketweek, 20);
                    //Console.WriteLine($"[{string.Join(", ", marketweek)}]");
                    Console.WriteLine($"Gen{i+1} TP: {string.Join("", repo.population[0].gene.Select(x => (int)x).OrderBy(x => x))} - {repo.population[0].fitness}");
                    //Print top performer of the generation
                }
                Console.WriteLine();
                Console.ReadKey();
            }
        }
    }
}
