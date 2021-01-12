using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreModel.Models;
namespace StoreModel.Data
{
    public class DbInitializer
    {
        public static void Initialize(StoreContext context)
        {
            context.Database.EnsureCreated();
            if (context.Smartphones.Any())
            {
                return; // BD a fost creata anterior
            }
            var smartphones = new Smartphone[]
            {
new Smartphone{Manufacturer="Samsung",Model="Galaxy S20",Price=Decimal.Parse("3499")},
new Smartphone{Manufacturer="Xiaomi",Model="Mi 10 Pro",Price=Decimal.Parse("2599")},
new Smartphone{Manufacturer="Huawei",Model="Nova 5T",Price=Decimal.Parse("1299")}
            };
            foreach (Smartphone s in smartphones)
            {
                context.Smartphones.Add(s);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {
new Customer{CustomerID=1050,Name="Barbu Marin",Adress=("Bulevardul Eroilor nr.24, Cluj-Napoca") },
new Customer{CustomerID=1045,Name="Pop Cornel",Adress=("Bulevardul Muncii nr.16, Cluj-Napioca")},
            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
new Order{SmartphoneID=1,CustomerID=1050},
new Order{SmartphoneID=3,CustomerID=1045},
new Order{SmartphoneID=1,CustomerID=1045},
new Order{SmartphoneID=2,CustomerID=1050},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();
        }
    }
}