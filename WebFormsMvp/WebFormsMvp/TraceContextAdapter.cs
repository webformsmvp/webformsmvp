using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace WebFormsMvp
{
    /// <summary>
    /// A bespoke substitute for the lack of System.Web.Abstractions.TraceContextWrapper.
    /// Lazy Microsoft.
    /// </summary>
    internal class TraceContextAdapter : ITraceContext
    {
        readonly TraceContext target;

        internal TraceContextAdapter(TraceContext target)
        {
            this.target = target;
        }

        /// <summary>
        /// Writes trace information to the trace log.
        /// </summary>
        /// <param name="source">The object writing the trace message.</param>
        /// <param name="messageCallback">A callback that builds the trace message to write to the log.</param>
        public void Write(object source, Func<string> messageCallback)
        {
            if (!target.IsEnabled)
                return;

            if (source == null)
                throw new ArgumentNullException("source");

            Write(source.GetType(), messageCallback);
        }

        /// <summary>
        /// Writes trace information to the trace log.
        /// </summary>
        /// <param name="sourceType">The type of the object writing the trace message.</param>
        /// <param name="messageCallback">A callback that builds the trace message to write to the log.</param>
        public void Write(Type sourceType, Func<string> messageCallback)
        {
            if (!target.IsEnabled)
                return;

            if (sourceType == null)
                throw new ArgumentNullException("sourceType");

            if (messageCallback == null)
                throw new ArgumentNullException("messageCallback");

            var message = messageCallback();

            WriteInternal(sourceType, message);
        }

        /// <summary>
        /// Writes trace information to the trace log.
        /// </summary>
        /// <param name="sourceType">The type of the object writing the trace message.</param>
        /// <param name="messagesCallback">A callback that builds a series of trace messages to write to the log.</param>
        public void Write(Type sourceType, Func<IEnumerable<string>> messagesCallback)
        {
            if (!target.IsEnabled)
                return;

            if (sourceType == null)
                throw new ArgumentNullException("sourceType");

            if (messagesCallback == null)
                throw new ArgumentNullException("messagesCallback");

            foreach (var messages in messagesCallback())
                WriteInternal(sourceType, messages);
        }

        void WriteInternal(Type sourceType, string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            target.Write("WebFormsMvp", string.Format(
                CultureInfo.InvariantCulture,
                "{0}: {1}",
                sourceType.Name,
                message
            ));
        }
    }
}