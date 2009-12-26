using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp
{
    /// <summary>
    /// A default implementation for cross presenter messaging.
    /// </summary>
    public class MessageCoordinator : IMessageCoordinator
    {
        readonly IDictionary<Type, IList> messages;
        readonly IDictionary<Type, IList<Action<object>>> messageReceivedCallbacks;
        readonly IDictionary<Type, IList<Action>> neverReceivedCallbacks;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCoordinator"/> class.
        /// </summary>
        public MessageCoordinator()
        {
            messages = new Dictionary<Type, IList>();
            messageReceivedCallbacks = new Dictionary<Type, IList<Action<object>>>();
            neverReceivedCallbacks = new Dictionary<Type, IList<Action>>();
        }

        /// <summary>
        /// Publishes a message to the bus. Any existing subscriptions to this type,
        /// or an assignable type such as a base class or an interface, will be notified
        /// at this time.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to publish</typeparam>
        /// <param name="message">The message to publish</param>
        public void Publish<TMessage>(TMessage message)
        {
            if (closed)
            {
                throw new InvalidOperationException("Messages can't be published or subscribed to after the message bus has been closed. In a typical page lifecycle, this happens during PreRenderComplete.");
            }

            AddMessage(message);
            PushMessage(message);
        }

        void AddMessage<TMessage>(TMessage message)
        {
            var messageList = messages.GetOrCreateValue(typeof(TMessage),
                () => new List<TMessage>());
            lock (messageList)
            {
                messageList.Add(message);
            }
        }

        void PushMessage<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);

            var callbackTypes = messageReceivedCallbacks
                .Keys
                .Where(k => k.IsAssignableFrom(messageType));

            var callbacks = callbackTypes
                .SelectMany(t => messageReceivedCallbacks[t]);

            foreach (var callback in callbacks)
            {
                callback(message);
            }
        }

        /// <summary>
        /// Registers a subscription to messages of the specified type. Any previously
        /// published messages that are valid for this subscription will be raised
        /// at this time.
        /// </summary>
        /// <typeparam name="TMessage">The type of messages to subscribe to</typeparam>
        /// <param name="messageReceivedCallback">A callback that will be invoked for each message received. This callback will be invoked once per message.</param>
        public void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            Subscribe(messageReceivedCallback, null);
        }

        /// <summary>
        /// Registers a subscription to messages of the specified type. Any previously
        /// published messages that are valid for this subscription will be raised
        /// at this time.
        /// </summary>
        /// <typeparam name="TMessage">The type of messages to subscribe to</typeparam>
        /// <param name="messageReceivedCallback">A callback that will be invoked for each message received. This callback will be invoked once per message.</param>
        /// <param name="neverReceivedCallback">A callback that will be invoked if no matching message is ever received. This callback will not be invoked more than once.</param>
        public void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback, Action neverReceivedCallback)
        {
            if (closed)
            {
                throw new InvalidOperationException("Messages can't be published or subscribed to after the message bus has been closed. In a typical page lifecycle, this happens during PreRenderComplete.");
            }
            if (messageReceivedCallback == null)
            {
                throw new ArgumentNullException("messageReceivedCallback");
            }

            AddMessageReceivedCallback(messageReceivedCallback);
            AddNeverReceivedCallback<TMessage>(neverReceivedCallback);
            PushPreviousMessages(messageReceivedCallback);
        }

        void AddMessageReceivedCallback<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            var intermediateReceivedCallback = new Action<object>(m => 
                messageReceivedCallback((TMessage)m));

            var receivedList = messageReceivedCallbacks.GetOrCreateValue(typeof(TMessage),
                () => new List<Action<object>>());
            lock (receivedList)
            {
                receivedList.Add(intermediateReceivedCallback);
            }
        }

        void AddNeverReceivedCallback<TMessage>(Action neverReceivedCallback)
        {
            if (neverReceivedCallback != null)
            {
                var neverReceivedList = neverReceivedCallbacks.GetOrCreateValue(typeof(TMessage),
                    () => new List<Action>());
                lock (neverReceivedList)
                {
                    neverReceivedList.Add(neverReceivedCallback);
                }
            }
        }

        void PushPreviousMessages<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            var previousMessageTypes = messages
                .Keys
                .Where(mt => typeof(TMessage).IsAssignableFrom(mt));

            var previousMessages = previousMessageTypes
                .SelectMany(t => messages[t].Cast<TMessage>());

            foreach (var previousMessage in previousMessages)
            {
                messageReceivedCallback(previousMessage);
            }
        }

        bool closed;
        readonly object closeLock = new object();

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
        ///     The <see cref="IMessageCoordinator.Close"/> method may be called multiple times and must not
        ///     fail in this scenario.
        /// </para>
        /// </summary>
        public void Close()
        {
            lock (closeLock)
            {
                if (closed)
                {
                    return;
                }
                closed = true;
            }

            FireNeverReceivedCallbacks();
        }

        void FireNeverReceivedCallbacks()
        {
            var neverReceivedMessageTypes = neverReceivedCallbacks
                .Keys
                .Where(neverReceivedMessageType =>
                    !messages.Keys.Any(messageType =>
                        messageType.IsAssignableFrom(neverReceivedMessageType)));

            var callbacks = neverReceivedMessageTypes
                .SelectMany(t => neverReceivedCallbacks[t]);

            foreach (var callback in callbacks)
            {
                callback();
            }
        }
    }
}