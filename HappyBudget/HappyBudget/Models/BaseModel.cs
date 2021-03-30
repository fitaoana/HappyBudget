using HappyBudget.Services.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyBudget.Models
{
    public abstract class BaseModel : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
