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
    public static class UserInfoUpdateService
    {
        private static readonly HttpClient client = RequestConst.client;
        private static string host = RequestConst.host;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public static async Task<(bool Success, string Message)> UpdateUserInfoAsync(string? nickname, string? email,string? pwd,string? rePwd)
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                nickname = null;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                email = null;
            }
            if (string.IsNullOrWhiteSpace(pwd))
            {
                pwd = null;
            }
            if (string.IsNullOrWhiteSpace(rePwd))
            {
                rePwd = null;
            }
            var body = new { newNickname=nickname, newEmail= email,newPassword=pwd,confirmNewPassword=rePwd };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            Debug.WriteLine("content " + JsonSerializer.Serialize(body));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());

            try
            {
                (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("patch", $"{host}/api/member", content);
                if (response == null)
                {
                    return (false, "액세스토큰 재발급 실패");
                }
                string json = await response.Content.ReadAsStringAsync();

                UserInfoUpdateResponse? result = JsonSerializer.Deserialize<UserInfoUpdateResponse>(json, _jsonOptions);

                return result?.Code == 200
                    ? (true, result.Message)
                    : (false, result?.Message ?? "회원정보 변경 실패");
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}");
            }
        }
    }
}
