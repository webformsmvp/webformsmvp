using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Threading;

namespace WebFormsMvp.Testing
{
    /// <summary>
    /// Represents a class that can be used when writing unit tests for presenters that utilise an IAsyncTaskManager.
    /// Ensure you call ExecuteTasks() before releasing the view and making your assertions.
    /// </summary>
    public class TestAsyncTaskManager : IAsyncTaskManager
    {
        readonly List<PageAsyncTask> tasks = new List<PageAsyncTask>();

        /// <summary>
        /// Registers the async task.
        /// </summary>
        /// <param name="beginHandler">The begin handler.</param>
        /// <param name="endHandler">The end handler.</param>
        /// <param name="timeout">The timeout handler.</param>
        /// <param name="state">The state.</param>
        /// <param name="executeInParallel">Ignored for purposes of testing.</param>
        public void RegisterAsyncTask(BeginEventHandler beginHandler, EndEventHandler endHandler, EndEventHandler timeout, object state, bool executeInParallel)
        {
            tasks.Add(new PageAsyncTask(beginHandler, endHandler, timeout, state, executeInParallel));
        }

        /// <summary>
        /// Executes the registered tasks.
        /// </summary>
        public void ExecuteTasks()
        {
            var resetEvent = new AutoResetEvent(false);
            tasks.ForEach(t =>
            {
                var beginResult = t.BeginHandler.Invoke(this, new EventArgs(), result => resetEvent.Set(), null);
                resetEvent.WaitOne(); // Wait here to ensure that end handler is called after begin handler has completed
                t.EndHandler(beginResult);
            });
        }
    }
}