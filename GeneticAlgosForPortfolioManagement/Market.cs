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
        private bool rndEnabled;
        private int totalWeeks; 
        public Markets(int totalWeeks, bool crisisEnabled, bool rndEnabled)
        {
            CrisisEnabled = crisisEnabled;
            this.rndEnabled = rndEnabled;
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
            if (crisis) Console.Write("CRISIS ");
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
            if (crisis) appreciation = rndEnabled ? (double)rnd.Next(-60, 0) / 100d : -0.3;
            else appreciation = rndEnabled ? (double)rnd.Next(-3,10) / 100d : 0.035; //-0.03% to 0.1% a week avg 0.35
            if (week % 5 == 0) appreciation += 0.3; //rent
            return appreciation;
        }
        private double GetStocksPerformance()
        {
            double appreciation;
            if (crisis) appreciation = rndEnabled ? (double)rnd.Next(-80, -20) / 100d : -0.5; 
            else appreciation = rndEnabled ? rnd.Next(-30, 50) / 100d : (week % 10 == 0 ? -0.5 : 0.2); //-0.3% to 0.5% a week avg 0.1%
            if (week % 5 == 0) appreciation += rndEnabled ? (double)rnd.Next(0, 10) / 10d : 0.5; //dividents
            return appreciation;
        }
        private double GetCryptoPerformance()
        {
            double appreciation;
            if (crisis) appreciation = rndEnabled ? (double)rnd.Next(-2500, 2500) / 100d : (week%2==0 ? -12.5:12.5);
            else appreciation = rndEnabled ? (double)rnd.Next(-400, 500) / 100d : (week%2==0 ? -2:2.5);
            return appreciation;
        }
        private double GetGoldSilverPerformance()
        {
            if (crisis) return rndEnabled ? (double)rnd.Next(0, 500) / 100d : 2.5;
            else return rndEnabled ? (double)rnd.Next(-3, 5) / 100d : 0.01;
        }
    }
}
