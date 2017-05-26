using System.Web;
using NUnit.Framework;
using System;

namespace WebFormsMvp.UnitTests
{
    [TestFixture]
    public class TypeListComparerTests
    {
        [Test]
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

        [Test]
        public void TypeListComparer_Equals_ShouldThrowExceptionWhenFirstArgumentIsNull()
        {
            // Arrange
            var x = null as object[];
            var y = new object[0];

            // Act
            Assert.Throws<ArgumentNullException>(
                // ReSharper disable once ExpressionIsAlwaysNull test null valued parameter
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed test null valued parameter
                () => new TypeListComparer<object>().Equals(x, y));

            // Assert
        }

        [Test]
        public void TypeListComparer_Equals_ShouldThrowExceptionWhenSecondArgumentIsNull()
        {
            // Arrange
            var x = new object[0];
            var y = null as object[];

            // Act
            Assert.Throws<ArgumentNullException>(
                // ReSharper disable once ExpressionIsAlwaysNull tests null parameter
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed tests null parameter
                () => new TypeListComparer<object>().Equals(x, y));

            // Assert
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public void TypeListComparer_GetHashCode_ShouldThrowExceptionWhenArgumentIsNull()
        {
            // Arrange
            var obj = null as object[];

            // Act
            Assert.Throws<ArgumentNullException>(
                // ReSharper disable once ExpressionIsAlwaysNull tests null value parameter
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed tests null value parameter
                () => new TypeListComparer<object>().GetHashCode(obj));

            // Assert
        }

        [Test]
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

        [Test]
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

        [Test]
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