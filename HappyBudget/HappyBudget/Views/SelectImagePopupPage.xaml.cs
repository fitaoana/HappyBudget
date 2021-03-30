using Autofac;
using HappyBudget.ViewModels;
using HappyBudget.ViewModels.Categories;
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
    public partial class SelectImagePopupPage : PopupPage
    {
        public SelectImagePopupPage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<SelectImageViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as SelectImageViewModel).InitializeAsync(null);
        }
    }
}