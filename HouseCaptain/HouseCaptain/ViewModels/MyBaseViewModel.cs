using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HouseCaptain.ViewModels
{
    public class MyBaseViewModel:BaseViewModel
    {
        protected bool _IsLoadingMore;




        public List<String> ShoppingItemCategoriesList { get; set; }
        public List<String> QuantityTypes { get; set; }

        public bool IsLoadingMore 
        { 
            get => _IsLoadingMore; 
            set => SetProperty(ref _IsLoadingMore,value); 
        }

        public MyBaseViewModel()
        {

            ShoppingItemCategoriesList = new List<string>();


            //Initializing Categories list
            List<string> tempList = new List<string>
            {
                "Fruits",
                "Vagetables",
                "Beverages",
                "Canned Goods",
                "Frozen Foods",
                "Meat",
                "Fish and shellfish",
                "Deli",
                "Condiments and Spices",
                "Sauces and Oils",
                "Snacks",
                "Bread and Bakery",
                "Pasta or Rice",
                "Baking",
                "Personal Care",
                "Health Care",
                "Paper and Wrap",
                "Household Supplies",
                "Baby Items",
                "Electric Appliances",
                "Gadgets",
                "Gardening",

            };

            ShoppingItemCategoriesList = tempList.OrderBy(x => x).ToList();
            ShoppingItemCategoriesList.Add("Other");


            QuantityTypes = new List<string>
            {
                "Items",
                "Packs",
                "g",
                "Kg",
                "ml",
                "L",           
            };
        }
    }
}
