using HouseCaptain.ViewModels;
using HouseCaptain.Views;
using HouseCaptain.Views.Shopping;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HouseCaptain
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();


            //Routing of Grocery
            Routing.RegisterRoute(nameof(AddShoppingItemPage), typeof(AddShoppingItemPage));
            Routing.RegisterRoute(nameof(ShoppingHistoryPage), typeof(ShoppingHistoryPage));
            Routing.RegisterRoute(nameof(ViewSingleShoppingItemPage), typeof(ViewSingleShoppingItemPage));
        }

    }
}
