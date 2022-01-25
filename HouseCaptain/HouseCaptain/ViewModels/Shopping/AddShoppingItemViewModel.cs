using MvvmHelpers.Commands;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HouseCaptain.ViewModels.Shopping
{
    public class AddShoppingItemViewModel:MyBaseViewModel
    {
        private bool _IsImageAvailable = false;
        private bool _IsImageNotAvailable = false;
        private String _ImagePath;

        public String ImagePath 
        { 
            get => _ImagePath; 
            set => SetProperty(ref _ImagePath,value); 
        }

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

        public AddShoppingItemViewModel()
        {
            //Will be used to flag controls of selecting image and hidding them if image is selected
            _IsImageAvailable = false;
            _IsImageNotAvailable = true;

            CaptureImageCommand = new AsyncCommand(CaptureAnImageAsync);
            SelectImageCommand = new AsyncCommand(SelectAnImageAsync);
            RemoveImageCommand = new AsyncCommand(RemoveImageAsync);
        }


        //Helper methods
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




    }
}
