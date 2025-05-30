using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserAccountManager.Models;

using Utility.RequestConst;

namespace UserAccountManager.Services
{
    public static class TokenManager
    {
        private static readonly HttpClient client = RequestConst.client;
        private static string host = RequestConst.host;
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
        }

        public static async Task<(bool Success, string Message)> RefreshTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(RefreshToken))
                return (false, "리프레시 토큰이 없습니다.");

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new
                {
                    refreshToken = RefreshToken
                }), Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{host}/api/auth/refresh", content);
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Code == 201 && result.Data != null)
                {
                    AccessToken = result.Data.AccessToken;
                    RefreshToken = result.Data.RefreshToken;
                    return (true, result.Message);
                }

                return (false, result?.Message ?? "토큰 갱신 실패");
            }
            catch (Exception ex)
            {
                return (false, $"에러: {ex.Message}");
            }
        }

        public static async Task<(bool Success, string Message)> EnsureValidTokenAsync()
        {
            if (IsTokenValid())
                return (true, "유효한 토큰");

            return await RefreshTokenAsync();
        }

    }
}
