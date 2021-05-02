using HappyBudget.Models;
using HappyBudget.Services.Database;
using HappyBudget.Services.Dialog;
using HappyBudget.Services.Navigation;
using HappyBudget.Views;
using HappyBudget.Views.Categories;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels.Categories
{
    [QueryProperty("Id", "id")]
    public class NewCategoryViewModel : BaseViewModel
    {
        private IDatabaseService<Category> _databaseService;
        private INavigationService _navigationService;
        private IDialogService _dialogService;

      

        public NewCategoryViewModel(IDatabaseService<Category> databaseService, INavigationService navigationService, IDialogService dialogService)
        {
            _databaseService = databaseService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            RecieveMessage();
        }

        private string _categoryImage;
        public string CategoryImage
        {
            get { return _categoryImage; }
            set { SetProperty(ref _categoryImage, value); }
        }

        private string _categoryColor;
        public string CategoryColor
        {
            get { return _categoryColor; }
            set { SetProperty(ref _categoryColor, value); }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set { SetProperty(ref _isChecked, value); }
        }

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set { SetProperty(ref _categoryName, value); }
        }

        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                _id = Uri.UnescapeDataString(value);
            }
        }

        public async override Task InitializeAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Id) || !int.TryParse(Id, out int categoryId))
            {
                IsChecked = true;
                CategoryColor = "#FFFFFF";
                return;
            }

            var selectedCategory = await _databaseService.GetById(categoryId);
            IsChecked = selectedCategory.Type == AppConstants.IS_INCOME_CATEGORY;
            CategoryColor = selectedCategory.Color;
            CategoryName = selectedCategory.Name;

            CategoryImage = selectedCategory.Image;


        }

        public ICommand AddCategoryCommand { get => new Command(async () => await AddCategory(), () => IsNotBusy); }
        private async Task AddCategory()
        {
            if (CategoryColor.Equals("#FFFFFF") && CategoryImage == null)
            {
                await _dialogService.DisplayAlert("Error", "Please select a color and an image!", "Ok");
                return;
            }
            else if (CategoryImage == null)
            {
                await _dialogService.DisplayAlert("Error", "Please select an image!", "OK");
                return;
            }
            else if (CategoryColor.Equals("#FFFFFF"))
            {
                await _dialogService.DisplayAlert("Error", "Please select a color!", "OK");
                return;
            }
            IsBusy = true;
            await SaveCategory();

            await _navigationService.PopAsync(); 
            IsBusy = false;
        }

        private async Task SaveCategory()
        {
            var category = new Category
            {
                Name = CategoryName,
                Type = IsChecked == true ? AppConstants.IS_INCOME_CATEGORY : AppConstants.IS_EXPENSE_CATEGORY,
                Image = CategoryImage,
                Color = CategoryColor,
                Id = string.IsNullOrEmpty(Id) ? 0 : int.Parse(Id),
            };

            await _databaseService.SaveAsync(category);
        }


        private void RecieveMessage()
        {
            MessagingCenter.Subscribe<SelectImageViewModel>(this, "_image", (s) =>
            {
                if (s != null)
                {
                    CategoryImage = s.SelectedImage.Name;
                }
            });

            MessagingCenter.Subscribe<SelectColorViewModel>(this, "_color", (s) =>
            {
                if (s != null)
                {
                    CategoryColor = s.SelectedColor.HexCode;
                }
            });
        }

        public ICommand ShowImagesCommand { get => new Command(() => ShowImages()); }
        private async void ShowImages()
        {
            await Shell.Current.Navigation.PushPopupAsync(new SelectImagePopupPage());
        }

        public ICommand ShowColorsCommand { get => new Command(() => ShowColors()); }
        private async void ShowColors()
        {
            await Shell.Current.Navigation.PushPopupAsync(new SelectColorPopupPage());
        }
    }
}
