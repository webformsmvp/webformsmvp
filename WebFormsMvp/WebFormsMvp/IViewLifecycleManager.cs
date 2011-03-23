using System;

namespace WebFormsMvp
{
    /// <summary>
    /// An intermediate interface while we slowly remove the
    /// ReleaseView method from presenters. No new code should
    /// use this interface.
    /// </summary>
    [Obsolete("You are no longer required to implement ReleaseView. If you have objects that need to be disposed at the end of your presenter's lifetime, implement IDisposable instead. This method will be removed altogether in a future release.")]
    public interface IViewLifecycleManager
    {
        /// <summary>
        /// Releases the view.
        /// </summary>
        [Obsolete("You are no longer required to implement ReleaseView. If you have objects that need to be disposed at the end of your presenter's lifetime, implement IDisposable instead. This method will be removed altogether in a future release.")]
        void ReleaseView();
    }
}