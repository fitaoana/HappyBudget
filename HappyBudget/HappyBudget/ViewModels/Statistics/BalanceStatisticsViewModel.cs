using HappyBudget.Controllers;
using HappyBudget.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels.Statistics
{
    public class BalanceStatisticsViewModel : BaseViewModel
    {
        private IController _controller;

        private Chart _balanceChart;
        public Chart BalanceChart
        {
            get => _balanceChart;
            set { SetProperty(ref _balanceChart, value); }
        }

        private decimal _totalBalance;
        public decimal TotalBalance
        {
            get => _totalBalance;
            set { SetProperty(ref _totalBalance, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }

        private bool _isBalanceChartVisible;
        public bool IsBalanceChartVisible
        {
            get => _isBalanceChartVisible;
            set { SetProperty(ref _isBalanceChartVisible, value); }
        }

       

        private bool _isZeroBalanceAccountsVisible;
        public bool IsZeroBalanceAccountsVisible
        {
            get => _isZeroBalanceAccountsVisible;
            set { SetProperty(ref _isZeroBalanceAccountsVisible, value); }
        }


        private ObservableCollection<Account> _positiveBalanceAccounts;
        public ObservableCollection<Account> PositiveBalanceAccounts
        {
            get => _positiveBalanceAccounts;
            set
            {
                SetProperty(ref _positiveBalanceAccounts, value);
                if (_positiveBalanceAccounts == null)
                {
                    
                    return;
                }
                if (_positiveBalanceAccounts.Count == 0)
                {
                    
                    PositiveBalanceAccountsHeight = 0;
                    return;
                }
                
                PositiveBalanceAccountsHeight = _positiveBalanceAccounts.Count * 75;
            }
        }

        private int _positiveBalanceAccountsHeight;
        public int PositiveBalanceAccountsHeight
        {
            get => _positiveBalanceAccountsHeight;
            set { SetProperty(ref _positiveBalanceAccountsHeight, value); }
        }


        private ObservableCollection<Account> _zeroBalanceAccounts;
        public ObservableCollection<Account> ZeroBalanceAccounts
        {
            get => _zeroBalanceAccounts;
            set
            {
                SetProperty(ref _zeroBalanceAccounts, value);
                if (_zeroBalanceAccounts == null)
                {
                    IsZeroBalanceAccountsVisible = false;
                    return;
                }
                if (_zeroBalanceAccounts.Count == 0)
                {
                    IsZeroBalanceAccountsVisible = false;
                    ZeroBalanceAccountsHeight = 0;
                    return;
                }
                IsZeroBalanceAccountsVisible = true;
                ZeroBalanceAccountsHeight = _zeroBalanceAccounts.Count * 75;
            }
        }

        private int _zeroBalanceAccountsHeight;
        public int ZeroBalanceAccountsHeight
        {
            get => _zeroBalanceAccountsHeight;
            set { SetProperty(ref _zeroBalanceAccountsHeight, value); }
        }

        
        public BalanceStatisticsViewModel(IController controller)
        {
            _controller = controller;
            PositiveBalanceAccounts = new ObservableCollection<Account>();
            ZeroBalanceAccounts = new ObservableCollection<Account>();
        }

        public ICommand RefreshBalanceStatisticsCommand { get => new Command(async () => await InitializeAsync(true)); }

        public override async Task InitializeAsync(object parameter)
        {

            IsBusy = true;
            IsRefreshing = true;

            var accounts = await _controller.GetAllAccounts();

            TotalBalance = accounts.Sum(a => a.Balance);

            //TopCategories = new ObservableCollection<Category>(categories.OrderByDescending(c => c.Spendings).Take(5));
            PositiveBalanceAccounts = new ObservableCollection<Account>(accounts.Where(a => a.Balance > 0).OrderByDescending(a => a.Balance));
            ZeroBalanceAccounts = new ObservableCollection<Account>(accounts.Where(a => a.Balance == 0).OrderByDescending(a => a.Balance));

            if (PositiveBalanceAccounts.Count == 0) /*| transactions.Count == 0*/
            {
                IsBalanceChartVisible = false;
            }
            else
            {
                IsBalanceChartVisible = true;
                BuildBalanceChart(PositiveBalanceAccounts.ToList());
            }

            IsRefreshing = false;
            IsBusy = false;
        }

        private void BuildBalanceChart(List<Account> accounts)
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            foreach (var item in accounts)
            {
                entries.Add(new ChartEntry((float)item.Balance)
                {
                    TextColor = SKColor.Parse(item.Color),
                    ValueLabel = item.Name,
                    Color = SKColor.Parse(item.Color),
                    //assign color to this property
                    ValueLabelColor = SKColor.Parse(item.Color),
                });
            }

            var balanceChart = new DonutChart
            {
                Entries = entries,
                HoleRadius = 0.60f,
                LabelTextSize = 18,
                GraphPosition = GraphPosition.Center,
                LabelMode = LabelMode.LeftAndRight
            };

            BalanceChart = balanceChart;
        }
    }
}
