using System;
using WebFormsMvp.FeatureDemos.Logic.Views;
using System.Threading;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class AsyncMessagesPresenter
        : Presenter<IAsyncMessagesView>
    {
        public AsyncMessagesPresenter(IAsyncMessagesView view)
            : base(view)
        {
            View.Load += View_Load;
        }

        public override void ReleaseView()
        {
            View.Load -= View_Load;
        }

        private readonly Func<string> doStuff1 = () =>
        {
            Thread.Sleep(3000);
            return ThreadMessage("Async task doStuff1 processed");
        };

        private readonly Func<string> doStuff2 = () =>
        {
            Thread.Sleep(1500);
            return ThreadMessage("Async task doStuff2 processed");
        };

        void View_Load(object sender, EventArgs e)
        {
            View.Model.Messages.Add(ThreadMessage("View.Load event handled"));

            AsyncManager.RegisterAsyncTask(
                (asyncSender, ea, callback, state) => // Begin
                {
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff1 begin handler"));
                    return doStuff1.BeginInvoke(callback, state);
                },
                result => // End
                {
                    var msg = doStuff1.EndInvoke(result);
                    View.Model.Messages.Add(msg);
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff1 end handler"));
                },
                result => // Timeout
                {
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff1 timeout handler"));
                },
                null,
                true
            );

            AsyncManager.RegisterAsyncTask(
                (asyncSender, ea, callback, state) => // Begin
                {
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff2 begin handler"));
                    return doStuff2.BeginInvoke(callback, state);
                },
                result => // End
                {
                    var msg = doStuff2.EndInvoke(result);
                    View.Model.Messages.Add(msg);
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff2 end handler"));
                },
                result => // Timeout
                {
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff2 timeout handler"));
                },
                null,
                true
            );
        }

        private static string ThreadMessage(string prefix)
        {
            return String.Format("{0} on thread {1} at {2}", prefix, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("HH:mm:ss.ss"));
        }
    }
}