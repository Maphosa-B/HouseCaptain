using HouseCaptain.Views.Homes;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Forms;
using MvvmHelpers;
using HouseCaptain.Models.Homes;
using HouseCaptain.Services.Version_1;
using System.Collections.Generic;
using Humanizer;
using HouseCaptain.Views.Shopping;

namespace HouseCaptain.ViewModels.Homes
{

    public class HomesListVieModel: MyBaseViewModel
    {
        private int IsNavigated = 0;
        private HomeModel _SelectedHome;
      
        //Commands
        public AsyncCommand GoToAddHomePageCommand { get; set; }
        public AsyncCommand GetListOfHomesCommand { get; set; }
        public AsyncCommand GoToSelectedHomeCommand { get; set; }
        public AsyncCommand GoToHomeSettingsCommand { get; set; }

        //Properties
        public HomeModel SelectedHome
        {
            get => _SelectedHome;
            set => SetProperty(ref _SelectedHome, value);
        }
        public  ObservableRangeCollection<HomeModel> HomesList { set; get; }

        

        //Constructor
        public HomesListVieModel()
        {
            HomesList = new ObservableRangeCollection<HomeModel>();

            //Instantiating commands
            GoToAddHomePageCommand = new AsyncCommand(GoToAddHomeAsync);
            GetListOfHomesCommand = new AsyncCommand(GetHomesAsync);
            GoToSelectedHomeCommand = new AsyncCommand(GoToSelectedHomeAsync);
        }

        //helper methods
        async Task GoToAddHomeAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            if (IsNavigated == 0)
            {
                IsNavigated++;
                await Shell.Current.GoToAsync(nameof(AddHomePage));
                IsNavigated = 0;
            }

            IsBusy = false;
            IsNotBusy = true;
        }

        private async Task GetHomesAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            //Clearing the list fisrt then retrive data from database
            HomesList.Clear();

            var HomesFromDb = await HomesService.GetAllHomesAsync();

            List<HomeModel> LocalListOfHomes = new List<HomeModel>();    
            foreach(var i in HomesFromDb)
            {
                HomeModel tempHome = new HomeModel
                {
                    Id = i.Id,
                    Name = i.Name.Humanize().Truncate(20,"...")
                };
                LocalListOfHomes.Add(tempHome);
            }
            HomesList.AddRange(LocalListOfHomes);

            IsBusy = false;
            IsNotBusy = true;
        }

        async Task GoToSelectedHomeAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            var route = $"{nameof(ShoppingListPage)}?HomeId={_SelectedHome.Id}";
            await Shell.Current.GoToAsync(route);

            IsBusy = false;
            IsNotBusy = true;
        }
    }


}
