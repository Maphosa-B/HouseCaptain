using HouseCaptain.Models.Shopping;
using HouseCaptain.Views.Shopping;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HouseCaptain.ViewModels.Shopping
{
    public class ShoppingListViewModel:MyBaseViewModel
    {
        //Will be used as a flag to prevent doublenavigation while loading
        private int IsNavigated { get; set; } = 0;
        public ShoppingItemModel SelectedItem { get; set; }
        public ObservableRangeCollection<ShoppingItemModel> GroceryList { get; set; }


        //Commands
        public AsyncCommand GoToAddShoppingItemPageCommand { get; set; }
        public AsyncCommand GoToShoppingListHistoryPageCommand { get; set; }
        public AsyncCommand GoToSingleShoppingItemPageCommand { get; set; }

        public ShoppingListViewModel()
        {
            Title = "Home Shopping List";

            //Activating Commands
            GoToAddShoppingItemPageCommand = new AsyncCommand(GoToAddShoppingItemAsync);
            GoToShoppingListHistoryPageCommand = new AsyncCommand(GoToShoppingListHistoryAsync);
            GoToSingleShoppingItemPageCommand = new AsyncCommand(GoToSingleShoppingItemAsync);

            IsNavigated = 0;

            GroceryList = new ObservableRangeCollection<ShoppingItemModel>
            {
                new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Shoes",
                    Notes = "Please buy right brand",
                    Quantity = 1,
                    ImgUrl ="Shoe.jpg"
                },
                new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Sun Glasses",
                    Notes = "Please buy right brand",
                    Quantity = 2,
                    ImgUrl = "Glass.jpg"
                },
                new ShoppingItemModel
                {
                    BarCode = "Coke",
                    CategoryId = 2,
                    Name = "Coca Coal Coke",
                    Notes = "Please buy right brand",
                    Quantity = 12,
                    ImgUrl = "Drink.jpg"
                },
                new ShoppingItemModel
                {
                    BarCode = "Coke",
                    CategoryId = 2,
                    Name = "Red Bull Energy Dr..",
                    Notes = "Please buy right brand",
                    Quantity = 4,
                    ImgUrl = "Drink.jpg"
                },
                new ShoppingItemModel
                {
                    BarCode = "Coke",
                    CategoryId = 2,
                    Name = "Savanna",
                    Notes = "Please buy right brand",
                    Quantity = 24,
                    ImgUrl = "Drink.jpg"
                },
                new ShoppingItemModel
                {
                    BarCode = "Coke",
                    CategoryId = 2,
                    Name = "Sprite",
                    Notes = "Please buy right brand",
                    Quantity = 13,
                    ImgUrl = "Drink.jpg"
                }
            };
        }

        //Helper Methods
        async Task GoToAddShoppingItemAsync()
        {
            if (IsNavigated == 0)
            {
                IsNavigated++;
                await Shell.Current.GoToAsync(nameof(AddShoppingItemPage));
                IsNavigated = 0;
            }
        }

        async Task GoToShoppingListHistoryAsync()
        {
            if (IsNavigated == 0)
            {
                IsNavigated++;
                await Shell.Current.GoToAsync(nameof(ShoppingHistoryPage));
                IsNavigated = 0;
            }
        }

        async Task GoToSingleShoppingItemAsync()
        {
            if (IsNavigated == 0)
            {
                IsNavigated++;
                await Shell.Current.GoToAsync(nameof(ViewSingleShoppingItemPage));
                IsNavigated = 0;
            }
        }

    }
}
