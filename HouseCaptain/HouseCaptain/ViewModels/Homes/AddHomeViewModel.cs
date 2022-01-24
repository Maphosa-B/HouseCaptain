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

        private String _HomeName;



        //Command to add Home
        public AsyncCommand AddHomeCommand { get; set; }

        public String HomeName
        {
            get => _HomeName;
            set => SetProperty(ref _HomeName, value);
        }

        public AddHomeViewModel()
        {
            AddHomeCommand = new AsyncCommand(AddHomeAsync);
        }

        //Helper methods
        async Task AddHomeAsync()
        {
            if(!String.IsNullOrEmpty(_HomeName))
            {
                HomesEntity newHome = new HomesEntity
                {
                    Name = _HomeName
                };

                var status = await HomesService.AddHomeAsync(newHome);

                if(status>0)
                {
                    await Application.Current.MainPage.DisplayAlert(null, "Home has bee added", "Okay");
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
        }
    }
}
