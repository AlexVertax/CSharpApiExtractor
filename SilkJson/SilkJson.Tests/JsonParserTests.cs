using System;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для JsonParser
    /// </summary>
    public class JsonParserTests
    {
        [Fact]
        public void JsonParser_ParseNull_ReturnsNullValue()
        {
            // Arrange & Act
            Json result = Json.Parse("null");

            // Assert
            Assert.True(result.IsMissed);
        }

        [Fact]
        public void JsonParser_ParseString_ReturnsStringValue()
        {
            // Arrange & Act
            Json result = Json.Parse("\"hello\"");

            // Assert
            Assert.Equal("hello", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseInteger_ReturnsNumberValue()
        {
            // Arrange & Act
            Json result = Json.Parse("42");

            // Assert
            Assert.Equal("42", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseDouble_ReturnsDoubleValue()
        {
            // Arrange & Act
            Json result = Json.Parse("3.14");

            // Assert
            Assert.Equal("3.14", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseNegativeNumber_ReturnsNegativeValue()
        {
            // Arrange & Act
            Json result = Json.Parse("-42");

            // Assert
            Assert.Equal("-42", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseTrue_ReturnsTrueBoolean()
        {
            // Arrange & Act
            Json result = Json.Parse("true");

            // Assert
            Assert.Equal("true", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseFalse_ReturnsFalseBoolean()
        {
            // Arrange & Act
            Json result = Json.Parse("false");

            // Assert
            Assert.Equal("false", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseEmptyObject_ReturnsEmptyObject()
        {
            // Arrange & Act
            JsonObject result = JsonObject.Parse("{}");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("{}", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseObject_ReturnsValidObject()
        {
            // Arrange & Act
            JsonObject result = JsonObject.Parse("{\"name\":\"John\",\"age\":30}");

            // Assert
            Assert.Equal("John", result["name"].ToString());
            Assert.Equal("30", result["age"].ToString());
        }

        [Fact]
        public void JsonParser_ParseEmptyArray_ReturnsEmptyArray()
        {
            // Arrange & Act
            JsonArray result = JsonArray.Parse("[]");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void JsonParser_ParseArray_ReturnsValidArray()
        {
            // Arrange & Act
            JsonArray result = JsonArray.Parse("[\"first\",2,true]");

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("first", result[0].ToString());
            Assert.Equal("2", result[1].ToString());
            string r3 = result[2].ToString();
            Assert.Equal("true", r3);
        }

        [Fact]
        public void JsonParser_ParseNestedObject_ReturnsValidNestedObject()
        {
            // Arrange & Act
            JsonObject result = JsonObject.Parse("{\"person\":{\"name\":\"John\",\"age\":30}}");

            // Assert
            Assert.Equal("John", result["person"]["name"].ToString());
            Assert.Equal("30", result["person"]["age"].ToString());
        }

        [Fact]
        public void JsonParser_ParseArrayOfObjects_ReturnsValidArray()
        {
            // Arrange & Act
            JsonArray result = JsonArray.Parse("[{\"id\":1},{\"id\":2}]");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("1", result[0]["id"].ToString());
            Assert.Equal("2", result[1]["id"].ToString());
        }

        [Fact]
        public void JsonParser_ParseStringWithEscapes_ParsesCorrectly()
        {
            // Arrange & Act
            Json result = Json.Parse("\"line\\ntab\\tquote\\\"\"");

            // Assert
            Assert.Contains("line\ntab", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseUnicode_ParsesCorrectly()
        {
            // Arrange & Act
            Json result = Json.Parse("\"\\u0041\"");

            // Assert
            Assert.Equal("A", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseWithWhitespace_ParsesCorrectly()
        {
            // Arrange & Act
            JsonObject result = JsonObject.Parse("{ \"name\" : \"John\" , \"age\" : 30 }");

            // Assert
            Assert.Equal("John", result["name"].ToString());
            Assert.Equal("30", result["age"].ToString());
        }

        [Fact]
        public void JsonParser_ParseScientificNotation_ParsesCorrectly()
        {
            // Arrange & Act
            Json result = Json.Parse("1.5e10");

            // Assert
            Assert.Equal("15000000000", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseNegativeExponent_ParsesCorrectly()
        {
            // Arrange & Act
            Json result = Json.Parse("1.5e-2");

            // Assert
            Assert.Equal("0.015", result.ToString());
        }

        [Fact]
        public void JsonParser_ParseEmptyString_ReturnsNull()
        {
            // Arrange & Act
            Json result = Json.Parse("");

            // Assert
            Assert.True(result.IsMissed);
        }

        [Fact]
        public void JsonParser_ParseNullString_ReturnsNull()
        {
            // Arrange & Act
            Json result = Json.Parse(null);

            // Assert
            Assert.True(result.IsMissed);
        }

        [Fact]
        public void JsonParser_ParseInvalidJson_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsAny<Exception>(() => Json.Parse("{invalid"));
        }

        [Fact]
        public void JsonParser_ParseUnterminatedString_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsAny<Exception>(() => Json.Parse("\"unterminated"));
        }

        [Fact]
        public void JsonParser_ParseInvalidNumber_ReturnsNullOrZero()
        {
            // Arrange & Act
            Json result = Json.Parse("123.456.789");

            // Assert
            Assert.NotNull(result);
        }
    }
}
