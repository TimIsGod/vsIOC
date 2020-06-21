using System;
using System.Collections.Generic;
using System.Text;
using 手写IOC.DemoDI.Interface;
using 手写IOC.DIYContainer.Attributes;

namespace 手写IOC.DemoDI.Entity
{
    /// <summary>
    /// 这个实现类的构造方法有一个三层依赖
    /// </summary>
    public class TestDI05:ITestDI05
    {

        public TestDI05()
        {
       
            Console.WriteLine($"{this.GetType().Name}被构造。。。");
        }
        public void Fun()
        {
            Console.WriteLine("This is ITestDI05 Fun !");
        }
    }
}
