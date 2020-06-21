using System;
using System.Collections.Generic;
using System.Text;

namespace 手写IOC.DIYContainer.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IOCPropertyInjectionAttribute:Attribute
    {
    }
}
