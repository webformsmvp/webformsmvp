using System;
using Rhino.Mocks.Interfaces;

namespace WebFormsMvp.Testing
{
    public static class MockHelpers
    {
        /// <summary>
        /// Executes the async callback when the method is called. Use this in conjunction with the TestAsyncTaskManager.
        /// </summary>
        public static IMethodOptions<IAsyncResult> ExecuteAsyncCallback(this IMethodOptions<IAsyncResult> methodOptions)
        {
            methodOptions.WhenCalled(m => new Action(() => { }).BeginInvoke(m.Arguments[1] as AsyncCallback, m.Arguments[2]));
            return methodOptions;
        }
    }
}