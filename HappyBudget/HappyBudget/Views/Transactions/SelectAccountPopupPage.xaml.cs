using Autofac;
using HappyBudget.ViewModels.Transactions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappyBudget.Views.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectAccountPopupPage : PopupPage
    {
        public SelectAccountPopupPage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<SelectAccountViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as SelectAccountViewModel).InitializeAsync(null);
        }
    }
}