using HouseCaptain.Entities;
using HouseCaptain.Services.Version_1;
using HouseCaptain.Views.Homes;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HouseCaptain.ViewModels.Homes
{
    public class AddHomeViewModel:MyBaseViewModel
    {

        private string _HomeName;

        //Command to add Home
        public AsyncCommand AddHomeCommand { get; set; }

        public String HomeName
        {
            get => _HomeName;
            set => SetProperty(ref _HomeName, value);
        }

        //Constructor
        public AddHomeViewModel()
        {
            AddHomeCommand = new AsyncCommand(AddHomeAsync);
        }

        //Helper methods
        async Task AddHomeAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            if(!String.IsNullOrEmpty(_HomeName))
            {
                HomesEntity newHome = new HomesEntity
                {
                    Name = _HomeName
                };

                var status = await HomesService.AddHomeAsync(newHome);

                if(status>0)
                {
                    await Application.Current.MainPage.DisplayAlert(null, $"{_HomeName} has been added", "Okay");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(null, "There was an error, home is not added. Please try again "+status, "Okay");
                    return;
                }

            }else
            {
                await Application.Current.MainPage.DisplayAlert(null, "Name cannot be blank", "Okay");
            }

            IsBusy = false;
            IsNotBusy = true;
        }
    }
}
