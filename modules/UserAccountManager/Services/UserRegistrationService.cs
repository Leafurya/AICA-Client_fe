using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using UserAccountManager.Models;


namespace UserAccountManager.Services
{
    public static class UserRegistrationService
    {
        public static async Task<bool> IsIdDuplicateAsync(string userId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"http://서버주소/api/users/check-id?userId={userId}";
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                return result.Contains("true");
            }
        }

        public static async Task<(bool Success, string Message)> RegisterUserAsync(UserRegistrationData data)
        {
            // 나중에 실제 API 엔드포인트로 교체할 것
            string apiUrl = "https://yourapi.com/api/users/register";

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(apiUrl, content);
                    if (!response.IsSuccessStatusCode)
                        return (false, "서버 응답 오류");

                    string result = await response.Content.ReadAsStringAsync();
                    return (true, "회원가입이 완료되었습니다.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"에러: {ex.Message}");
            }
        }
    }




}
