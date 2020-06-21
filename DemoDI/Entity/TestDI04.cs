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
    public class TestDI04:ITestDI04
    {
        /// <summary>
        /// 这里实现属性注入
        /// </summary>
        [IOCPropertyInjection]
        private ITestDI01 PropertyTestDI01 { get; set; }

        private ITestDI03 _iTestDI03 = null;
        public TestDI04(ITestDI03 testDI03)
        {
            this._iTestDI03 = testDI03;
            Console.WriteLine($"{this.GetType().Name}被构造。。。");
        }
        public void Fun()
        {
            Console.WriteLine("This is ITestDI04 Fun !");
        }
    }
}
