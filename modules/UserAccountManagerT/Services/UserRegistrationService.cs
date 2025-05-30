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
        static private HttpClient client=RequestConst.client;
        static private string host=RequestConst.host;
        public static async Task<bool> IsIdDuplicateAsync(string userId)
        {
            string url = $"{host}/api/users/check-id?userId={userId}";
            HttpResponseMessage response = await client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();

            return result.Contains("true");
        }

        public static async Task<(bool Success, string Message)> RegisterUserAsync(UserRegistrationData data)
        {
            // 나중에 실제 API 엔드포인트로 교체할 것
            string apiUrl = $"{host}/api/users/register";

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(apiUrl, content);
                if (!response.IsSuccessStatusCode)
                    return (false, "서버 응답 오류");

                string result = await response.Content.ReadAsStringAsync();
                return (true, "회원가입이 완료되었습니다.");
            }
            catch (Exception ex)
            {
                return (false, $"에러: {ex.Message}");
            }
        }
    }
}
