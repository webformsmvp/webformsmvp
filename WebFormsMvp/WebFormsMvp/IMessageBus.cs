using System;

namespace WebFormsMvp
{
    /// <summary>
    /// Defines the basic methods of a cross-presenter messaging bus.
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Publishes a message to the bus. Any existing subscriptions to this type,
        /// or an assignable type such as a base class or an interface, will be notified
        /// at this time.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to publish</typeparam>
        /// <param name="message">The message to publish</param>
        void Publish<TMessage>(TMessage message);

        /// <summary>
        /// Registers a subscription to messages of the specified type. Any previously
        /// published messages that are valid for this subscription will be raised
        /// at this time.
        /// </summary>
        /// <typeparam name="TMessage">The type of messages to subscribe to</typeparam>
        /// <param name="messageReceivedCallback">A callback that will be invoked for each message received. This callback will be invoked once per message.</param>
        void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback);

        /// <summary>
        /// Registers a subscription to messages of the specified type. Any previously
        /// published messages that are valid for this subscription will be raised
        /// at this time.
        /// </summary>
        /// <typeparam name="TMessage">The type of messages to subscribe to</typeparam>
        /// <param name="messageReceivedCallback">A callback that will be invoked for each message received. This callback will be invoked once per message.</param>
        /// <param name="neverReceivedCallback">A callback that will be invoked if no matching message is ever received. This callback will not be invoked more than once.</param>
        void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback, Action neverReceivedCallback);
    }
}