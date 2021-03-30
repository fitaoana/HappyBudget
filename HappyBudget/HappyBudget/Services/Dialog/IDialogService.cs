using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HappyBudget.Services.Dialog
{
    public interface IDialogService
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
        Task<string> DisplayPrompt(string title, string message, string accept, string cancel);
        Task<string> DisplayActionSheet(string title, string destruction, params string[] buttons);
    }
}
