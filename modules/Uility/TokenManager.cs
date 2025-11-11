using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using UserAccountManager.Models;

//using Utility.RequestConst;

namespace Utility
{
    namespace TokenManager
    {
        using System.Collections;
        using System.Diagnostics;
        using System.Runtime.Intrinsics.Arm;
        using Utility.DataBase;
        using Utility.RequestConst;
        using static System.Net.Mime.MediaTypeNames;

        public class TokenData
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }

        public class TokenResponse
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public TokenData Data { get; set; }
        }
        public class TokenDB : DBManager
        {
            public void Save(string accessToken,string refreshToken)
            {
                Connect();
                string sql = @"
                    CREATE TABLE IF NOT EXISTS tokens (
                        id INTEGER NOT NULL,
                        access TEXT NOT NULL,
                        refresh TEXT NOT NULL,
                        PRIMARY KEY (id)
                    );";
                ExecuteNonQuery(sql);
                sql = $@"INSERT OR REPLACE INTO tokens VALUES (0,""{accessToken}"", ""{refreshToken}"");";
                ExecuteNonQuery(sql);
                Disconnect();
            }
            public void Remove()
            {
                Connect();
                string sql = @"
                        DROP TABLE tokens;
                    ";
                ExecuteNonQuery(sql);
                Disconnect();
            }
            public (string?,string?) Load()
            {
                Connect();
                try
                {
                    string[] columns = { "access", "refresh" };
                    string sql = @"SELECT * FROM tokens;";
                    List<object[]> result = ExecuteQuery(sql, columns);
                    result.ForEach((item) =>
                    {
                        Debug.WriteLine("access: " + Convert.ToString(item[0]));
                        Debug.WriteLine("refresh: " + Convert.ToString(item[1]));
                    });
                    Disconnect();
                    return (Convert.ToString(result[0][0]), Convert.ToString(result[0][1]));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                Disconnect();

                return (null, null);
            }
        }
        public static class TokenManager
        {
            private static readonly HttpClient client = RequestConst.client;
            private static string host = RequestConst.host;
            private static string AccessToken = "";
            private static string RefreshToken = "";
            delegate Task<HttpResponseMessage> MethodFunc1(string? url,HttpContent? httpContent=null);
            delegate Task<HttpResponseMessage> MethodFunc2(string? url);
            //delegate Task<HttpResponseMessage> MethodFunc(string? url);

            public static void SaveTokensToDB()
            {
                TokenDB db= new TokenDB();
                db.Save(AccessToken, RefreshToken);
            }
            public static void RemoveTokensFromDB()
            {
                TokenDB db = new TokenDB();
                db.Remove();
            }
            public static void LoadTokens()
            {
                TokenDB db = new TokenDB();
                string? access, refresh;
                (access,refresh)=db.Load();
                if (access == null || refresh == null)
                {
                    return;
                }
                SetTokens(access, refresh);
            }
            public static void SetTokens(string accessToken, string refreshToken)
            {
                AccessToken = accessToken;
                RefreshToken = refreshToken;
            }

            public static bool IsTokenValid()
            {
                return !string.IsNullOrWhiteSpace(AccessToken);
            }

            public static async Task<(bool Success, string Message)> RefreshTokenAsync()
            {
                //Debug.WriteLine("request RefreshTokenAsync");
                if (string.IsNullOrWhiteSpace(RefreshToken))
                    return (false, "리프레시 토큰이 없습니다.");

                try
                {
                    var content = new StringContent(JsonSerializer.Serialize(new
                    {
                        refreshToken = RefreshToken
                    }), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{host}/api/reissue", content);
                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result?.Code == 201 && result.Data != null)
                    {
                        AccessToken = result.Data.AccessToken;
                        RefreshToken = result.Data.RefreshToken;
                        return (true, result.Message);
                    }

                    return (false, result?.Message ?? "토큰 갱신 실패");
                }
                catch (Exception ex)
                {
                    return (false, $"에러: {ex.Message}");
                }
            }

            public static async Task<(bool Success, string Message)> EnsureValidTokenAsync()
            {
                if (IsTokenValid())
                    return (true, "유효한 토큰");

                return await RefreshTokenAsync();
            }
            public static string GetAccessToken()
            {
                return AccessToken;
            }

            static public async Task<(bool, HttpResponseMessage?)> RequestWithTokenCheck(string method, string url, HttpContent? content=null)
            {
                MethodFunc1? func1 = null;
                MethodFunc2? func2 = null;
                switch (method)
                {
                    case "post":
                        func1 = client.PostAsync;
                        break;
                    case "get":
                        func2 = client.GetAsync;
                        break;
                    case "patch":
                        func1 = client.PatchAsync;
                        break;
                    case "delete":
                        func2 = client.DeleteAsync;
                        break;
                    case "put":
                        func1 = client.PutAsync;
                        break;
                }
                HttpResponseMessage response;
                if (func1 != null)
                {
                    response = await func1(url, content);
                    //Debug.WriteLine((int)response.StatusCode);
                    if ((int)response.StatusCode == 401)
                    {
                        (bool suc, string msg) = await EnsureValidTokenAsync();
                        if (suc)
                        {
                            response = await func1(url, content);
                            if ((int)response.StatusCode == 401)
                            {
                                return (false, null);
                            }
                        }
                        else
                        {
                            return (false, null);
                        }
                    }
                    return (true, response);
                }
                response = await func2(url);
                //Debug.WriteLine((int)response.StatusCode);
                if ((int)response.StatusCode == 401)
                {
                    (bool suc, string msg) = await EnsureValidTokenAsync();
                    if (suc)
                    {
                        response = await func2(url);
                        if ((int)response.StatusCode == 401)
                        {
                            return (false, null);
                        }
                    }
                    else
                    {
                        return (false, null);
                    }
                }
                return (true, response);

            }
        }
    }
}
