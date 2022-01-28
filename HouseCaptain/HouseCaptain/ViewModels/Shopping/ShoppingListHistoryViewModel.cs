using HouseCaptain.Models.Homes;
using HouseCaptain.Models.Shopping;
using HouseCaptain.Services.Version_1;
using HouseCaptain.Views.Shopping;
using Humanizer;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace HouseCaptain.ViewModels.Shopping
{
    public class ShoppingListHistoryViewModel:MyBaseViewModel,IQueryAttributable
    {
        //Private properties
        private String HomeId { get; set; }
        private ShoppingItemModel _SelectedItem;

        //Variables Properties
        public ObservableRangeCollection<ShoppingItemModel> ShoppingListHistory { get; set; }
        public ShoppingItemModel SelectedItem 
        {
            get =>_SelectedItem; 
            set => SetProperty(ref _SelectedItem,value); 
        }

        //Commands
        public AsyncCommand GetInitialList { get; set; }
        public AsyncCommand GoToSingleItemCommand { get; set; }

        public ShoppingListHistoryViewModel()
        {
            //Initializing page title
            Title = "Shopping History";

            //Instantiating list object
            ShoppingListHistory = new ObservableRangeCollection<ShoppingItemModel>();
            

            //Activating commands
            GetInitialList = new AsyncCommand(PopulateListInitiallyAsnyc);
            GoToSingleItemCommand = new AsyncCommand(GoToSingleItemAsync);       
        }


        //Helper methods
        async Task PopulateListInitiallyAsnyc()
        {
           
            IsBusy = true;
            IsNotBusy = false;

            ShoppingListHistory.Clear();

            var tempList = await ShoppingService.GetShoppingItemsHistoryAsync(Convert.ToInt32(HomeId), null);
            var tempModelsList = new List<ShoppingItemModel>();
            
            foreach(var i in tempList)
            {
                ShoppingItemModel aa = new ShoppingItemModel
                {
                    Id = i.Id,
                    ImgUrl = i.ImgUrl,
                    Name = i.Name.Truncate(18, "..."),
                    CategoryId = 1,
                    Notes = i.Notes.Truncate(35, "..."),
                    Quantity = i.Quantity,
                    QuantityType = i.QuantityType
                };
                tempModelsList.Add(aa);
            }
                 
            if(tempModelsList!=null)
            {
                ShoppingListHistory.AddRange(tempModelsList);
            }
            IsBusy = false;
            IsNotBusy = true;
        }

        async Task GoToSingleItemAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            await Shell.Current.GoToAsync($"{nameof(ViewSingleHistoryItem)}?ItemId={SelectedItem.Id}");

            IsBusy = false;
            IsNotBusy = true;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            HomeId = HttpUtility.UrlDecode(query["HomeId"]);
        }
    }
}
