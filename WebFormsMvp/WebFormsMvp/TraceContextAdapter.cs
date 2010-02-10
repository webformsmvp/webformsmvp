using System.Web;

namespace WebFormsMvp
{
    /// <summary>
    /// A bespoke substitute for the lack of System.Web.Abstractions.TraceContextWrapper.
    /// Lazy Microsoft.
    /// </summary>
    public class TraceContextAdapter : ITraceContext
    {
        readonly TraceContext original;

        internal TraceContextAdapter(TraceContext original)
        {
            this.original = original;
        }

        /// <summary>
        /// Writes trace information to the trace log, including a message and any user-defined
        /// categories.
        /// </summary>
        /// <param name="category">The trace category that receives the message.</param>
        /// <param name="message">The trace message to write to the log.</param>
        public void Write(string category, string message)
        {
            original.Write(category, message);
        }
    }
}