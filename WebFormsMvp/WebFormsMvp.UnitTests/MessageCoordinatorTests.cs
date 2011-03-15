using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebFormsMvp.UnitTests
{
    [TestClass]
    public class MessageCoordinatorTests
    {
        [TestMethod]
        public void MessageCoordinator_Publish_ShouldFireExistingSubscribers()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            string receviedMessage = null;
            coordinator.Subscribe((string message) =>
            {
                receviedMessage = message;
            });

            // Act
            var publishedMessage = "hello";
            coordinator.Publish(publishedMessage);

            // Assert
            Assert.AreEqual(publishedMessage, receviedMessage);
        }

        [TestMethod]
        public void MessageCoordinator_Publish_ShouldAcceptNullMessage()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            string receviedMessage = "something else";
            coordinator.Subscribe((string message) =>
            {
                receviedMessage = message;
            });

            // Act
            coordinator.Publish((string)null);

            // Assert
            Assert.IsNull(receviedMessage);
        }

        [TestMethod]
        public void MessageCoordinator_Publish_ShouldFireMultipleSubscribersInOrderOfSubscription()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            var subscriberCount = 20;
            var firedSubscribers = new List<int>();
            for (var i = 0; i < subscriberCount; i++)
            {
                var subscriberIndex = i;
                coordinator.Subscribe((string message) =>
                {
                    firedSubscribers.Add(subscriberIndex);
                });
            }

            // Act
            coordinator.Publish("message");

            // Assert
            var expectedFiringOrder = Enumerable.Range(0, subscriberCount).ToArray();
            CollectionAssert.AreEquivalent(expectedFiringOrder, firedSubscribers);
        }

        [TestMethod]
        public void MessageCoordinator_Publish_ShouldFireSubscribersOfBaseTypes()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            TestMessage receviedMessage = null;
            coordinator.Subscribe((TestMessage message) =>
            {
                receviedMessage = message;
            });

            // Act
            var publishedMessage = new InheritedTestMessage();
            coordinator.Publish(publishedMessage);

            // Assert
            Assert.AreEqual(publishedMessage, receviedMessage);
        }

        [TestMethod]
        public void MessageCoordinator_Publish_ShouldNotFireSubscribersOfInheritedTypes()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            coordinator.Subscribe((InheritedTestMessage message) =>
            {
                Assert.Fail("Callback should never have been called.");
            });

            // Act
            var publishedMessage = new TestMessage();
            coordinator.Publish(publishedMessage);

            // Assert
        }

        [TestMethod]
        public void MessageCoordinator_Close_ShouldNotFireNotDeliveredForSubscribersOfBaseType()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            
            coordinator.Subscribe<TestMessage>(
                (TestMessage message) => { },
                () => { Assert.Fail("The not recieved callback should not be called"); });

            //Act
            coordinator.Publish(new InheritedTestMessage());
            coordinator.Close();

            // Assert
        }

        [TestMethod]
        public void MessageCoordinator_Close_ShouldFireNotDeliveredForSubscribersOfInheritedTypes()
        {
            // Arrange
            var coordinator = new MessageCoordinator();

            coordinator.Subscribe<InheritedTestMessage>(
                (InheritedTestMessage message) => { Assert.Fail("The recieved callback should not be called"); },
                () => { });

            //Act
            coordinator.Publish(new TestMessage());
            coordinator.Close();

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void MessageCoordinator_Publish_ShouldNotFireSubsequentSubscribersIfOneFails()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            coordinator.Subscribe((string message) =>
            {
                throw new ApplicationException("Test exception");
            });
            coordinator.Subscribe((string message) =>
            {
                Assert.Fail();
            });

            // Act
            try
            {
                coordinator.Publish("message");
            }
            catch (ApplicationException ex)
            {
                Assert.AreEqual("Test exception", ex.Message);
                throw;
            }

            // Assert
        }

        [TestMethod]
        public void MessageCoordinator_Publish_ShouldNotFailIfThereAreNoMatchingSubscribers()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            
            // Act
            coordinator.Publish("message");

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MessageCoordinator_Publish_ShouldThrowInvalidOperationExceptionIfCalledAfterClose()
        {
            // Arrange
            var coordinator = new MessageCoordinator();

            // Act
            coordinator.Close();
            coordinator.Publish("message");

            // Assert
        }

        [TestMethod]
        public void MessageCoordinator_Publish_ShouldAcceptNewMessagesWithinACallback()
        {
            // Arrange
            var coordinator = new MessageCoordinator();

            // Act
            var intReceived = false;
            coordinator.Subscribe<int>(m => { intReceived = m == 123; });
            coordinator.Subscribe<string>(m => coordinator.Publish(123));
            coordinator.Publish("message");

            // Assert
            Assert.IsTrue(intReceived);
        }

        [TestMethod]
        public void MessageCoordinator_Publish_ShouldAcceptNewMessagesWithinALateSubscribeCallback()
        {
            // Arrange
            var coordinator = new MessageCoordinator();

            // Act
            var intReceived = false;
            coordinator.Publish("message");
            coordinator.Subscribe<int>(m => { intReceived = m == 123; });
            coordinator.Subscribe<string>(m => coordinator.Publish(123));

            // Assert
            Assert.IsTrue(intReceived);
        }

        [TestMethod]
        public void MessageCoordinator_Subscribe_ShouldReceivePreviousMessages()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            var publishedMessage = "hello";
            coordinator.Publish(publishedMessage);

            // Act
            string receviedMessage = null;
            coordinator.Subscribe((string message) =>
            {
                receviedMessage = message;
            });

            // Assert
            Assert.AreEqual(publishedMessage, receviedMessage);
        }

        [TestMethod]
        public void MessageCoordinator_Subscribe_ShouldReceiveMultiplePreviousMessagesInOrderOfPublishing()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            var messageCount = 20;
            var publishedMessages = Enumerable.Range(0, messageCount).ToList();
            foreach (var message in publishedMessages)
            {
                coordinator.Publish(message);
            }

            // Act
            var receviedMessages = new List<int>();
            coordinator.Subscribe((int message) =>
            {
                receviedMessages.Add(message);
            });

            // Assert
            CollectionAssert.AreEquivalent(publishedMessages, receviedMessages);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MessageCoordinator_Subscribe_ShouldThrowInvalidOperationExceptionIfCalledAfterClose()
        {
            // Arrange
            var coordinator = new MessageCoordinator();

            // Act
            coordinator.Close();
            coordinator.Subscribe((string message) =>
            {
                Assert.Fail();
            });

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MessageCoordinator_Subscribe_ShouldThrowArgumentNullExceptionIfMessageReceivedCallbackIsNull()
        {
            // Arrange
            var coordinator = new MessageCoordinator();

            // Act
            coordinator.Subscribe((Action<object>)null);

            // Assert
        }

        [TestMethod]
        public void MessageCoordinator_Subscribe_ShouldAcceptNullForNeverReceivedCallback()
        {
            // Arrange
            var coordinator = new MessageCoordinator();

            // Act
            coordinator.Subscribe((object message) => { }, null);

            // Assert
        }

        [TestMethod]
        public void MessageCoordinator_Close_ShouldFireNeverReceivedCallback()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            bool neverReceivedCallbackFired = false;
            coordinator.Subscribe(
                (int message) =>
                {
                    Assert.Fail("Callback should never have been called.");
                },
                () =>
                {
                    neverReceivedCallbackFired = true;
                });

            // Act
            coordinator.Publish("hello");
            coordinator.Close();

            // Assert
            Assert.IsTrue(neverReceivedCallbackFired);
        }

        [TestMethod]
        public void MessageCoordinator_Close_ShouldFireNeverReceivedCallbackInOrderOfSubscription()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            var subscriberCount = 20;
            var firedCallbacks = new List<int>();
            for (var i = 0; i < subscriberCount; i++)
            {
                var subscriberIndex = i;
                coordinator.Subscribe((string message) =>
                {
                    Assert.Fail("Callback should never have been called.");
                },
                () =>
                {
                    firedCallbacks.Add(subscriberIndex);
                });
            }

            // Act
            coordinator.Close();

            // Assert
            var expectedFiringOrder = Enumerable.Range(0, subscriberCount).ToArray();
            CollectionAssert.AreEquivalent(expectedFiringOrder, firedCallbacks);
        }

        [TestMethod]
        public void MessageCoordinator_Close_ShouldNotFireNeverReceivedCallbackForMessagesThatHaveBeenReceived()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            bool messageReceivedCallbackFired = false;
            coordinator.Subscribe(
                (string message) =>
                {
                    messageReceivedCallbackFired = true;
                },
                () =>
                {
                    Assert.Fail("Callback should never have been called.");
                });

            // Act
            coordinator.Publish("hello");
            coordinator.Close();

            // Assert
            Assert.IsTrue(messageReceivedCallbackFired);
        }

        [TestMethod]
        public void MessageCoordinator_Close_ShouldSupportBeingCalledMultipleTimes()
        {
            // Arrange
            var coordinator = new MessageCoordinator();
            
            // Act
            coordinator.Close();
            coordinator.Close();

            // Assert
        }

        interface ITestMessage {}
        class TestMessage : ITestMessage{ }
        class InheritedTestMessage : TestMessage { }
    }
}