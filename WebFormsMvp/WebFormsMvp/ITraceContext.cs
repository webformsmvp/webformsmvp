namespace WebFormsMvp
{
    /// <summary>
    /// A bespoke substitute for the lack of System.Web.Abstractions.TraceContextBase.
    /// Lazy Microsoft.
    /// </summary>
    public interface ITraceContext
    {
        /// <summary>
        /// Writes trace information to the trace log, including a message and any user-defined
        /// categories.
        /// </summary>
        /// <param name="category">The trace category that receives the message.</param>
        /// <param name="message">The trace message to write to the log.</param>
        void Write(string category, string message);
    }
}