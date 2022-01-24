using HouseCaptain.ViewModels;
using HouseCaptain.Views;
using HouseCaptain.Views.Shopping;
using HouseCaptain.Views.Homes;
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


            //Routing of Shopping
            Routing.RegisterRoute(nameof(AddShoppingItemPage), typeof(AddShoppingItemPage));
            Routing.RegisterRoute(nameof(ShoppingHistoryPage), typeof(ShoppingHistoryPage));
            Routing.RegisterRoute(nameof(ViewSingleShoppingItemPage), typeof(ViewSingleShoppingItemPage));
            Routing.RegisterRoute(nameof(ShoppingListPage), typeof(ShoppingListPage));

            //Routing of Homes
            Routing.RegisterRoute(nameof(AddHomePage), typeof(AddHomePage));
            Routing.RegisterRoute(nameof(HomeDetailsPage), typeof(HomeDetailsPage));
            Routing.RegisterRoute(nameof(HomesListPage), typeof(HomesListPage));

        }

    }
}
