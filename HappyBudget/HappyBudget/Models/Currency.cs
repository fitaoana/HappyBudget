using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyBudget.Models
{
    public class Currency /*: BaseModel*/
    { 
        public string Name { get; set; }
        public string Code { get; set; }

        //[OneToMany]
        //public List<Account> Accounts { get; set; }

        
    }
}
