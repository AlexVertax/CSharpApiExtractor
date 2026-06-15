using System;
using System.Collections.Generic;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для JsonObject
    /// </summary>
    public class JsonObjectTests
    {
        [Fact]
        public void JsonObject_CreateEmpty_ReturnsValidObject()
        {
            // Arrange & Act
            JsonObject obj = new JsonObject();

            // Assert
            Assert.NotNull(obj);
            Assert.Equal("{}", obj.ToString());
        }

        [Fact]
        public void JsonObject_AddStringProperty_ReturnsValidJson()
        {
            // Arrange
            JsonObject obj = new JsonObject();

            // Act
            obj["name"] = "John";
            obj["city"] = "Moscow";

            // Assert
            Assert.Equal("{\"name\":\"John\",\"city\":\"Moscow\"}", obj.ToString());
        }

        [Fact]
        public void JsonObject_GetExistingProperty_ReturnsCorrectValue()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["key"] = "value";

            // Act
            Json result = obj["key"];

            // Assert
            Assert.Equal("value", result.ToString());
        }

        [Fact]
        public void JsonObject_GetNonExistingProperty_ReturnsNull()
        {
            // Arrange
            JsonObject obj = new JsonObject();

            // Act
            Json result = obj["nonexistent"];

            // Assert
            Assert.True(result.IsMissed);
        }

        [Fact]
        public void JsonObject_GetByIndex_ReturnsCorrectValue()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["first"] = "1";
            obj["second"] = "2";
            obj["third"] = "3";

            // Act
            Json first = obj[0];
            Json second = obj[1];

            // Assert
            Assert.Equal("1", first.ToString());
            Assert.Equal("2", second.ToString());
        }

        [Fact]
        public void JsonObject_ContainsKey_ReturnsCorrectResult()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["exists"] = "value";

            // Act & Assert
            Assert.True(obj.Contains("exists"));
            Assert.False(obj.Contains("notexists"));
        }

        [Fact]
        public void JsonObject_Remove_RemovesKey()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["key1"] = "value1";
            obj["key2"] = "value2";

            // Act
            bool removed = obj.Remove("key1");

            // Assert
            Assert.True(removed);
            Assert.True(obj["key1"].IsMissed);
            Assert.Equal("{\"key2\":\"value2\"}", obj.ToString());
        }

        [Fact]
        public void JsonObject_Clear_RemovesAllKeys()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["key1"] = "value1";
            obj["key2"] = "value2";

            // Act
            obj.Clear();

            // Assert
            Assert.Equal("{}", obj.ToString());
        }

        [Fact]
        public void JsonObject_Merge_CombinesObjects()
        {
            // Arrange
            JsonObject obj1 = new JsonObject();
            obj1["name"] = "John";
            obj1["age"] = 30;

            JsonObject obj2 = new JsonObject();
            obj2["city"] = "Moscow";
            obj2["age"] = 31;

            // Act
            obj1.Merge(obj2);

            // Assert
            Assert.Equal("John", obj1["name"].ToString());
            Assert.Equal("Moscow", obj1["city"].ToString());
            Assert.Equal(31, int.Parse(obj1["age"].ToString()));
        }

        [Fact]
        public void JsonObject_MergeWithConflictResolver_UsesResolver()
        {
            // Arrange
            JsonObject obj1 = new JsonObject();
            obj1["value"] = "original";

            JsonObject obj2 = new JsonObject();
            obj2["value"] = "new";

            // Act
            obj1.Merge(obj2, (key, oldVal, newVal) => oldVal);

            // Assert
            Assert.Equal("original", obj1["value"].ToString());
        }

        [Fact]
        public void JsonObject_IndexOf_ReturnsCorrectIndex()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["first"] = "1";
            obj["second"] = "2";
            obj["third"] = "3";

            // Act
            int index = obj.IndexOf("second");

            // Assert
            Assert.Equal(1, index);
        }

        [Fact]
        public void JsonObject_Insert_InsertsAtPosition()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["first"] = "1";
            obj["third"] = "3";

            // Act
            obj.Insert(1, "second", "2");

            // Assert
            string second = obj[1].ToString();
            Assert.Equal("2", second);
        }

        [Fact]
        public void JsonObject_DynamicAccess_WorksCorrectly()
        {
            // Arrange
            dynamic obj = new JsonObject();
            
            // Act
            obj.Name = "Test";
            obj.Value = 123;

            // Assert
            Assert.Equal("Test", obj.Name.ToString());
            Assert.Equal("123", obj.Value.ToString());
        }

        [Fact]
        public void JsonObject_FromDictionary_CreatesValidObject()
        {
            // Arrange
            var dict = new Dictionary<string, object>
            {
                ["key1"] = "value1",
                ["key2"] = 42
            };

            // Act
            JsonObject obj = new JsonObject(dict);

            // Assert
            Assert.Equal("value1", obj["key1"].ToString());
            Assert.Equal("42", obj["key2"].ToString());
        }

        [Fact]
        public void JsonObject_DeepNesting_SerializesCorrectly()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            JsonObject current = obj;
            for (int i = 0; i < 10; i++)
            {
                current["level"] = new JsonObject();
                current = (JsonObject)current["level"];
            }
            current["value"] = "deep";

            // Act
            string result = obj.ToString();

            // Assert
            Assert.Contains("deep", result);
        }

        [Fact]
        public void JsonObject_LargeObject_SerializesCorrectly()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            for (int i = 0; i < 100; i++)
            {
                obj[$"key{i}"] = $"value{i}";
            }

            // Act
            string result = obj.ToString();

            // Assert
            Assert.NotEmpty(result);
            Assert.Contains("key0", result);
            Assert.Contains("key99", result);
        }

        [Fact]
        public void JsonObject_GetIndexOutOfRange_ReturnsNull()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["key"] = "value";

            // Act
            Json result = obj[100];

            // Assert
            Assert.True(result.IsMissed);
        }

        [Fact]
        public void JsonObject_RemoveNonExistingKey_ReturnsFalse()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["key"] = "value";

            // Act
            bool removed = obj.Remove("nonexistent");

            // Assert
            Assert.False(removed);
        }
    }
}
