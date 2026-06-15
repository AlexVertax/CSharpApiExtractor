using System;
using System.Collections.Generic;
using System.Linq;

namespace SilkJson.Demo
{
    // Models for deserialization
    class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }

    class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public List<string> Categories { get; set; }
    }

    class ApiResponse
    {
        [Alias("user_id")]
        public int UserId { get; set; }

        [Alias("user_name")]
        public string UserName { get; set; }

        [Alias("is_verified")]
        public bool IsVerified { get; set; }
    }

    class Department
    {
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }

    class Employee
    {
        public string Name { get; set; }
        public int Salary { get; set; }
    }

    class Examples
    {
        public static void Run()
        {
            Console.WriteLine("=== SilkJson Demo ===\n");

            // 1. Quick Start
            QuickStart();

            // 2. Creating JSON Objects
            JsonObjectCreation();

            // 3. Creating JSON Arrays
            JsonArrayCreation();

            // 4. Dynamic Usage
            DynamicUsage();
            
            // 5. Serialization
            Serialization();

            // 6. Deserialization (Simple Types, Objects, Aliases, Nested Objects)
            Deserialization();

            // 7. Type Casting & Checking
            TypeHandling();

            // 8. Json Queries (Key, Chain, FindAll, Indexer)
            JsonQueries();

            // 9. Linq Queries
            LinqQueries();

            Console.WriteLine("\n=== All Tests Passed Successfully! ===");
        }

        static void QuickStart()
        {
            Console.WriteLine("1. Quick Start");

            // === Parsing JSON ===
            Json user = Json.Parse(@"{
                ""name"": ""John"",
                ""age"": 30,
                ""settings"": {
                    ""theme"": ""dark"",
                    ""notifications"": true
                }
            }");

            // === Accessing Values ===
            string name = user["name"].String(); // Implicit cast
            int age = user["age"]; // Explicit cast
            Console.WriteLine($"   Name: {name}, Age: {age}");
            // Output: Name: John, Age: 30
            
            // === Missing Keys ===
            Json missedNode = user["missed/email"]; // Returns an empty object
            string missedEmail = missedNode; // Implicit cast returns null string
            Json subNode = missedNode["notifications"]; // Still empty object, no exception
            bool isMissed = subNode.IsMissed; // Check if a node is missed
            Console.WriteLine($"   Missed Email: {missedEmail ?? "null"}, SubNode is missed: {isMissed}");
            // Output: Missed Email: null, SubNode is missed: True

            // === Nested Objects ===
            string theme = user["settings/theme"];
            string theme2 = user["//theme"]; // Deep search for all "theme" keys
            Console.WriteLine($"   Theme: {theme}, Theme2: {theme2}");
            // Output: Theme: dark, Theme2: dark

            // === Creating JSON ===
            JsonObject newUser = new JsonObject(new
            {
                name = "Alice",
                age = 25
            });
            newUser["email"] = "alice@example.com"; // Add new key
            Console.WriteLine($"   Created: {newUser}");
            // Output: Created: {"name":"Alice","age":25,"email":"alice@example.com"}

            // === Serialization (Object → JSON) ===
            Product product = new Product { Name = "Laptop", Price = 999.99m, InStock = true };
            string json = Json.From(product); // same: Json.Serialize(product);
            Console.WriteLine($"   Serialized: {json}");
            // Output: Serialized: {"Name":"Laptop","Price":999.99,"InStock":true}

            // === Deserialization (JSON → Object) ===
            string jsonStr = @"{""Name"":""Bob"",""Age"":28,""Email"":""bob@example.com""}";
            User parsedUser = Json.To<User>(jsonStr); // same: Json.Deserialize<User>(jsonStr);
            Console.WriteLine($"   Deserialized: {parsedUser.Name}, {parsedUser.Email}");
            // Output: Deserialized: Bob, bob@example.com

            // === Modifying Values ===
            user["age"] = 31; // Update value
            user["settings/theme"] = "light"; // Update nested
            Console.WriteLine($"   Updated age: {user["age"]}");
            // Output: Updated age: 31

            // === Check if Key Exists ===
            Console.WriteLine($"   Has 'email': {user.Contains("email")}, Has 'phone': {user.Contains("phone")}");
            // Output: Has 'email': False, Has 'phone': False
            
            // === Pretty Print ===
            string pretty = user.Pretty();
            Console.WriteLine($"   Pretty print:\n{pretty}");
            // Output:   Pretty print:
            // {
            //     "name": "John",
            //     "age": 31,
            //     "settings": {
            //         "theme": "light",
            //         "notifications": true
            //     }
            // }
            
            Console.WriteLine();
        }

        static void JsonObjectCreation()
        {
            Console.WriteLine("2. Creating JsonObject:");

            // Assignment by key
            JsonObject userByKey = new JsonObject();
            userByKey["name"] = "Mary";
            userByKey["age"] = 25;

            // Object initializer
            JsonObject userInitializer = new JsonObject
            {
                ["name"] = "Alex",
                ["age"] = 28
            };

            // Constructor from object (can use anonymous type)
            JsonObject userFromObject = new JsonObject(new
            {
                name = "Mark",
                age = 35
            });

            // Fluent API for JsonObject (returns the object itself for chaining)
            JsonObject fluentUser = new JsonObject()
                .Set("name", "Jenny")
                .Set("age", 22);

            // Dynamic object
            dynamic dynamicUser = new JsonObject();
            dynamicUser.name = "David";
            dynamicUser.age = 40;

            Console.WriteLine($"   {userByKey}");
            Console.WriteLine($"   {userInitializer}");
            Console.WriteLine($"   {userFromObject}");
            Console.WriteLine($"   {fluentUser}");
            Console.WriteLine($"   {dynamicUser}");
            Console.WriteLine();
        }

        static void JsonArrayCreation()
        {
            Console.WriteLine("3. Creating JsonArray:");

            // Using Add method
            JsonArray addNumbers = new JsonArray();
            addNumbers.Add(1);
            addNumbers.Add(2);

            // Using AddRange method
            JsonArray rangeNumbers = new JsonArray();
            rangeNumbers.AddRange(3, 4);

            // Object initializer
            JsonArray initializerNumbers = new JsonArray { 5, 6 };

            // Fluent API for JsonArray (returns the array itself for chaining)
            JsonArray fluentNumbers = new JsonArray()
                .Add(7)
                .Add(8);

            Console.WriteLine($"   {addNumbers}");
            Console.WriteLine($"   {rangeNumbers}");
            Console.WriteLine($"   {initializerNumbers}");
            Console.WriteLine($"   {fluentNumbers}");
            Console.WriteLine();
        }
        
        static void DynamicUsage()
        {
            Console.WriteLine("4. Dynamic Usage");

            // Parsing JSON into dynamic object
            dynamic user = Json.Parse(@"{
                ""name"": ""John"",
                ""settings"": {
                    ""theme"": ""dark"",
                    ""notifications"": true
                }
            }");

            Console.WriteLine($"   Name: {user.name}");
            Console.WriteLine($"   Theme: {user.settings.theme}");
    
            // Updating values dynamically
            user.name = "Jane";
            Console.WriteLine($"   Updated name: {user.name}");
            Console.WriteLine("\n");
        }

        static void Serialization()
        {
            Console.WriteLine("5. Serialization:");

            // Simple types
            Console.WriteLine($"   String: {Json.From("Hello")}");
            Console.WriteLine($"   Number: {Json.From(42)}");
            Console.WriteLine($"   Double: {Json.From(3.14)}");
            Console.WriteLine($"   Boolean: {Json.From(true)}");
            Console.WriteLine($"   Null: {Json.From(null)}");

            // Collections
            Console.WriteLine($"   List: {Json.From(new List<string> { "apple", "banana", "cherry" })}");
            Console.WriteLine($"   Dictionary: {Json.From(new Dictionary<string, int> { { "Alice", 95 }, { "Bob", 87 } })}");
            Console.WriteLine($"   Array: {Json.From(new[] { 1, 2, 3, 4, 5 })}");

            // Complex object
            Product laptop = new Product { Name = "Laptop", Price = 999.99m, InStock = true, Categories = new List<string> { "Electronics", "Computers" } };
            Console.WriteLine($"   Object: {Json.From(laptop)}");
            Console.WriteLine();
        }

        static void Deserialization()
        {
            Console.WriteLine("6. Deserialization:");

            // Simple types
            Console.WriteLine($"   String: {Json.To<string>("\"Hello\"")}");
            Console.WriteLine($"   Int: {Json.To<int>("42")}");
            Console.WriteLine($"   Double: {Json.To<double>("3.14")}");
            Console.WriteLine($"   Bool: {Json.To<bool>("true")}");

            // Simple object
            User user = Json.To<User>(@"{""Name"":""John"",""Age"":30,""Email"":""john@example.com""}");
            Console.WriteLine($"   User: {user.Name}, {user.Age}, {user.Email}");

            // With AliasAttribute
            ApiResponse response = Json.To<ApiResponse>(@"{""user_id"":12345,""user_name"":""Mary"",""is_verified"":true}");
            Console.WriteLine($"   Aliased: {response.UserId}, {response.UserName}, {response.IsVerified}");

            // Nested objects
            Department dept = Json.To<Department>(@"{""Name"":""IT"",""Employees"":[{""Name"":""Anna"",""Salary"":100000},{""Name"":""Bob"",""Salary"":120000}]}");
            Console.WriteLine($"   Department: {dept.Name}");
            dept.Employees.ForEach(emp => Console.WriteLine($"     {emp.Name}: {emp.Salary}"));

            Console.WriteLine();
        }

        static void TypeHandling()
        {
            Console.WriteLine("7. Type Casting & Checking:");

            string json = @"{
                ""integer"": 42,
                ""floating"": 3.14,
                ""text"": ""hello"",
                ""boolean"": true
            }";

            Json obj = Json.Parse(json);

            // Type checking
            Console.WriteLine($"   integer IsLong: {obj["integer"].IsLong()}, IsDouble: {obj["integer"].IsDouble()}");
            Console.WriteLine($"   floating IsDouble: {obj["floating"].IsDouble()}");
            Console.WriteLine($"   text IsString: {obj["text"].IsString()}");
            Console.WriteLine($"   boolean IsBool: {obj["boolean"].IsBool()}");

            // Type casting
            string strVal = obj["text"]; // Implicit cast to string
            int intVal = obj["integer"].Int(); // Explicit cast to int
            long longVal = obj["integer"].Long(100); // Explicit cast to long with default
            double doubleVal = (double)obj["floating"]; // Explicit cast to double
            decimal decimalVal = obj["floating"].Cast<decimal>(); // Explicit cast to decimal

            Console.WriteLine($"   String: {strVal}, Int: {intVal}");
            Console.WriteLine($"   Double: {doubleVal}, Decimal: {decimalVal}");
            
            // Wrong type casting
            int wrongInt = obj["floating"].Int(); // Not an integer, but returns 3 (truncated) instead of throwing
            bool wrongBool = obj["integer"].Bool(); // Not a boolean, returns false
            bool wrongBool2 = obj["integer"].Bool(true); // Not a boolean, returns default true instead of false
            Console.WriteLine($"   Wrong Int: {wrongInt}, Wrong Bool: {wrongBool}, Wrong Bool with default: {wrongBool2}");
            Console.WriteLine();
        }

        static void JsonQueries()
        {
            Console.WriteLine("8. Json Queries (Key, Chain, FindAll, Indexer):");

            // Unified JSON demonstrating all query patterns
            string json = @"{
                ""company"": ""TechCorp"",
                ""id"": 999,
                ""departments"": [
                    {
                        ""name"": ""IT"",
                        ""head"": {
                            ""name"": ""Anna"",
                            ""email"": ""anna@company.com"",
                            ""id"": 1,
                            ""active"": true
                        },
                        ""teams"": [
                            {
                                ""name"": ""Backend"",
                                ""lead"": {
                                    ""name"": ""Bob"",
                                    ""phone"": ""+1-555-0101"",
                                    ""id"": 2,
                                    ""active"": true
                                }
                            },
                            {
                                ""name"": ""Frontend"",
                                ""lead"": {
                                    ""name"": ""Eve"",
                                    ""phone"": ""+1-555-0102"",
                                    ""id"": 3,
                                    ""active"": false
                                }
                            }
                        ]
                    },
                    {
                        ""name"": ""HR"",
                        ""head"": {
                            ""name"": ""Grace"",
                            ""phone"": ""+1-555-0200"",
                            ""id"": 4,
                            ""active"": true
                        },
                        ""teams"": []
                    }
                ]
            }";

            Json obj = Json.Parse(json);

            // === By Key ===
            Console.WriteLine("   By Key:");
            Console.WriteLine($"      Company: {obj["company"]}");
            Console.WriteLine($"      IT Dept: {obj["departments", 0, "name"]}");

            // === By Chain (Get method) ===
            Console.WriteLine("   By Chain:");
            string itHead = obj.Get("departments", 0, "head", "name");
            Console.WriteLine($"      IT Head: {itHead}");

            // === FindDeep ===
            Console.WriteLine("   FindDeep:");
            obj.FindDeep("id").ForEach(id => Console.WriteLine($"      id: {id}"));

            Console.WriteLine("      Active users:");
            obj.FindDeep((node, _) => node["active"] == true)
                .ForEach(u => Console.WriteLine($"         {u["name"]} ({u["email"] ?? u["phone"]})"));

            // === Indexer Query ===
            Console.WriteLine("   Indexer Query (// = deep search, / = direct children, * = wildcard):");
            Console.WriteLine("      All names:");
            obj["//name"].ForEach(n => Console.WriteLine($"         {n}"));

            Console.WriteLine("      Team names:");
            obj["//teams/*/name"].ForEach(n => Console.WriteLine($"         {n}"));

            Console.WriteLine();
        }

        static void LinqQueries()
        {
            Console.WriteLine("9. Linq Queries:");

            string json = @"[
                {""name"": ""Alice"", ""age"": 25, ""department"": ""IT""},
                {""name"": ""Bob"", ""age"": 30, ""department"": ""HR""},
                {""name"": ""Charlie"", ""age"": 35, ""department"": ""IT""},
                {""name"": ""Diana"", ""age"": 28, ""department"": ""Finance""}
            ]";

            Json users = Json.Parse(json);

            // Filter users by department
            Console.WriteLine("   IT Department users:");
            users.Children()
                .Where(u => u["department"] == "IT")
                .ForEach(u => Console.WriteLine($"      {u["name"]}, Age: {u["age"]}"));

            // Find users older than 27
            Console.WriteLine("   Users older than 27:");
            users.Children()
                .Where(u => u["age"] > 27)
                .ForEach(u => Console.WriteLine($"      {u["name"]}, Age: {u["age"]}"));

            // Count users per department
            Console.WriteLine("   Users per department:");
            users.Children()
                .GroupBy(u => u["department"].String())
                .ForEach((key, items) => Console.WriteLine($"      {key}: {items.Count()}"));

            // Order by age
            IEnumerable<string> orderedUsers = users.Children().OrderBy(u => (int)u["age"]).Select(u => (string)u["name"]);
            Console.WriteLine($"   Ordered by age: {string.Join(", ", orderedUsers)}");
            Console.WriteLine();
        }
    }
}