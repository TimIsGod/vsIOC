using System;
using System.Collections.Generic;
using System.Text;
using 手写IOC.DemoDI.Interface;
using 手写IOC.DIYContainer.Attributes;

namespace 手写IOC.DemoDI.Entity
{
    /// <summary>
    /// 这个实现类有两个依赖，其中一个是双层依赖
    /// </summary>
    public class TestDI03:ITestDI03
    {
        private ITestDI01 _iTestDI01 = null;
        private ITestDI02 _iTestDI02 = null;
        //指定构造函数，不能使用默认最多参数构造器规则
        [IOCConstructor]
        public TestDI03(ITestDI01 testDI01, ITestDI02 testDI02)
        {
            this._iTestDI01 = testDI01;
            this._iTestDI02 = testDI02;
            Console.WriteLine($"{this.GetType().Name}被构造。。。");
        }
        //这里的值类型参数，在ioc容器中注册，实现起来会有障碍，需要在实例化时候必须指定值
        public TestDI03(ITestDI01 testDI01, ITestDI02 testDI02,int i )
        {
            this._iTestDI01 = testDI01;
            this._iTestDI02 = testDI02;
            Console.WriteLine($"{this.GetType().Name}({i})被构造。。。");
        }

        public void Fun()
        {
            Console.WriteLine("This is ITestDI03 Fun !");
        }
    }
}
