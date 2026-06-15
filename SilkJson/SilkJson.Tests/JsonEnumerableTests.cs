using System.Linq;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для JsonEnumerable
    /// </summary>
    public class JsonEnumerableTests
    {
        [Fact]
        public void JsonEnumerable_CreateEmpty_ReturnsNull()
        {
            // Arrange
            JsonEnumerable enumerable = new JsonEnumerable(System.Linq.Enumerable.Empty<Json>());

            // Assert
            Assert.True(enumerable.IsMissed);
        }

        [Fact]
        public void JsonEnumerable_CreateWithItems_ReturnsNotNull()
        {
            // Arrange
            JsonEnumerable enumerable = new JsonEnumerable(new[] { new JsonValue("test") });

            // Assert
            Assert.False(enumerable.IsMissed);
        }

        [Fact]
        public void JsonEnumerable_GetEnumerator_ReturnsItems()
        {
            // Arrange
            var items = new[] { new JsonValue("a"), new JsonValue("b") };
            JsonEnumerable enumerable = new JsonEnumerable(items);

            // Act
            var result = enumerable.ToList();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void JsonEnumerable_StringifySingleItem_ReturnsItemString()
        {
            // Arrange
            JsonEnumerable enumerable = new JsonEnumerable(new[] { new JsonValue("test") });

            // Act
            string result = enumerable.ToString();

            // Assert
            Assert.Equal("\"test\"", result);
        }

        [Fact]
        public void JsonEnumerable_StringifyMultipleItems_ReturnsArrayString()
        {
            // Arrange
            JsonEnumerable enumerable = new JsonEnumerable(new[] { new JsonValue("a"), new JsonValue("b") });

            // Act
            string result = enumerable.ToString();

            // Assert
            Assert.Equal("[\"a\",\"b\"]", result);
        }

        [Fact]
        public void JsonEnumerable_Children_ReturnsDirectChildren()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["key1"] = "value1";
            obj["key2"] = "value2";

            // Act
            JsonEnumerable children = obj.Children();

            // Assert
            Assert.Equal(2, children.Count());
        }

        [Fact]
        public void JsonEnumerable_Values_ReturnsTypedValues()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["int"] = 42;

            // Act
            System.Collections.Generic.IEnumerable<int> values = obj.Values<int>();

            // Assert
            Assert.Equal(42, values.FirstOrDefault());
        }

        [Fact]
        public void JsonEnumerable_IndexByKey_ReturnsFilteredObjects()
        {
            // Arrange
            JsonArray array = new JsonArray();
            JsonObject obj1 = new JsonObject();
            obj1["name"] = "John";
            JsonObject obj2 = new JsonObject();
            obj2["name"] = "Jane";
            array.Add(obj1);
            array.Add(obj2);

            // Act
            Json result = array.FindDeep("name");

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void JsonEnumerable_IndexByInt_ReturnsFilteredArrays()
        {
            // Arrange
            JsonArray outer = new JsonArray();
            JsonArray inner1 = new JsonArray();
            inner1.Add("a");
            JsonArray inner2 = new JsonArray();
            inner2.Add("b");
            outer.Add(inner1);
            outer.Add(inner2);

            // Act
            Json result = outer[0];

            // Assert
            Assert.Single(result);
        }
    }
}
