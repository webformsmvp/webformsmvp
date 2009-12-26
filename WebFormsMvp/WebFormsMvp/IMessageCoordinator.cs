using System;

namespace WebFormsMvp
{
    /// <summary>
    /// Defines the basic methods of a cross-presenter messaging coordinator.
    /// </summary>
    public interface IMessageCoordinator : IMessageBus
    {
        /// <summary>
        /// <para>
        ///     Closes the message bus, causing any subscribers that have not yet received
        ///     a message to have their "never received" callback fired.
        /// </para>
        /// <para>
        ///     After this method is called, any further calls to <see cref="IMessageBus.Publish{TMessage}"/> or
        ///     <see cref="IMessageBus.Subscribe{TMessage}(System.Action{TMessage})"/> will result in an
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