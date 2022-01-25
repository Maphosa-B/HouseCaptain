using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HouseCaptain.Entities;
using HouseCaptain.Models.Homes;
using HouseCaptain.Views.Homes;
using HouseCaptain.Views.Shopping;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms;

namespace HouseCaptain.ViewModels.Homes
{
    public class HomesListVieModel: MyBaseViewModel
    {
        public AsyncCommand AddHomePage { get; set; 
        }public AsyncCommand GoToSelectedHomeCommand { get; set; }

        public HomesEntity SelectedHome { get; set; }
       
        
        public ObservableRangeCollection<HomeModel> HomesList { get; set; }

        public HomesListVieModel()
        {
            //Initializing commands
            AddHomePage = new AsyncCommand(GoToAddHomeAsync);
            GoToSelectedHomeCommand = new AsyncCommand(GoToSelectedHomeAsync);
        }

        async Task GoToAddHomeAsync() => await Shell.Current.GoToAsync(nameof(ShoppingHistoryPage));

        async Task GoToSelectedHomeAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            await Shell.Current.GoToAsync($"{nameof(ShoppingListPage)}?HomeId={SelectedHome.Id}");

            IsBusy = false;
            IsNotBusy = true;
        }

    }
}
