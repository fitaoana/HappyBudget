using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyBudget.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }


        [OneToMany]
        public List<Transaction> Transactions { get; set; }

        public Category(string name, string image, string color, string type)
        {
            Name = name;
            Image = image;
            Color = color;
            Type = type;
        }
        //ICON PICKER CU IMAGINI CA LA COLORPICKER

        public Category()
        {

        }

        public static List<Category> GetDefaultCategories()
        {
            return new List<Category> ///ADAUGARE IN DB APOI GET IN FUNCTIE DE TYPE 2 LISTE SI IN NEW TRANSACTION IF 
            {
                new Category("Bar/Cafe", "bar_cafe.png", "#00CED1", "Expense"),
                new Category("Groceries", "groceries.png", "#008080", "Expense"),
                new Category("Restaurant", "restaurant.png", "#990000", "Expense"),
                new Category("Clothes/Shoes", "clothes_shoes.png", "#FF99CC", "Expense"),
                new Category("Farmacy", "farmacy.png", "#CC00FF", "Expense"),
                new Category("Electronics", "electronics.png", "#66A3FF", "Expense"),
                new Category("Gifts", "gifts.png", "#0000FF","Expense"),
                new Category("Home/Garden", "home_garden.png", "#66FFE0", "Expense"),
                new Category("Kids", "kids.png", "#FF3399", "Expense"),
                new Category("Pets", "pets.png", "#FFA366", "Expense"),
                new Category("Utilities", "utilities.png", "#00FFCC", "Expense"),
                new Category("Rent", "rent.png", "#003D99", "Expense"),
                new Category("Taxi", "taxi.png", "#CC0066", "Expense"),
                new Category("Public Transport", "public_transport.png", "#87CEEB", "Expense"),
                new Category("Travel", "travel.png", "#993D00", "Expense"),
                new Category("Cosmetics", "cosmetics.png", "#0066FF", "Expense"),
                new Category("Maintenance/Repairs", "maintenance_repairs.png", "#708090", "Expense"),
                new Category("Loans", "loans.png", "#7A0099", "Expense"),
                new Category("Services", "services.png", "#191970", "Expense"),
                new Category("Sport", "sport.png", "#00997A", "Expense"),
                new Category("Fun", "fun.png", "#FF6600", "Expense"),
                new Category("Education", "education.png", "#E066FF", "Expense"),
                new Category("Health", "health.png", "#FF6666", "Expense"),
                new Category("Insurances", "insurances.png", "#DEB887", "Expense"),

                new Category("Salary", "salary.png", "#FF0000", "Income"),
                new Category("Child Support", "child_support.png", "#E066FF", "Income"),
                new Category("Scholarship", "scholarship.png", "#87CEEB", "Income"),
                new Category("Lending/Renting", "lending_renting.png", "#0066FF", "Income"),
            };
        }
    }
}
