using HappyBudget.Models;
using HappyBudget.Services.Dialog;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels
{
    public class SelectImageViewModel : BaseViewModel
    {
        private IDialogService _dialogService;

        private ObservableCollection<ImagePicker> _images;
        public ObservableCollection<ImagePicker> Images
        {
            get { return _images; }
            set { SetProperty(ref _images, value); }
        }

        private ImagePicker _selectedImage;
        public ImagePicker SelectedImage
        {
            get { return _selectedImage; }
            set { SetProperty(ref _selectedImage, value); }
        }

        public SelectImageViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            Images = new ObservableCollection<ImagePicker>(ImagePicker.GetImages());
            SelectedImage = null;
        }

        public ICommand SelectImageCommand { get => new Command(() => SelectImage()); }
        private async void SelectImage()
        {
            if (SelectedImage == null)
            {
                await _dialogService.DisplayAlert("Error", "Choose an image!", "Ok");
                return;
            }

            MessagingCenter.Send(this, "_image");
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
