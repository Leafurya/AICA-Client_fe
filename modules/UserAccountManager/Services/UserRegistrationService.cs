using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using UserAccountManager.Models;

using Utility.RequestConst;

namespace UserAccountManager.Services
{
    public static class UserRegistrationService
    {
        private static readonly HttpClient client = RequestConst.client;
        private static readonly string host = RequestConst.host;

        public static async Task<bool> IsIdDuplicateAsync(string userId)
        {
            string url = $"{host}/api/users/check-id?userId={userId}";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                return result.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return true; // 네트워크 오류 시 중복된 것으로 처리
            }
        }

        public static async Task<(bool Success, string Message)> RegisterUserAsync(UserRegistrationData data)
        {
            string apiUrl = $"{host}/api/auth/register";

            string json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string resultJson = await response.Content.ReadAsStringAsync();

                RegistrationResponse? result = JsonSerializer.Deserialize<RegistrationResponse>(resultJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Code == 201)
                    return (true, result.Message);
                else
                    return (false, result?.Message ?? "회원가입 실패");

            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}");
            }
        }
    }
}
