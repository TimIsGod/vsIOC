using System;
using System.Collections.Generic;
using System.Text;
using 手写IOC.IDAL;
using 手写IOC.Models;

namespace 手写IOC.DAL
{
    public class DalPGBook : IDalBook
    {
        public bool Add(ModelBook book)
        {
            Console.WriteLine("Added a book use dalpg.");
            return true;
        }

        public ModelBook FindOne(string name)
        {
            Console.WriteLine("Find one book use dalpg");

            ModelBook book = new ModelBook();
            book.Name = $"{this.GetType().Name}大全";
            book.Author = "zhangsan";
            book.Price = 9.9M;
            book.CreatedTime = DateTime.Now;

            return book;
        }
    }
}
