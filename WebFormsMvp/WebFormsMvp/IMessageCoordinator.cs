using System;

namespace WebFormsMvp
{
    public interface IMessageCoordinator : IDisposable
    {
        void Publish<TMessage>(TMessage message);
        void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback);
        void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback, Action neverReceivedCallback);
    }
}