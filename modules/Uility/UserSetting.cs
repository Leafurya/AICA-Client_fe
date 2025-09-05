using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Data.User;
using Utility.DataBase;

namespace Utility
{
    namespace UserSetting{
        public class UserSetting : DBManager
        {
            private UserSettingData data { get; set; }
            public UserSetting(UserSettingData data)
            {
                Connect();
                try
                {
                    string query = "CREATE TABLE setting (rememberMe BOOLEAN DEFAULT 0)";
                    ExecuteNonQuery(query);
                    query = "INSERT INTO setting VALUES (0)";
                    ExecuteNonQuery(query);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                this.data = data;
                try
                {
                    string query = "select * from setting";
                    string[] columns = { "rememberMe" };
                    List<object[]> result = ExecuteQuery(query, columns);
                    this.data.rememberMe = Convert.ToBoolean(result[0][0]);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    this.data.rememberMe = false;
                }
                Disconnect();
            }
            public void UpdateRememberMe(bool val)
            {
                Connect();
                this.data.rememberMe = val;
                try
                {
                    string query = $"UPDATE setting SET rememberMe={val}";
                    ExecuteNonQuery(query);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                Disconnect();
            }
        }
        public class Interface{
            static private UserSetting setting { get; set; }
            static public void Init(UserSettingData data)
            {
                setting=new UserSetting(data);
            }
            static public void UpdateRememberMe(bool val)
            {
                setting.UpdateRememberMe(val);
            }
        }
    }
}
