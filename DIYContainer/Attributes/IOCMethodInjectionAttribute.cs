using System;
using System.Collections.Generic;
using System.Text;

namespace 手写IOC.DIYContainer.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IOCMethodInjectionAttribute:Attribute
    {
    }
}
