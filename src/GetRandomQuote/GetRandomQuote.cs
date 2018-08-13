using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Brucelee.Quotes
{
    public static class GetRandomQuote
    {
        [FunctionName("GetRandomQuote")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
            [Blob("$root/quotes.txt", FileAccess.Read)] Stream myBlob,
            TraceWriter log)
        {
            int GetRandomIndex(int maxValue)
            {
                var random = new Random();

                return random.Next(0, maxValue);
            }

            async Task<string[]> ReadLinesAsync()
            {
                using (var reader = new StreamReader(myBlob))
                {
                    string text = await reader.ReadToEndAsync()
                        .ConfigureAwait(false);

                    return text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                }
            }

            string[] lines = await ReadLinesAsync();

            int index = GetRandomIndex(lines.Length);

            log.Info($"get quote at index {index}");

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(lines[index])
            };
        }
    }
}
