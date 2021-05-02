using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace HappyBudget.Models
{
    public class Account : BaseModel
    {


        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }

       
        [OneToMany]
        public List<Transaction> Transactions { get; set; }

        //[ForeignKey(typeof(Currency))]
        //public int CurrencyId { get; set; }

        public Account(string name, decimal balance, string image, string type, string color)
        {
            Name = name;
            Balance = balance;
            Image = image;
            Type = type;
            Color = color;
        }

        public Account()
        {

        }

        public static List<Account> GetDefaultAccounts()
        {
            return new List<Account>
            {
                new Account("Bank", 0, "card.png", "Card", "#DEB887"),
                new Account("Wallet", 0, "cash.png", "Cash", "#191970"),
            };
        }
    }
}
