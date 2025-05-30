using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows;
using System.Diagnostics;
using System.Data.Common;

namespace Utility
{
    namespace DataBase
    {
        using RecordList = List<object[]>;
        public class DBManager
        {
            string dataSource = @"Data Source=database.db;";
            SQLiteConnection conn = null;

            protected void Connect()
            {
                conn = new SQLiteConnection(dataSource);
                conn.Open();
            }
            protected void ExecuteNonQuery(string query)
            {
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            protected RecordList ExecuteQuery(string query, string[] columns)
            {
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();

                RecordList result =new RecordList();

                while (reader.Read())
                {
                    object[] dicts = new object[columns.Length];
                    for(int i = 0; i < columns.Length; i++)
                    {
                        dicts[i]= reader[columns[i]];
                        Debug.Write(dicts[i]+" ");
                    }
                    Debug.WriteLine("");
                    result.Add(dicts);
                }

                reader.Close();
                cmd.Dispose();

                return result;
            }
            protected void Disconnect()
            {
                conn.Close();
            }
        }
    }
    
}
