using System;
using System.Collections.Generic;
using System.Text;

namespace HappyBudget.Models
{
    public class OnboardingItem
    {
       
        public string Title { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }

        public OnboardingItem(string title, string image, string content)
        {
            Title = title;
            Image = image;
            Content = content;
        }
        
        public OnboardingItem()
        {

        }

        public static List<OnboardingItem> GetOnboardingItems()
        {
            return new List<OnboardingItem> ///ADAUGARE IN DB APOI GET IN FUNCTIE DE TYPE 2 LISTE SI IN NEW TRANSACTION IF 
            {
                new OnboardingItem("Title1", "xamarin_logo.png", "Content1"),
                new OnboardingItem("Title2", "xamarin_logo.png", "Content2"),
                new OnboardingItem("Title3", "xamarin_logo.png", "Content3"),
                new OnboardingItem("Title4", "xamarin_logo.png", "Content4"),
            };
        }
    }
}
