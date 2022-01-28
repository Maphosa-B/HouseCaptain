using HouseCaptain.Entities;
using HouseCaptain.Services.Version_1;
using Humanizer;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Web;
using HouseCaptain.Views.Homes;

namespace HouseCaptain.ViewModels.Homes
{
    public class HomeDetailsViewModel: MyBaseViewModel,IQueryAttributable
    {
        //Private variables
        private string HomeId { get; set; }
        private HomesEntity _RetrivedHome;
        private string _HomeName;


        //Commands        
        public AsyncCommand GetHomeFromDbCommand { get; set; }
        public AsyncCommand UpdateHomeDetailsCommand { get; set; }
        public AsyncCommand DeleteHomeDetailsCommand { get; set; }

        //Properties
        public HomesEntity RetrivedHome
        {
            get => _RetrivedHome;
            set => SetProperty(ref _RetrivedHome, value);
        }

        public String HomeName
        {
            get => _HomeName;
            set => SetProperty(ref _HomeName, value);
        }

        
        //Constructor
        public HomeDetailsViewModel()
        {
            //Activating Commands
            GetHomeFromDbCommand = new AsyncCommand(RetriveHomeAsync);
            UpdateHomeDetailsCommand = new AsyncCommand(UpdateHomeName);
            DeleteHomeDetailsCommand = new AsyncCommand(DeleteHomeName);
        }

        //Helper methods
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            HomeId = HttpUtility.UrlDecode(query["HomeId"]);
        }

        private async Task RetriveHomeAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            _RetrivedHome =await HomesService.GetSingleHomesAsync(Convert.ToInt32(HomeId));
            HomeName = _RetrivedHome.Name;
            Title = $"Update {HomeName} Name";

            IsBusy = false;
            IsNotBusy = true;
        }

        private async Task UpdateHomeName()
        {
            IsBusy = true;
            IsNotBusy = false;

            //Getting name from name property
            RetrivedHome.Name = _HomeName;
            int Status = await HomesService.UpdateHomesAsync(RetrivedHome);

            if (Status > 0)
            {
                await Application.Current.MainPage.DisplayAlert(null,$"Home has been updated to {RetrivedHome.Name.Transform(To.TitleCase)}","Okay");
                await Shell.Current.GoToAsync(nameof(HomesListPage));
            }else
            {
                await Application.Current.MainPage.DisplayAlert(null, $"there was an error,{RetrivedHome.Name} was not updated. Please try again", "Okay");
                return;
            }

            IsBusy = false;
            IsNotBusy = true;
        }

        private async Task DeleteHomeName()
        {
            IsBusy = true;
            IsNotBusy = false;

            var AlertStatus = await Application.Current.MainPage.DisplayActionSheet($"Are you sure you want to delete {_HomeName}?", "No", "Yes");

            if(AlertStatus.Equals("Yes"))
            {
                var Status = HomesService.DeleteHomesAsync(RetrivedHome.Id);
                await Application.Current.MainPage.DisplayAlert(null, $"{RetrivedHome.Name.Transform(To.TitleCase)} has been deleted", "Okay");
                await Shell.Current.GoToAsync($"{nameof(HomesListPage)}");
            }
            else
            {
                return;
            }

            IsBusy = false;
            IsNotBusy = true;
        }
    }
}
