using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UserAccountManager.Services
{
    public class EmailService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private class AuthResponse
        {
            public string status { get; set; }
            public string code { get; set; }
        }

        private class VerifyResponse
        {
            public bool success { get; set; }
        }

        public static async Task<(bool Success, string Code, string Message)> RequestAuthCodeFromServerAsync(string email)
        {
            var body = new { email };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://yourapi.com/api/auth/send-code", content);
                if (!response.IsSuccessStatusCode)
                    return (false, null, "서버 오류");

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AuthResponse>(json);

                return (true, result.code, "인증번호가 발송되었습니다.");
            }
            catch (Exception ex)
            {
                return (false, null, $"에러: {ex.Message}");
            }
        }

        public static async Task<(bool Success, string Message)> VerifyCodeWithServerAsync(string email, string inputCode)
        {
            var body = new { email, code = inputCode };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://yourapi.com/api/auth/verify-code", content);
                if (!response.IsSuccessStatusCode)
                    return (false, "서버 인증 실패");

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<VerifyResponse>(json);

                return (result.success, result.success ? "인증 완료" : "인증번호 불일치");
            }
            catch (Exception ex)
            {
                return (false, $"에러: {ex.Message}");
            }
        }

    }
}
