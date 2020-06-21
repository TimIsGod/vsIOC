using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace 手写IOC.DIYContainer
{
    public class ThreadCallContext<T>
    {
        /// <summary>
        /// 用容器注册对象key+线程ID作为字典的Key，来保证在同一线程内对象独立
        /// 知识点： ConcurrentDictionary是可以多线程访问进行操作，而Dictionary不能在多线程访问状态下进行赋值操作
        ///          AsyncLocal是保证对象在线程传递中不会丢失值
        /// </summary>
        public static ConcurrentDictionary<string, AsyncLocal<T>> _ConcurrentThreadDic = new ConcurrentDictionary<string, AsyncLocal<T>>();

        public static void Set(string name,T data)
        {
            _ConcurrentThreadDic.GetOrAdd(name, r => new AsyncLocal<T>()).Value = data;
        }

        public static T Get(string name)
        {
            return _ConcurrentThreadDic.TryGetValue(name, out AsyncLocal<T> data) ? data.Value : default(T);
        }
    }
}
