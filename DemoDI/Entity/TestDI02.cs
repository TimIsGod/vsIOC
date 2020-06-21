using System;
using System.Collections.Generic;
using System.Text;
using 手写IOC.DemoDI.Interface;
using 手写IOC.DIYContainer.Attributes;

namespace 手写IOC.DemoDI.Entity
{
    /// <summary>
    /// 这个构造类有一个单层依赖
    /// </summary>
    public class TestDI02 : ITestDI02
    {
        private ITestDI01 _iTestDI01 = null;

        public TestDI02(ITestDI01 testDI01)
        {
            this._iTestDI01 = testDI01;
            Console.WriteLine($"{this.GetType().Name}被构造。。。");
        }
        public void Fun()
        {
            Console.WriteLine("This is ITestDI02 Fun !");
            
        }
        [IOCMethodInjection]
        public void Fun2(ITestDI05 testDI05)
        {
            Console.WriteLine($"This Fun2 prarms is {testDI05.GetType().Name}");
        }
    }
}
