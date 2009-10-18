using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp
{
    public class MessageCoordinator : IMessageCoordinator
    {
        readonly IDictionary<Type, IList> messages;
        readonly IDictionary<Type, IList<Action<object>>> messageReceivedCallbacks;
        readonly IDictionary<Type, IList<Action>> neverReceivedCallbacks;

        public MessageCoordinator()
        {
            messages = new Dictionary<Type, IList>();
            messageReceivedCallbacks = new Dictionary<Type, IList<Action<object>>>();
            neverReceivedCallbacks = new Dictionary<Type, IList<Action>>();
        }

        public void Publish<TMessage>(TMessage message)
        {
            if (closed)
            {
                throw new InvalidOperationException("Messages can't be published or subscribed to after the message bus has been closed. In a typical page lifecycle, this happens during PreRenderComplete.");
            }

            AddMessage<TMessage>(message);
            PushMessage<TMessage>(message);
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

        public void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            Subscribe(messageReceivedCallback, null);
        }

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

            AddMessageReceivedCallback<TMessage>(messageReceivedCallback);
            AddNeverReceivedCallback<TMessage>(neverReceivedCallback);
            PushPreviousMessages<TMessage>(messageReceivedCallback);
        }

        void AddMessageReceivedCallback<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            var intermediateReceivedCallback = new Action<object>(m =>
            {
                messageReceivedCallback((TMessage)m);
            });

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
        object closeLock = new object();
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