using Autofac;

using HappyBudget.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappyBudget.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAccountPage : ContentPage
    {
        public NewAccountPage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<NewAccountViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as NewAccountViewModel).InitializeAsync(null);
        }

       
    }
}