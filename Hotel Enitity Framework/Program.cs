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
                    Console.WriteLine(item.Guest_No+"   "+item.Name+" "+item.Address);
                }


                Console.WriteLine("********************************");

                // List hotelnavn, adresse, samt værelsesinformation(nr, type, pris) om de værelser hotellerne har. 
                var joinAlleVærelser = from r1 in db.Room
                                       join h1 in db.Hotel
                                       on r1.Hotel_No equals h1.Hotel_No
                                       select new { h1.Name, h1.Address, r1.Room_No,r1.Types,r1.Price };

                foreach (var item in joinAlleVærelser)
                {
                    Console.WriteLine(item.Name+ " " + item.Address + " " + item.Room_No + "  " + item.Types+" "+item.Price);
                }


                Console.WriteLine();
                Console.WriteLine("List alle de reservationer hver enkelt værelse har.");

                var reservationerPrVærelse = from b2 in db.Booking
                                             join r2 in db.Room
                                             on b2.Room_No equals r2.Room_No
                                             select new { r2.Room_No, r2.Hotel.Name, b2.Booking_id, guestnanme = b2.Guest.Name, b2.Date_From, b2.Date_To };

                foreach (var item in reservationerPrVærelse)
                {
                    Console.WriteLine(item.Name + " VærNo: " + item.Room_No + " BookingID: " + item.Booking_id + " " + item.guestnanme + " " + item.Date_From.ToShortDateString() + " " + item.Date_To.ToShortDateString());
                }




                //Console.WriteLine("\n Indsæt dig selv som gæst i Guest Tabellen.");

                //var alleGæster = from g in db.Guest
                //                 select g;
                //Guest TilføjMig = new Guest { Name="Anders", Address="Bernstorffsvej 67", Guest_No=666};
                //db.Guest.Add(TilføjMig);
                //db.SaveChanges();

                //foreach(var item in alleGæster)
                //{
                //    Console.WriteLine(item.Guest_No + "   " + item.Name + " " + item.Address);
                //}




                //Console.WriteLine("\n Opret en reservation på hotellet “Lucky Star” på lørdag");

                //var allebookinger = from b in db.Booking
                //                    select b;

                //Booking luckyStar = new Booking { Guest_No = 666, Room_No = 21, Booking_id = 99, Hotel_No = 7, Date_From = new DateTime(2017, 3, 18), Date_To = new DateTime(2017, 3, 19) };
                //db.Booking.Add(luckyStar);
                //db.SaveChanges();

                //foreach (var item in allebookinger)
                //{
                //    Console.WriteLine(item.Booking_id + " " + item.Room_No + " " + item.Guest_No + " " + item.Date_From + " " + item.Date_To);
                //}


                //Console.WriteLine("\n Opdater adressen for Arn , nr 30 i Guest tabellen. Adressen skal Ændes til Elisagaardsvej 5");
                //var update1 = (from g in db.Guest
                //              where g.Guest_No == 30
                //              select g).FirstOrDefault();

                //update1.Address = "Elisagaardsvej 5";
                //db.SaveChanges();

                //Console.WriteLine("\n "+update1.Guest_No+" "+update1.Name+" "+update1.Address);



                //Console.WriteLine("\n Opdater navn for det hotel der hedder Prindsen til at hedde “Roskilde First Hotel”");
                //var update2 = (from h in db.Hotel
                //               where h.Name == "Prindsen"
                //               select h).FirstOrDefault();
                //update2.Name = "Roskilde First Hotel";
                //db.SaveChanges();



                //Console.WriteLine("Slet Bookingen med id: 24");
                //var slet1 = (from b in db.Booking
                //             where b.Booking_id == 24
                //             select b).FirstOrDefault();
                //db.Booking.Remove(slet1);
                //db.SaveChanges();




                //Console.WriteLine("Slet Gæsten med Guestno:30 (slet først bookinger på værelset)");

                //var slet4list = from b in db.Booking
                //                where b.Guest_No == 30
                //                select b;

                //foreach(var item in slet4list)
                //{
                //    db.Booking.Remove(item);
                //}

                //db.SaveChanges();


                //var slet5 = (from g in db.Guest
                //             where g.Guest_No==30
                //             select g).FirstOrDefault();
                //db.Guest.Remove(slet5);
                //db.SaveChanges();


                Console.WriteLine("**************GROUP***************");
                Console.WriteLine("lav et Linq der viser hvor mange bookinger hver gæst har haft, samt summen af prisen på bookingerne");

                var group1 = from b in db.Booking
                             group b by b.Guest_No into newGroup
                             select newGroup;

                foreach (var g1 in group1)
                {
                    Console.WriteLine("Key: "+g1.Key);

                    foreach(var g2 in g1)
                    {
                        Console.WriteLine("GuestNo: "+g2.Guest_No+" BookingID: "+g2.Booking_id);

                    }
                }


                // Rigtig løsning her:


                var group2 = from b in db.Booking
                             group b by b.Guest_No into newGroup
                             select new { keyen = newGroup.Key, numberofbookings = newGroup.Count(), sumafpris = newGroup.Sum(x => x.Room.Price) };

                foreach(var i in group2)
                {
                    Console.WriteLine("Værelsesnr: "+i.keyen+" Antal bookinger: "+i.numberofbookings+" Pris ialt: "+i.sumafpris);
                }







                Console.WriteLine("Join ovenstående liste sammen med gæste listen så du også kan udskrive navnet");

                var group3 = from g2 in group2
                             join g in db.Guest
                             on g2.keyen equals g.Guest_No
                             select new { g2.keyen, g2.numberofbookings, g2.sumafpris, gæstenavn=g.Name };

                foreach (var i in group3)
                {
                    Console.WriteLine("Værelsesnr: " + i.keyen + " Antal bookinger: " + i.numberofbookings + " Pris ialt: " + i.sumafpris+" Navn: "+i.gæstenavn);
                }



                Console.ReadLine();
            }
        }
    }
}
