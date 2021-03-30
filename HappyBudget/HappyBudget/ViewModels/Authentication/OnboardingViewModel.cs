using HappyBudget.Models;
using HappyBudget.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels.Authentication
{
    public class OnboardingViewModel : BaseViewModel
    {
        private INavigationService _navigationService;

        public ObservableCollection<OnboardingItem> OnboardingItems { get; set; }
        

        public OnboardingViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            OnboardingItems = new ObservableCollection<OnboardingItem>(OnboardingItem.GetOnboardingItems());
        }

        public ICommand SkipCommand { get => new Command(async () => await Skip()); }
        private async Task Skip()
        {
            await _navigationService.InsertAsRoot<LoginViewModel>();
        }
    }
}
