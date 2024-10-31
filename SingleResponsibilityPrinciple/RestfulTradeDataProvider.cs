using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class RestfulTradeDataProvider : ITradeDataProvider
    {
        string url;
        ILogger logger;

        public RestfulTradeDataProvider(string url, ILogger logger)
        {
            this.url = url;
            this.logger = logger;
        }

        public IEnumerable<string> GetTradeData()
        {
            List<string> tradeData = new List<string>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Asynchronously get the JSON data from the server
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogWarning($"Failed to retrieve data from URL: {url}. Status Code: {response.StatusCode}");
                        throw new Exception($"Failed to retrieve trade data from {url}. Status: {response.StatusCode}");
                    }

                    logger.LogInfo("Successfully connected to the server. Reading data...");

                    // Read the data as a JSON string
                    string jsonData = response.Content.ReadAsStringAsync().Result;

                    // Deserialize JSON data into a list of trade strings
                    tradeData = JsonSerializer.Deserialize<List<string>>(jsonData);

                    logger.LogInfo("Successfully read and deserialized trade data.");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning($"An error occurred while retrieving trade data: {ex.Message}");
                throw;
            }

            return tradeData;
        }
    }
}
