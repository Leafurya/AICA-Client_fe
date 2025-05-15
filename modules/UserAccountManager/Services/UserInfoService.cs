using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserAccountManager.Models;

namespace UserAccountManager.Services
{
    public static class UserInfoService
    {
        public static async Task<(bool Success, UserInfo Data, string Message)> GetUserInfoAsync()
        {
            using var client = new HttpClient();

            // ✅ TokenManager에서 AccessToken 직접 가져옴
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

            try
            {
                var response = await client.GetAsync("https://your-api.com/api/user/me");
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return (false, null, "회원 정보 요청 실패");

                var data = JsonSerializer.Deserialize<UserInfo>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return (true, data, "회원 정보 불러오기 성공");
            }
            catch
            {
                return (false, null, "서버에 연결할 수 없습니다.");
            }
        }

        public static async Task<bool> VerifyPasswordAsync(string password)
        {
            try
            {
                using var client = new HttpClient();

                var content = new StringContent(JsonSerializer.Serialize(new
                {
                    password = password
                }), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

                var response = await client.PostAsync("https://your-api.com/api/user/verify-password", content);

                if (!response.IsSuccessStatusCode)
                    return false;

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PasswordVerifyResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result != null && result.Success;
            }
            catch
            {
                return false;
            }
        }
    }

}
