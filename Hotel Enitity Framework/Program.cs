using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Enitity_Framework
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var db = new HotelContext())
            {
                var allehoteller = from h in db.Hotel
                                   select h;
                
                foreach(var item in allehoteller)
                {
                    Console.WriteLine(item.Hotel_No+" "+item.Name+" "+item.Address);
                }

                Console.WriteLine();

                var allekunder = from g in db.Guest
                                 select g;

                foreach(var item in allekunder)
                {
                    Console.WriteLine(item.Guest_No+"   "+item.Name+" "+item.Address+"  "+item.Booking);
                }

                Console.ReadLine();
            }
        }
    }
}
