using System;
using System.Collections.Generic;
using System.Text;

namespace 手写IOC.DIYContainer.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class IOCConstructorAttribute:Attribute
    {
    }
}
