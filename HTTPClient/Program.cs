using System.Text;
using System.Text.Json;
namespace HTTPClient
{
    public class Program
    {
        private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:8080") };

        static async Task Main(string[] args)
        {
            // Call the method to send a string and receive Echo objects
            await PostEcho("Hello from C#");
        }

        // Method to POST to /echo and receive a list of Echo objects
        public static async Task PostEcho(string content)
        {
            // Prepare the string content
            var contentBody = new StringContent(content, Encoding.UTF8, "text/plain");
            var response = await client.PostAsync("/echo", contentBody);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                
                // Deserialize the response into a List<Echo>
                var echoes = JsonSerializer.Deserialize<List<Echo>>(responseBody);

                // Output the response
                Console.WriteLine("Echo Response:");
                echoes?.ForEach(echo => Console.WriteLine($"EchoId: {echo.EchoId}, Content: {echo.Content}"));
            }
            else
            {
                Console.WriteLine($"Failed to POST to /echo. Status Code: {response.StatusCode}");
            }
        }
    }
}