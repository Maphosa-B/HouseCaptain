using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseCaptain.ViewModels
{
    public class MyBaseViewModel:BaseViewModel
    {
        public List<String> ShoppingItemCategoriesList { get; set; }

        public MyBaseViewModel()
        {
            //Initializing Categories list
            ShoppingItemCategoriesList = new List<string>
            {
                "Fruits",
                "Vagetables",
                "Beverages",
                "Canned Goods",
                "Frozen Foods",
                "Meat",
                "Fish and shellfish",
                "Deli",
                "Condiments & Spices",
                "Sauces & Oils",
                "Snacks",
                "Bread & Bakery",
                "Beverages",
                "Pasta/Rice",
                "Baking",
                "Personal Care",
                "Health Care",
                "Paper & Wrap",
                "Household Supplies",
                "Baby Items",
                "Electric Appliance",
                "Gadget",
                "Gardening",
                "Other"
            };
        }
    }
}
