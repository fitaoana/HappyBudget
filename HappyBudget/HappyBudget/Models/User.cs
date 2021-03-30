using System;
using System.Collections.Generic;
using System.Text;

namespace HappyBudget.Models
{
    public class User : BaseModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
