namespace WebFormsMvp
{
    /// <summary>
    /// An intermediate interface while we slowly remove the
    /// ReleaseView method from presenters. No new code should
    /// use this interface, and it should 
    /// </summary>
    public interface IViewLifecycleManager
    {
        /// <summary>
        /// Releases the view.
        /// </summary>
        void ReleaseView();
    }
}