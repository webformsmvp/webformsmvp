using System;
using System.Linq;

namespace WebFormsMvp.FeatureDemos.Logic.Views
{
    public interface ITimeServiceView : IView
    {
        event EventHandler<GetCurrentTimeCalledEventArgs> GetCurrentTimeCalled;
    }

    public class GetCurrentTimeCalledEventArgs : EventArgs
    {
        readonly bool localTime;

        public GetCurrentTimeCalledEventArgs(bool localTime)
        {
            this.localTime = localTime;
        }

        public bool LocalTime { get { return localTime; } }

        public DateTime Result { get; set; }
    }
}
