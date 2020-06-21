using System;
using System.Collections.Generic;
using System.Text;
using 手写IOC.DemoDI.Interface;

namespace 手写IOC.DemoDI.Entity
{
    /// <summary>
    /// 这个实现类没有构造依赖
    /// </summary>
    public class TestDI01 : ITestDI01
    {
        public int MyProperty { get; set; }

        public TestDI01()
        {
            Console.WriteLine($"{this.GetType().Name}被构造。。。");
        }
        public void Fun()
        {
            Console.WriteLine("This is ITestDI01 Fun !");
        }
    }
}
