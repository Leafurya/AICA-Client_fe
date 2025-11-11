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
using Utility.TokenManager;
using System.Diagnostics;

namespace UserAccountManager.Services
{
    public static class UserInfoService
    {
        private static readonly HttpClient client = RequestConst.client;
        private static string host = RequestConst.host;

        public static async Task<(bool Success, string Message, UserInfoData? Data)> GetUserInfoAsync()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());

            try
            {
                (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("get", $"{host}/api/member");
                if (response == null)
                {
                    return (false, "액세스토큰 재발급 실패",null);
                }
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<UserInfoData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                if (response.IsSuccessStatusCode)
                    Debug.WriteLine($"{result.id},{result.userId},{result.nickname},{result.email}");
                    return (true, "회원 정보 요청 성공", result);

                return (false, "회원 정보 요청 실패", null);
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}", null);
            }
        }

        public static async Task<(bool Success, string Message)> VerifyPasswordAsync(string password)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new
                {
                    currentPassword=password
                }), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());

                var response = await client.PostAsync($"{host}/api/member/verify-password", content);
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<PasswordVerifyResponse>(json);

                return result.code == 200
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
