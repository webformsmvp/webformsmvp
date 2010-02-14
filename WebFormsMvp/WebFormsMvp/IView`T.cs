namespace WebFormsMvp
{
    /// <summary>
    /// Represents a class that is a view with a strongly typed view model in a Web Forms Model-View-Presenter application.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IView<TModel> : IView
        where TModel : class, new()
    {
        /// <summary>
        /// Gets the model instance. The default presenter base class
        /// (<see cref="Presenter{TView}"/>) initializes this automatically.
        /// </summary>
        TModel Model { get; set; }
    }
}