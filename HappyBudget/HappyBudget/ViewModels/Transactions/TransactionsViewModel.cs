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

namespace HappyBudget.ViewModels.Transactions
{
    public class TransactionsViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IDatabaseService<Transaction> _databaseService;
        private IDialogService _dialogService;
        private IController _controller;

        public TransactionsViewModel(INavigationService navigationService, IController controller, IDatabaseService<Transaction> databaseService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;
            _controller = controller;
            _dialogService = dialogService;
            Transactions = new ObservableCollection<Transaction>();
        }

        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set { SetProperty(ref _transactions, value); }
        }

        private Transaction _selectedTransaction;
        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set { SetProperty(ref _selectedTransaction, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }

        private string _accountName;
        public string AccountName
        {
            get => _accountName;
            set { SetProperty(ref _accountName, value); }
        }

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set { SetProperty(ref _categoryName, value); }
        }

        public override async Task InitializeAsync(object parameter)
        {
            await GetTransactions();
        }

        public ICommand RefreshTransactionsCommand { get => new Command(async () => await RefreshTransactions()); }
        private async Task RefreshTransactions()
        {
            await GetTransactions();
        }

        public ICommand NewTransactionCommand { get => new Command(async () => await NewTransaction()); }
        private async Task NewTransaction()
        {
            await _navigationService.PushAsync<NewTransactionViewModel>();
        }

        public ICommand SelectTransactionCommand { get => new Command(async () => await SelectTransaction()); }
        private async Task SelectTransaction()
        {
            if (SelectedTransaction == null)
            {
                return;
            }
            await _navigationService.PushAsync<NewTransactionViewModel>($"id={SelectedTransaction.Id}");
            SelectedTransaction = null;
        }

        public async Task GetTransactions()
        {
            IsRefreshing = true;
            var transactions = await _controller.GetAllTransactions();
            Transactions = new ObservableCollection<Transaction>(transactions);
            IsRefreshing = false;
        }

        public ICommand DeleteTransactionCommand { get => new Command<Transaction>(async (transaction) => await DeleteTransaction(transaction)); }
        public async Task DeleteTransaction(Transaction transaction)
        {
            if (Transactions.Contains(transaction))
            {
                var response = await _dialogService.DisplayAlert("Delete", "Are you sure you want to delete this transaction?", "Yes", "No");
                if (response == true)
                {
                    await _databaseService.DeleteAsync(transaction);
                }
            }
            IsRefreshing = true;
            return;

        }
    }
    }
