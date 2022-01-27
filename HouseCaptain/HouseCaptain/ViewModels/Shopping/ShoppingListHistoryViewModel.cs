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
        }
    }
}
