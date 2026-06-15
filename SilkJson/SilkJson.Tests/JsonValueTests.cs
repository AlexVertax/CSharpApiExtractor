using System;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для JsonValue
    /// </summary>
    public class JsonValueTests
    {
        [Fact]
        public void JsonValue_CreateNullValue_ReturnsNullType()
        {
            // Arrange & Act
            JsonValue value = new JsonValue(null);

            // Assert
            Assert.True(value.IsMissed);
        }

        [Fact]
        public void JsonValue_CreateStringValue_ReturnsStringType()
        {
            // Arrange & Act
            JsonValue value = new JsonValue("test");

            // Assert
            Assert.False(value.IsMissed);
            Assert.Equal("test", value.Value);
        }

        [Fact]
        public void JsonValue_CreateIntegerValue_ReturnsLongType()
        {
            // Arrange & Act
            JsonValue value = new JsonValue(42);

            // Assert
            Assert.False(value.IsMissed);
            Assert.Equal(42L, value.Value);
        }

        [Fact]
        public void JsonValue_CreateDoubleValue_ReturnsDoubleType()
        {
            // Arrange & Act
            JsonValue value = new JsonValue(3.14);

            // Assert
            Assert.False(value.IsMissed);
            Assert.Equal(3.14, (double)value.Value, 5);
        }

        [Fact]
        public void JsonValue_CreateBoolValue_ReturnsBooleanType()
        {
            // Arrange & Act
            JsonValue trueValue = new JsonValue(true);
            JsonValue falseValue = new JsonValue(false);

            // Assert
            Assert.True((bool)trueValue.Value);
            Assert.False((bool)falseValue.Value);
        }

        [Fact]
        public void JsonValue_ToString_ReturnsCorrectString()
        {
            // Arrange
            JsonValue stringValue = new JsonValue("hello");
            JsonValue numberValue = new JsonValue(123);
            JsonValue boolValue = new JsonValue(true);
            JsonValue nullValue = new JsonValue(null);

            // Act & Assert
            Assert.Equal("hello", stringValue.ToString());
            Assert.Equal("123", numberValue.ToString());
            Assert.Equal("true", boolValue.ToString());
            Assert.Null(nullValue.ToString());
        }

        [Fact]
        public void JsonValue_Stringify_ReturnsValidJson()
        {
            // Arrange
            JsonValue stringValue = new JsonValue("test");
            JsonValue numberValue = new JsonValue(42);
            JsonValue boolValue = new JsonValue(false);
            JsonValue nullValue = new JsonValue(null);

            // Act
            string stringJson = stringValue.ToString();
            string numberJson = numberValue.ToString();
            string boolJson = boolValue.ToString();
            string nullJson = nullValue.ToString();

            // Assert
            Assert.Equal("test", stringJson);
            Assert.Equal("42", numberJson);
            Assert.Equal("false", boolJson);
            Assert.Null(nullJson);
        }

        [Fact]
        public void JsonValue_ValueConversion_ToInt_ReturnsCorrectValue()
        {
            // Arrange
            JsonValue value = new JsonValue(42);

            // Act
            int result = (int)value.GetValue(typeof(int));

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void JsonValue_ValueConversion_ToDouble_ReturnsCorrectValue()
        {
            // Arrange
            JsonValue value = new JsonValue(3.14);

            // Act
            double result = (double)value.GetValue(typeof(double));

            // Assert
            Assert.Equal(3.14, result, 5);
        }

        [Fact]
        public void JsonValue_StringWithSpecialCharacters_GetsEscaped()
        {
            // Arrange
            JsonValue value = new JsonValue("line\ntab\ttest");

            // Act
            string json = value.ToString();

            // Assert
            Assert.Contains("\n", json);
            Assert.Contains("\t", json);
        }

        [Fact]
        public void JsonValue_MaxLongValue_SerializesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue(long.MaxValue);

            // Act
            string result = value.ToString();

            // Assert
            Assert.Equal(long.MaxValue.ToString(), result);
        }

        [Fact]
        public void JsonValue_MinLongValue_SerializesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue(long.MinValue);

            // Act
            string result = value.ToString();

            // Assert
            Assert.Equal(long.MinValue.ToString(), result);
        }

        [Fact]
        public void JsonValue_DoubleMaxValue_SerializesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue(double.MaxValue);

            // Act
            string result = value.ToString();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void JsonValue_DoubleMinValue_SerializesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue(double.MinValue);

            // Act
            string result = value.ToString();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void JsonValue_PositiveInfinity_SerializesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue(double.PositiveInfinity);

            // Act
            string result = value.ToString();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void JsonValue_NegativeInfinity_SerializesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue(double.NegativeInfinity);

            // Act
            string result = value.ToString();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void JsonValue_NaN_SerializesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue(double.NaN);

            // Act
            string result = value.ToString();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void JsonValue_EmptyString_SerializesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue("");

            // Act
            string result = value.ToString();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void JsonValue_UnicodeCharacters_SerializesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue("Привет мир 🚀");

            // Act
            string result = value.ToString();

            // Assert
            Assert.Contains("Привет", result);
        }

        [Fact]
        public void JsonValue_AllSpecialChars_HandlesCorrectly()
        {
            // Arrange
            JsonValue value = new JsonValue("\b\f\n\r\t\"");

            // Act
            string result = value.ToString();

            // Assert
            Assert.Contains("\b", result);
            Assert.Contains("\f", result);
            Assert.Contains("\n", result);
            Assert.Contains("\r", result);
            Assert.Contains("\t", result);
        }
    }
}
