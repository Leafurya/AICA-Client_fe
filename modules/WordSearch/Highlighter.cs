using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.DataBase;

namespace WordSearch
{
    using RecordList = List<object[]>;
    class Highlighter
    {
    }
    class DBTester :  DBManager
    {
        public void GetText()
        {
            Connect();
            RecordList result =new RecordList();
            string[] columns = { "textid", "text" };
            result=ExecuteQuery("SELECT * FROM texts", columns);

            result.ForEach(item =>
            {
                Debug.WriteLine($"{item[0]}, {item[1]}");
            });
            //foreach (RecordDict row in result)
            //{
            //    //Debug.WriteLine($"{row["textid"]}, {row["text"]}");
            //}

            Disconnect();
        }
    }
}
