using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserAccountManager.Models;

using Utility.RequestConst;

namespace UserAccountManager.Services
{
    public static class UserInfoService
    {
        private static readonly HttpClient client = RequestConst.client;
        private static string host = RequestConst.host;

        public static async Task<(bool Success, string Message, UserInfoData? Data)> GetUserInfoAsync()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

            try
            {
                var response = await client.GetAsync($"{host}/api/user/me");
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<UserInfoResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Code == 200 && result.Data != null)
                    return (true, result.Message, result.Data);

                return (false, result?.Message ?? "회원 정보 요청 실패", null);
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}", null);
            }
        }

        // ✅ 회원정보 수정 진입 전 비밀번호 확인 기능 리팩터링
        public static async Task<(bool Success, string Message)> VerifyPasswordAsync(string password)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new
                {
                    password
                }), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

                var response = await client.PostAsync($"{host}/api/user/verify-password", content);
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<PasswordVerifyResponse>(json);

                return result?.Success == true
                    ? (true, "비밀번호 확인 성공")
                    : (false, "비밀번호가 일치하지 않습니다.");
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}");
            }
        }
    }

}
