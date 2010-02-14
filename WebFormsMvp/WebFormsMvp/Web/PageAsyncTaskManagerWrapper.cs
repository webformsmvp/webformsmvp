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

        /// <summary />
        public PageAsyncTaskManagerWrapper(Page page)
        {
            this.page = page;
        }

        /// <summary>
        /// Starts the execution of an asynchronous task.
        /// </summary>
        public void ExecuteRegisteredAsyncTasks()
        {
            page.ExecuteRegisteredAsyncTasks();
        }

        /// <summary>
        /// Registers a new asynchronous task with the page.
        /// </summary>
        /// <param name="beginHandler">The handler to call when beginning an asynchronous task.</param>
        /// <param name="endHandler">The handler to call when the task is completed successfully within the time-out period.</param>
        /// <param name="timeout">The handler to call when the task is not completed successfully within the time-out period.</param>
        /// <param name="state">The object that represents the state of the task.</param>
        /// <param name="executeInParallel">The vlaue that indicates whether the task can be executed in parallel with other tasks.</param>
        public void RegisterAsyncTask(BeginEventHandler beginHandler, EndEventHandler endHandler, EndEventHandler timeout, object state, bool executeInParallel)
        {
            page.RegisterAsyncTask(new PageAsyncTask(beginHandler, endHandler, timeout, state, executeInParallel));
        }
    }
}