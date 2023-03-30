using GoldSaving.Lib;
using GoldSaving.Lib.Model;
using System.Diagnostics;

namespace GoldTask
{
 
    internal class Program
    {
     
        public static async Task TopAndLow3()
        {

            Console.WriteLine("What are the TOP 3 highest and TOP 3 lowest prices of gold within the last year?");
            var goldPriceService = new GoldClient();

            DateTime startDate = new DateTime(2022, 03, 30);
            DateTime endDate = new DateTime(2023, 03, 30);
            List<GoldPrice> prices = await goldPriceService.GetGoldPrices(startDate, endDate);
            IEnumerable<GoldPrice> sortedprices = prices.OrderBy(s => s.Price);

            var low3 = sortedprices.Take(3); 
            var top3 = sortedprices.TakeLast(3); 

            Console.WriteLine("The Top 3 : ");
            foreach (var price in top3)
            {
                Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
            }
            Console.WriteLine("The Lowest 3 : ");
            foreach (var price in low3)
            {
                Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
            }
        }
        public static async void Earn5Per()
        {
            Console.WriteLine("Part 3");

            var goldPriceService = new GoldClient();
            DateTime startDate = new DateTime(2020, 01, 01);
            DateTime endDate = new DateTime(2020, 01, 31);
            List<GoldPrice> prices = await goldPriceService.GetGoldPrices(startDate, endDate);

            double startPrice = prices.First().Price;

            var pricesAboveStart = prices.Where(x => ((x.Price - startPrice) / startPrice * 100) >= 5);
            Console.WriteLine("Results :");
            foreach (var price in pricesAboveStart)
            {
                Console.WriteLine($"Date: {price.Date.ToString("yyyy-MM-dd")}");
            }
        }

        public static async void part4()
        {
            Console.WriteLine("Part 4");
            var goldPriceService = new GoldClient();

            DateTime startDate = new DateTime(2022, 01, 01);
            DateTime endDate = new DateTime(2022, 12, 31);
            List<GoldPrice> prices = await goldPriceService.GetGoldPrices(startDate, endDate);
            var sortedData = prices.OrderByDescending(x => x.Price);
            var secondTen = sortedData.Skip(10).Take(10);
            var openingDates =
                (from x in prices
                 orderby x.Price descending
                 select x.Date)
                .Skip(10)
                .Take(10);

            Console.WriteLine("Opening dates for the second ten of prices in 2022:");
            foreach (var date in openingDates)
            {
                Console.WriteLine(date.ToString("yyyy-MM-dd"));
            }
        }

        public static async void part8()
        {
            Console.WriteLine("What are the averages of gold prices in 2020, 2021, 2022?");

            var goldPriceService = new GoldClient();

            DateTime startDate2020 = new DateTime(2020, 01, 01);
            DateTime endDate2020 = new DateTime(2020, 12, 31);
            DateTime startDate2021 = new DateTime(2021, 01, 01);
            DateTime endDate2021 = new DateTime(2021, 12, 31);
            DateTime startDate2022 = new DateTime(2022, 01, 01);
            DateTime endDate2022 = new DateTime(2022, 12, 31);

            List<GoldPrice> prices2020 = await goldPriceService.GetGoldPrices(startDate2020, endDate2020);
            List<GoldPrice> prices2021 = await goldPriceService.GetGoldPrices(startDate2021, endDate2021);
            List<GoldPrice> prices2022 = await goldPriceService.GetGoldPrices(startDate2022, endDate2022);

            var avgPrice2020 = (from x in prices2020 select x.Price).Average();
            var avgPrice2021 = (from x in prices2021 select x.Price).Average();
            var avgPrice2022 = (from x in prices2022 select x.Price).Average();

            Console.WriteLine($"Average price of gold in 2020: {avgPrice2020:C2}");
            Console.WriteLine($"Average price of gold in 2021: {avgPrice2021:C2}");
            Console.WriteLine($"Average price of gold in 2022: {avgPrice2022:C2}");
        }



        static void Main(string[] args)
        {
             TopAndLow3();
             Earn5Per();
             part4();
             part8();
        }

        
    }
}