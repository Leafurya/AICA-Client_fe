using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Utility.RequestConst;
using UserAccountManager.Models;

namespace UserAccountManager.Services
{
    public static class LogoutService
    {
        private static readonly HttpClient client = RequestConst.client;
        private static string host = RequestConst.host;

        public static async Task<(bool Success, string Message)> LogoutAsync()
        {
            var body = new
            {
                accessToken = TokenManager.AccessToken
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

            try
            {
                HttpResponseMessage response = await client.PostAsync($"{host}/api/logout", content);
                string json = await response.Content.ReadAsStringAsync();

                LogoutResponse? result = JsonSerializer.Deserialize<LogoutResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.Code == 200
                    ? (true, result.Message)
                    : (false, result?.Message ?? "로그아웃 실패");
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}");
            }
        }
    }
}
