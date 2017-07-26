using DB.Interface;
using DB.SQLserver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //反射获取信息、创建对象、调用方法
            //实现程序可配置可扩展 
            //反射1();

            //去掉接口,反射调用方法,包括私有方法
            //反射2();

            //反射获取属性和赋值
            //封装数据库访问层 
            反射3();
            Console.ReadKey();
        }


        private static void 反射1()
        {
            Console.WriteLine("~~~~~~~~~~~~正常的new然后执行方法~~~~~~~~~~~~~~~~~~~~~");
            IDBHelper dbHelper = new DBHelper();
            dbHelper.Query();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("~~~~~~~~~~~~反射的一些小方法~~~~~~~~~~~~~~~~~~~~~");

            string namespaces = ConfigurationManager.AppSettings["DB.Interface"];
            string[] nameSpaceArray = namespaces.Split(',');
            Assembly assembly = Assembly.Load(nameSpaceArray[0]);
            foreach (Module module in assembly.GetModules())
            {
                Console.WriteLine($"module名称{module.FullyQualifiedName}");
            }
            foreach (Type type in assembly.GetTypes())
            {
                Console.WriteLine($"type名称{type.FullName}");
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("~~~~~~~~~~~~使用反射执行方法~~~~~~~~~~~~~~~~~~~~");
            Type typeDBHelper = assembly.GetType(nameSpaceArray[1]);//找出完整类型
            object oDBHelper = Activator.CreateInstance(typeDBHelper);
            if (oDBHelper is IDBHelper)
            {
                IDBHelper db = oDBHelper as IDBHelper;
                db.Query();
            }
        }

        private static void 反射2()
        {
            string namespaces = ConfigurationManager.AppSettings["DB.Interface"];
            string[] nameSpaceArray = namespaces.Split(',');
            Assembly assembly = Assembly.Load(nameSpaceArray[0]);
            Type type = assembly.GetType(nameSpaceArray[1]);//找出完整类型 
            object oDBHelper = Activator.CreateInstance(type);
            MethodInfo show1 = type.GetMethod("Show1");
            show1.Invoke(oDBHelper, null);

            MethodInfo show2 = type.GetMethod("Show2");
            show2.Invoke(oDBHelper, new object[] { 1 });

            MethodInfo show31 = type.GetMethod("Show3", new Type[] { });
            show31.Invoke(oDBHelper, new object[] { });

            MethodInfo show32 = type.GetMethod("Show3", new Type[] { typeof(string) });
            show32.Invoke(oDBHelper, new object[] { "1" });

            MethodInfo show33 = type.GetMethod("Show3", new Type[] { typeof(string), typeof(int) });
            show33.Invoke(oDBHelper, new object[] { "dd", 11 });

            MethodInfo show4 = type.GetMethod("Show4", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            show4.Invoke(oDBHelper, new object[] { });
        }


        private static void 反射3()
        { 
            People p = new People() { id = 1, name = "张三" };
            Console.WriteLine($"一般方法获取数据:{p.name}的学号是{p.id}");


            Console.WriteLine("~~~~~~~~~~~~~~封装数据库访问层~~~~~~~~~~~~~~~~");
            People ppp = GetOnly<People>(); 
            Console.WriteLine($"通过反射方法获取对象:id值是{ppp.id},姓名值是{ppp.name},性别是{ppp.sex}");

        }
        //封装数据库访问层
        private static T GetOnly<T>()
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);

            //将属性横向输出,可以用于拼接sql语句用
            Console.WriteLine("将属性横向输出:" + string.Join(",", type.GetProperties().Select(p => p.Name)));

            //假装查询数据库,并返回了其中的一条数据的datatable
            List<People> list = new List<People>();
            list.Add(new People() { id = 1, sex = "男", name = "格罗姆·地狱咆哮" });
            list.Add(new People() { id = 2, sex = "男", name = "阿尔萨斯·米奈希尔" });
            list.Add(new People() { id = 3, sex = "男", name = "沃金" });
            list.Add(new People() { id = 4, sex = "男", name = "陈·风暴烈酒" });
            list.Add(new People() { id = 5, sex = "男", name = "达利乌斯·克罗雷领主" });
            list.Add(new People() { id = 6, sex = "女", name = "吉安娜·普罗德摩尔" });
            list.Add(new People() { id = 7, sex = "男", name = "大领主达里安·莫格莱尼" });
            list.Add(new People() { id = 8, sex = "男", name = "布莱恩·铜须" });
            list.Add(new People() { id = 9, sex = "男", name = "萨尔" });
            People people = list.First(p => p.id == 6); 
            List<People> listResult = new List<People>();
            listResult.Add(people); 
            DataTable dt = Tools.ToDataTable<People>(listResult); 

            foreach (var prop in type.GetProperties())
            {
                prop.SetValue(t, dt.Rows[0][prop.Name]);
                Console.WriteLine($"属性名是{prop.Name},值是{prop.GetValue(t)}");
            }
            return t;
        }


    }

    public static class Tools 
    { 
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {

            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    

            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列    
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }
    }


    public class People
    {
        public People()
        {
            Console.WriteLine("Pople的无参构造函数");
        }

        public int id { get; set; }
        public string name { get; set; }

        public string sex { get; set; }


    }
}
