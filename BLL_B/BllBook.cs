using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using 手写IOC.IBLL;
using 手写IOC.IDAL;
using 手写IOC.Models;

namespace 手写IOC.BLL_B
{
    public class BllBook : IBllBook
    {

        private IDalBook _iDalBook = null;

        public BllBook(IDalBook dalBook)
        {
            this._iDalBook = dalBook;
        }

        public bool Add(ModelBook book)
        {
            Console.WriteLine("BLL_B bllBook Add.");
            return _iDalBook.Add(book);
        }

        public ModelBook FindOne(string name)
        {
            Console.WriteLine("BLL_B bllBook FindOne.");
            return _iDalBook.FindOne(name);
        }
    }
}
