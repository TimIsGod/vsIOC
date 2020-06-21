using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using 手写IOC.DIYContainer.Attributes;
using 手写IOC.IDAL;

namespace 手写IOC.DIYContainer
{
    public class Container
    {
        //只能支持瞬时对象
        //private Dictionary<string, Type> _ContainerDic = new Dictionary<string, Type>();
        //支持多种生命周期
        private Dictionary<string, ContainerObject> _ContainerDic = new Dictionary<string, ContainerObject>();
        //存放作用域单例对象
        private Dictionary<string, object> _ScopeInstanceDic = new Dictionary<string, object>();


        #region 构造方法

        public Container() { }

        public Container CreateScopeContainer()
        {
            //这里生成新的域容器对象，通过这个对象获得注册实例才会在不同域中
            //参数1 要传入父容器的字典，以便不论在哪个域中都可以从一个字典中得到注册的实例化类型
            //参数2 因为是新的域容器，所以域内的单例对象要再域外绝缘，所以给一个新的对象
            return new Container(this._ContainerDic, new Dictionary<string, object>());
        }

        private Container(Dictionary<string, ContainerObject> containerDic, Dictionary<string, object> scopeInstanceDic) {
            /**
             *如果是 CreateScopeContainer调用的本方法，这里的this指的新的域容器(子容器)
             *知识点：一个对象的建立并不是在调用构造函数之后
             *        一个对象的建立是在New的时候就已经分配了内存
             *        而构造函数只是对这个对象初始化
             */
            this._ContainerDic = containerDic;
            this._ScopeInstanceDic = scopeInstanceDic;
        }

        #endregion 构造方法

        #region 注册方法
        /// <summary>
        /// 依赖注册
        /// 要声明泛型约束
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        public void RegisterTransient<TInterface, TEntity>() where TEntity :TInterface
        {
            Register<TInterface, TEntity>(LifetimeType.Transient);
        }
        public void RegisterSingleton<TInterface, TEntity>() where TEntity : TInterface
        {
            Register<TInterface, TEntity>(LifetimeType.Singleton);
        }
        public void RegisterScope<TInterface, TEntity>() where TEntity : TInterface
        {

            Register<TInterface, TEntity>(LifetimeType.Scope);
        }

        public void RegisterThread<TInterface, TEntity>() where TEntity : TInterface
        {

            Register<TInterface, TEntity>(LifetimeType.PerThread);
        }
        private void Register<TInterface, TEntity>(LifetimeType lifetimeType) where TEntity : TInterface
        {
            ContainerObject @object = new ContainerObject();
            @object.Lifetime = lifetimeType;
            @object.ObjectType = typeof(TEntity);
            this._ContainerDic.Add(typeof(TInterface).FullName, @object);
        }

        #endregion 注册方法

        /// <summary>
        /// 根据注册依赖返回注册实体类对象
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public TInterface Resolve<TInterface>() {
            string key = typeof(TInterface).FullName;
            var @object = this._ContainerDic.GetValueOrDefault(key);
            Type t = @object.ObjectType;


            #region 生命周期
            switch (@object.Lifetime)
            {
                case LifetimeType.Transient:
                    break;
                case LifetimeType.Singleton:
                    if (@object.ObjectInstance != null)
                    {
                        return (TInterface) @object.ObjectInstance;
                    }
                    break;
                case LifetimeType.Scope:
                    //注意这里的this是域内(子容器)
                    if (this._ScopeInstanceDic.ContainsKey(key))
                    {
                        return (TInterface)this._ScopeInstanceDic.GetValueOrDefault(key) ;
                    }
                    break;
                case LifetimeType.PerThread:
                    object tmpObjcet = ThreadCallContext<object>.Get($"{key}{Thread.CurrentThread.ManagedThreadId}");
                    if (tmpObjcet != null)
                    {
                        return (TInterface)tmpObjcet;
                    }
                    break;
                default:
                    break;
            }
            #endregion 生命周期
            /*
             第三进阶 可以递归解决构造函数参数的嵌套依赖
             */
            {
                return (TInterface)this.GetResolveObject(typeof(TInterface));
            }

            /*
             第二进阶 有参构造函数
             缺点，如果构造函数依赖的参数对象在构造时，也需要参数，无法处理嵌套依赖
                   无法适用于多个参数的构造函数
             */
            {
                ////获取该类的所有构造函数,这里我们假设第一个是需要参数的
                //var constructors = t.GetConstructors()[0];
                ////获取所有参数类型
                //List<object> paramsArray = new List<object>();
                //foreach (var p in constructors.GetParameters())
                //{
                //    Type paramType = p.ParameterType;
                //    string paramKey = paramType.FullName;
                //    //找注册过的参数类的实体类型
                //    Type paramTargetType = this._ContainerDic.GetValueOrDefault(paramKey);
                //    paramsArray.Add(Activator.CreateInstance(paramTargetType));
                //}
                ////这里如果构造函数是无参的，也不会影响，array为空
                //object obj = Activator.CreateInstance(t,paramsArray.ToArray());
                //return (TInterface)obj;
            }


            /*
             第一阶，无参数构造函数
             缺点，无法满足有参构造函数
             */
            {
                //object obj = Activator.CreateInstance(t);
                //return (TInterface)obj;
            }

            
        }

        private object GetResolveObject(Type t)
        {
            string key = t.FullName;
            var @object = this._ContainerDic.GetValueOrDefault(key);
            Type type = @object.ObjectType;
            //Type type = this._ContainerDic.GetValueOrDefault(key);
            //选择适用的构造函数
            
            //方案2 特性指定
            var constructor = type.GetConstructors().FirstOrDefault(r => r.IsDefined(typeof(IOCConstructorAttribute), true));
            if (constructor == null)
            {
                //方案1 我们选用参数个数最多的构造函数。 .NetCore的IOC容器是选择超集构造函数
                 constructor = type.GetConstructors().OrderByDescending(r => r.GetParameters().Length).FirstOrDefault();
            }

            //获取所有参数类型
            List<object> paramsArray = new List<object>();
            foreach (var p in constructor.GetParameters())
            {
                Type paramType = p.ParameterType;
                object paramInstance = this.GetResolveObject(paramType);
                paramsArray.Add(paramInstance);
            }
            //这里如果构造函数是无参的，也不会影响，array为空
            object obj = Activator.CreateInstance(type, paramsArray.ToArray());


            //属性注入
            foreach (var item in type.GetProperties().Where(r=>r.IsDefined(typeof(IOCPropertyInjectionAttribute),true)))
            {
                Type propType = item.PropertyType;
                var propInstance = this.GetResolveObject(propType);
                item.SetValue(obj, propInstance);
            }


            //方法注入
            foreach (var item in type.GetMethods().Where(r=>r.IsDefined(typeof(IOCMethodInjectionAttribute),true)))
            {
                List<object> paramsList = new List<object>();
                foreach (var p in item.GetParameters())
                {
                    Type paramType = p.ParameterType;
                    var paramInstance = this.GetResolveObject(paramType);
                    paramsList.Add(paramInstance);

                }
                item.Invoke(obj, paramsList.ToArray());
            }


            switch (@object.Lifetime)
            {
                case LifetimeType.Transient:
                    break;
                case LifetimeType.Singleton:
                    @object.ObjectInstance = obj;
                    break;
                case LifetimeType.Scope:
                    this._ScopeInstanceDic[key] = obj;
                    break;
                case LifetimeType.PerThread:
                    ThreadCallContext<object>.Set($"{key}{Thread.CurrentThread.ManagedThreadId}",obj);
                    break;
                default:
                    break;
            }

            return obj;
        }
    }
}
