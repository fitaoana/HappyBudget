using HappyBudget.Controllers;
using HappyBudget.Models;
using HappyBudget.Services.Dialog;
using HappyBudget.Services.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels.Transactions
{
    public class SelectCategoryViewModel : BaseViewModel
    {
        private IController _controller;
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private string _filter = string.Empty;

        public SelectCategoryViewModel(IController controller, INavigationService navigationService, IDialogService dialogService)
        {
            _controller = controller;
            _navigationService = navigationService;
            _dialogService = dialogService;
            Categories = new ObservableCollection<Category>();
            SelectedCategory = null;
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set { SetProperty(ref _selectedCategory, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set 
            { 
                SetProperty(ref _categories, value);
               
            }
        }

        
        public override async Task InitializeAsync(object parameter)
        {
            _filter = parameter.ToString();
            await GetCategories();
        }

        private async Task GetCategories()
        {
            var categories = await _controller.GetAllCategories();
            if (!string.IsNullOrEmpty(_filter))
            {
                categories = categories.Where(c => c.Type == _filter).ToList();
            }
            Categories = new ObservableCollection<Category>(categories);
            
        }

        public ICommand SelectCategoryCommand { get => new Command(() => SelectCategory()); }
        private async void SelectCategory()
        {
            if (SelectedCategory== null)
            {
                await _dialogService.DisplayAlert("Error", "Choose a category!", "Ok");
                return;
            }

            MessagingCenter.Send(this, "_category");
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
