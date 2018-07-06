using System;
using System.Collections.Generic;
using System.Data.Common;
using DewCore.Database.MySQL;


namespace RunningApp
{
    class Program
    {
        static void Main(string[] args)
        {

            RootComposer c = new RootComposer();
            var val = c.Select().From("tab").OrderByDesc("name", "surname").ComposedQuery();
            Console.WriteLine(val);
            val = c.Delete("barb").ComposedQuery();
            Console.WriteLine(val);
            val = c.Insert("barb", "col1", "col2").Select("col1", "col2").From("barb1").ComposedQuery();
            Console.WriteLine(val);
            val = c.Insert("barb", "col1", "col2").Compose(new RootComposer().Select("col1", "col2").From("barb1")).ComposedQuery();
            Console.WriteLine(val);
            //MySQLQueryComposer c = new MySQLQueryComposer();
            //var c1 = new MySQLQueryComposer();
            ////c1.Select("id1").Where("a", ">", "b");
            //Console.WriteLine(c.Select("IdBarberShop", "ShopName")
            //                    .From("dev_BarberShop")
            //                    .Join("pippo as P").ON("id", "=", "p.id")
            //                    .Where("IdBarberShop", ">", "@val", 535)
            //                    .AND("Date", ">", DateTime.Now.ToString("dd/MM/yyyy"))
            //                    .OR().Brackets(c1)
            //                    .OrderBy("IdBarberShop").ComposedQuery());

            Console.ReadLine();
        }
    }
}
