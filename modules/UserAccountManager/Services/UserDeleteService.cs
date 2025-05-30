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

namespace UserAccountManager.Services
{
    public static class UserDeleteService
    {
        private static readonly HttpClient client = RequestConst.client;
        private static string host = RequestConst.host;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<(bool Success, string Message)> DeleteUserAsync()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"{host}/api/user");
                string json = await response.Content.ReadAsStringAsync();

                DeleteUserResponse? result = JsonSerializer.Deserialize<DeleteUserResponse>(json, _jsonOptions);

                return result?.Code == 200
                    ? (true, result.Message)
                    : (false, result?.Message ?? "회원 탈퇴 실패");
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}");
            }
        }
    }
}
