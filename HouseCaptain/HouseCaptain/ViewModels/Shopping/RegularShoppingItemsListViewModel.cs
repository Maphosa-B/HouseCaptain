using HouseCaptain.Entities;
using HouseCaptain.Services.Version_1;
using HouseCaptain.Views.Shopping;
using Humanizer;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace HouseCaptain.ViewModels.Shopping
{
    public class RegularShoppingItemsListViewModel:MyBaseViewModel,IQueryAttributable
    {
        //private variables
        private RegularItemsEntity _SelectedItem;
        private int Range = 0;

        //Commands
        public AsyncCommand GetRegularItemsCommand { get; set; }
        public AsyncCommand GoToAddSelectedItemCommand { get; set; }
        public AsyncCommand LoadMoreItemsCommand { get; set; }
        public ObservableRangeCollection<RegularItemsEntity> ItemsList { get; set; }


        //properties
        private int _HomeId { get; set; }
        public RegularItemsEntity SelectedItem 
        {
            get => _SelectedItem; 
            set =>SetProperty(ref _SelectedItem,value); 
        }


        //Constructor
        public RegularShoppingItemsListViewModel()
        {
            //instatntiating list 
            ItemsList = new ObservableRangeCollection<RegularItemsEntity>();

            //Activating Commands 
            GetRegularItemsCommand = new AsyncCommand(PopulateItemsList);
            GoToAddSelectedItemCommand = new AsyncCommand(GoToAddSelectedItemAsync);
            LoadMoreItemsCommand = new AsyncCommand(LoadMoreRegularShoppingListAsync);
     
        }


        //helper methods
        async Task PopulateItemsList()
        {
            IsBusy = true;
            IsNotBusy = false;

            ItemsList.Clear();

            var tempList = await ShoppingService.GetInitialRegulaItemsAsync(_HomeId);
          
            ItemsList.AddRange(tempList);

            IsBusy = false;
            IsNotBusy = true;
        }

        async Task GoToAddSelectedItemAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            await Shell.Current.GoToAsync($"{nameof(AddShoppingItemFromRegularListPage)}?ItemId={SelectedItem.Id}&HomeId={_HomeId}");

            IsBusy = false;
            IsNotBusy = true;
        }

        async Task LoadMoreRegularShoppingListAsync()
        {
            if(ItemsList.Count>10)
            {
                IsLoadingMore = true;

                Range += 3;
                var temItemsList = await ShoppingService.LoadMoreRegularItemsAsync(Convert.ToInt32(_HomeId), Range);

                List<RegularItemsEntity> RegularItemsList = new List<RegularItemsEntity>();

                foreach (var i in temItemsList)
                {
                    RegularItemsEntity aa = new RegularItemsEntity
                    {
                        Id = i.Id,
                        ImgUrl = i.ImgUrl,
                        Name = i.Name.Truncate(18, "..."),
                        QuantityType = i.QuantityType,
                        Category = i.Category,
                        AddCount = i.AddCount,
                        HomeId = i.HomeId
                    };

                    RegularItemsList.Add(aa);
                }

                if (RegularItemsList != null)
                {
                    ItemsList.AddRange(RegularItemsList);
                    RegularItemsList.OrderBy(x => x.AddCount);
                }

                IsLoadingMore = false;
            }         
        }


        //getting url parameter and store it in a variable
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            _HomeId = Convert.ToInt32(HttpUtility.UrlDecode(query["HomeId"]));
            
        }
    }
}
