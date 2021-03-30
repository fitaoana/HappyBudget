using Autofac;
using HappyBudget.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappyBudget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectColorPopupPage : PopupPage
    {
        public SelectColorPopupPage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<SelectColorViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as SelectColorViewModel).InitializeAsync(null);
        }
    }
}