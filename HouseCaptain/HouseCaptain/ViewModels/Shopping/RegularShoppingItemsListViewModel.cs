using HouseCaptain.Entities;
using HouseCaptain.Services.Version_1;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace HouseCaptain.ViewModels.Shopping
{
    public class RegularShoppingItemsListViewModel:MyBaseViewModel,IQueryAttributable
    {
        //private variables
        public int HomeId { get; set; }

        //Commands
        public AsyncCommand GetRegularItemsCommand { get; set; }

        public List<RegularItemsEntity> ItemsList { get; set; }



        //Constructor
        public RegularShoppingItemsListViewModel()
        {
            //instatntiating list 
            ItemsList = new List<RegularItemsEntity>();

            //Activating Commands 
            GetRegularItemsCommand = new AsyncCommand(PopulateItemsList);
        }


        //helper methods
        async Task PopulateItemsList()
        {
            IsBusy = true;
            IsNotBusy = false;

            ItemsList.Clear();

            var tempList = await ShoppingService.GetInitialRegulaItemsAsync(HomeId);
            ItemsList.AddRange(tempList);

            IsBusy = false;
            IsNotBusy = true;
        }


        //getting url parameter and store it in a variable
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            HomeId = Convert.ToInt32(HttpUtility.UrlDecode(query["HomeId"]));
        }
    }
}
