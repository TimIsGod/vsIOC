using System;
using System.Collections.Generic;
using System.Text;
using 手写IOC.Models;

namespace 手写IOC.IDAL
{
    public interface IDalBook
    {
        ModelBook FindOne(string name);

        bool Add(ModelBook book);
    }
}
