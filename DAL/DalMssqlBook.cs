using System;
using System.Collections.Generic;
using System.Text;
using 手写IOC.IDAL;
using 手写IOC.Models;

namespace 手写IOC.DAL
{
    public class DalMssqlBook : IDalBook
    {
        public bool Add(ModelBook book)
        {
            Console.WriteLine("Added a book use dalmssql.");
            return true;
        }

        public ModelBook FindOne(string name)
        {
            Console.WriteLine("Find one book use dalmssql");

            ModelBook book = new ModelBook();
            book.Name = $"{this.GetType().Name}大全";
            book.Author = "lisi";
            book.Price = 10.9M;
            book.CreatedTime = DateTime.Now;

            return book;
        }
    }
}
