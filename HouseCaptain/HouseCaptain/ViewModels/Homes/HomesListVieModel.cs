using HouseCaptain.Views.Homes;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Forms;
using MvvmHelpers;
using HouseCaptain.Models.Homes;
using HouseCaptain.Services.Version_1;
using System.Collections.Generic;
using Humanizer;

namespace HouseCaptain.ViewModels.Homes
{

    public class HomesListVieModel: MyBaseViewModel
    {
        private int IsNavigated = 0;
        public AsyncCommand GoToAddHomePageCommand { get; set; }
        public AsyncCommand GetListOfHomesCommand { get; set; }

        //Variables
       public  ObservableRangeCollection<HomeModel> HomesList { set; get; }
      
        
        //Constructor
        public HomesListVieModel()
        {
            HomesList = new ObservableRangeCollection<HomeModel>();

            //Instantiating commands
            GoToAddHomePageCommand = new AsyncCommand(GoToAddHomeAsync);
            GetListOfHomesCommand = new AsyncCommand(GetHomesAsync);
        }

        //helper methods
        async Task GoToAddHomeAsync()
        {
            if (IsNavigated == 0)
            {
                IsNavigated++;
                await Shell.Current.GoToAsync(nameof(AddHomePage));
                IsNavigated = 0;
            }
        }

        async Task GetHomesAsync()
        {
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
        }

    }


}
