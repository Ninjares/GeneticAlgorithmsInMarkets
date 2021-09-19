using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgosForPortfolioManagement
{
    public class Markets
    {
        //0 - Cash - loses 2-3 percent every 52 weeks
        //1 - Real Estate - gets a small return of a constant 0.7 percent (rent) every 5th week (-1.5 on the 52th due to taxes and repairs)
        //2 - Stocks - gets a random return between 0 and 3 percent every 5th week
        //3 - Crypto - Extreme volatility, random pumps with 10-15% per week and single short lived crashes with 20% a week 
        //4 - GoldSilver - move 0.05 up percent per week. Explode with 5% per week during a crisis


        private int week; //max weeks 3000
        private Random rnd;
        private bool crisis;
        private bool CrisisEnabled;
        private int totalWeeks; 
        public Markets(int totalWeeks, bool crisisEnabled)
        {
            CrisisEnabled = crisisEnabled;
            this.totalWeeks = totalWeeks;
            crisis = false;
            rnd = new Random();
            week = 1;
        }
        public double[] GetWeeklyResults()
        {
            if ((double)week / (double)totalWeeks >= 0.7 && (double)week / (double)totalWeeks < 0.9 && CrisisEnabled) 
                crisis = true; 
            else crisis = false;
            if (crisis) Console.WriteLine("CRISIS");
            double[] weeklyMoves = new double[5];
            weeklyMoves[0] = GetCashPerformance();
            weeklyMoves[1] = GetRealEstatePerformance();
            weeklyMoves[2] = GetStocksPerformance();
            weeklyMoves[3] = GetCryptoPerformance();
            weeklyMoves[4] = GetGoldSilverPerformance();
            week++;
            return weeklyMoves;
        }

        private double GetCashPerformance()
        {
            if (crisis) return -1;
            else if (week % 52 == 52) return -2;
            else return 0;
        }
        private double GetRealEstatePerformance()
        {
            double appreciation;
            if (crisis) appreciation = -0.3;
            else appreciation = (double)rnd.Next(-50,150) / 1000d; //-0.05% to 0.15% a week
            if (week % 5 == 0) appreciation += 0.3; //rent
            return appreciation;
        }
        private double GetStocksPerformance()
        {
            double appreciation;
            if (crisis) appreciation = -0.5;
            else appreciation = rnd.Next(-80, 100) / 100d; //-0.8% -1% a week
            if (week % 5 == 0) appreciation += (double)rnd.Next(0, 10) / 10d; //dividents
            return appreciation;
        }
        private double GetCryptoPerformance()
        {
            double appreciation;
            if (crisis) appreciation = (double)rnd.Next(-2200, 2500) / 100d;
            else appreciation = (double)rnd.Next(-400, 500) / 100d;
            return appreciation;
        }
        private double GetGoldSilverPerformance()
        {
            if (crisis) return (double)rnd.Next(50, 500) / 100d;
            else return (double)rnd.Next(-10, 20) / 100d;
        }
    }
}
