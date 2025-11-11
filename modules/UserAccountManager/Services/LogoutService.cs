using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Utility.RequestConst;
using UserAccountManager.Models;
using Utility.TokenManager;
using System.Diagnostics;

namespace UserAccountManager.Services
{
    public static class LogoutService
    {
        private static readonly HttpClient client = RequestConst.client;
        private static string host = RequestConst.host;

        public static async Task<(bool Success, string Message)> LogoutAsync()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());

            try
            {
                (bool suc, HttpResponseMessage? response)=await TokenManager.RequestWithTokenCheck("post",$"{host}/api/logout", null);
                if (response == null)
                {
                    return (false, "액세스토큰 재발급 실패");
                }
                string json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("logout response body " + json);
                LogoutResponse? result = JsonSerializer.Deserialize<LogoutResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.Code == 200
                    ? (true, result.Message)
                    : (false, result?.Message ?? "로그아웃 실패");
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}");
            }
        }
    }
}
