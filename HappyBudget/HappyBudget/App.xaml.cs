using Autofac;
using HappyBudget.Models;
using HappyBudget.Services.Database;
using System;
using System.Reflection;
using Xamarin.Forms;


namespace HappyBudget
{
    public partial class App : Application
    {
        public static IContainer Container;

        public App()
        {
            InitializeComponent();
            
            var builder = new ContainerBuilder();
            var component = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(component).AsImplementedInterfaces().AsSelf();
            builder.RegisterType<DatabaseService<Account>>().As<IDatabaseService<Account>>();
            builder.RegisterType<DatabaseService<Category>>().As<IDatabaseService<Category>>();
            //builder.RegisterType<DatabaseService<Currency>>().As<IDatabaseService<Currency>>();
            builder.RegisterType<DatabaseService<Transaction>>().As<IDatabaseService<Transaction>>();
            Container = builder.Build();
            
            MainPage = new AppShell();
        }
    }
}
