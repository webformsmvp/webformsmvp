using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebFormsMvp.UnitTests
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        [TestMethod]
        public void CollectionExtensions_IList_AddRange_AddsItemsToInstance()
        {
            // Arrange
            IList<string> actual = new List<string> { "1", "2", "3" };

            // Act
            actual.AddRange(new [] {"4", "5"});

            // Assert
            var expecting = new List<string> { "1", "2", "3", "4", "5" };
            CollectionAssert.AreEqual(expecting, (List<string>)actual);
        }

        [TestMethod]
        public void CollectionExtensions_ICollection_AddRange_AddsItemsToInstance()
        {
            // Arrange
            ICollection<string> actual = new List<string> { "1", "2", "3" };

            // Act
            actual.AddRange(new[] { "4", "5" });

            // Assert
            var expecting = new List<string> { "1", "2", "3", "4", "5" };
            CollectionAssert.AreEqual(expecting, (List<string>)actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectionExtensions_IList_AddRange_ThrowsIfTargetArgumentIsNull()
        {
            // Arrange
            IList<string> actual = null;

            // Act
            actual.AddRange(new[] { "1", "2" });

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectionExtensions_IList_AddRange_ThrowsIfListArgumentIsNull()
        {
            // Arrange
            IList<string> actual = new List<string> { "1", "2", "3" };

            // Act
            actual.AddRange(null);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectionExtensions_ICollection_AddRange_ThrowsIfTargetArgumentIsNull()
        {
            // Arrange
            ICollection<string> actual = null;

            // Act
            actual.AddRange(new[] { "1", "2" });

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectionExtensions_ICollection_AddRange_ThrowsIfListArgumentIsNull()
        {
            // Arrange
            ICollection<string> actual = new List<string> { "1", "2", "3" };

            // Act
            actual.AddRange(null);

            // Assert
        }

        [TestMethod]
        public void CollectionExtensions_IDictionary_GetOrCreateValue_GetsValueIfContainedInDictionary()
        {
            // Arrange
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("blah", "value");

            // Act
            var result = dictionary.GetOrCreateValue("blah", () => "yo");

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void CollectionExtensions_IDictionary_GetOrCreateValue_AddsValueIfNotContainedInDictionary()
        {
            // Arrange
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            
            // Act
            var result = dictionary.GetOrCreateValue("blah", () => "yo");

            // Assert
            Assert.AreEqual("yo", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectionExtensions_IDictionary_GetOrCreateValue_ThrowsIfDictionaryArgumentIsNull()
        {
            // Arrange
            IDictionary<string, string> dictionary = null;

            // Act
            var result = dictionary.GetOrCreateValue("blah", () => "yo");

            // Assert
        }
    }
}
