using HappyBudget.Controllers;
using HappyBudget.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HappyBudget
{
    public class AppShellViewModel : Xamarin.Forms.Shell
    {
        private INavigationService _navigationService;

        public AppShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
