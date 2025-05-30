using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserAccountManager.Models;

namespace UserAccountManager.Services
{
    public static class TokenManager
    {
        public static string AccessToken { get; private set; }
        public static string RefreshToken { get; private set; }

        public static void SetTokens(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public static bool IsTokenValid()
        {
            return !string.IsNullOrWhiteSpace(AccessToken);
            // 만약 만료 시각을 도입한다면 여기에 시간 체크 추가
        }

        public static async Task<bool> RefreshTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(RefreshToken))
                return false;

            try
            {
                using var client = new HttpClient();
                var content = new StringContent(JsonSerializer.Serialize(new
                {
                    refreshToken = RefreshToken
                }), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://your-api.com/api/auth/refresh", content);

                if (!response.IsSuccessStatusCode)
                    return false;

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                AccessToken = result.AccessToken;
                RefreshToken = result.RefreshToken;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> EnsureValidTokenAsync()
        {
            if (IsTokenValid())
                return true;

            return await RefreshTokenAsync();
        }
    }

}
