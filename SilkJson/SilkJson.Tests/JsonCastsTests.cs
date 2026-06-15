using System;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для Type Cast Methods в JsonCasts
    /// </summary>
    public class JsonCastsTests
    {
        [Fact]
        public void JsonCasts_ImplicitInt_CreatesJsonValue()
        {
            // Arrange & Act
            Json json = 42;

            // Assert
            Assert.Equal("42", json.ToString());
        }

        [Fact]
        public void JsonCasts_ImplicitString_CreatesJsonValue()
        {
            // Arrange & Act
            Json json = "test";

            // Assert
            Assert.Equal("test", json.ToString());
        }

        [Fact]
        public void JsonCasts_ImplicitBool_CreatesJsonValue()
        {
            // Arrange & Act
            Json json = true;

            // Assert
            Assert.Equal("true", json.ToString());
        }

        [Fact]
        public void JsonCasts_ImplicitDouble_CreatesJsonValue()
        {
            // Arrange & Act
            Json json = 3.14;

            // Assert
            Assert.Equal("3.14", json.ToString());
        }

        [Fact]
        public void JsonCasts_CastToInt_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(42);

            // Act
            int result = (int)json;

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void JsonCasts_CastToDouble_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(3.14);

            // Act
            double result = (double)json;

            // Assert
            Assert.Equal(3.14, result, 5);
        }

        [Fact]
        public void JsonCasts_CastToBool_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(true);

            // Act
            bool result = (bool)json;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void JsonCasts_CastToString_ReturnsStringValue()
        {
            // Arrange
            Json json = new JsonValue("hello");

            // Act
            string result = (string)json;

            // Assert
            Assert.Equal("hello", result);
        }

        [Fact]
        public void JsonCasts_CastToLong_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(1234567890123L);

            // Act
            long result = (long)json;

            // Assert
            Assert.Equal(1234567890123L, result);
        }

        [Fact]
        public void JsonCasts_CastToDateTime_ReturnsCorrectValue()
        {
            // Arrange
            DateTime dt = new DateTime(2023, 6, 15, 10, 30, 0);
            Json json = new JsonValue(dt.ToString("o"));

            // Act
            DateTime result = (DateTime)json;

            // Assert
            Assert.Equal(dt, result);
        }

        [Fact]
        public void JsonCasts_CastMethod_Int_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(42);

            // Act
            int result = json.Cast<int>();

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void JsonCasts_CastMethod_Double_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(3.14159);

            // Act
            double result = json.Cast<double>();

            // Assert
            Assert.Equal(3.14159, result, 5);
        }

        [Fact]
        public void JsonCasts_CastMethod_InvalidType_ReturnsDefault()
        {
            // Arrange - using a number that can't be parsed
            Json json = new JsonValue("not a number");

            // Act - when ThrowOnInvalidCast is false (default), should return default
            // Note: JsonValue stores strings as-is, and Convert.ChangeType throws FormatException
            // The library catches Exception (including FormatException) and returns default
            int result = json.Cast<int>(throwOnInvalidCast: false);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void JsonCasts_CastMethod_NullJson_ReturnsDefault()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act - when ThrowOnInvalidCast is false, should return default
            int result = json.Cast<int>(throwOnInvalidCast: false);

            // Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(999999)]
        public void JsonCasts_CastMethod_VariousInts_ReturnsCorrectValue(int value)
        {
            // Arrange
            Json json = new JsonValue(value);

            // Act
            int result = json.Cast<int>();

            // Assert
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(1.5)]
        [InlineData(-2.7)]
        [InlineData(3.14159)]
        public void JsonCasts_CastMethod_VariousDoubles_ReturnsCorrectValue(double value)
        {
            // Arrange
            Json json = new JsonValue(value);

            // Act
            double result = json.Cast<double>();

            // Assert
            Assert.Equal(value, result, 5);
        }

        #region Bool Method Tests

        [Fact]
        public void JsonCasts_Bool_ReturnsTrue_WhenValueIsTrue()
        {
            // Arrange
            Json json = new JsonValue(true);

            // Act
            bool result = json.Bool();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void JsonCasts_Bool_ReturnsFalse_WhenValueIsFalse()
        {
            // Arrange
            Json json = new JsonValue(false);

            // Act
            bool result = json.Bool();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void JsonCasts_Bool_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            bool result = json.Bool(true);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void JsonCasts_Bool_ReturnsDefaultValue_WhenInvalidCast()
        {
            // Arrange
            Json json = new JsonValue("not a bool");

            // Act
            bool result = json.Bool(true);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Int Method Tests

        [Fact]
        public void JsonCasts_Int_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(42);

            // Act
            int result = json.Int();

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void JsonCasts_Int_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            int result = json.Int(-1);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void JsonCasts_Int_ReturnsDefaultValue_WhenInvalidCast()
        {
            // Arrange
            Json json = new JsonValue("not a number");

            // Act
            int result = json.Int(-1);

            // Assert
            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(999999)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void JsonCasts_Int_VariousValues_ReturnsCorrectValue(int value)
        {
            // Arrange
            Json json = new JsonValue(value);

            // Act
            int result = json.Int();

            // Assert
            Assert.Equal(value, result);
        }

        #endregion

        #region Long Method Tests

        [Fact]
        public void JsonCasts_Long_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(1234567890123L);

            // Act
            long result = json.Long();

            // Assert
            Assert.Equal(1234567890123L, result);
        }

        [Fact]
        public void JsonCasts_Long_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            long result = json.Long(-1L);

            // Assert
            Assert.Equal(-1L, result);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(-1L)]
        [InlineData(9223372036854775807L)]
        public void JsonCasts_Long_VariousValues_ReturnsCorrectValue(long value)
        {
            // Arrange
            Json json = new JsonValue(value);

            // Act
            long result = json.Long();

            // Assert
            Assert.Equal(value, result);
        }

        #endregion

        #region Double Method Tests

        [Fact]
        public void JsonCasts_Double_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(3.14159);

            // Act
            double result = json.Double();

            // Assert
            Assert.Equal(3.14159, result, 5);
        }

        [Fact]
        public void JsonCasts_Double_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            double result = json.Double(-1.5);

            // Assert
            Assert.Equal(-1.5, result, 5);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(1.5)]
        [InlineData(-2.7)]
        [InlineData(3.14159265359)]
        [InlineData(double.MaxValue)]
        public void JsonCasts_Double_VariousValues_ReturnsCorrectValue(double value)
        {
            // Arrange
            Json json = new JsonValue(value);

            // Act
            double result = json.Double();

            // Assert
            Assert.Equal(value, result, 5);
        }

        #endregion

        #region Float Method Tests

        [Fact]
        public void JsonCasts_Float_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(3.14f);

            // Act
            float result = json.Float();

            // Assert
            Assert.Equal(3.14f, result, 2);
        }

        [Fact]
        public void JsonCasts_Float_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            float result = json.Float(-1.5f);

            // Assert
            Assert.Equal(-1.5f, result, 2);
        }

        #endregion

        #region Decimal Method Tests

        [Fact]
        public void JsonCasts_Decimal_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(123.456m);

            // Act
            decimal result = json.Decimal();

            // Assert
            Assert.Equal(123.456m, result);
        }

        [Fact]
        public void JsonCasts_Decimal_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            decimal result = json.Decimal(-1.5m);

            // Assert
            Assert.Equal(-1.5m, result);
        }

        #endregion

        #region Byte Method Tests

        [Fact]
        public void JsonCasts_Byte_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue(255);

            // Act
            byte result = json.Byte();

            // Assert
            Assert.Equal(255, result);
        }

        [Fact]
        public void JsonCasts_Byte_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            byte result = json.Byte(128);

            // Assert
            Assert.Equal(128, result);
        }

        #endregion

        #region Short Method Tests

        [Fact]
        public void JsonCasts_Short_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue((short)32000);

            // Act
            short result = json.Short();

            // Assert
            Assert.Equal(32000, result);
        }

        [Fact]
        public void JsonCasts_Short_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            short result = json.Short(-1);

            // Assert
            Assert.Equal(-1, result);
        }

        #endregion

        #region UShort Method Tests

        [Fact]
        public void JsonCasts_UShort_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue((ushort)65535);

            // Act
            ushort result = json.UShort();

            // Assert
            Assert.Equal(65535, result);
        }

        [Fact]
        public void JsonCasts_UShort_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            ushort result = json.UShort(100);

            // Assert
            Assert.Equal(100, result);
        }

        #endregion

        #region UInt Method Tests

        [Fact]
        public void JsonCasts_UInt_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue((uint)4294967295);

            // Act
            uint result = json.UInt();

            // Assert
            Assert.Equal(4294967295u, result);
        }

        [Fact]
        public void JsonCasts_UInt_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            uint result = json.UInt(100u);

            // Assert
            Assert.Equal(100u, result);
        }

        #endregion

        #region ULong Method Tests

        [Fact]
        public void JsonCasts_ULong_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue((ulong)18446744073709551615);

            // Act
            ulong result = json.ULong();

            // Assert
            Assert.Equal(18446744073709551615ul, result);
        }

        [Fact]
        public void JsonCasts_ULong_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            ulong result = json.ULong(100ul);

            // Assert
            Assert.Equal(100ul, result);
        }

        #endregion

        #region Char Method Tests

        [Fact]
        public void JsonCasts_Char_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue('A');

            // Act
            char result = json.Char();

            // Assert
            Assert.Equal('A', result);
        }

        [Fact]
        public void JsonCasts_Char_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            char result = json.Char('Z');

            // Assert
            Assert.Equal('Z', result);
        }

        [Fact]
        public void JsonCasts_Char_ReturnsDefaultValue_WhenInvalidCast()
        {
            // Arrange
            Json json = new JsonValue("AB");

            // Act
            char result = json.Char('Z');

            // Assert
            Assert.Equal('Z', result);
        }

        #endregion

        #region String Method Tests

        [Fact]
        public void JsonCasts_String_ReturnsCorrectValue()
        {
            // Arrange
            Json json = new JsonValue("hello world");

            // Act
            string result = json.String();

            // Assert
            Assert.Equal("hello world", result);
        }

        [Fact]
        public void JsonCasts_String_ReturnsNull_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            string result = json.String();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void JsonCasts_String_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);

            // Act
            string result = json.String("default");

            // Assert
            Assert.Equal("default", result);
        }

        [Fact]
        public void JsonCasts_String_ReturnsEmptyString_WhenNonNullJsonValue()
        {
            // Arrange
            Json json = new JsonValue(42);

            // Act
            string result = json.String();

            // Assert
            Assert.Equal("42", result);
        }

        [Fact]
        public void JsonCasts_String_WithNumbers_ReturnsStringValue()
        {
            // Arrange
            Json json = new JsonValue(12345);

            // Act
            string result = json.String("fallback");

            // Assert
            Assert.Equal("12345", result);
        }

        #endregion

        #region DateTime Method Tests

        [Fact]
        public void JsonCasts_DateTime_ReturnsCorrectValue()
        {
            // Arrange
            DateTime dt = new DateTime(2023, 6, 15, 10, 30, 0);
            Json json = new JsonValue(dt.ToString("o"));

            // Act
            DateTime result = json.DateTime();

            // Assert
            Assert.Equal(dt, result);
        }

        [Fact]
        public void JsonCasts_DateTime_ReturnsDefaultValue_WhenValueIsNull()
        {
            // Arrange
            DateTime defaultDt = new DateTime(2000, 1, 1);
            Json json = new JsonValue(null);

            // Act
            DateTime result = json.DateTime(defaultDt);

            // Assert
            Assert.Equal(defaultDt, result);
        }

        #endregion

        #region ThrowOnInvalidCast Tests

        [Fact]
        public void JsonCasts_Cast_ThrowsOnInvalidCast_WhenThrowOnInvalidCastIsTrue()
        {
            // Arrange
            Json json = new JsonValue("not a number");
            Json.ThrowOnInvalidCast = true;

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => json.Cast<int>());
        }

        [Fact]
        public void JsonCasts_Cast_ThrowsOnInvalidCast_WhenJsonIsNull()
        {
            // Arrange
            Json json = new JsonValue(null);
            Json.ThrowOnInvalidCast = true;

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => json.Cast<int>());
        }

        [Fact]
        public void JsonCasts_Cast_ThrowsOnInvalidCast_WhenJsonIsNotJsonValue()
        {
            // Arrange
            Json json = new JsonArray();
            Json.ThrowOnInvalidCast = true;

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => json.Cast<int>());
        }

        [Fact]
        public void JsonCasts_Cast_ThrowsOnInvalidCast_WhenNullValue()
        {
            // Arrange
            Json json = new JsonValue(null);
            Json.ThrowOnInvalidCast = true;

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => json.Cast<int>());
        }

        [Fact]
        public void JsonCasts_Cast_ThrowsOnInvalidCast_ViaParameter()
        {
            // Arrange
            Json json = new JsonValue("not a number");

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => json.Cast<int>(throwOnInvalidCast: true));
        }

        [Fact]
        public void JsonCasts_Cast_ReturnsDefault_ViaParameter_WhenThrowIsFalse()
        {
            // Arrange
            Json json = new JsonValue("not a number");
            Json.ThrowOnInvalidCast = true; // Global setting is true, but parameter overrides

            // Act
            int result = json.Cast<int>(throwOnInvalidCast: false);

            // Assert
            Assert.Equal(0, result);
        }

        #endregion

        #region Type Check Method Tests

        [Fact]
        public void JsonCasts_IsBool_ReturnsTrue_WhenValueIsBool()
        {
            // Arrange
            Json json = new JsonValue(true);

            // Act & Assert
            Assert.True(json.IsBool());
        }

        [Fact]
        public void JsonCasts_IsBool_ReturnsFalse_WhenValueIsNotBool()
        {
            // Arrange
            Json json = new JsonValue(42);

            // Act & Assert
            Assert.False(json.IsBool());
        }

        [Fact]
        public void JsonCasts_IsDouble_ReturnsTrue_WhenValueIsDouble()
        {
            // Arrange
            Json json = new JsonValue(3.14);

            // Act & Assert
            Assert.True(json.IsDouble());
        }

        [Fact]
        public void JsonCasts_IsLong_ReturnsTrue_WhenValueIsLong()
        {
            // Arrange
            Json json = new JsonValue(1234567890123L);

            // Act & Assert
            Assert.True(json.IsLong());
        }

        [Fact]
        public void JsonCasts_IsString_ReturnsTrue_WhenValueIsString()
        {
            // Arrange
            Json json = new JsonValue("test");

            // Act & Assert
            Assert.True(json.IsString());
        }

        #endregion
    }
}
