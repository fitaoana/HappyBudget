using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyBudget.Models
{
    public class Transaction : BaseModel
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }

        [Ignore]
        public string Account { get; set; }

        [Ignore]
        public string Category { get; set; }

        [ForeignKey(typeof(Account))]
        public int AccountId { get; set; }

        [ForeignKey(typeof(Category))]
        public int CategoryId { get; set; }
    }
}
