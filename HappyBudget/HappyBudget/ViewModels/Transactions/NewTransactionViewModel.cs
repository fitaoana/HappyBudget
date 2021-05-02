using HappyBudget.Controllers;
using HappyBudget.Models;
using HappyBudget.Services.Database;
using HappyBudget.Services.Dialog;
using HappyBudget.Services.Navigation;
using HappyBudget.Views;
using HappyBudget.Views.Transactions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels.Transactions
{
    [QueryProperty("Id", "id")]
    public class NewTransactionViewModel : BaseViewModel
    {
        private IDatabaseService<Transaction> _databaseServiceTransaction;
        private IDatabaseService<Account> _databaseServiceAccount;
        private IDatabaseService<Category> _databaseServiceCategory;
        private INavigationService _navigationService;
        private IDialogService _dialogService;

        private Account _transactionAccount;
        public Account TransactionAccount
        {
            get { return _transactionAccount; }
            set { SetProperty(ref _transactionAccount, value); }
        }

        private Category _transactionCategory;
        public Category TransactionCategory
        {
            get { return _transactionCategory; }
            set { SetProperty(ref _transactionCategory, value); }
        }


        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set { SetProperty(ref _isChecked, value); }
        }

        private DateTime _transactionDate;
        public DateTime TransactionDate
        {
            get => _transactionDate;
            set { SetProperty(ref _transactionDate, value); }
        }

        private decimal _transactionAmount;
        public decimal TransactionAmount
        {
            get => _transactionAmount;
            set { SetProperty(ref _transactionAmount, value); }
        }

        public NewTransactionViewModel(IDatabaseService<Transaction> databaseServiceTransaction, IDatabaseService<Account> databaseServiceAccount, IDatabaseService<Category> databaseServiceCategory, INavigationService navigationService, IDialogService dialogService)
        {
            _databaseServiceTransaction = databaseServiceTransaction;
            _databaseServiceAccount = databaseServiceAccount;
            _databaseServiceCategory = databaseServiceCategory;
            _navigationService = navigationService;
            _dialogService = dialogService;

            RecieveMessage();
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

        private string _transactionAccountName;
        public string TransactionAccountName
        {
            get { return _transactionAccountName; }
            set { SetProperty(ref _transactionAccountName, value); }
        }

        private string _transactionCategoryName;
        public string TransactionCategoryName
        {
            get { return _transactionCategoryName; }
            set { SetProperty(ref _transactionCategoryName, value); }
        }

        private void RecieveMessage()
        {
            MessagingCenter.Subscribe<SelectAccountViewModel>(this, "_account", (s) =>
            {
                if (s != null)
                {
                    TransactionAccount = s.SelectedAccount;
                    TransactionAccountName = s.SelectedAccount?.Name;
                }
            });

            MessagingCenter.Subscribe<SelectCategoryViewModel>(this, "_category", (s) =>
            {
                if (s != null)
                {
                    TransactionCategoryName = s.SelectedCategory?.Name;
                    TransactionCategory = s.SelectedCategory;
                }
            });
        }

        public ICommand ShowAccountsCommand { get => new Command(() => ShowAccounts()); }
        private async void ShowAccounts()
        {
            await Shell.Current.Navigation.PushPopupAsync(new SelectAccountPopupPage());
        }

        public ICommand ShowCategoriesCommand { get => new Command(() => ShowCategories()); }
        private async void ShowCategories()
        {
            if (IsChecked == true)
            {
                await Shell.Current.Navigation.PushPopupAsync(new SelectIncomeCategoryPopupPage());
            }
            else
            {
                await Shell.Current.Navigation.PushPopupAsync(new SelectExpenseCategoryPopupPage());
            }

        }

        public ICommand AddTransactionCommand { get => new Command(async () => await AddTransaction()); }
        private async Task AddTransaction()
        {

            if (TransactionAccount == null && TransactionCategory == null)
            {
                await _dialogService.DisplayAlert("Error", "Please select an account and a category!", "Ok");
                return;
            }
            else if (TransactionAccount == null)
            {
                await _dialogService.DisplayAlert("Error", "Please select an account!", "OK");
            }
            else if (TransactionCategory == null)
            {
                await _dialogService.DisplayAlert("Error", "Please select a category!", "OK");
            }
            IsBusy = true;
            await SaveTransaction();

            await _navigationService.PopAsync();
            IsBusy = false;
        }

        private async Task SaveTransaction()
        {
            var transaction = new Transaction
            {
                Id = string.IsNullOrEmpty(Id) ? 0 : int.Parse(Id),
                Amount = TransactionAmount,
                Date = TransactionDate,
                Type = IsChecked == true ? AppConstants.IS_INCOME : AppConstants.IS_EXPENSE,
                Image = IsChecked == true ? AppConstants.INCOME_IMAGE : AppConstants.EXPENSE_IMAGE,
                Color = IsChecked == true ? AppConstants.INCOME_COLOR : AppConstants.EXPENSE_COLOR,
                Account = TransactionAccount.Name,
                Category = TransactionCategoryName,
                AccountId = TransactionAccount.Id,
                CategoryId = TransactionCategory.Id

                //UserEmail = user
            };
            await _databaseServiceTransaction.SaveAsync(transaction);
        }

        public async override Task InitializeAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Id) || !int.TryParse(Id, out int transactionId))
            {
                IsChecked = true;
                TransactionDate = DateTime.Now;
                return;
            }

            var selectedTransaction = await _databaseServiceTransaction.GetById(transactionId);
            IsChecked = selectedTransaction.Type == AppConstants.IS_INCOME;
            TransactionAccount = await _databaseServiceAccount.GetById(selectedTransaction.AccountId);
            TransactionCategory = await _databaseServiceCategory.GetById(selectedTransaction.CategoryId);
            TransactionAccountName = TransactionAccount.Name;
            TransactionCategoryName = TransactionCategory.Name;
            TransactionDate = selectedTransaction.Date;
            TransactionAmount = selectedTransaction.Amount;
        }
    }
}
 