using HappyBudget.Controllers;
using HappyBudget.Models;
using HappyBudget.Services.Database;
using HappyBudget.Services.Dialog;
using HappyBudget.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels.Categories
{
    public class CategoriesViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private IDatabaseService<Category> _databaseService;
        private IController _controller;

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set { SetProperty(ref _selectedCategory, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set { SetProperty(ref _categories, value); }
        }

        public CategoriesViewModel(INavigationService navigationService, IDialogService dialogService, IDatabaseService<Category> databaseService, IController controller)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _controller = controller;
            _databaseService = databaseService;
            Categories = new ObservableCollection<Category>();
        }

        public override async Task InitializeAsync(object parameter)
        {
            await GetCategories();
        }

        public ICommand NewCategoryCommand { get => new Command(async () => await NewCategory()); }
        private async Task NewCategory()
        {
            await _navigationService.PushAsync<NewCategoryViewModel>();
        }

        public ICommand SelectCategoryCommand { get => new Command(async () => await SelectCategory()); }
        private async Task SelectCategory()
        {
            if (SelectedCategory == null)
            {
                return;
            }
            await _navigationService.PushAsync<NewCategoryViewModel>($"id={SelectedCategory.Id}");
            SelectedCategory = null;
        }

        public ICommand RefreshCategoriesCommand { get => new Command(async () => await RefreshCategories()); }
        private async Task RefreshCategories()
        {
            await GetCategories();
        }

        public async Task GetCategories()
        {
            IsRefreshing = true;
            var categories = await _controller.GetAllCategories();
            Categories = new ObservableCollection<Category>(categories);
            IsRefreshing = false;
        }

        public ICommand DeleteCategoryCommand { get => new Command<Category>(async (category) => await DeleteCategory(category)); }
        public async Task DeleteCategory(Category category)
        {
            if (Categories.Contains(category))
            {
                var response = await _dialogService.DisplayAlert("Delete", "Are you sure you want to delete this category?", "Yes", "No");
                if (response == true)
                {
                    await _databaseService.DeleteAsync(category);
                }
            }
            IsRefreshing = true;
            return;
        }

        public async Task SaveDefaultCategories()
        {
            
            var defaultCategories = Category.GetDefaultCategories();

                foreach (var item in defaultCategories)
            {
                if (Categories.Contains(item))
                {
                    return;
                }
                else
                {
                    await _databaseService.SaveAsync(item);
                }
                
            }
        }
    }
}
