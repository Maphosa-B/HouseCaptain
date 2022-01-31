using HouseCaptain.Entities;
using HouseCaptain.Services.Version_1;
using HouseCaptain.Views.Shopping;
using Humanizer;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace HouseCaptain.ViewModels.Shopping
{
    public class AddShoppingItemFromRegularListViewModel : MyBaseViewModel, IQueryAttributable
    {

        private int _ItemId;

        private bool _IsQuantityEmpty = false;

        public RegularItemsEntity SelectedItem  { get; set; }



        //Private variables to collect data
        private String _ImgUrl;
        private String _ItemName;

        private String _Notes;
        private String _Category;
        private String _QuantityType;
        private String _Quantity;
        private int _HomeId;


        //Properties of data collection
        public String ItemName
        {
            get => _ItemName;
            set => SetProperty(ref _ItemName, value);
        }

        public String CategoryName
        {
            get => _Category;
            set => SetProperty(ref _Category, value);
        }


        public bool IsQuantityValid
        {
            get => _IsQuantityEmpty;
            set => SetProperty(ref _IsQuantityEmpty, value);
        }


        public String QuantityType
        {
            get => _QuantityType;
            set => SetProperty(ref _QuantityType, value);
        }

        public String Notes
        {
            get => _Notes;
            set => SetProperty(ref _Notes, value);
        }

        public String Quantity
        {
            get => _Quantity;
            set => SetProperty(ref _Quantity, value);
        }

        public String ImgUrl
        {
            get => _ImgUrl;
            set => SetProperty(ref _ImgUrl, value);
        }


        //Commands
        public AsyncCommand GetSingleItemDataCommand { get; set; }
        public AsyncCommand AddItemCommand { get; set; }
        public AsyncCommand DeleteItemCommand { get; set; }

        public AddShoppingItemFromRegularListViewModel()
        {
            IsBusy = true;
            IsNotBusy = true;


            //Activating commands
            GetSingleItemDataCommand = new AsyncCommand(GetSelectedItemDataAsync);
            AddItemCommand = new AsyncCommand(AddShoppingItemAsync);
            DeleteItemCommand = new AsyncCommand(DeleteItemAsync);

            //Initial values
            CategoryName = ShoppingItemCategoriesList[0];
            QuantityType = QuantityTypes[0];

            IsBusy = false;
            IsNotBusy = true;
        }


        //Helper methods
        async Task AddShoppingItemAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            //Validating inputs

            if (IsQuantityValid == false || String.IsNullOrEmpty(Quantity))
            {
                await Application.Current.MainPage.DisplayAlert(null, "Please enter a quantity", "Okay");
                IsBusy = false;
                IsNotBusy = true;
                return;
            }

            ShoppingItemEntity Item = new ShoppingItemEntity
            {
                Name = SelectedItem.Name,
                Notes = Notes,
                Quantity = Convert.ToInt32(Quantity),
                Category = SelectedItem.Category,
                HomeId = _HomeId,
                QuantityType = QuantityType,
                ImgUrl = SelectedItem.ImgUrl
            };


            //Adding the iteminto database 
            var Status = await ShoppingService.AddShoppingItemAsync(Item);

            if (Status > 0)
            {
                await Application.Current.MainPage.DisplayAlert(null,"Item has been added","Okay");
                await Shell.Current.GoToAsync($"../../..");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(null, "There was an error, item was not added. Please try again", "Okay");
                return;
            }

            IsBusy = false;
            IsNotBusy = true;
        }

        async Task GetSelectedItemDataAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            SelectedItem = await ShoppingService.GetSingleRegularShoppingItemsAsync(_ItemId);
            ItemName = SelectedItem.Name;
            CategoryName = SelectedItem.Category;
            ImgUrl = SelectedItem.ImgUrl;

            Title = $"Add {ItemName.Truncate(15,"..")}";

            IsBusy = false;
            IsNotBusy = true;
        }
        async Task DeleteItemAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            var Confirmation = await Application.Current.MainPage.DisplayActionSheet($"Do you want to delete {SelectedItem.Name}?", "Delete", "Cancel");
            if(Confirmation.Equals("Delete"))
            {
                await ShoppingService.DeleteRegularItemAsync(SelectedItem.Id);
                await Shell.Current.GoToAsync("..");
            }

            IsBusy = false;
            IsNotBusy = true;
        }


        //Retriving Item Id from url parametr
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            _ItemId = Convert.ToInt32(HttpUtility.UrlDecode(query["ItemId"]));
            _HomeId = Convert.ToInt32(HttpUtility.UrlDecode(query["HomeId"]));
        }
    }
}
