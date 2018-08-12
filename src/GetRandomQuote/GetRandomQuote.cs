using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Brucelee.Quotes
{
    public static class GetRandomQuote
    {
        [FunctionName("GetRandomQuote")]
        public static async Task<string> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
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

            return lines[index];
        }
    }
}
