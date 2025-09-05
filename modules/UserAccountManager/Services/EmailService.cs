using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Utility.RequestConst;

namespace UserAccountManager.Services
{
    public class EmailService
    {
        private static readonly HttpClient _httpClient = RequestConst.client;
        private static string host=RequestConst.host;

        private class AuthResponse
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        private class VerifyResponse
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        public static async Task<(bool Success, string Message)> RequestAuthCodeFromServerAsync(string email)
        {
            var body = new { email= email };
            Debug.WriteLine(JsonSerializer.Serialize(body));
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{host}/api/auth/email/request", content);
                Debug.WriteLine("RequestAuthCodeFromServerAsync " + response.IsSuccessStatusCode);
                if (!response.IsSuccessStatusCode)
                    return (false, "서버 오류");


                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AuthResponse>(json);

                return (true, "인증번호가 발송되었습니다.");
            }
            catch (Exception ex)
            {
                return (false, $"에러: {ex.Message}");
            }
        }

        public static async Task<(bool Success, string Message)> VerifyCodeWithServerAsync(string email, string inputCode)
        {
            var body = new { email= email, code = inputCode };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{host}/api/auth/email/verify", content);
                if (!response.IsSuccessStatusCode)
                    return (false, "서버 인증 실패");

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<VerifyResponse>(json);

                return (response.IsSuccessStatusCode, result.message);
            }
            catch (Exception ex)
            {
                return (false, $"에러: {ex.Message}");
            }
        }

    }
}
