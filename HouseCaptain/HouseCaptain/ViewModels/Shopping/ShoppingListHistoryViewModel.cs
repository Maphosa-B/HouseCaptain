using HouseCaptain.Models.Shopping;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseCaptain.ViewModels.Shopping
{
    public class ShoppingListHistoryViewModel:MyBaseViewModel
    {
        public ObservableRangeCollection<ShoppingItemModel> ShoppingListHistory { get; set; }

        public ShoppingListHistoryViewModel()
        {
            ShoppingListHistory = new ObservableRangeCollection<ShoppingItemModel>
            {
                new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Magarine",
                    Notes = "Please buy right brand",
                    Quantity = 7
                },
                new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Magarine",
                    Notes = "Please buy right brand",
                    Quantity = 7
                },
                new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Magarine",
                    Notes = "Please buy right brand",
                    Quantity = 7
                },
                new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Magarine",
                    Notes = "Please buy right brand",
                    Quantity = 7
                }
                ,new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Magarine",
                    Notes = "Please buy right brand",
                    Quantity = 7
                }
                ,new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Magarine",
                    Notes = "Please buy right brand",
                    Quantity = 7
                }
                ,new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Magarine",
                    Notes = "Please buy right brand",
                    Quantity = 7
                }
                ,new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Magarine",
                    Notes = "Please buy right brand",
                    Quantity = 7
                }
                ,new ShoppingItemModel
                {
                    BarCode = "ssd",
                    CategoryId = 2,
                    Name = "Magarine",
                    Notes = "Please buy right brand",
                    Quantity = 7
                }
            };
        }
    }
}
