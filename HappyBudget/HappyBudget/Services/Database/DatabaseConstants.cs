using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HappyBudget.Services.Database
{
    public static class DatabaseConstants
    {
        public const string DatabaseName = "HappyBudget.db3";
        
        public const SQLite.SQLiteOpenFlags Flags = 
            //The connection can read and write data
            SQLite.SQLiteOpenFlags.ReadWrite | 
            //The connection will automaticaly create the database file if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            //The connection will participate in the shared cache, if it's enabled
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(path, DatabaseName);
            }
        }
    }
}
