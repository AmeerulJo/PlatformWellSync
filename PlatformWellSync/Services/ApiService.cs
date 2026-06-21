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
                Email = "user@aemenersol.com",
                Password = "Test@123"
            };

            var response =
                await _httpClient.PostAsJsonAsync(
                    "/api/account/login",
                    request);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content
                    .ReadFromJsonAsync<LoginResponse>();

            return result?.Token ?? string.Empty;
        }

        public async Task<List<PlatformDto>> GetPlatformWellActualAsync(
            string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.GetAsync(
                    "/api/platform/GetPlatformWellActual");

            response.EnsureSuccessStatusCode();

            var json =
                await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<PlatformDto>>(
                       json,
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       })
                   ?? new List<PlatformDto>();
        }
    }
}
