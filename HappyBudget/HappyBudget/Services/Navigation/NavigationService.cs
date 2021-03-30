using HappyBudget.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HappyBudget.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public void GoToLoginFlow()
        {
            throw new NotImplementedException();
        }

        public void GoToMainFlow()
        {
            throw new NotImplementedException();
        }

        public Task PopAsync()
        {
            return Shell.Current.Navigation.PopAsync();
        }

        //Add the page in the navigation stack
        public Task PushAsync<T>(string parameters = null) where T : BaseViewModel
        {
            return GoToAsync<T>("", parameters);
        }

        //Navigate to the page and insert it as root
        public Task InsertAsRoot<T>(string parameters = null) where T : BaseViewModel
        {
            return GoToAsync<T>("//", parameters);
        }

        //Build the route
        private Task GoToAsync<T>(string prefix, string parameters) where T : BaseViewModel
        {
            var route = prefix + typeof(T).Name;
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                route += $"?{parameters}";
            }
            return Shell.Current.GoToAsync(route);
        }
    }
}
