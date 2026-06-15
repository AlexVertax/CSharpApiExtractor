using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Общие тесты для Json
    /// </summary>
    public class JsonGeneralTests
    {
        [Fact]
        public void Json_GetWithMultipleKeys_ReturnsNestedValue()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["level1"] = new JsonObject();
            obj["level1"]["level2"] = "deep value";

            // Act
            Json result = obj["level1", "level2"];

            // Assert
            Assert.Equal("deep value", result.ToString());
        }

        [Fact]
        public void Json_GetWithMultipleIndices_ReturnsNestedValue()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add(new JsonArray());
            (array[0] as JsonArray).Add("nested value");

            // Act
            Json result = array[0, 0];

            // Assert
            Assert.Equal("nested value", result.ToString());
        }

        [Fact]
        public void Json_FindAll_ReturnsMatchingValues()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["name"] = "John";
            obj["nested", "name"] = "Jane";
            obj["another"] = new JsonObject();
            obj["another"]["name"] = "Bob";

            // Act
            JsonEnumerable results = obj.FindDeep("name");

            // Assert
            int count = results.Count();
            Assert.Equal(3, count);
        }

        [Fact]
        public void Json_RemoveAll_RemovesMatchingValues()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["keep"] = "1";
            obj["remove"] = "2";
            obj["also_remove"] = "3";

            // Act
            int removed = obj.RemoveAll((key, v) =>
            {
                if (key is string s) return s.Contains("remove");
                return false;
            });

            // Assert
            Assert.Equal(2, removed);
            Assert.False(obj["keep"].IsMissed);
            Assert.True(obj["remove"].IsMissed);
            Assert.True(obj["also_remove"].IsMissed);
        }

        [Fact]
        public void Json_Children_ReturnsDirectChildren()
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
        public void Json_ToString_ProducesValidJson()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["name"] = "John";
            obj["age"] = 30;
            obj["active"] = true;
            obj["nested"] = new JsonObject();
            obj["nested"]["value"] = "test";

            // Act
            string json = obj.ToString();

            // Assert
            Assert.NotNull(json);
            Assert.Contains("name", json);
            Assert.Contains("John", json);
            Assert.Contains("age", json);
            Assert.Contains("30", json);
        }

        [Fact]
        public void Json_Update_ModifiesValue()
        {
            // Arrange
            JsonObject obj = new JsonObject();
            obj["name"] = "john";

            // Act
            obj["name"].Update(value => ((string)value).ToUpper());

            // Assert
            Assert.Equal("JOHN", obj["name"].ToString());
        }

        [Fact]
        public void JsonComplex_NestedArraysAndObjects_ParsesCorrectly()
        {
            // Arrange
            string json = "{\"data\":[{\"id\":1,\"values\":[1,2,3]},{\"id\":2,\"values\":[4,5,6]}],\"metadata\":{\"total\":2}}";

            // Act
            JsonObject result = JsonObject.Parse(json);

            // Assert
            Assert.Equal("1", result["data"][0]["id"].ToString());
            Assert.Equal("2", result["data"][1]["id"].ToString());
            Assert.Equal("1", result["data"][0]["values"][0].ToString());
            Assert.Equal("2", result["metadata"]["total"].ToString());
        }

        [Fact]
        public void JsonComplex_RoundTrip_SerializesAndParses()
        {
            // Arrange
            JsonObject original = new JsonObject();
            original["name"] = "Test";
            original["age"] = 30;
            original["active"] = true;
            original["items"] = new JsonArray();
            ((JsonArray)original["items"]).Add("a");
            ((JsonArray)original["items"]).Add("b");

            // Act
            string serialized = original.ToString();
            JsonObject parsed = JsonObject.Parse(serialized);

            // Assert
            Assert.Equal("Test", parsed["name"].ToString());
            Assert.Equal("30", parsed["age"].ToString());
            Assert.Equal("true", parsed["active"].ToString());
            Assert.Equal(2, ((JsonArray)parsed["items"]).Count);
        }

        [Fact]
        public void JsonComplex_MixedTypesArray_ParsesCorrectly()
        {
            // Arrange
            string json = "[\"string\",123,45.67,true,false,null,{\"obj\":1},[1,2,3]]";

            // Act
            JsonArray result = JsonArray.Parse(json);

            // Assert
            Assert.Equal("string", result[0].ToString());
            Assert.Equal("123", result[1].ToString());
            Assert.Equal("45.67", result[2].ToString());
            Assert.Equal("true", result[3].ToString());
            Assert.Equal("false", result[4].ToString());
            Assert.True(result[5].IsMissed);
            Assert.Equal("1", result[6]["obj"].ToString());
            Assert.Equal(3, ((JsonArray)result[7]).Count);
        }

        [Fact]
        public void JsonComplex_FindInNestedStructure_FindsAllMatches()
        {
            // Arrange
            JsonObject root = new JsonObject();
            root["users"] = new JsonArray();
            JsonArray users = (JsonArray)root["users"];
            
            JsonObject user1 = new JsonObject();
            user1["name"] = "John";
            user1["email"] = "john@test.com";
            users.Add(user1);
            
            JsonObject user2 = new JsonObject();
            user2["name"] = "Jane";
            user2["email"] = "jane@test.com";
            users.Add(user2);

            // Act
            JsonEnumerable emails = root.FindDeep("email");

            // Assert
            Assert.Equal(2, emails.Count());
        }

        [Fact]
        public void JsonComplex_SerializeDeserialize_ListOfObjects()
        {
            // Arrange
            var list = new List<TestPerson>
            {
                new TestPerson { Name = "John", Age = 30 },
                new TestPerson { Name = "Jane", Age = 25 }
            };

            // Act
            Json json = Json.From(list);
            string str = json.ToString();
            JsonArray parsed = JsonArray.Parse(str);

            // Assert
            Assert.Equal(2, parsed.Count);
            Assert.Equal("John", parsed[0]["Name"].ToString());
            Assert.Equal("30", parsed[0]["Age"].ToString());
        }

        [Fact]
        public void JsonComplex_SerializeDeserialize_Dictionary()
        {
            // Arrange
            var dict = new Dictionary<string, TestPerson>
            {
                ["john"] = new TestPerson { Name = "John", Age = 30 },
                ["jane"] = new TestPerson { Name = "Jane", Age = 25 }
            };

            // Act
            Json json = Json.From(dict);
            string str = json.ToString();
            JsonObject parsed = JsonObject.Parse(str);

            // Assert
            Assert.Equal("John", parsed["john"]["Name"].ToString());
            Assert.Equal("30", parsed["john"]["Age"].ToString());
        }

        [Fact]
        public void Json_PrettyString_FormatsWithoutBuildingJsonTree()
        {
            // Arrange
            string json = "{\"user\":{\"name\":\"John\",\"tags\":[\"admin\",true,null]}}";

            // Act
            string pretty = Json.Prettify(json);

            // Assert
            Assert.Equal("{\n  \"user\": {\n    \"name\": \"John\",\n    \"tags\": [\n      \"admin\",\n      true,\n      null\n    ]\n  }\n}", pretty);
            Assert.Equal("John", Json.Parse(pretty)["user"]["name"].ToString());
        }

        [Fact]
        public void Json_PrettyString_PreservesEscapedContentInsideStrings()
        {
            // Arrange
            string json = "{\"text\":\"line 1\\n\\\"quoted\\\" // not a comment\",\"path\":\"C:\\\\temp\\\\file.txt\"}";

            // Act
            string pretty = Json.Prettify(json);
            string compact = Json.Compact(pretty);

            // Assert
            Assert.Contains("\\\"quoted\\\" // not a comment", pretty);
            Assert.Contains("C:\\\\temp\\\\file.txt", pretty);
            Assert.Equal(json, compact);
        }

        [Fact]
        public void Json_CompactString_RemovesWhitespaceAndLineComments()
        {
            // Arrange
            string json = "{\n  // user payload\n  \"user\": {\n    \"name\": \"John\", // display name\n    \"roles\": [ \"admin\", \"editor\" ]\n  }\n}";

            // Act
            string compact = Json.Compact(json);

            // Assert
            Assert.Equal("{\"user\":{\"name\":\"John\",\"roles\":[\"admin\",\"editor\"]}}", compact);
            Assert.Equal("editor", Json.Parse(compact)["user"]["roles"][1].ToString());
        }

        private class TestPerson
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
