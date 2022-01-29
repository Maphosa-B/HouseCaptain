using HouseCaptain.Models.Shopping;
using HouseCaptain.Services.Version_1;
using HouseCaptain.Views.Homes;
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

    public class ShoppingListViewModel:MyBaseViewModel, IQueryAttributable
    {
     
        //Will be used as a flag to prevent doublenavigation while loading
        private int IsNavigated { get; set; } = 0;
        private string _HomeId { get; set; }

        private String _SelectedFilter;

        private int _RangeStart { get; set; } = 0;



        public String SelectedFilter 
        {
            get => _SelectedFilter; 
            set => SetProperty(ref _SelectedFilter,value); 
        }

        //Properties
        public ShoppingItemModel SelectedItem { get; set; }
        public ObservableRangeCollection<ShoppingItemModel> ShoppingList { get; set; }


        //Commands
        public AsyncCommand GoToAddShoppingItemPageCommand { get; set; }
        public AsyncCommand GoToShoppingListHistoryPageCommand { get; set; }
        public AsyncCommand GoToSingleShoppingItemPageCommand { get; set; }
        public AsyncCommand GoToSelectedHomeSettingsCommand { get; set; }
        public AsyncCommand GetListOfItemFromDbCommand { get; set; }
        public AsyncCommand FilterAlistCommand { get; set; }

        public ShoppingListViewModel()
        {
            IsNavigated = 0;

            ShoppingList = new ObservableRangeCollection<ShoppingItemModel>();

            
            Title = "Home Shopping List";

            //Activating Commands
            GoToAddShoppingItemPageCommand = new AsyncCommand(GoToAddShoppingItemAsync);
            GoToShoppingListHistoryPageCommand = new AsyncCommand(GoToShoppingListHistoryAsync);
            GoToSingleShoppingItemPageCommand = new AsyncCommand(GoToSingleShoppingItemAsync);
            GoToSelectedHomeSettingsCommand = new AsyncCommand(GoToSelectedHomeSettingsAsync);
          
            GetListOfItemFromDbCommand = new AsyncCommand(GetShoppingList);

            FilterAlistCommand = new AsyncCommand(GetFilteredShoppingList);

            IsNavigated = 0;
        }

        //Helper Methods
        async Task GoToAddShoppingItemAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            if (IsNavigated == 0)
            {
                IsNavigated++;
                await Shell.Current.GoToAsync( $"{nameof(AddShoppingItemPage)}?HomeId={_HomeId}");
                IsNavigated = 0;
            }

            IsBusy = false;
            IsNotBusy = true;
        }

        async Task GoToShoppingListHistoryAsync()
        {
            IsBusy = true;
            IsNotBusy = false;


            IsNavigated++;
            await Shell.Current.GoToAsync($"{nameof(ShoppingHistoryPage)}?HomeId={_HomeId}");
            IsNavigated = 0;

            IsBusy = false;
            IsNotBusy = true;
        }

        async Task GoToSingleShoppingItemAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            if (IsNavigated == 0)
            {
                IsNavigated++;
                await Shell.Current.GoToAsync($"{nameof(ViewSingleShoppingItemPage)}?HomeId={_HomeId}&ItemId={SelectedItem.Id}" );
                IsNavigated = 0;
            }

            IsBusy = false;
            IsNotBusy = true;
        }

        async Task GoToSelectedHomeSettingsAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            if (IsNavigated == 0)
            {
                IsNavigated++;
                await Shell.Current.GoToAsync( $"{nameof(HomeDetailsPage)}?HomeId={_HomeId}");
                IsNavigated = 0;
            }

            IsBusy = false;
            IsNotBusy = true;
        }

        async Task GetShoppingList()
        {

            IsBusy = true;
            IsNotBusy = false;


            //Clearing List
            if(ShoppingList!=null)
            {
                ShoppingList.Clear();
            }
           

            var temItemsList =  await ShoppingService.GetAllShoppingItemsAsync(Convert.ToInt32(_HomeId));

            List<ShoppingItemModel> listOfShoppingItemModel = new List<ShoppingItemModel>();
           
            foreach(var i in temItemsList)
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

                listOfShoppingItemModel.Add(aa);
            }

            if (listOfShoppingItemModel != null)
            {
                ShoppingList.AddRange(listOfShoppingItemModel);
            }

            IsBusy = false;
            IsNotBusy = true;
        } 
        
        async Task GetFilteredShoppingList()
        {

            IsBusy = true;
            IsNotBusy = false;

            //Clearing List
            if (ShoppingList!=null)
            {
                ShoppingList.Clear();
            }
           

            var temItemsList =  await ShoppingService.GetFilteredLisAsync(Convert.ToInt32(_HomeId),SelectedFilter,_RangeStart);

            List<ShoppingItemModel> listOfShoppingItemModel = new List<ShoppingItemModel>();
           
            foreach(var i in temItemsList)
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

                listOfShoppingItemModel.Add(aa);
            }

            if (listOfShoppingItemModel != null)
            {
                ShoppingList.AddRange(listOfShoppingItemModel);
            }

            IsBusy = false;
            IsNotBusy = true;
        }

        //Getting a sent Id From shell urlParameter
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            _HomeId = HttpUtility.UrlDecode(query["HomeId"]);
        }
    }
}
