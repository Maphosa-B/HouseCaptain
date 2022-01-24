using System;
using System.Collections.Generic;
using System.Text;

namespace HouseCaptain.ViewModels.Shopping
{
    public class AddShoppingItemViewModel:MyBaseViewModel
    {

        public List<String> ShoppingItemsCategories { get; set; }

        public AddShoppingItemViewModel()
        {
            ShoppingItemsCategories = new List<string>
            {
                "Toletory",
                "Appliance",
                "Stationary",
                "Fruit",
                "Vaggy",
                "Breakfast",
                "Clothing",
                "Food"
            };
        }




    }
}
