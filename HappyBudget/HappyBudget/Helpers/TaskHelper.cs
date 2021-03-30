using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HappyBudget.Helpers
{

    public static class TaskHelper
    {
        public static async void FireAndForget(this Task task, bool returnToCallingContext, Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
