using Autofac;
using HappyBudget.Views;
using HappyBudget.Views.Accounts;
using HappyBudget.Views.Categories;
using HappyBudget.Views.Statistics;
using HappyBudget.Views.Transactions;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HappyBudget
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<AppShellViewModel>();
            Routing.RegisterRoute("NewAccountViewModel", typeof(NewAccountPage));
            Routing.RegisterRoute("SelectColorViewModel", typeof(SelectColorPopupPage));

            Routing.RegisterRoute("NewTransactionViewModel", typeof(NewTransactionPage));
            Routing.RegisterRoute("SelectAccountViewModel", typeof(SelectAccountPopupPage));

            Routing.RegisterRoute("NewCategoryViewModel", typeof(NewCategoryPage));
            Routing.RegisterRoute("SelectImageViewModel", typeof(SelectImagePopupPage));
            Routing.RegisterRoute("SelectCategoryViewModel", typeof(SelectIncomeCategoryPopupPage));
            //Routing.RegisterRoute("SelectColorViewModel", typeof(SelectCategoryColorPopupPage));

            Routing.RegisterRoute("SelectFilterViewModel", typeof(SelectFilterPopupPage));
        }

        
    }
}
