using HouseCaptain.Entities;
using HouseCaptain.Services.Version_1;
using HouseCaptain.Views.Shopping;
using MvvmHelpers.Commands;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HouseCaptain.ViewModels.Shopping
{
    public class AddShoppingItemViewModel:MyBaseViewModel,IQueryAttributable
    {
        //Variable to flag UI
        private bool _IsImageAvailable = false;
        private bool _IsImageNotAvailable = false;
        private bool _IsQuantityEmpty = false;



        //Private variables to collect data
        private String _ImagePath;
        private String _ItemName;
        private String _Notes;
        private String _Category;
        private String _QuantityType;
        private String _Quantity;
        private int _HomeId;


        //Properties of data collection
        public String ImagePath
        {
            get => _ImagePath;
            set => SetProperty(ref _ImagePath, value);
        }

        public String ItemName 
        { 
            get => _ItemName; 
            set => SetProperty(ref _ItemName,value); 
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

        public String Category
        {
            get => _Category;
            set => SetProperty(ref _Category, value);
        }

        public String Quantity
        {
            get => _Quantity;
            set => SetProperty(ref _Quantity, value);
        }



        //Properties of UI
        public bool IsImageAvailable
        {
            get => _IsImageAvailable;
            set => SetProperty(ref _IsImageAvailable, value);
        } 
        
        public bool IsImageNotAvailable
        {
            get => _IsImageNotAvailable;
            set => SetProperty(ref _IsImageNotAvailable, value);
        }

        //Commands 
        public AsyncCommand CaptureImageCommand { get; set; }
        public AsyncCommand SelectImageCommand { get; set; }
        public AsyncCommand RemoveImageCommand { get; set; }
        public AsyncCommand AddShoppingIteCommand{ get; set; }
        public AsyncCommand GoToRegularShoppingIyemsListCommand{ get; set; }

        public AddShoppingItemViewModel()
        {
            //Will be used to flag controls of selecting image and hidding them if image is selected
            _IsImageAvailable = false;
            _IsImageNotAvailable = true;

            //Initial selected Category 
            Category = ShoppingItemCategoriesList[0];
            QuantityType = QuantityTypes[0];
            IsQuantityValid = true;


            CaptureImageCommand = new AsyncCommand(CaptureAnImageAsync);
            SelectImageCommand = new AsyncCommand(SelectAnImageAsync);
            RemoveImageCommand = new AsyncCommand(RemoveImageAsync);
            AddShoppingIteCommand = new AsyncCommand(AddShoppingItemAsync);
            GoToRegularShoppingIyemsListCommand = new AsyncCommand(GoToRegularShoppingItemsListAsync);
        }


        //Helper methods
        //Getting a sent Id From shell urlParameter
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            _HomeId =  Convert.ToInt32(HttpUtility.UrlDecode(query["HomeId"]));
        }


        async Task CaptureAnImageAsync()
        {
            PermissionStatus MyPermissionStatus = PermissionStatus.Denied;

            if(await Permissions.CheckStatusAsync<Permissions.Camera>() != PermissionStatus.Granted)
            {
                MyPermissionStatus = await Permissions.RequestAsync<Permissions.Camera>();

                if(MyPermissionStatus==PermissionStatus.Denied)
                {
                    await Application.Current.MainPage.DisplayAlert(null, "Ooops! sorry, you cannot use this feature without granting the app a permission of using your cammera", "Okay");
                    return;
                }
            }

            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "HomeCaptain",
                Name = $"{Guid.NewGuid().ToString().Substring(15).Replace('-','_')}.jpg",
                SaveToAlbum = true,
                AllowCropping = true,
                CompressionQuality = 40
            });

            if(file!=null)
            {
                ImagePath =  file.Path;

                //Image controls flags
                IsImageAvailable = true;
                IsImageNotAvailable = false;
            }

        }

        async Task SelectAnImageAsync()
        {
            PermissionStatus MyPermissionStatus = PermissionStatus.Denied;

            if (await Permissions.CheckStatusAsync<Permissions.StorageRead>() != PermissionStatus.Granted)
            {
                MyPermissionStatus = await Permissions.RequestAsync<Permissions.StorageRead>();

                if (MyPermissionStatus == PermissionStatus.Denied)
                {
                    await Application.Current.MainPage.DisplayAlert(null, "Ooops! sorry, you cannot use this feature without granting the app a permission of reading images from gallery", "Okay");
                    return;
                }
            }

            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 40
            });

            if (file != null)
            {
                ImagePath = file.Path;

                //Image controls flags
                IsImageAvailable = true;
                IsImageNotAvailable = false;
            }

        }


        async Task RemoveImageAsync()
        {
            //First need confirmation from user to remove it
            if("Yes".Equals( await Application.Current.MainPage.DisplayActionSheet("Are you sure you want to remove the image?","No","Yes")))
            {
                //Image controls flags
                IsImageAvailable = false;
                IsImageNotAvailable = true;

                //Clearing image path of previous captured of selected image
                ImagePath = "";
            }
            return;
        }  
        
        async Task GoToRegularShoppingItemsListAsync()
        {
            IsBusy = true;
            IsNotBusy = false;
            await Shell.Current.GoToAsync($"{nameof(RegularShoppingItemsListPage)}?HomeId={_HomeId}");           
            IsBusy = false;
            IsNotBusy = true;           
        } 
        
        
        async Task AddShoppingItemAsync()
        {
            //Validating inputs
            if(String.IsNullOrEmpty(ItemName))
            {
                await Application.Current.MainPage.DisplayAlert(null, "Name cannot be blank", "Okay");
                return;
            }

            if (String.IsNullOrEmpty(Category))
            {
                await Application.Current.MainPage.DisplayAlert(null, "Please select a category of the item", "Okay");
                return;
            }

            if (IsQuantityValid==false || String.IsNullOrEmpty(Quantity))
            {
                await Application.Current.MainPage.DisplayAlert(null, "Please enter a quantity", "Okay");
                return;
            }

            ShoppingItemEntity Item = new ShoppingItemEntity
            {
                Name = ItemName,
                Notes = Notes,
                Quantity =  Convert.ToInt32(Quantity),
                Category = Category,
                HomeId = _HomeId,
                QuantityType = QuantityType
            };

            //if a user doe not select or capture an image for that item then we use our display image
            if(String.IsNullOrEmpty(ImagePath))
            {
                Item.ImgUrl = $"{Category.Trim()}.jpg";
            }else
            {
                Item.ImgUrl = ImagePath;
            }

            //Adding the iteminto database 
            var Status = await ShoppingService.AddShoppingItemAsync(Item);

            if(Status>0)
            {
                await Application.Current.MainPage.DisplayAlert(null, $"{ItemName} has been added", "Okay");
               
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(null, "There was an error, item was not added. Please try again", "Okay");
                return;
            }
        }




    }
}
