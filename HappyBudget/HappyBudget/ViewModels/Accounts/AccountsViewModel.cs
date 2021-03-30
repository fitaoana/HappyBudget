using HappyBudget.Controllers;
using HappyBudget.Models;
using HappyBudget.Services.Database;
using HappyBudget.Services.Dialog;
using HappyBudget.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels.Accounts
{
    public class AccountsViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IController _controller;
        private IDialogService _dialogService;
        private IDatabaseService<Account> _databaseService;

        private ObservableCollection<Account> _accounts;
        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set { SetProperty(ref _accounts, value); }
        }

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set { SetProperty(ref _selectedAccount, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }

        public AccountsViewModel(INavigationService navigationService, IDialogService dialogService, IController controller, IDatabaseService<Account> databaseService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _controller = controller;
            _databaseService = databaseService;
            Accounts = new ObservableCollection<Account>();
        }

        public override async Task InitializeAsync(object parameter)
        {
            await GetAccounts();
        }

        public ICommand NewAccountCommand { get => new Command(async () => await NewAccount()); }
        private async Task NewAccount()
        {
            await _navigationService.PushAsync<NewAccountViewModel>();
        }

        //Metoda ce paseaza id-ul tranzactiei selectata din view
        public ICommand SelectAccountCommand { get => new Command(async () => await SelectAccount()); }
        private async Task SelectAccount()
        {
            if (SelectedAccount == null)
            {
                return;
            }
            await _navigationService.PushAsync<NewAccountViewModel>($"id={SelectedAccount.Id}");
            SelectedAccount = null;
        }

        public ICommand RefreshAccountsCommand { get => new Command(async () => await RefreshAccounts()); }
        private async Task RefreshAccounts()
        {
            await GetAccounts();
        }

        public async Task GetAccounts()
        {
            IsRefreshing = true;
            var accounts = await _controller.GetAllAccounts();
            Accounts = new ObservableCollection<Account>(accounts);
            IsRefreshing = false;
        }

        public ICommand DeleteAccountCommand { get => new Command<Account>(async (account) => await DeleteAccount(account)); }
        public async Task DeleteAccount(Account account)
        {
            if (Accounts.Contains(account))
            {
                var response = await _dialogService.DisplayAlert("Delete", "Are you sure you want to delete this account?", "Yes", "No");
                if (response == true)
                {
                    await _databaseService.DeleteAsync(account);
                }
            }
            IsRefreshing = true;
            return;
        }
    }
}
