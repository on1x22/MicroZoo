namespace MicroZoo.ZookeepersApi.Models
{
    public class RequestHelper
    {
        private readonly HttpClient _httpClient;

        public RequestHelper()
        {
            _httpClient = new HttpClient();
        }

        internal async Task<T> GetResponseAsync<T>(HttpMethod method, string requestUri)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(requestUri)
            };

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }

    }
}
