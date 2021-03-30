using HappyBudget.Controllers;
using HappyBudget.Models;
using HappyBudget.Services.Dialog;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels.Transactions
{
    public class SelectAccountViewModel : BaseViewModel
    {
        private IDialogService _dialogService;
        private IController _controller;

        private ObservableCollection<Account> _accounts;
        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }

        private Account _selectedAccount;
        public Account SelectedAccount
                {
                    get { return _selectedAccount; }
                    set { SetProperty(ref _selectedAccount, value); }
                }

        public override async Task InitializeAsync(object parameter)
        {
            await GetAccounts();
        }


        public SelectAccountViewModel(IDialogService dialogService, IController controller)
        {
            _controller = controller;
            _dialogService = dialogService;
            Accounts = new ObservableCollection<Account>();
            SelectedAccount = null;
        }

        public async Task GetAccounts()
        {
            
            var accounts = await _controller.GetAllAccounts();
            Accounts = new ObservableCollection<Account>(accounts);
            
        }

        public ICommand SelectAccountCommand { get => new Command(() => SelectAccount()); }
        private async void SelectAccount()
        {
            if (SelectedAccount == null)
            {
                await _dialogService.DisplayAlert("Error", "Choose an account!", "Ok");
                return;
            }

            MessagingCenter.Send(this, "_account");
            await PopupNavigation.Instance.PopAsync(true);
        }

    }
}
