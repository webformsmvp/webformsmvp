using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WebFormsMvp.UnitTests
{
    [TestClass]
    public class TypeListComparerTests
    {
        [TestMethod]
        public void TypeListComparer_Equals_ShouldReturnTrueForTwoEmptyLists()
        {
            // Arrange
            var x = new object[0];
            var y = new object[0];

            // Act
            var actual = new TypeListComparer<object>().Equals(x, y);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TypeListComparer_Equals_ShouldThrowExceptionWhenFirstArgumentIsNull()
        {
            // Arrange
            var x = null as object[];
            var y = new object[0];

            // Act
            var actual = new TypeListComparer<object>().Equals(x, y);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TypeListComparer_Equals_ShouldThrowExceptionWhenSecondArgumentIsNull()
        {
            // Arrange
            var x = new object[0];
            var y = null as object[];

            // Act
            var actual = new TypeListComparer<object>().Equals(x, y);

            // Assert
        }

        [TestMethod]
        public void TypeListComparer_Equals_ShouldReturnTrueForTwoSortedLists()
        {
            // Arrange
            var a = new HttpCookie("a");
            var b = new HttpCookie("b");
            var c = new HttpCookie("c");
            var d = new HttpCookie("d");

            var x = new[] { a, b, c, d };
            var y = new[] { a, b, c, d };

            // Act
            var actual = new TypeListComparer<HttpCookie>().Equals(x, y);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TypeListComparer_Equals_ShouldReturnTrueForTwoUnsortedLists()
        {
            // Arrange
            var a = new HttpCookie("a");
            var b = new HttpCookie("b");
            var c = new HttpCookie("c");
            var d = new HttpCookie("d");

            var x = new[] { a, b, c, d };
            var y = new[] { d, c, b, a };

            // Act
            var actual = new TypeListComparer<HttpCookie>().Equals(x, y);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TypeListComparer_Equals_ShouldReturnFalseForTwoListsOfDifferentLength()
        {
            // Arrange
            var a = new HttpCookie("a");
            var b = new HttpCookie("b");
            var c = new HttpCookie("c");
            var d = new HttpCookie("d");

            var x = new[] { a, b, c, d };
            var y = new[] { d, c, b };

            // Act
            var actual = new TypeListComparer<HttpCookie>().Equals(x, y);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void TypeListComparer_Equals_ShouldReturnFalseForTwoListsOfDifferentValues()
        {
            // Arrange
            var a = new HttpCookie("a");
            var b = new HttpCookie("b");
            var c = new HttpCookie("c");
            var d = new HttpCookie("d");

            var x = new[] { a, b, c };
            var y = new[] { d, c, b };

            // Act
            var actual = new TypeListComparer<HttpCookie>().Equals(x, y);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TypeListComparer_GetHashCode_ShouldThrowExceptionWhenArgumentIsNull()
        {
            // Arrange
            var obj = null as object[];

            // Act
            var actual = new TypeListComparer<object>().GetHashCode(obj);

            // Assert
        }

        [TestMethod]
        public void TypeListComparer_GetHashCode_ShouldReturnZeroForEmptyList()
        {
            // Arrange
            var obj = new object[] { };

            // Act
            var actual = new TypeListComparer<object>().GetHashCode(obj);

            // Assert
            var expected = 0;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TypeListComparer_GetHashCode_ShouldReturnValueOfItemInSingleItemList()
        {
            // Arrange
            var obj = new object[] { 5 };

            // Act
            var actual = new TypeListComparer<object>().GetHashCode(obj);

            // Assert
            var expected = 5;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TypeListComparer_GetHashCode_ShouldReturnAggregateOrOfListItems()
        {
            // Arrange
            var obj = new object[] { 1, 2, 3 };

            // Act
            var actual = new TypeListComparer<object>().GetHashCode(obj);

            // Assert
            var expected = 1 | 2 | 3;
            Assert.AreEqual(expected, actual);
        }
    }
}