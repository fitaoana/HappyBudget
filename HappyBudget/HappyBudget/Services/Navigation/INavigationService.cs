using HappyBudget.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HappyBudget.Services.Navigation
{
    public interface INavigationService
    {   
        void GoToMainFlow();
        void GoToLoginFlow();
        Task PopAsync();
        Task PushAsync<T>(string parameters = null) where T : BaseViewModel;
        Task InsertAsRoot<T>(string parameters = null) where T : BaseViewModel;
    }
}
