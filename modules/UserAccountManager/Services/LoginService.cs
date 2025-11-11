using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserAccountManager.Models;

using Utility.RequestConst;
using Utility.TokenManager;

namespace UserAccountManager.Services
{
    public static class LoginService
    {
        private static readonly HttpClient client = RequestConst.client;
        private static string host = RequestConst.host;

        public static async Task<(bool Success, string Message)> LoginAsync(string userId, string password,bool rememberMe)
        {
            var loginInfo = new { userId, password, rememberMe };
            var content = new StringContent(JsonSerializer.Serialize(loginInfo), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync($"{host}/api/login", content);
                if ((int)response.StatusCode == 401)
                {
                    (bool suc,string msg)=await TokenManager.EnsureValidTokenAsync();
                    if (suc)
                    {
                        response = await client.PostAsync($"{host}/api/login", content);
                        if ((int)response.StatusCode == 401)
                        {
                            return (false, msg);
                        }
                    }
                    else
                    {
                        return (false, msg);
                    }
                }
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (result?.Code == 200 && result.Data != null)
                {
                    TokenManager.SetTokens(result.Data.AccessToken, result.Data.RefreshToken);
                    return (true, result.Message);
                }

                return (false, result?.Message ?? "로그인 실패");
            }
            catch
            {
                return (false, "서버에 연결할 수 없습니다.");
            }
        }
    }
}
