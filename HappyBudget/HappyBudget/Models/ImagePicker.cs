using System;
using System.Collections.Generic;
using System.Text;

namespace HappyBudget.Models
{
    public class ImagePicker
    {
        public string Name { get; set; }

        public ImagePicker(string name)
        {
            Name = name;
        }

        public static List<ImagePicker> GetImages()
        {
            return new List<ImagePicker>
            {
                new ImagePicker("bar_cafe.png"),
                new ImagePicker("child_support.png"),
                new ImagePicker("clothes_shoes.png"),
                new ImagePicker("cosmetics.png"),
                new ImagePicker("education.png"),
                new ImagePicker("electronics.png"),
                new ImagePicker("farmacy.png"),
                new ImagePicker("fun.png"),
                new ImagePicker("gifts.png"),
                new ImagePicker("groceries.png"),
                new ImagePicker("health.png"),
                new ImagePicker("home_garden.png"),
                new ImagePicker("insurances.png"),
                new ImagePicker("kids.png"),
                new ImagePicker("lending_renting.png"),
                new ImagePicker("loans.png"),
                new ImagePicker("maintenance_repairs.png"),
                new ImagePicker("pets.png"),
                new ImagePicker("public_transport.png"),
                new ImagePicker("rent.png"),
                new ImagePicker("restaurant.png"),
                new ImagePicker("salary.png"),
                new ImagePicker("scholarship.png"),
                new ImagePicker("services.png"),
                new ImagePicker("sport.png"),
                new ImagePicker("taxi.png"),
                new ImagePicker("travel.png"),
                new ImagePicker("utilities.png"),
            };
        }
    }
}
