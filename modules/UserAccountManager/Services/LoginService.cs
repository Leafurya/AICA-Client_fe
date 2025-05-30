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
    public static class LoginService
    {
        public static async Task<(bool Success, string Message)> LoginAsync(string userId, string password)
        {
            var loginInfo = new { userId, password };
            var content = new StringContent(JsonSerializer.Serialize(loginInfo), Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            try
            {
                var response = await client.PostAsync("https://your-api.com/api/login", content);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return (false, "서버 오류 또는 로그인 실패");

                var result = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.Success)
                {
                    // ✅ 로그인 성공 → 토큰 저장
                    TokenManager.SetTokens(result.AccessToken, result.RefreshToken);
                    return (true, "로그인 성공");
                }
                else
                {
                    return (false, "아이디 또는 비밀번호가 틀립니다.");
                }
            }
            catch
            {
                return (false, "서버에 연결할 수 없습니다.");
            }
        }
    }
}
