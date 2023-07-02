namespace MicroZoo.ZookeepersApi.Models
{
    public class RequestHelper
    {
        private static HttpClient _httpClient = new HttpClient();

        public RequestHelper(HttpClient client)
        {
            _httpClient = client;
        }

        internal async Task<T> GetResponseAsync<T>(HttpMethod method, string requestUri)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(requestUri)
            };

            var response = await _httpClient./*GetAsync(request.RequestUri);*/ SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }

    }
}
