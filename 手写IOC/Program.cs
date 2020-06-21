using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using 手写IOC.BLL_B;
using 手写IOC.DAL;
using 手写IOC.DemoDI.Entity;
using 手写IOC.DemoDI.Interface;
using 手写IOC.DIYContainer;
using 手写IOC.IBLL;
using 手写IOC.IDAL;

namespace 手写IOC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //最基础方式
            //ContainerRegisterNormal();

            //ContainerRegisterScope();

            ContainerRegisterThread();


            Console.Read();
        }

        private static void ContainerRegisterNormal()
        {
            /*
                所有需要自动实例化的类都必须要注册
                .NetCore 自带的IOC容器是选择超集的构造函数。超集就是涵盖所有构造函数重载的构造函数，如果有不包含的构造函数存在，会报错
                对构造函数需要传值类型参数的类，不友好，需要在注册时给定固定值，不然嵌套依赖处理不了
                */
            Container container = new Container();
            container.RegisterTransient<IDalBook, DalMssqlBook>();
            //注意上面的引用，这里用的BLL_B的BllBook
            container.RegisterTransient<IBllBook, BllBook>();
            //注册嵌套实现使用到的所有依赖
            container.RegisterTransient<ITestDI01, TestDI01>();
            container.RegisterTransient<ITestDI02, TestDI02>();//这里包含了方法注入
            container.RegisterTransient<ITestDI03, TestDI03>();//这里包含了构造方法注入
            container.RegisterTransient<ITestDI04, TestDI04>();//这里包含了属性注入
            container.RegisterTransient<ITestDI05, TestDI05>();//用于方法注入的参数

            IBllBook book = container.Resolve<IBllBook>();
            ITestDI04 t = container.Resolve<ITestDI04>();
        }

        private static void ContainerRegisterScope()
        {
            //Http请求---Asp.NetCore内置Kestrel，初始化一个容器实例；然后每次来一个Http请求，就clone一个，或者叫创建子容器(包含注册关系)，然后一个请求就一个新作用域(容器)实例，那么就可以做到作用域单例了(其实就是子容器单例)
            //下面模拟从同一个作用域容器里进行实例化，注意这里并没有真正实现在同一个Http请求下的处理，真正的处理需要进行请求的区分，然后同一个请求下的实例化要使用同一个子容器对象。
            //如果想真正实现区分http请求的域范围容器，要参考一下.NetCore的IOC实现方法。

            Container container = new Container();
            container.RegisterScope<ITestDI01, TestDI01>();

            Container scopeContainer = container.CreateScopeContainer();

            ITestDI01 t01 = scopeContainer.Resolve<ITestDI01>();
            ITestDI01 t02 = scopeContainer.Resolve<ITestDI01>();

            Console.WriteLine(object.ReferenceEquals(t01, t02));
        }

        private static void ContainerRegisterThread()
        {
            Container container = new Container();
            container.RegisterThread<ITestDI01, TestDI01>();
            ITestDI01 t01 = null;
            ITestDI01 t02 = null;
            ITestDI01 t03 = null;
            ITestDI01 t04 = null;
            ITestDI01 t05 = container.Resolve<ITestDI01>();
            ITestDI01 t06 = container.Resolve<ITestDI01>();

            List<Task> tasks = new List<Task>();
            tasks.Add(
            Task.Run(() =>
            {
                Console.WriteLine($"This is {Thread.CurrentThread.ManagedThreadId} 1");
                t01 = container.Resolve<ITestDI01>();
                t02 = container.Resolve<ITestDI01>();
            }).ContinueWith((t) =>
            {
                Console.WriteLine($"This is {Thread.CurrentThread.ManagedThreadId} 2");
                t03 = container.Resolve<ITestDI01>();
            })
            );
            tasks.Add(
            Task.Run(() =>
            {
                Console.WriteLine($"This is {Thread.CurrentThread.ManagedThreadId} 3");
                t04 = container.Resolve<ITestDI01>();
            }));

            Thread.Sleep(1000);

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine(object.ReferenceEquals(t01, t02));
            Console.WriteLine(object.ReferenceEquals(t01, t03));
            Console.WriteLine(object.ReferenceEquals(t01, t04));
            Console.WriteLine(object.ReferenceEquals(t01, t05));
            Console.WriteLine(object.ReferenceEquals(t06, t05));
        }
    }
}
