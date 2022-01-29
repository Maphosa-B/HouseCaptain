using HouseCaptain.Services.Version_1;
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
    public class ViewSingleHistoryItemViewModel:MyBaseViewModel,IQueryAttributable
    {
        private String _Name;
        private String _Notes;
        private String _ImgUrl;
        private String _AddDate;
        private String _Updatedate;
        private int _Quantity;
        private String _QuantityType;
        private bool _AreNotesAdded;





        public String Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }
        public String Notes
        {
            get => _Notes;
            set => SetProperty(ref _Notes, value);
        }
        public String AddDate
        {
            get => _AddDate;
            set => SetProperty(ref _AddDate, value);
        }
        public String Updatedate
        {
            get => _Updatedate;
            set => SetProperty(ref _Updatedate, value);
        }
        public String ImgUrl
        {
            get => _ImgUrl;
            set => SetProperty(ref _ImgUrl, value);
        }
        public int Quantity
        {
            get => _Quantity;
            set => SetProperty(ref _Quantity, value);
        }
        public String QuantityType
        {
            get => _QuantityType;
            set => SetProperty(ref _QuantityType, value);
        }

        public bool AreNotesAdded
        {
            get => _AreNotesAdded;
            set => SetProperty(ref _AreNotesAdded, value);
        }


        private string ItemId { get; set; }


        //Commands
        public AsyncCommand CheckoutItemCommad { get; set; }
        public AsyncCommand CancelItemCommad { get; set; }

        public AsyncCommand PopulateItemDataCommad { get; set; }


        public ViewSingleHistoryItemViewModel()
        {
            CheckoutItemCommad = new AsyncCommand(CheckoutAsync);
            CancelItemCommad = new AsyncCommand(CamcelItemFromlistAsync);
            PopulateItemDataCommad = new AsyncCommand(populateItemDetailsAsync);

        }


        //Helper methods
        async Task CheckoutAsync()
        {
            IsBusy = true;
            IsNotBusy = false;

            var Status = await ShoppingService.CheckoutItemAsync(Convert.ToInt32(ItemId));

            if (Status > 0)
            {
                IsBusy = false;
                IsNotBusy = true;

                await Application.Current.MainPage.DisplayAlert(null, "Item has been checked out", "Okay");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                IsBusy = false;
                IsNotBusy = true;

                await Application.Current.MainPage.DisplayAlert(null, "There was some error, item was not checked out. please try again", "Okay");
                return;
            }


        }

        async Task CamcelItemFromlistAsync()
        {
            if ((await Application.Current.MainPage.DisplayActionSheet("Are you sure you want to delete this item from the list", "Yes", "No")).Equals("Yes"))
            {
                IsBusy = true;
                IsNotBusy = false;

                var Status = await ShoppingService.CamcelItemAsync(Convert.ToInt32(ItemId));

                if (Status > 0)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(null, "There was some error, item was not deleted. please try again", "Okay");
                    return;
                }

                IsBusy = false;
                IsNotBusy = true;
            }
        }


        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            ItemId = HttpUtility.UrlDecode(query["ItemId"]);

        }

        async Task populateItemDetailsAsync()
        {
            var item = await ShoppingService.GetSingleShoppingItemsAsync(Convert.ToInt32(ItemId));

            Name = item.Name.Truncate(20, "...");
            Notes = item.Notes;
            Quantity = item.Quantity;
            QuantityType = item.QuantityType;
            ImgUrl = item.ImgUrl;
            AddDate = item.AddDate.ToOrdinalWords();
            Updatedate = item.LastModificationDate.ToOrdinalWords();

            if (String.IsNullOrEmpty(Notes))
            {
                AreNotesAdded = false;
            }
            else
            {
                AreNotesAdded = true;

            }

        }
    }
}
