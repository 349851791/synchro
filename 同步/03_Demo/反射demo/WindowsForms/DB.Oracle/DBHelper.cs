using DB.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Oracle
{
    public class DBHelper : IDBHelper
    {
        public DBHelper()
        {
            Console.WriteLine("DBHelper的无参数构造函数");
        }
        public void Query()
        {
            Console.WriteLine("这里是{0}的Query.", this.GetType().FullName);
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }


        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
