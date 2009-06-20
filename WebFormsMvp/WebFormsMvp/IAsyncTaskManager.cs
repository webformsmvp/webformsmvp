using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a class that can register async tasks to be completed for this page request
    /// </summary>
    public interface IAsyncTaskManager
    {
        void RegisterAsyncTask(BeginEventHandler beginHandler, EndEventHandler endHandler, EndEventHandler timeout, object state, bool executeInParallel);
    }
}