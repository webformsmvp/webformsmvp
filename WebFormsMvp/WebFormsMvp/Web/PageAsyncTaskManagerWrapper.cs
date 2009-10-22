using System;
using System.Web.UI;
using System.Web;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a class that wraps the page's async task methods
    /// </summary>
    public class PageAsyncTaskManagerWrapper : IAsyncTaskManager
    {
        readonly Page page;

        public PageAsyncTaskManagerWrapper(Page page)
        {
            this.page = page;
        }

        public void ExecuteRegisteredAsyncTasks()
        {
            page.ExecuteRegisteredAsyncTasks();
        }

        /// <summary>
        /// Registers the async task with the page.
        /// </summary>
        /// <param name="beginHandler">The begin handler.</param>
        /// <param name="endHandler">The end handler.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="state">The state.</param>
        /// <param name="executeInParallel">if set to <c>true</c> the task will be executed in parallel with other registered async page tasks.</param>
        public void RegisterAsyncTask(BeginEventHandler beginHandler, EndEventHandler endHandler, EndEventHandler timeout, object state, bool executeInParallel)
        {
            page.RegisterAsyncTask(new PageAsyncTask(beginHandler, endHandler, timeout, state, executeInParallel));
        }
    }
}