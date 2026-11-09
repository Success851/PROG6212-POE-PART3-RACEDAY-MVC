
using System.Text;
using System.Text.Json;

namespace RaceDayMvc.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(
            IHttpClientFactory factory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpClient Client()
        {
            var client = _factory.CreateClient();

            client.BaseAddress = new Uri(
                _configuration["ApiSettings:BaseUrl"]!);

            var apiCookie = _httpContextAccessor
                .HttpContext?
                .Session
                .GetString("ApiSessionCookie");

            if (!string.IsNullOrEmpty(apiCookie))
            {
                client.DefaultRequestHeaders.Remove("Cookie");
                client.DefaultRequestHeaders.Add("Cookie", apiCookie);
            }

            return client;
        }

        // GET: Retrieve a list of records
        public async Task<List<T>> GetAsync<T>(string url)
        {
            var response = await Client().GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<T>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<T>();
        }

        // GET: Retrieve one record
        public async Task<T?> GetSingleAsync<T>(string url)
        {
            var response = await Client().GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        // POST: Send data to the API
        public async Task<HttpResponseMessage> PostAsync<T>(
            string url,
            T data)
        {
            var json = JsonSerializer.Serialize(data);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            return await Client().PostAsync(url, content);
        }

        // PUT: Update data through the API
        public async Task<HttpResponseMessage> PutAsync<T>(
            string url,
            T data)
        {
            var json = JsonSerializer.Serialize(data);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            return await Client().PutAsync(url, content);
        }


        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await Client().DeleteAsync(endpoint);
        }
    }
}
