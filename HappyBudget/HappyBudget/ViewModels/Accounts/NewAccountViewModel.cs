 using HappyBudget.Models;
using HappyBudget.Services.Database;
using HappyBudget.Services.Dialog;
using HappyBudget.Services.Navigation;
using HappyBudget.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using HappyBudget.Views.Accounts;

namespace HappyBudget.ViewModels.Accounts
{
    [QueryProperty("Id", "id")]
    public class NewAccountViewModel : BaseViewModel
    {
        private IDatabaseService<Account> _databaseService;
        private INavigationService _navigationService;
        private IDialogService _dialogService;

        private string _accountName;
        public string AccountName
        {
            get => _accountName;
            set { SetProperty(ref _accountName, value); }
        }

        private decimal _accountBalance; //INITIAL BALANCE SCHIMBA
        public decimal AccountBalance
        {
            get => _accountBalance;
            set { SetProperty(ref _accountBalance, value); }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set { SetProperty(ref _isChecked, value); }
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

        private string _accountColor;
        public string AccountColor
        {
            get { return _accountColor; }
            set { SetProperty(ref _accountColor, value); }
        }

        public NewAccountViewModel(IDatabaseService<Account> databaseService, INavigationService navigationService, IDialogService dialogService)
        {
            _databaseService = databaseService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            
            RecieveMessage();
        }

        public async override Task InitializeAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Id) || !int.TryParse(Id, out int accountId))
            {
                IsChecked = true;
                return;
            }

            var selectedAccount = await _databaseService.GetById(accountId);
            IsChecked = selectedAccount.Type == AppConstants.IS_CARD;
            AccountBalance = selectedAccount.Balance;
            AccountName = selectedAccount.Name;
           
            AccountColor = selectedAccount.Color;
         

        }

        public ICommand AddAccountCommand { get => new Command(async () => await AddAccount(), () => IsNotBusy); }
        private async Task AddAccount()
        {
            //Salveaza in baza de date
            IsBusy = true;
            await SaveAccount();
           
            await _navigationService.PopAsync(); //Navigheaza spre pagina anterioara
            IsBusy = false;
        }

        private async Task SaveAccount()
        {
            var account = new Account
            {
               Id = string.IsNullOrEmpty(Id) ? 0 : int.Parse(Id),
               Name = AccountName,
               Balance = AccountBalance,
               Type = IsChecked == true ? AppConstants.IS_CARD : AppConstants.IS_CASH,
               Image = IsChecked == true ? AppConstants.CARD_IMAGE : AppConstants.CASH_IMAGE,
                Color = AccountColor
            };
            
            await _databaseService.SaveAsync(account);
        }

        //public ICommand DeleteAccountCommand { get => new Command(async () => await DeleteAccount()); }
        //private async Task DeleteAccount()
        //{
        //    if (string.IsNullOrEmpty(Id))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        var currentAccountId = int.Parse(Id);
        //        CurrentAccount = _databaseService.GetById(currentAccountId).Result;
        //        var response = await _dialogService.DisplayAlert("Delete", "Are you sure you want to delete this account?", "Yes", "No");

        //        if (response == true)
        //        {
        //            await _databaseService.DeleteAsync(CurrentAccount);
        //            await _navigationService.PopAsync();
        //        }

        //        return;
        //    }
        //}

        private void RecieveMessage()
        {
            MessagingCenter.Subscribe<SelectColorViewModel>(this, "_color", (s) =>
            {
                if (s != null)
                {
                    AccountColor = s.SelectedColor?.HexCode;
                }
            });
        }

        public ICommand ShowColorsCommand { get => new Command(() => ShowColors()); }
        private async void ShowColors()
        {
            await Shell.Current.Navigation.PushPopupAsync(new SelectColorPopupPage());
        }
    }
}
