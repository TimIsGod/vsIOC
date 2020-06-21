using System;
using System.Collections.Generic;
using System.Text;

namespace 手写IOC.DIYContainer
{
    

    public class ContainerObject
    {

        public Type ObjectType { get; set; }

        public LifetimeType Lifetime { get; set; }

        public object ObjectInstance { get; set; }
    }

    public enum LifetimeType
    {
        Transient,
        Singleton,
        Scope,
        PerThread
    }
}
