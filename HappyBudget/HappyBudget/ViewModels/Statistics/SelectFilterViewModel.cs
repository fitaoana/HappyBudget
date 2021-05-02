using HappyBudget.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyBudget.ViewModels.Statistics
{
    public class SelectFilterViewModel : BaseViewModel
    {
        private ObservableCollection<PeriodFilter> _periodFilters;
        public ObservableCollection<PeriodFilter> PeriodFilters
        {
            get => _periodFilters;
            set { SetProperty(ref _periodFilters, value); }
        }

        private PeriodFilter _selectedFilter;
        public PeriodFilter SelectedFilter
        {
            get => _selectedFilter;
            set { SetProperty(ref _selectedFilter, value); }
        }

        public SelectFilterViewModel()
        {
            PeriodFilters = new ObservableCollection<PeriodFilter>(PeriodFilter.GetPeriodFilters());
            SelectedFilter = null;
        }

        public ICommand SelectFilterCommand { get => new Command(() => SelectFilter()); }
        private async void SelectFilter()
        {
            if (SelectedFilter == null)
            {
                SelectedFilter = PeriodFilters.First(f => f.NumberOfDays == 30);
                return;
            }

            MessagingCenter.Send(this, "_periodFilter");
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}

