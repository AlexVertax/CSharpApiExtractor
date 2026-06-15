using System.Linq;
using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для комплексных запросов через индексатор (секция 17 Examples.cs)
    /// </summary>
    public class JsonIndexerQueryTests
    {
        #region Deep Search Tests (// prefix)

        [Fact]
        public void IndexerQuery_DeepSearchSingleKey_ReturnsAllMatches()
        {
            // Arrange
            string json = @"{
                ""name"": ""TechCorp"",
                ""departments"": [
                    {
                        ""name"": ""Engineering"",
                        ""head"": { ""name"": ""Anna"" }
                    },
                    {
                        ""name"": ""HR"",
                        ""head"": { ""name"": ""Grace"" }
                    }
                ]
            }";

            Json obj = Json.Parse(json);

            // Act - deep search for all 'name' keys
            var names = obj["//name"];

            // Assert - FindAll recursively searches all children, including root
            // Found: company.name, departments[0].name, departments[0].head.name, departments[1].name, departments[1].head.name
            Assert.Equal(5, names.Count());
        }

        [Fact]
        public void IndexerQuery_DeepSearchReturnsValues_CorrectValues()
        {
            // Arrange
            string json = @"{
                ""company"": ""TechCorp"",
                ""departments"": [
                    {
                        ""name"": ""Engineering"",
                        ""head"": { ""name"": ""Anna"" }
                    }
                ]
            }";

            Json obj = Json.Parse(json);

            // Act - deep search for all 'name' keys
            var names = obj["//name"].ToList();

            // Assert - FindAll recursively searches all children
            // Found: departments[0].name, departments[0].head.name
            Assert.Equal(2, names.Count);
        }

        [Fact]
        public void IndexerQuery_DeepSearchNoMatches_ReturnsEmpty()
        {
            // Arrange
            string json = @"{
                ""company"": ""TechCorp"",
                ""departments"": []
            }";

            Json obj = Json.Parse(json);

            // Act - deep search for non-existing key
            var results = obj["//nonexistent"];

            // Assert
            Assert.Equal(0, results.Count());
        }

        [Fact]
        public void IndexerQuery_DeepSearchWithMultipleNestedLevels_FindsAll()
        {
            // Arrange
            string json = @"{
                ""level1"": {
                    ""level2"": {
                        ""level3"": {
                            ""level4"": {
                                ""value"": ""deep""
                            }
                        }
                    }
                }
            }";

            Json obj = Json.Parse(json);

            // Act
            var values = obj["//value"];

            // Assert
            Assert.Equal(1, values.Count());
            Assert.Equal("deep", values.First().ToString());
        }

        #endregion

        #region Wildcard Tests (*)

        [Fact]
        public void IndexerQuery_Wildcard_ReturnsAllChildren()
        {
            // Arrange
            string json = @"{
                ""departments"": [
                    { ""name"": ""Engineering"" },
                    { ""name"": ""HR"" },
                    { ""name"": ""Finance"" }
                ]
            }";

            Json obj = Json.Parse(json);

            // Act
            var departments = obj["departments", "*"];

            // Assert
            Assert.Equal(3, departments.Count());
        }

        [Fact]
        public void IndexerQuery_DeepSearchWithWildcard_ReturnsCorrectResults()
        {
            // Arrange
            string json = @"{
                ""company"": {
                    ""departments"": [
                        {
                            ""name"": ""Engineering"",
                            ""teams"": [
                                { ""name"": ""Backend"" },
                                { ""name"": ""Frontend"" }
                            ]
                        },
                        {
                            ""name"": ""HR"",
                            ""teams"": [
                                { ""name"": ""Recruiting"" }
                            ]
                        }
                    ]
                }
            }";

            Json obj = Json.Parse(json);

            // Act - get all team names
            var teamNames = obj["//teams", "*", "name"];

            // Assert
            Assert.Equal(3, teamNames.Count());
        }

        [Fact]
        public void IndexerQuery_WildcardWithDeepSearch_FindsAllAtPath()
        {
            // Arrange
            string json = @"{
                ""items"": [
                    { ""details"": { ""id"": 1 } },
                    { ""details"": { ""id"": 2 } },
                    { ""details"": { ""id"": 3 } }
                ]
            }";

            Json obj = Json.Parse(json);

            // Act
            var ids = obj["//items", "*", "details", "id"];

            // Assert
            Assert.Equal(3, ids.Count());
        }

        #endregion

        #region Complex Query Tests (Examples from Section 17)

        [Fact]
        public void IndexerQuery_CompanyStructure_ReturnsAllNames()
        {
            // Arrange
            string json = @"{
                ""company"": {
                    ""name"": ""TechCorp"",
                    ""departments"": [
                        {
                            ""id"": 1,
                            ""name"": ""Engineering"",
                            ""head"": {
                                ""name"": ""Anna"",
                                ""email"": ""anna@techcorp.com""
                            },
                            ""teams"": [
                                {
                                    ""name"": ""Backend"",
                                    ""lead"": {
                                        ""name"": ""Bob"",
                                        ""contacts"": {
                                            ""phone"": ""+1-555-0101""
                                        }
                                    }
                                },
                                {
                                    ""name"": ""Frontend"",
                                    ""lead"": {
                                        ""name"": ""Eve"",
                                        ""contacts"": {
                                            ""phone"": ""+1-555-0102""
                                        }
                                    }
                                }
                            ]
                        },
                        {
                            ""id"": 2,
                            ""name"": ""HR"",
                            ""head"": {
                                ""name"": ""Grace""
                            },
                            ""teams"": []
                        }
                    ]
                }
            }";

            Json obj = Json.Parse(json);

            // Act - find all 'name' keys
            var allNames = obj["//name"];

            // Assert
            // FindAll recursively searches all children at all levels
            // company.name, Engineering, Anna, Backend, Eve, HR, Grace
            Assert.Equal(9, allNames.Count());
        }

        [Fact]
        public void IndexerQuery_TeamNames_ReturnsCorrectValues()
        {
            // Arrange
            string json = @"{
                ""company"": {
                    ""departments"": [
                        {
                            ""teams"": [
                                { ""name"": ""Backend"" },
                                { ""name"": ""Frontend"" }
                            ]
                        },
                        {
                            ""teams"": [
                                { ""name"": ""Recruiting"" }
                            ]
                        }
                    ]
                }
            }";

            Json obj = Json.Parse(json);

            // Act - find all team names
            var teamNames = obj["//teams", "*", "name"];

            // Assert
            Assert.Equal(3, teamNames.Count());
        }

        [Fact]
        public void IndexerQuery_TeamLeadPhones_ReturnsPhoneNumbers()
        {
            // Arrange
            string json = @"{
                ""company"": {
                    ""departments"": [
                        {
                            ""teams"": [
                                {
                                    ""lead"": {
                                        ""contacts"": {
                                            ""phone"": ""+1-555-0101""
                                        }
                                    }
                                },
                                {
                                    ""lead"": {
                                        ""contacts"": {
                                            ""phone"": ""+1-555-0102""
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            }";

            Json obj = Json.Parse(json);

            // Act - find all phone numbers of team leads
            var phones = obj["//teams", "*", "lead", "contacts", "phone"];

            // Assert
            Assert.Equal(2, phones.Count());
            Assert.Contains("+1-555-0101", phones.Select(p => p.ToString()));
            Assert.Contains("+1-555-0102", phones.Select(p => p.ToString()));
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void IndexerQuery_EmptyArray_ReturnsEmpty()
        {
            // Arrange
            string json = @"{
                ""departments"": []
            }";

            Json obj = Json.Parse(json);

            // Act
            var results = obj["//departments", "*", "name"];

            // Assert
            Assert.Equal(0, results.Count());
        }

        [Fact]
        public void IndexerQuery_NonExistentPath_ReturnsEmpty()
        {
            // Arrange
            string json = @"{
                ""company"": ""TechCorp""
            }";

            Json obj = Json.Parse(json);

            // Act
            var results = obj["//nonexistent", "*", "name"];

            // Assert
            Assert.Equal(0, results.Count());
        }

        [Fact]
        public void IndexerQuery_MixedTypesInArray_HandlesCorrectly()
        {
            // Arrange
            string json = @"{
                ""items"": [
                    { ""name"": ""first"" },
                    ""string"",
                    42,
                    { ""name"": ""second"" }
                ]
            }";

            Json obj = Json.Parse(json);

            // Act - find all 'name' keys recursively
            var names = obj["//name"];

            // Assert - FindAll searches all children recursively including nested
            Assert.Equal(2, names.Count());
        }

        [Fact]
        public void IndexerQuery_ForEachIteration_WorksCorrectly()
        {
            // Arrange
            string json = @"{
                ""users"": [
                    { ""name"": ""Alice"" },
                    { ""name"": ""Bob"" }
                ]
            }";

            Json obj = Json.Parse(json);
            var names = obj["//users", "*", "name"];

            // Act
            var results = new System.Text.StringBuilder();
            names.ForEach(name => results.Append(name.ToString()).Append(","));

            // Assert
            Assert.Equal("Alice,Bob,", results.ToString());
        }

        [Fact]
        public void IndexerQuery_ChainedWithGet_WorksCorrectly()
        {
            // Arrange
            string json = @"{
                ""company"": {
                    ""departments"": [
                        {
                            ""name"": ""Engineering"",
                            ""teams"": [
                                { ""name"": ""Backend"" }
                            ]
                        }
                    ]
                }
            }";

            Json obj = Json.Parse(json);

            // Act - combine Get with indexer query
            var teamNames = obj.Get("company", "departments", 0, "teams", 0, "name");

            // Assert
            Assert.Equal("Backend", teamNames.ToString());
        }

        #endregion

        #region Real-world Scenario Tests

        [Fact]
        public void IndexerQuery_ApiResponse_ReturnsAllIds()
        {
            // Arrange
            string json = @"{
                ""data"": {
                    ""users"": [
                        { ""id"": 1, ""name"": ""Alice"" },
                        { ""id"": 2, ""name"": ""Bob"" }
                    ],
                    ""posts"": [
                        { ""id"": 101, ""userId"": 1, ""title"": ""Post 1"" },
                        { ""id"": 102, ""userId"": 2, ""title"": ""Post 2"" }
                    ]
                },
                ""meta"": {
                    ""id"": ""root"",
                    ""requestId"": ""12345""
                }
            }";

            Json obj = Json.Parse(json);

            // Act - find all 'id' keys
            var ids = obj["//id"];

            // Assert
            Assert.Equal(5, ids.Count()); // data.users[0].id, data.users[1].id, data.posts[0].id, data.posts[1].id, meta.id
        }

        [Fact]
        public void IndexerQuery_NestedConfig_ReturnsAllSettings()
        {
            // Arrange
            string json = @"{
                ""config"": {
                    ""database"": {
                        ""host"": ""localhost"",
                        ""port"": 5432,
                        ""settings"": {
                            ""timeout"": 30,
                            ""pool"": 10
                        }
                    },
                    ""cache"": {
                        ""host"": ""redis"",
                        ""port"": 6379,
                        ""settings"": {
                            ""ttl"": 3600
                        }
                    }
                }
            }";

            Json obj = Json.Parse(json);

            // Act
            var hosts = obj["//host"];
            var ports = obj["//port"];

            // Assert
            Assert.Equal(2, hosts.Count());
            Assert.Equal(2, ports.Count());
            Assert.Contains("localhost", hosts.Select(h => h.ToString()));
            Assert.Contains("redis", hosts.Select(h => h.ToString()));
        }

        #endregion
    }
}
