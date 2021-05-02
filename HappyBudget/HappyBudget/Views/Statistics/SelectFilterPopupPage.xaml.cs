using Autofac;
using HappyBudget.ViewModels.Statistics;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappyBudget.Views.Statistics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectFilterPopupPage : PopupPage
    {
        public SelectFilterPopupPage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<SelectFilterViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as SelectFilterViewModel).InitializeAsync(null);
        }
    }
}