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
        public async Task<List<Account>> GetAllAccounts(bool reload = false)
        {
            List<Account> accounts = await LoadAccounts(reload);
            
            if (accounts.Count == 0)
            {
                return accounts;
            }
            
            return accounts;
        }

        private async Task<List<Account>> LoadAccounts(bool reload)
        {
            var accounts = await _databaseServiceAccount.GetAllAsync();
            var defaultAccounts = Account.GetDefaultAccounts();
            if (accounts.Count == 0)
            {

                foreach (var item in defaultAccounts)
                {
                    if (!accounts.Contains(item))
                    {
                        await _databaseServiceAccount.SaveAsync(item);
                    }
                }

            }
            accounts = accounts.OrderBy(a => a.Name).ToList();
            return accounts;
        }

        //Transaction
        public async Task<List<Transaction>> GetAllTransactions(bool reload = false)
        {
            List<Transaction> transactions = await LoadTransactions(reload);
            
            if (transactions.Count == 0)
            {
                return transactions;
            }

            return transactions;
        }

        private async Task<List<Transaction>> LoadTransactions(bool reload)
        {
            var transactions = await _databaseServiceTransaction.GetAllAsync();
            transactions = transactions.OrderByDescending(t => t.Date).ToList();
            return transactions;
        }

        //Category
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
                        if (!categories.Contains(item))
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
        
    }
}
