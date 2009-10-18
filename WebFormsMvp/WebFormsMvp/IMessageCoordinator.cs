using System;

namespace WebFormsMvp
{
    public interface IMessageCoordinator : IMessageBus
    {
        /// <summary>
        /// <para>
        ///     Closes the message bus, causing any subscribers that have not yet received
        ///     a message to have their "never received" callback fired.
        /// </para>
        /// <para>
        ///     After this method is called, any further calls to <see cref="Publish"/> or
        ///     <see cref="Subscribe"/> will result in a
        ///     <see cref="InvalidOperationException"/>.
        /// </para>
        /// <para>
        ///     The <see cref="Close"/> method may be called multiple times and must not
        ///     fail in this scenario.
        /// </para>
        /// </summary>
        void Close();
    }
}