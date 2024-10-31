using Microsoft.Identity.Client;
using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class URLTradeDataProvider : ITradeDataProvider
    {
        string url;
        ILogger logger;

        public URLTradeDataProvider(string url, ILogger logger)
        {
            this.url = url;
            this.logger = logger;
        }

        public IEnumerable<string> GetTradeData()
        {
            List<string> tradeData = new List<string>();
            logger.LogInfo("Reading trades from URL: " + url);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning($"Failed to retrieve data from URL: {url}. Status Code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                    throw new Exception($"Failed to retrieve trade data from {url}. Status: {response.StatusCode} - {response.ReasonPhrase}");
                }
                // set up a Stream and StreamReader to access the data
                using (Stream stream = response.Content.ReadAsStreamAsync().Result)
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    // Read each line of the text file using reader.ReadLine()
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Add each line to the tradeData list
                        tradeData.Add(line);
                    }
                }
                return tradeData;
            }





        }
    }
}
