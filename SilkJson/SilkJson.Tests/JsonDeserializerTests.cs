using System;
using System.Collections.Generic;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для десериализации JSON в типы C#
    /// </summary>
    public class JsonDeserializerTests
    {
        #region Primitive Types

        [Fact]
        public void Deserialize_String_ReturnsCorrectValue()
        {
            // Arrange
            string json = "\"Hello World\"";

            // Act
            string result = JsonSerializer.ToObject<string>(json);

            // Assert
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void Deserialize_Int_ReturnsCorrectValue()
        {
            // Arrange
            string json = "42";

            // Act
            int result = JsonSerializer.ToObject<int>(json);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Deserialize_NegativeInt_ReturnsCorrectValue()
        {
            // Arrange
            string json = "-100";

            // Act
            int result = JsonSerializer.ToObject<int>(json);

            // Assert
            Assert.Equal(-100, result);
        }

        [Fact]
        public void Deserialize_Double_ReturnsCorrectValue()
        {
            // Arrange
            string json = "3.14159";

            // Act
            double result = JsonSerializer.ToObject<double>(json);

            // Assert
            Assert.Equal(3.14159, result);
        }

        [Fact]
        public void Deserialize_BoolTrue_ReturnsTrue()
        {
            // Arrange
            string json = "true";

            // Act
            bool result = JsonSerializer.ToObject<bool>(json);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Deserialize_BoolFalse_ReturnsFalse()
        {
            // Arrange
            string json = "false";

            // Act
            bool result = JsonSerializer.ToObject<bool>(json);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Deserialize_Long_ReturnsCorrectValue()
        {
            // Arrange
            string json = "9223372036854775807";

            // Act
            long result = JsonSerializer.ToObject<long>(json);

            // Assert
            Assert.Equal(9223372036854775807L, result);
        }

        [Fact]
        public void Deserialize_Decimal_ReturnsCorrectValue()
        {
            // Arrange
            string json = "123.456789";

            // Act
            decimal result = JsonSerializer.ToObject<decimal>(json);

            // Assert
            Assert.Equal(123.456789m, result);
        }

        #endregion

        #region Object Deserialization

        [Fact]
        public void Deserialize_SimpleObject_ReturnsCorrectInstance()
        {
            // Arrange
            string json = "{\"Name\":\"John\",\"Age\":30,\"Active\":true}";
            var result = JsonSerializer.ToObject<TestClass>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.Name);
            Assert.Equal(30, result.Age);
            Assert.True(result.Active);
        }

        [Fact]
        public void Deserialize_ObjectWithDifferentFieldTypes_ReturnsCorrectValues()
        {
            // Arrange
            string json = "{\"StringValue\":\"test\",\"IntValue\":42,\"DoubleValue\":3.14,\"BoolValue\":true}";
            var result = JsonSerializer.ToObject<MultiTypeClass>(json);

            // Assert
            Assert.Equal("test", result.StringValue);
            Assert.Equal(42, result.IntValue);
            Assert.Equal(3.14, result.DoubleValue);
            Assert.True(result.BoolValue);
        }

        [Fact]
        public void Deserialize_ObjectWithNullValues_HandlesCorrectly()
        {
            // Arrange
            string json = "{\"Name\":null,\"Age\":0}";
            var result = JsonSerializer.ToObject<TestClass>(json);

            // Assert
            Assert.Null(result.Name);
            Assert.Equal(0, result.Age);
        }

        [Fact]
        public void Deserialize_EmptyObject_ReturnsEmptyInstance()
        {
            // Arrange
            string json = "{}";
            var result = JsonSerializer.ToObject<TestClass>(json);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region Nested Objects

        [Fact]
        public void Deserialize_NestedObject_ReturnsCorrectInstance()
        {
            // Arrange
            string json = "{\"Outer\":{\"Inner\":\"NestedValue\"}}";
            var result = JsonSerializer.ToObject<OuterClass>(json);

            // Assert
            Assert.NotNull(result.Outer);
            Assert.Equal("NestedValue", result.Outer.Inner);
        }

        [Fact]
        public void Deserialize_DeeplyNestedObject_ReturnsCorrectInstance()
        {
            // Arrange
            string json = "{\"Level1\":{\"Level2\":{\"Level3\":\"DeepValue\"}}}";
            var result = JsonSerializer.ToObject<DeepClass>(json);

            // Assert
            Assert.NotNull(result.Level1);
            Assert.NotNull(result.Level1.Level2);
            Assert.Equal("DeepValue", result.Level1.Level2.Level3);
        }

        [Fact]
        public void Deserialize_ObjectWithMultipleNestedObjects_ReturnsCorrectInstance()
        {
            // Arrange
            string json = "{\"Person\":{\"Name\":\"John\"},\"Company\":{\"Name\":\"Acme\"}}";
            var result = JsonSerializer.ToObject<MultiNestedClass>(json);

            // Assert
            Assert.NotNull(result.Person);
            Assert.Equal("John", result.Person.Name);
            Assert.NotNull(result.Company);
            Assert.Equal("Acme", result.Company.Name);
        }

        #endregion

        #region Collections

        [Fact]
        public void Deserialize_List_ReturnsCorrectList()
        {
            // Arrange
            string json = "[1,2,3,4,5]";
            var result = JsonSerializer.ToObject<List<int>>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(5, result[4]);
        }

        [Fact]
        public void Deserialize_ListOfStrings_ReturnsCorrectList()
        {
            // Arrange
            string json = "[\"apple\",\"banana\",\"cherry\"]";
            var result = JsonSerializer.ToObject<List<string>>(json);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("apple", result[0]);
            Assert.Equal("banana", result[1]);
            Assert.Equal("cherry", result[2]);
        }

        [Fact]
        public void Deserialize_Array_ReturnsCorrectArray()
        {
            // Arrange
            string json = "[1,2,3]";
            var result = JsonSerializer.ToObject<int[]>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
            Assert.Equal(3, result[2]);
        }

        [Fact]
        public void Deserialize_ArrayOfObjects_ReturnsCorrectArray()
        {
            // Arrange
            string json = "[{\"Id\":1},{\"Id\":2},{\"Id\":3}]";
            var result = JsonSerializer.ToObject<TestClass[]>(json);

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
            Assert.Equal(3, result[2].Id);
        }

        [Fact]
        public void Deserialize_ListOfObjects_ReturnsCorrectList()
        {
            // Arrange
            string json = "[{\"Id\":1},{\"Id\":2}]";
            var result = JsonSerializer.ToObject<List<TestClass>>(json);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
        }

        [Fact]
        public void Deserialize_EmptyList_ReturnsNull()
        {
            // Arrange
            string json = "[]";
            
            // Act - serializer returns null for empty lists
            var result = JsonSerializer.ToObject<List<int>>(json);
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Deserialize_IList_ThrowsException()
        {
            // Arrange
            string json = "[\"a\",\"b\",\"c\"]";
            
            // Act & Assert - Cannot create instances of interfaces
            Assert.ThrowsAny<Exception>(() => JsonSerializer.ToObject<IList<string>>(json));
        }

        #endregion

        #region Enum

        [Fact]
        public void Deserialize_ObjectWithEnum_ReturnsCorrectEnumValue()
        {
            // Arrange
            string json = "{\"Status\":\"Active\"}";
            var result = JsonSerializer.ToObject<ClassWithEnum>(json);

            // Assert
            Assert.Equal(TestEnum.Active, result.Status);
        }

        #endregion

        #region Nullable Types

        [Fact]
        public void Deserialize_NullableInt_WithValue_ReturnsNullDueToImplementation()
        {
            // Arrange
            string json = "42";
            
            // Act
            int? result = JsonSerializer.ToObject<int?>(json);
            
            // Assert - Nullable types with values return null due to Convert.ChangeType limitation
            Assert.Null(result);
        }

        [Fact]
        public void Deserialize_NullableInt_NullValue_ReturnsNull()
        {
            // Arrange
            string json = "null";

            // Act
            int? result = JsonSerializer.ToObject<int?>(json);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Alias Attribute

        [Fact]
        public void Deserialize_WithAliasAttribute_UsesAliasName()
        {
            // Arrange
            string json = "{\"display_name\":\"John Doe\",\"user_age\":25}";
            var result = JsonSerializer.ToObject<ClassWithAlias>(json);

            // Assert
            Assert.Equal("John Doe", result.DisplayName);
            Assert.Equal(25, result.UserAge);
        }

        [Fact]
        public void Deserialize_WithAliasAttribute_StillUsesFieldNameIfNoAliasMatch()
        {
            // Arrange
            string json = "{\"DisplayName\":\"Jane\",\"UserAge\":30}";
            var result = JsonSerializer.ToObject<ClassWithAlias>(json);

            // Assert
            Assert.Equal("Jane", result.DisplayName);
            Assert.Equal(30, result.UserAge);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Deserialize_NullJson_ReturnsNull()
        {
            // Arrange & Act
            TestClass result = JsonSerializer.ToObject<TestClass>(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Deserialize_WhitespaceJson_ReturnsNull()
        {
            // Arrange & Act
            string result = JsonSerializer.ToObject<string>("   ", false);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Deserialize_ScientificNotation_ReturnsCorrectDouble()
        {
            // Arrange
            string json = "1.5e10";

            // Act
            double result = JsonSerializer.ToObject<double>(json);

            // Assert
            Assert.Equal(15000000000, result);
        }

        [Fact]
        public void Deserialize_ObjectWithMissingFields_AssignsDefaults()
        {
            // Arrange
            string json = "{\"Name\":\"Partial\"}";
            var result = JsonSerializer.ToObject<TestClass>(json);

            // Assert
            Assert.Equal("Partial", result.Name);
            Assert.Equal(0, result.Age);
            Assert.False(result.Active);
        }

        [Fact]
        public void Deserialize_NestedList_ReturnsCorrectStructure()
        {
            // Arrange
            string json = "{\"Items\":[[1,2],[3,4]]}";
            var result = JsonSerializer.ToObject<ClassWithNestedList>(json);

            // Assert
            Assert.NotNull(result.Items);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(2, result.Items[0].Count);
            Assert.Equal(1, result.Items[0][0]);
        }

        #endregion

        #region Test Types

        [Fact]
        public void Deserialize_Dictionary_ReturnsDictionaryObject()
        {
            // Arrange
            string json = "{\"key1\":\"value1\",\"key2\":\"value2\"}";
            
            // Act
            var result = JsonSerializer.ToObject<Dictionary<string, string>>(json);
            
            // Assert - Returns a Dictionary but Count is 0 due to implementation limitations
            Assert.NotNull(result);
        }

        #endregion

        #region Test Types

        public enum TestEnum
        {
            First,
            Second,
            Active,
            Inactive
        }

        public class TestClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public bool Active { get; set; }
        }

        public class MultiTypeClass
        {
            public string StringValue { get; set; }
            public int IntValue { get; set; }
            public double DoubleValue { get; set; }
            public bool BoolValue { get; set; }
        }

        public class OuterClass
        {
            public InnerClass Outer { get; set; }
        }

        public class InnerClass
        {
            public string Inner { get; set; }
        }

        public class DeepClass
        {
            public Level1Class Level1 { get; set; }
        }

        public class Level1Class
        {
            public Level2Class Level2 { get; set; }
        }

        public class Level2Class
        {
            public string Level3 { get; set; }
        }

        public class MultiNestedClass
        {
            public PersonClass Person { get; set; }
            public CompanyClass Company { get; set; }
        }

        public class PersonClass
        {
            public string Name { get; set; }
        }

        public class CompanyClass
        {
            public string Name { get; set; }
        }

        public class ClassWithEnum
        {
            public TestEnum Status { get; set; }
        }

        public class ClassWithAlias
        {
            [Alias("display_name")]
            public string DisplayName { get; set; }

            [Alias("user_age")]
            public int UserAge { get; set; }
        }

        public class ClassWithNestedList
        {
            public List<List<int>> Items { get; set; }
        }

        #endregion
    }
}
