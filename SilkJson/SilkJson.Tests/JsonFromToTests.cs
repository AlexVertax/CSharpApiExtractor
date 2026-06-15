using System;
using System.Collections.Generic;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для Json.From и Json.To методов
    /// </summary>
    public class JsonFromToTests
    {
        #region Json.From Tests

        [Fact]
        public void JsonFrom_FromString_ReturnsJsonValue()
        {
            // Arrange
            string value = "hello";

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("hello", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromInteger_ReturnsJsonNumber()
        {
            // Arrange
            int value = 42;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("42", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromLong_ReturnsJsonNumber()
        {
            // Arrange
            long value = 1234567890L;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("1234567890", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromDouble_ReturnsJsonNumber()
        {
            // Arrange
            double value = 3.14159;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("3.14159", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromBool_ReturnsJsonBoolean()
        {
            // Arrange
            bool value = true;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("true", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromFalseBool_ReturnsFalse()
        {
            // Arrange
            bool value = false;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("false", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromNull_ReturnsNullJson()
        {
            // Act
            Json result = Json.From(null);

            // Assert
            Assert.True(result.IsMissed);
        }

        [Fact]
        public void JsonFrom_FromDateTime_ReturnsISOString()
        {
            // Arrange
            DateTime value = new DateTime(2023, 6, 15, 14, 30, 0);

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("2023-06-15T14:30:00", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromEnum_ReturnsEnumName()
        {
            // Arrange
            TestStatus status = TestStatus.Active;

            // Act
            Json result = Json.From(status);

            // Assert
            Assert.Equal("Active", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromList_ReturnsJsonArray()
        {
            // Arrange
            List<int> list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            Json result = Json.From(list);

            // Assert
            Assert.Equal("[1,2,3,4,5]", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromStringList_ReturnsJsonArray()
        {
            // Arrange
            List<string> list = new List<string> { "a", "b", "c" };

            // Act
            Json result = Json.From(list);

            // Assert
            Assert.Equal("[\"a\",\"b\",\"c\"]", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromDictionary_ReturnsJsonObject()
        {
            // Arrange
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                ["name"] = "John",
                ["age"] = 30
            };

            // Act
            Json result = Json.From(dict);

            // Assert
            string str = result.ToString();
            Assert.Contains("name", str);
            Assert.Contains("John", str);
            Assert.Contains("age", str);
        }

        [Fact]
        public void JsonFrom_FromComplexObject_ReturnsJsonObject()
        {
            // Arrange
            TestPerson person = new TestPerson
            {
                Name = "Alice",
                Age = 25,
                Email = "alice@example.com"
            };

            // Act
            Json result = Json.From(person);

            // Assert
            string str = result.ToString();
            Assert.Contains("Name", str);
            Assert.Contains("Alice", str);
            Assert.Contains("Age", str);
            Assert.Contains("25", str);
        }

        [Fact]
        public void JsonFrom_FromNestedObject_ReturnsNestedJson()
        {
            // Arrange
            TestOrder order = new TestOrder
            {
                Id = 1,
                Customer = new TestPerson { Name = "Bob", Age = 35 },
                Items = new List<string> { "Item1", "Item2" }
            };

            // Act
            Json result = Json.From(order);

            // Assert
            string str = result.ToString();
            Assert.Contains("Id", str);
            Assert.Contains("Customer", str);
            Assert.Contains("Bob", str);
            Assert.Contains("Items", str);
        }

        [Fact]
        public void JsonFrom_FromDecimal_ConvertsToDouble()
        {
            // Arrange
            decimal value = 123.456m;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("123.456", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromShort_ReturnsNumber()
        {
            // Arrange
            short value = 100;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("100", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromFloat_ReturnsNumber()
        {
            // Arrange
            float value = 1.5f;

            // Act
            Json result = Json.From(value);

            // Assert
            Assert.Equal("1.5", result.ToString());
        }

        [Fact]
        public void JsonFrom_FromArray_ReturnsJsonArray()
        {
            // Arrange
            int[] array = new int[] { 10, 20, 30 };

            // Act
            Json result = Json.From(array);

            // Assert
            Assert.Equal("[10,20,30]", result.ToString());
        }

        #endregion

        #region Json.To<T> Tests (Instance Method)

        [Fact]
        public void JsonTo_ToString_ReturnsOriginalString()
        {
            // Arrange
            Json json = Json.From("test value");

            // Act
            string result = json.To<string>();

            // Assert
            Assert.Equal("test value", result);
        }

        [Fact]
        public void JsonTo_ToInt_ReturnsOriginalInt()
        {
            // Arrange
            Json json = Json.From(42);

            // Act
            int result = json.To<int>();

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void JsonTo_ToDouble_ReturnsOriginalDouble()
        {
            // Arrange
            Json json = Json.From(3.14);

            // Act
            double result = json.To<double>();

            // Assert
            Assert.Equal(3.14, result);
        }

        [Fact]
        public void JsonTo_ToBool_ReturnsOriginalBool()
        {
            // Arrange
            Json json = Json.From(true);

            // Act
            bool result = json.To<bool>();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void JsonTo_ToObject_ReturnsDeserializedObject()
        {
            // Arrange
            Json json = Json.From(new TestPerson { Name = "Charlie", Age = 40 });

            // Act
            TestPerson result = json.To<TestPerson>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Charlie", result.Name);
            Assert.Equal(40, result.Age);
        }

        [Fact]
        public void JsonTo_ToNestedObject_ReturnsDeserializedNestedObject()
        {
            // Arrange
            TestOrder order = new TestOrder
            {
                Id = 100,
                Customer = new TestPerson { Name = "Diana", Age = 28 },
                Items = new List<string> { "X", "Y" }
            };
            Json json = Json.From(order);

            // Act
            TestOrder result = json.To<TestOrder>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.Id);
            Assert.NotNull(result.Customer);
            Assert.Equal("Diana", result.Customer.Name);
            Assert.Equal(28, result.Customer.Age);
            Assert.NotNull(result.Items);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public void JsonTo_ToList_ReturnsDeserializedList()
        {
            // Arrange
            Json json = Json.From(new List<int> { 1, 2, 3 });

            // Act
            List<int> result = json.To<List<int>>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
            Assert.Equal(3, result[2]);
        }

        [Fact]
        public void JsonTo_ToArray_ReturnsDeserializedArray()
        {
            // Arrange
            Json json = Json.From(new string[] { "first", "second" });

            // Act
            string[] result = json.To<string[]>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal("first", result[0]);
            Assert.Equal("second", result[1]);
        }

        [Fact]
        public void JsonTo_ToEnum_ReturnsEnumValue()
        {
            // Arrange
            Json json = Json.From(TestStatus.Pending);

            // Act
            TestStatus result = json.To<TestStatus>();

            // Assert
            Assert.Equal(TestStatus.Pending, result);
        }

        [Fact]
        public void JsonTo_ToBoolFalse_ReturnsFalse()
        {
            // Arrange
            Json json = Json.From(false);

            // Act
            bool result = json.To<bool>();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void JsonTo_ToComplexObjectWithNullFields_ReturnsObject()
        {
            // Arrange
            Json json = Json.Parse("{\"Name\": \"Eve\", \"Age\": 22}");

            // Act
            TestPerson result = json.To<TestPerson>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Eve", result.Name);
            Assert.Equal(22, result.Age);
        }

        #endregion

        #region Json.To (Static Method) Tests

        [Fact]
        public void JsonToStatic_FromJsonString_ReturnsDeserializedObject()
        {
            // Arrange
            string json = "{\"Name\": \"Frank\", \"Age\": 45}";

            // Act
            TestPerson result = JsonSerializer.ToObject<TestPerson>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Frank", result.Name);
            Assert.Equal(45, result.Age);
        }

        [Fact]
        public void JsonToStatic_FromJsonArrayString_ReturnsDeserializedList()
        {
            // Arrange
            string json = "[1, 2, 3, 4, 5]";

            // Act
            List<int> result = JsonSerializer.ToObject<List<int>>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void JsonToStatic_FromJsonStringArray_ReturnsStringList()
        {
            // Arrange
            string json = "[\"a\", \"b\", \"c\"]";

            // Act
            List<string> result = JsonSerializer.ToObject<List<string>>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("a", result[0]);
            Assert.Equal("b", result[1]);
            Assert.Equal("c", result[2]);
        }

        [Fact]
        public void JsonToStatic_FromNestedJson_ReturnsNestedObject()
        {
            // Arrange
            string json = "{\"Id\": 1, \"Customer\": {\"Name\": \"Grace\", \"Age\": 31}, \"Items\": [\"P1\", \"P2\"]}";

            // Act
            TestOrder result = JsonSerializer.ToObject<TestOrder>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.NotNull(result.Customer);
            Assert.Equal("Grace", result.Customer.Name);
            Assert.Equal(31, result.Customer.Age);
            Assert.NotNull(result.Items);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public void JsonToStatic_FromSimpleIntJson_ReturnsInt()
        {
            // Arrange
            string json = "42";

            // Act
            int result = JsonSerializer.ToObject<int>(json);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void JsonToStatic_FromSimpleStringJson_ReturnsString()
        {
            // Arrange
            string json = "\"hello world\"";

            // Act
            string result = JsonSerializer.ToObject<string>(json);

            // Assert
            Assert.Equal("hello world", result);
        }

        [Fact]
        public void JsonToStatic_FromBoolJson_ReturnsBool()
        {
            // Arrange
            string json = "true";

            // Act
            bool result = JsonSerializer.ToObject<bool>(json);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void JsonToStatic_ToObject_UpdatesExistingObject()
        {
            // Arrange
            TestPerson person = new TestPerson { Name = "Initial", Age = 0 };
            string json = "{\"Name\": \"Henry\", \"Age\": 50}";

            // Act
            Json.To(json, person);

            // Assert
            Assert.Equal("Henry", person.Name);
            Assert.Equal(50, person.Age);
        }

        #endregion

        #region Round-trip Tests

        [Fact]
        public void RoundTrip_Object_SurvivesRoundTrip()
        {
            // Arrange
            TestPerson original = new TestPerson { Name = "Ivan", Age = 60 };

            // Act
            Json json = Json.From(original);
            TestPerson result = json.To<TestPerson>();

            // Assert
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.Age, result.Age);
        }

        [Fact]
        public void RoundTrip_List_SurvivesRoundTrip()
        {
            // Arrange
            List<double> original = new List<double> { 1.1, 2.2, 3.3 };

            // Act
            Json json = Json.From(original);
            List<double> result = json.To<List<double>>();

            // Assert
            Assert.Equal(original.Count, result.Count);
            Assert.Equal(original[0], result[0]);
            Assert.Equal(original[1], result[1]);
            Assert.Equal(original[2], result[2]);
        }

        #endregion

        #region Test Helper Classes

        private enum TestStatus
        {
            Active,
            Pending,
            Inactive
        }

        private class TestPerson
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }
        }

        private class TestOrder
        {
            public int Id { get; set; }
            public TestPerson Customer { get; set; }
            public List<string> Items { get; set; }
        }

        #endregion
    }
}
