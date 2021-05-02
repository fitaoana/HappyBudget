using Autofac;
using HappyBudget.ViewModels.Statistics;
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
    public partial class StatisticsPage : TabbedPage
    {
        public StatisticsPage()
        {
            InitializeComponent();
        }
    }
}