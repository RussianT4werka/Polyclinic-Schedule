using System.Net;
using System.Text.Json;
using System.Text;

namespace BlazorView.Tools
{
    public class HttpTool
    {
        static HttpClient client = new HttpClient();

        static string host = "https://localhost:7041/api/";
        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            PropertyNameCaseInsensitive = true
        };

        public static async Task<(HttpStatusCode, string)> SendPostAsync(string controller, string method, object body)
        {
            string url = host + controller;
            if (!string.IsNullOrEmpty(method))
                url += $"/{method}";
            string json = "";
            if (body != null)
                json = JsonSerializer.Serialize(body, body.GetType(), options);
            var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            return (response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        public static T Deserialize<T>(string json)
        {
            T result = JsonSerializer.Deserialize<T>(json, options);
            return result;
        }
    }
}
