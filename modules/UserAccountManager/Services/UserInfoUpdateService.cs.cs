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
    public static class UserInfoUpdateService
    {
        private static readonly HttpClient client = RequestConst.client;
        private static string host = RequestConst.host;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };




        public static async Task<(bool Success, string Message)> UpdatePasswordAsync(
            string currentPassword,
            string newPassword,
            string confirmNewPassword)
        {
            if (newPassword != confirmNewPassword)
                return (false, "새 비밀번호가 일치하지 않습니다.");

            var body = new
            {
                currentPassword,
                newPassword,
                confirmNewPassword
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

            try
            {
                HttpResponseMessage response = await client.PutAsync($"{host}/api/user/password", content);
                string json = await response.Content.ReadAsStringAsync();

                UserInfoUpdateResponse? result = JsonSerializer.Deserialize<UserInfoUpdateResponse>(json, _jsonOptions);

                return result?.Code == 200
                    ? (true, result.Message)
                    : (false, result?.Message ?? "비밀번호 변경 실패");
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}");
            }
        }

        

        public static async Task<(bool Success, string Message)> UpdateEmailAsync(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                return (false, "이메일을 입력해주세요.");

            var body = new { newEmail };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

            try
            {
                HttpResponseMessage response = await client.PutAsync($"{host}/api/user/email", content);
                string json = await response.Content.ReadAsStringAsync();

                UserInfoUpdateResponse? result = JsonSerializer.Deserialize<UserInfoUpdateResponse>(json, _jsonOptions);

                return result?.Code == 200
                    ? (true, result.Message)
                    : (false, result?.Message ?? "이메일 변경 실패");
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}");
            }
        }


        public static async Task<(bool Success, string Message)> UpdateNicknameAsync(string newNickname)
        {
            if (string.IsNullOrWhiteSpace(newNickname))
                return (false, "닉네임을 입력해주세요.");

            var body = new { nickname = newNickname };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

            try
            {
                HttpResponseMessage response = await client.PutAsync($"{host}/api/user/nickname", content);
                string json = await response.Content.ReadAsStringAsync();

                UserInfoUpdateResponse? result = JsonSerializer.Deserialize<UserInfoUpdateResponse>(json, _jsonOptions);

                return result?.Code == 200
                    ? (true, result.Message)
                    : (false, result?.Message ?? "닉네임 변경 실패");
            }
            catch (Exception ex)
            {
                return (false, $"서버 오류: {ex.Message}");
            }
        }

        public static async Task<(bool Success, string Message)> UpdateUserInfoAsync(string nickname, string email)
        {
            var body = new { nickname, email };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.AccessToken);

            try
            {
                HttpResponseMessage response = await client.PutAsync($"{host}/api/user/info", content);
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
