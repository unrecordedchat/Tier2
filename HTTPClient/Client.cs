namespace HTTPClient
{
    public static class Client
    {
        private static readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(() =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://127.0.0.1:9002")
            };
            return client;
        });

        public static HttpClient GetHttpClient() => _httpClient.Value;
    }
}