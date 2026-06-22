using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PlatformWellSync.Models;

namespace PlatformWellSync.Services
{
    internal class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync()
        {
            var request = new LoginRequest
            {
                Username = "user@aemenersol.com",
                Password = "Test@123"
            };

            var response =
                await _httpClient.PostAsJsonAsync(
                    "/api/account/login",
                    request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Login failed with {(int)response.StatusCode} {response.ReasonPhrase}: {body}");
            }

            var json = await response.Content.ReadAsStringAsync();

            // The API may return either a bare JSON string ("<token>") or an
            // object with a Token property ({ "token": "<token>" }). Handle
            // both forms to avoid JsonException when the response is a string.
            try
            {
                var trimmed = json?.TrimStart();
                if (!string.IsNullOrEmpty(trimmed) && trimmed.StartsWith("\""))
                {
                    // Response is a JSON string containing the token
                    var token = JsonSerializer.Deserialize<string>(json);
                    return token ?? string.Empty;
                }

                var result = JsonSerializer.Deserialize<LoginResponse>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result?.Token ?? string.Empty;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Failed to deserialize login response. Raw response: {json}", ex);
            }
        }

        public async Task<List<PlatformDto>> GetPlatformWellActualAsync(
            string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("GetPlatformWellActualAsync: token is null or empty.");
                throw new InvalidOperationException("Missing auth token from LoginAsync.");
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.GetAsync(
                    "/api/PlatformWell/GetPlatformWellActual");

            var uri = response.RequestMessage?.RequestUri?.ToString() ?? "<unknown>";
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"REQUEST: GET {uri}");
            Console.WriteLine($"STATUS: {(int)response.StatusCode} {response.ReasonPhrase}");
            Console.WriteLine("RESPONSE BODY:");
            Console.WriteLine(body);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"GET {uri} failed with {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

            return JsonSerializer.Deserialize<List<PlatformDto>>(
                       body,
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       })
                   ?? new List<PlatformDto>();
        }
    }
}
