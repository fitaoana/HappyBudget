using HappyBudget.Models;
using HappyBudget.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyBudget.Controllers
{
    class Controller : IController
    {
        private IDatabaseService<Account> _databaseServiceAccount;
        private IDatabaseService<Transaction> _databaseServiceTransaction;
        private IDatabaseService<Category> _databaseServiceCategory;

        public Controller(IDatabaseService<Account> databaseServiceAccount, IDatabaseService<Transaction> databaseServiceTransaction, IDatabaseService<Category> databaseServiceCategory)
        {
            _databaseServiceAccount = databaseServiceAccount;
            _databaseServiceTransaction = databaseServiceTransaction;
            _databaseServiceCategory = databaseServiceCategory;
        }

        //Accounts
        //public async Task<List<Account>> GetAllAccounts(bool reload = false)
        //{
        //    List<Account> accounts = await LoadAccounts(reload);

        //    if (accounts.Count == 0)
        //    {
        //        return accounts;
        //    }

        //    return accounts;
        //}

        public async Task<List<Account>> GetAllAccounts(bool reload = false)
        {
            List<Transaction> transactions = await LoadTransactions(reload);

            List<Account> accounts = await LoadAccounts(reload);

            var result = new List<Account>();
            if (accounts.Count == 0)
            {
                return result;
            }
            else if (transactions.Count == 0)
            {
                return accounts;
            }

            foreach (var item in accounts)
            {
                decimal expenses = 0;
                decimal incomes = 0;

                var groupedTransactions = transactions.GroupBy(t => t.AccountId);

                foreach (var group in groupedTransactions)
                {
                    
                    if(group.Key == item.Id)
                    {
                        expenses = group.Where(g => g.Type == AppConstants.IS_EXPENSE).Sum(g => g.Amount);
                        incomes = group.Where(g => g.Type == AppConstants.IS_INCOME).Sum(g => g.Amount);
                    }
                    
                    //var account = accounts.FirstOrDefault(c => c.Id == group.Key);
                }

                //if (account == null)
                //{
                //    return result;
                //}
                //else
                //{
                //    var newAccount = new Account
                //    {
                //        Id = account.Id,
                //        Balance = account.Balance + incomes - expenses,
                //        Name = account.Name,
                //        Color = account.Color,
                //        Image = account.Image,
                //        Type = account.Type
                //    };
                //    //await _databaseServiceAccount.SaveAsync(newAccount);
                //    result.Add(newAccount);
                //}
                var newAccount = new Account
                {
                    Id = item.Id,
                    Balance = item.Balance + incomes - expenses,
                    Name = item.Name,
                    Color = item.Color,
                    Image = item.Image,
                    Type = item.Type
                };
                result.Add(newAccount);
            }
            return result.OrderByDescending(c => c.Name).ToList();
        } 

           
        


    private async Task<List<Account>> LoadAccounts(bool reload)
        {
            var accounts = await _databaseServiceAccount.GetAllAsync();
            var defaultAccounts = Account.GetDefaultAccounts();
            if (accounts.Count == 0)
            {

                foreach (var item in defaultAccounts)
                {
                    if (!(accounts.Contains(item)))
                    {
                        await _databaseServiceAccount.SaveAsync(item);
                    }
                }

            }
            accounts = accounts.OrderBy(a => a.Name).ToList();
            return accounts;
        }

        //Transaction
        public async Task<List<Transaction>> GetExpensesByDate(DateTime date, bool reload = false)
        {
            List<Transaction> transactions = await LoadTransactions(reload);
            
            var result = new List<Transaction>();
            if (transactions.Count == 0)
            {
                return result;
            }
            var filteredTransactions = transactions.Where(t => t.Type == AppConstants.IS_EXPENSE && t.Date >= date.Date);

            if (filteredTransactions == null)
            {
                return result;
            }
            else
            {
                
                foreach (var item in filteredTransactions)
                {

                    var account = await _databaseServiceAccount.GetById(item.AccountId);
                    var category = await _databaseServiceCategory.GetById(item.CategoryId);

                    var newTransaction = new Transaction
                    {
                        Id = item.Id,
                        Amount = item.Amount,
                        Date = item.Date,
                        Color = item.Color,
                        Image = item.Image,
                        Type = item.Type,
                        Account = account.Name,
                        Category = category.Name,
                        CategoryId = item.CategoryId,
                        AccountId = item.AccountId
                    };
                    result.Add(newTransaction);
                }
            }

            return result.ToList();
        }

        public async Task<List<Transaction>> GetIncomesByDate(DateTime date, bool reload = false)
        {
            List<Transaction> transactions = await LoadTransactions(reload);

            var result = new List<Transaction>();
            if (transactions.Count == 0)
            {
                return result;
            }
            var filteredTransactions = transactions.Where(t => t.Type == AppConstants.IS_INCOME && t.Date >= date.Date);

            if (filteredTransactions == null)
            {
                return result;
            }
            else
            {

                foreach (var item in filteredTransactions)
                {
                    var account = await _databaseServiceAccount.GetById(item.AccountId);
                    var category = await _databaseServiceCategory.GetById(item.CategoryId);

                    var newTransaction = new Transaction
                    {
                        Id = item.Id,
                        Amount = item.Amount,
                        Date = item.Date,
                        Color = item.Color,
                        Image = item.Image,
                        Type = item.Type,
                        Account = account.Name,
                        Category = category.Name,
                        CategoryId = item.CategoryId,
                        AccountId = item.AccountId
                    };
                    result.Add(newTransaction);
                }
            }

            return result.ToList();
        }

        public async Task<List<Transaction>> GetAllTransactions(bool reload = false)
        {
            List<Transaction> transactions = await LoadTransactions(reload);
            //List<Account> accounts = await LoadAccounts(reload);
            //List<Category> categories = await LoadCategories(reload);

            var result = new List<Transaction>();

            if (transactions.Count == 0)
            {
                return result;
            }

            foreach (var item in transactions)
            {
                var account = await _databaseServiceAccount.GetById(item.AccountId);
                var category = await _databaseServiceCategory.GetById(item.CategoryId);

                var newTransaction = new Transaction
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    Date = item.Date,
                    Color = item.Color,
                    Image = item.Image,
                    Type = item.Type,
                    Account = account.Name,
                    Category = category.Name,
                    CategoryId = item.CategoryId,
                    AccountId = item.AccountId
                };
                result.Add(newTransaction);
            }
            return result.OrderByDescending(t => t.Date).ToList();
        }

        private async Task<List<Transaction>> LoadTransactions(bool reload)
        {
            var transactions = await _databaseServiceTransaction.GetAllAsync();
            transactions = transactions.OrderByDescending(t => t.Date).ToList();
            return transactions;
        }

        //Category
        public async Task<List<Category>> GetExpensesByCategory(DateTime date, bool reload = false)
        {
            List<Transaction> transactions = await LoadTransactions(reload);
            List<Category> categories = await LoadCategories(reload);

            var result = new List<Category>();
            if (transactions.Count == 0 || categories.Count == 0)
            {
                return result;
            }
            var filteredTransactions = transactions.Where(t => t.Type == AppConstants.IS_EXPENSE && t.Date >= date.Date);

            if (filteredTransactions == null)
            {
                return result;
            }
            else
            {
                var groupedTransactions = filteredTransactions.GroupBy(t => t.CategoryId);

                foreach (var group in groupedTransactions)
                {
                    var expenses = group.Where(g => g.Type == AppConstants.IS_EXPENSE).Sum(g => g.Amount);
                    var category = categories.FirstOrDefault(c => c.Id  == group.Key);
                    string color;
                    string name;
                    string image;

                    if (category == null)
                    {
                        color = "#FFFFFF";
                        name = string.Empty;
                        image = string.Empty;
                    }
                    else
                    {
                        color = category.Color;
                        name = category.Name;
                        image = category.Image;
                    }
                    var newCategory = new Category
                    {
                        
                        Spendings = expenses,
                        Name = name,
                        Color = color,
                        Image = image
                    };
                    result.Add(newCategory);
                }
            }
            
            return result.OrderByDescending(c => c.Spendings).ToList();
        }

        public async Task<List<Category>> GetAllCategories(bool reload = false)
        {
            List<Category> categories = await LoadCategories(reload);
           

            if (categories.Count == 0)
            {
                return categories;
            }

            return categories;
        }

        private async Task<List<Category>> LoadCategories(bool reload)
        {
            var categories = await _databaseServiceCategory.GetAllAsync();
            var defaultCategories = Category.GetDefaultCategories();
            if (categories.Count == 0)
            {
               
                    foreach (var item in defaultCategories)
                    {
                        if (!(categories.Contains(item)))
                        {
                            await _databaseServiceCategory.SaveAsync(item);
                        }
                    }
                
            }
                categories = categories.OrderBy(c => c.Type).ThenBy(c => c.Name).ToList();
            return categories;
        }

       
    }

    public interface IController
    {
        Task<List<Transaction>> GetAllTransactions(bool reload = false);
        Task<List<Account>> GetAllAccounts(bool reload = false);
        Task<List<Category>> GetAllCategories(bool reload = false);
        Task<List<Transaction>> GetExpensesByDate(DateTime date, bool reload = false);
        Task<List<Category>> GetExpensesByCategory(DateTime date, bool reload = false);
        Task<List<Transaction>> GetIncomesByDate(DateTime date, bool reload = false);
    }
}
