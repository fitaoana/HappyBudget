using HappyBudget.Controllers;
using HappyBudget.Models;
using HappyBudget.Views.Statistics;
using Microcharts;
using Rg.Plugins.Popup.Extensions;
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
    public class ExpensesStatisticsViewModel : BaseViewModel
    {
        private IController _controller;


        private Chart _categoryChart;
        public Chart CategoryChart
        {
            get => _categoryChart;
            set { SetProperty(ref _categoryChart, value); }
        }

        private Chart _cashFlowChart;
        public Chart CashFlowChart
        {
            get => _cashFlowChart;
            set { SetProperty(ref _cashFlowChart, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }

        private decimal? _expenses;
        public decimal? Expenses
        {
            get => _expenses;
            set { SetProperty(ref _expenses, value); }
        }

        private decimal? _incomes;
        public decimal? Incomes
        {
            get => _incomes;
            set { SetProperty(ref _incomes, value); }
        }

        private int _filterDays;
        public int FilterDays
        {
            get => _filterDays;
            set { SetProperty(ref _filterDays, value); }
        }

        private string _filterName;
        public string FilterName
        {
            get => _filterName;
            set { SetProperty(ref _filterName, value); }
        }

        private PeriodFilter _filter;
        public PeriodFilter Filter
        {
            get => _filter;
            set { SetProperty(ref _filter, value); }
        }

        private int _topTransactionsHeight;
        public int TopTransactionsHeight
        {
            get => _topTransactionsHeight;
            set { SetProperty(ref _topTransactionsHeight, value); }
        }

        private ObservableCollection<Transaction> _topTransactions;
        public ObservableCollection<Transaction> TopTransactions
        {
            get => _topTransactions;
            set
            {
                SetProperty(ref _topTransactions, value);
                if (_topTransactions == null)
                {
                    return;
                }
                if (_topTransactions.Count == 0)
                {
                    TopTransactionsHeight = 0;
                    return;
                }
                TopTransactionsHeight = _topTransactions.Count * 75;
            }
        }

        private ObservableCollection<Category> _topCategories;
        public ObservableCollection<Category> TopCategories
        {
            get => _topCategories;
            set
            {
                SetProperty(ref _topCategories, value);
                if (_topCategories == null)
                {
                    return;
                }
                if (_topCategories.Count == 0)
                {
                    TopCategoriesHeight = 0;
                    return;
                }
                TopCategoriesHeight = _topCategories.Count * 75;
            }
        }

        private int _topCategoriesHeight;
        public int TopCategoriesHeight
        {
            get => _topCategoriesHeight;
            set { SetProperty(ref _topCategoriesHeight, value); }
        }

        private bool _isCategoryChartVisible;
        public bool IsCategoryChartVisible
        {
            get => _isCategoryChartVisible;
            set { SetProperty(ref _isCategoryChartVisible, value); }
        }

        private bool _isCashFlowChartVisible;
        public bool IsCashFlowChartVisible
        {
            get => _isCashFlowChartVisible;
            set { SetProperty(ref _isCashFlowChartVisible, value); }
        }

        public ExpensesStatisticsViewModel(IController controller)
        {
            _controller = controller;
            TopTransactions = new ObservableCollection<Transaction>();
            TopCategories = new ObservableCollection<Category>();
            RecieveMessage();
        }

        public ICommand RefreshExpensesStatisticsCommand { get => new Command(async () => await InitializeAsync(true)); }

        public override async Task InitializeAsync(object parameter)
        {

            IsBusy = true;
            IsRefreshing = true;

            if (Filter == null)
            {
                FilterDays = PeriodFilter.GetPeriodFilters().First(f => f.NumberOfDays == 30).NumberOfDays;
                FilterName = PeriodFilter.GetPeriodFilters().First(f => f.NumberOfDays == 30).Name;
            }

            var categories = await _controller.GetExpensesByCategory(DateTime.Now - TimeSpan.FromDays(FilterDays));

            TopCategories = new ObservableCollection<Category>(categories.OrderByDescending(c => c.Spendings).Take(5));

            //var transactionsTypes = await _controller.GetTransactionsByType(DateTime.Now - TimeSpan.FromDays(_filter));

            var expenses = await _controller.GetExpensesByDate(DateTime.Now - TimeSpan.FromDays(FilterDays));
            var incomes = await _controller.GetIncomesByDate(DateTime.Now - TimeSpan.FromDays(FilterDays));

            TopTransactions = new ObservableCollection<Transaction>(expenses.OrderByDescending(t => t.Amount).Take(5));

            if (categories.Count == 0) /*| transactions.Count == 0*/
            {
                IsCategoryChartVisible = false;
            }
            else
            {
                IsCategoryChartVisible = true;
                //TopTransactions = new ObservableCollection<Transaction>(transactions.Where(t => t.Type == AppConstants.IS_EXPENSE).OrderByDescending(t => t.Amount).Take(5));
                BuildCategoryChart(categories);
                //BuildCashFlowChart(transactions);
            }

            if (expenses.Count == 0 || incomes.Count == 0) /*| transactions.Count == 0*/
            {
                IsCashFlowChartVisible = false;
            }
            else
            {
                IsCashFlowChartVisible = true;
                

                BuildCashFlowChart(expenses, incomes);
            }

            IsRefreshing = false;
            IsBusy = false;

        }

        private void BuildCategoryChart(List<Category> categories)
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            foreach (var item in categories)
            {
                entries.Add(new ChartEntry((float)item.Spendings)
                {
                    TextColor = SKColor.Parse(item.Color),
                    ValueLabel = item.Name,
                    Color = SKColor.Parse(item.Color),
                    //assign color to this property
                    ValueLabelColor = SKColor.Parse(item.Color),
                });
            }

            var categoryChart = new DonutChart
            {
                Entries = entries,
                HoleRadius = 0.60f,
                LabelTextSize = 18,
                GraphPosition = GraphPosition.Center,
                LabelMode = LabelMode.LeftAndRight
            };

            CategoryChart = categoryChart;
        }

        private void BuildCashFlowChart(List<Transaction> expenses, List<Transaction> incomes)
        {
            var expensesAmount = expenses?.Sum(t => t.Amount);
            var incomesAmount = incomes?.Sum(t => t.Amount);

            var expense = expenses?.FirstOrDefault(t => t.Type == AppConstants.IS_EXPENSE);
            var income = incomes?.FirstOrDefault(t => t.Type == AppConstants.IS_INCOME);

            string expenseColor;
            string incomeColor;

            if (expense == null || income == null)
            {
                expenseColor = incomeColor = "#FFFFFF";
            }
            else
            {
                expenseColor = expense.Color;
                incomeColor = income.Color;
            }

            List<ChartEntry> entries = new List<ChartEntry>();

            entries.Add(new ChartEntry((float)expensesAmount)
            {
                TextColor = SKColor.Parse(expenseColor),
                ValueLabel = "Expenses",
                Color = SKColor.Parse(expenseColor),
                //assign color to this property
                ValueLabelColor = SKColor.Parse(expenseColor),
            });

            entries.Add(new ChartEntry((float)incomesAmount)
            {
                TextColor = SKColor.Parse(incomeColor),
                ValueLabel = "Incomes",
                Color = SKColor.Parse(incomeColor),
                //assign color to this property
                ValueLabelColor = SKColor.Parse(incomeColor),
            });


            var cashFlowChart = new RadialGaugeChart
            {
                Entries = entries,
                LabelTextSize = 18,
                LineSize = 50
            };

            CashFlowChart = cashFlowChart;
            Expenses = expensesAmount;
            Incomes = incomesAmount;
        }

        public ICommand FilterStatisticsCommand { get => new Command(() => FilterStatistics()); }
        private async void FilterStatistics()
        {
            await Shell.Current.Navigation.PushPopupAsync(new SelectFilterPopupPage());

        }

        private void RecieveMessage()
        {
            MessagingCenter.Subscribe<SelectFilterViewModel>(this, "_periodFilter", (s) =>
            {
                if (s != null)
                {
                    Filter = s.SelectedFilter;
                    FilterDays = s.SelectedFilter.NumberOfDays;
                    FilterName = s.SelectedFilter.Name;
                    IsRefreshing = true;

                }
                
                   
                
            });
        }

    }
}
