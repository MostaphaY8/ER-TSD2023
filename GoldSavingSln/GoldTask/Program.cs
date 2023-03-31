using GoldSaving.Lib;
using GoldSaving.Lib.Model;
using System.Diagnostics;
using System.Xml.Linq;

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

        public static async void part9()
        {
            Console.WriteLine("When would it be best to buy gold and sell it between 2019 and 2022? What would be the return on investment?");
            var goldPriceService = new GoldClient();

            DateTime startDate = new DateTime(2019, 01, 01);
            DateTime endDate = new DateTime(2019, 12, 31);
            List<GoldPrice> prices = await goldPriceService.GetGoldPrices(startDate, endDate);

            startDate = new DateTime(2020, 01, 01);
            endDate = new DateTime(2020, 12, 31);
            prices = prices.Concat(await goldPriceService.GetGoldPrices(startDate, endDate)).ToList();

            startDate = new DateTime(2021, 01, 01);
            endDate = new DateTime(2021, 12, 31);
            prices = prices.Concat(await goldPriceService.GetGoldPrices(startDate, endDate)).ToList();

            startDate = new DateTime(2022, 01, 01);
            endDate = new DateTime(2022, 12, 31);
            prices = prices.Concat(await goldPriceService.GetGoldPrices(startDate, endDate)).ToList();

            double minPrice = prices.Min(p => p.Price); 
            double maxPrice = prices.Max(p => p.Price); 

            DateTime minDate = prices.First(p => p.Price == minPrice).Date;
            DateTime maxDate = prices.First(p => p.Price == maxPrice).Date;

            double investment = 1000; 
            double buyPrice = minPrice;
            double sellPrice = maxPrice;
            double roi = ((sellPrice - buyPrice) / buyPrice) * 100;
            double profit = investment * (roi / 100);

            Console.WriteLine($"best time to buy gold was on {minDate.ToString("yyyy-MM-dd")} at a price of {minPrice:C2}");
            Console.WriteLine($"best time to sell gold was on {maxDate.ToString("yyyy-MM-dd")} at a price of {maxPrice:C2}");
            Console.WriteLine($"you investe {investment:C2}, your return is {roi:F2}%");
            Console.WriteLine($" profit : {profit:C2}");
        }
        public static void SavePriceInXmlFile(IEnumerable<decimal> prices, string filePath)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("Prices");
            doc.Add(root);
            foreach (decimal price in prices)
            {
                XElement priceElement = new XElement("Price", price);
                root.Add(priceElement);
            }
            doc.Save(filePath);
        }
        static void Main(string[] args)
        {
             TopAndLow3();
             Earn5Per();
             part4();
             part8();
             part9();
        }

        
    }
}