using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DB.SQLserver
{
    public class ReflectionTest
    {
        public ReflectionTest()
        { 
            Console.WriteLine($"{this.GetType()}的无参数构造函数");
        }
        public ReflectionTest(string name)
        {
            Console.WriteLine($"{this.GetType()}的有参数构造函数,参数为{name}");
        }
        public void Show1() {
            Console.WriteLine($"{this.GetType()}的{MethodBase.GetCurrentMethod().Name}方法");
        }
        public void Show2(int i)
        {
            Console.WriteLine($"{this.GetType()}的{MethodBase.GetCurrentMethod().Name}方法");
        }
        public void Show3()
        {
            Console.WriteLine($"{this.GetType()}的{MethodBase.GetCurrentMethod().Name}方法");
        }
        public void Show3(string str)
        { 
            Console.WriteLine($"{this.GetType()}的{MethodBase.GetCurrentMethod().Name}方法");
        }

        public void Show3(string str, int i)
        {
            Console.WriteLine($"{this.GetType()}的{MethodBase.GetCurrentMethod().Name}方法");
        }

        public void Show3(int i, string str)
        {
            Console.WriteLine($"{this.GetType()}的{MethodBase.GetCurrentMethod().Name}方法");
        }

        private void Show4()
        {
            Console.WriteLine($"{this.GetType()}的{MethodBase.GetCurrentMethod().Name}方法");
        }
    }
}
