using HtmlAgilityPack;
using SonicServer.JsonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace SonicServer
{
    internal class PricingScraper
    {
        public const string prices_provider = "https://www.fastfoodmenuprices.com/sonic-prices/";
        static Logger logger = new Logger("Scraper", Color.SlateBlue);
        public static List<PricingEntry> GetPrices()
        {
            List<PricingEntry> output = new List<PricingEntry>();

            var doc = new HtmlDocument();

            logger.Debug("Sending GET request to provider");
            using (HttpClient client = new HttpClient())
            {
                Task<string> webTask = client.GetStringAsync(prices_provider);
                webTask.Wait();
                doc.LoadHtml(webTask.Result);
                logger.Debug("Successfully grabbed html doc");
            }

            var rows = doc.DocumentNode.SelectNodes("//table[@id='tablepress-22']/tbody/tr");

            if (rows != null)
            {
                var parsedRows = new List<(string, string, string)>();

                foreach (var row in rows)
                {
                    // Ensure that the row has at least 3 columns
                    var columns = row.SelectNodes("td");
                    if (columns != null && columns.Count >= 3)
                    {
                        string column1 = columns[0].InnerText.Trim();
                        string column2 = columns[1].InnerText.Trim();
                        string column3 = columns[2].InnerText.Trim();

                        parsedRows.Add((column1, column2, column3));
                    }
                }

                foreach (var (col1, col2, col3) in parsedRows)
                {
                    output.Add(new PricingEntry()
                    {
                        ItemIdentifier = (col1 + " " + col2).Replace("  ", " ").ToLower().Trim(),
                        Price = col3.Replace("$", "")
                    });
                    logger.Debug($"Column 1: {col1}, Column 2: {col2}, Column 3: {col3}");
                }
            }
            else
            {
                logger.Error("No rows found.");
            }
            return output;
        }

        public static string GetPriceForCatalogItem(ProductCatalog.Item item, List<PricingEntry> prices)
        {
            foreach (PricingEntry pr in prices)
            {
                foreach(ProductCatalog.Item.DescriptionProperty desc in item.Descriptions.Long)
                {
                    if (desc.Text.ToLower().Contains(pr.ItemIdentifier.ToLower()))
                        return pr.Price;
                }
            }

            return "2.99";
            //return prices.Where(pe => item.Descriptions.Long.FirstOrDefault(new ProductCatalog.Item.DescriptionProperty()
            //{
            //    Text = "",
            //    Language = ""
            //}).Text.ToLower().Contains(pe.ItemIdentifier)).FirstOrDefault(new PricingEntry()
            //{
            //    Price = null,
            //    ItemIdentifier = null
            //}).Price ?? "2.99"; // GARBAGE
        }
    }
}
