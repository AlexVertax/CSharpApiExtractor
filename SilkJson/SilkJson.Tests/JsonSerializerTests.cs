using System;
using System.Collections.Generic;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для JsonSerializer
    /// </summary>
    public class JsonSerializerTests
    {
        [Fact]
        public void JsonSerializer_SerializeString_ReturnsStringValue()
        {
            // Arrange
            string value = "test";

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("test", result.ToString());
        }

        [Fact]
        public void JsonSerializer_SerializeInteger_ReturnsNumberValue()
        {
            // Arrange
            int value = 42;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("42", result.ToString());
        }

        [Fact]
        public void JsonSerializer_SerializeDouble_ReturnsNumberValue()
        {
            // Arrange
            double value = 3.14;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("3.14", result.ToString());
        }

        [Fact]
        public void JsonSerializer_SerializeBool_ReturnsBooleanValue()
        {
            // Arrange
            bool value = true;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("true", result.ToString());
        }

        [Fact]
        public void JsonSerializer_SerializeDateTime_ReturnsISOString()
        {
            // Arrange
            DateTime value = new DateTime(2023, 1, 15, 10, 30, 0);

            // Act
            Json result = Json.From(value);

            // Assert
            string str = result.ToString();
            Assert.Equal("2023-01-15T10:30:00", str);
        }

        [Fact]
        public void JsonSerializer_SerializeEnum_ReturnsName()
        {
            // Arrange
            TestEnum value = TestEnum.Second;

            // Act
            Json result = Json.From(value);

            // Assert
            string str = result.ToString();
            Assert.Equal("Second", str);
        }

        [Fact]
        public void JsonSerializer_SerializeList_ReturnsArray()
        {
            // Arrange
            List<int> list = new List<int> { 1, 2, 3 };

            // Act
            Json result = Json.From(list);

            // Assert
            Assert.Equal("[1,2,3]", result.ToString());
        }

        [Fact]
        public void JsonSerializer_SerializeDictionary_ReturnsObject()
        {
            // Arrange
            Dictionary<string, int> dict = new Dictionary<string, int>
            {
                ["a"] = 1,
                ["b"] = 2
            };

            // Act
            Json result = Json.From(dict);

            // Assert
            Assert.Contains("a", result.ToString());
            Assert.Contains("b", result.ToString());
        }

        [Fact]
        public void JsonSerializer_SerializeObject_ReturnsObject()
        {
            // Arrange
            TestClass obj = new TestClass
            {
                Name = "Test",
                Value = 42
            };

            // Act
            Json result = Json.From(obj);

            // Assert
            Assert.Contains("Name", result.ToString());
            Assert.Contains("Test", result.ToString());
        }

        [Fact]
        public void JsonSerializer_SerializeNull_ReturnsNull()
        {
            // Arrange & Act
            Json result = Json.From(null);

            // Assert
            Assert.True(result.IsMissed);
        }

        [Fact]
        public void JsonSerializer_SerializeJson_ReturnsSameJson()
        {
            // Arrange
            JsonObject original = new JsonObject();
            original["key"] = "value";

            // Act
            Json result = Json.From(original);

            // Assert
            Assert.Equal(original.ToString(), result.ToString());
        }

        [Fact]
        public void JsonSerializer_SerializeArray_ReturnsArray()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("a");
            array.Add("b");

            // Act
            Json result = Json.From(array);

            // Assert
            Assert.Equal("[\"a\",\"b\"]", result.ToString());
        }

        private enum TestEnum
        {
            First,
            Second,
            Third
        }

        private class TestClass
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
    }
}
