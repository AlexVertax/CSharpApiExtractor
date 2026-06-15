# SilkJson

A lightweight JSON library for C# with a fluent API and intuitive syntax.

## Quick Start

### Parsing JSON

```csharp
Json user = Json.Parse(@"{
    ""name"": ""John"",
    ""age"": 30,
    ""settings"": {
        ""theme"": ""dark"",
        ""notifications"": true
    }
}");
```

### Accessing Values

```csharp
string name = user["name"].String();  // Implicit cast
int age = user["age"];                  // Explicit cast
Console.WriteLine($"Name: {name}, Age: {age}");
// Output: Name: John, Age: 30
```

### Missing Keys

SilkJson gracefully handles missing keys without throwing exceptions:

```csharp
Json missedNode = user["missed/email"];      // Returns an empty object
string missedEmail = missedNode;              // Implicit cast returns null string
Json subNode = missedNode["notifications"];  // Still empty object, no exception
bool isMissed = subNode.isMissed;            // Check if a node is missed
Console.WriteLine($"Missed Email: {missedEmail ?? "null"}, SubNode is missed: {isMissed}");
// Output: Missed Email: null, SubNode is missed: True
```

### Nested Objects

Access nested values using path notation:

```csharp
string theme = user["settings/theme"];       // Path traversal
string theme2 = user["//theme"];             // Deep search for all "theme" keys
Console.WriteLine($"Theme: {theme}, Theme2: {theme2}");
// Output: Theme: dark, Theme2: dark
```

### Creating JSON

```csharp
JsonObject newUser = new JsonObject(new
{
    name = "Alice",
    age = 25
});
newUser["email"] = "alice@example.com";      // Add new key
Console.WriteLine($"Created: {newUser}");
// Output: Created: {"name":"Alice","age":25,"email":"alice@example.com"}
```

### Serialization (Object → JSON)

```csharp
Product product = new Product { Name = "Laptop", Price = 999.99m, InStock = true };
string json = Json.From(product);            // Same: Json.Serialize(product);
Console.WriteLine($"Serialized: {json}");
```

### Deserialization (JSON → Object)

```csharp
string jsonStr = @"{""Name"":""Bob"",""Age"":28,""Email"":""bob@example.com""}";
User parsedUser = Json.To<User>(jsonStr);    // Same: Json.Deserialize<User>(jsonStr);
Console.WriteLine($"Deserialized: {parsedUser.Name}, {parsedUser.Email}");
// Output: Deserialized: Bob, bob@example.com
```

### Modifying Values

```csharp
user["age"] = 31;                    // Update value
user["settings/theme"] = "light";    // Update nested value
Console.WriteLine($"Updated age: {user["age"]}");
// Output: Updated age: 31
```

### Check if Key Exists

```csharp
Console.WriteLine($"Has 'email': {user.Contains("email")}, Has 'phone': {user.Contains("phone")}");
// Output: Has 'email': False, Has 'phone': False
```

### Pretty Print

```csharp
string pretty = user.Pretty();
Console.WriteLine($"Pretty print:\n{pretty}");
```

### Format Raw JSON String

```csharp
string rawJson = @"{ ""user"" : { ""name"" : ""John"", ""roles"" : [ ""admin"", ""editor"" ] } }";

string prettyJson = Json.Prettify(rawJson);
string compactJson = rawJson.CompactJson();
```

---

## Creating JsonObject

### Assignment by Key

```csharp
JsonObject userByKey = new JsonObject();
userByKey["name"] = "Mary";
userByKey["age"] = 25;
```

### Object Initializer

```csharp
JsonObject userInitializer = new JsonObject
{
    ["name"] = "Alex",
    ["age"] = 28
};
```

### Constructor from Object (Anonymous Type)

```csharp
JsonObject userFromObject = new JsonObject(new
{
    name = "Mark",
    age = 35
});
```

### Fluent API

Returns the object itself for chaining:

```csharp
JsonObject fluentUser = new JsonObject()
    .Set("name", "Jenny")
    .Set("age", 22);
```

### Dynamic Object

```csharp
dynamic dynamicUser = new JsonObject();
dynamicUser.name = "David";
dynamicUser.age = 40;
```

---

## Creating JsonArray

### Using Add Method

```csharp
JsonArray addNumbers = new JsonArray();
addNumbers.Add(1);
addNumbers.Add(2);
```

### Using AddRange Method

```csharp
JsonArray rangeNumbers = new JsonArray();
rangeNumbers.AddRange(3, 4);
```

### Object Initializer

```csharp
JsonArray initializerNumbers = new JsonArray { 5, 6 };
```

### Fluent API

```csharp
JsonArray fluentNumbers = new JsonArray()
    .Add(7)
    .Add(8);
```

---

## Dynamic Usage

Parse JSON into a dynamic object for intuitive property access:

```csharp
dynamic user = Json.Parse(@"{
    ""name"": ""John"",
    ""settings"": {
        ""theme"": ""dark"",
        ""notifications"": true
    }
}");

Console.WriteLine($"Name: {user.name}");
Console.WriteLine($"Theme: {user.settings.theme}");

// Updating values dynamically
user.name = "Jane";
Console.WriteLine($"Updated name: {user.name}");
```

---

## Serialization

### Simple Types

```csharp
Json.From("Hello");                  // "Hello"
Json.From(42);                       // 42
Json.From(3.14);                     // 3.14
Json.From(true);                     // true
Json.From(null);                     // null
```

### Collections

```csharp
Json.From(new List<string> { "apple", "banana", "cherry" });
// ["apple","banana","cherry"]

Json.From(new Dictionary<string, int> { { "Alice", 95 }, { "Bob", 87 } });
// {"Alice":95,"Bob":87}

Json.From(new[] { 1, 2, 3, 4, 5 });
// [1,2,3,4,5]
```

### Complex Object

```csharp
Product laptop = new Product { 
    Name = "Laptop", 
    Price = 999.99m, 
    InStock = true, 
    Categories = new List<string> { "Electronics", "Computers" } 
};
Json.From(laptop);
// {"Name":"Laptop","Price":999.99,"InStock":true,"Categories":["Electronics","Computers"]}
```

---

## Deserialization

### Simple Types

```csharp
Json.To<string>("\"Hello\"");    // "Hello"
Json.To<int>("42");              // 42
Json.To<double>("3.14");         // 3.14
Json.To<bool>("true");           // true
```

### Simple Object

```csharp
User user = Json.To<User>(@"{""Name"":""John"",""Age"":30,""Email"":""john@example.com""}");
// user.Name = "John", user.Age = 30, user.Email = "john@example.com"
```

### With AliasAttribute

Map JSON keys to different property names using aliases:

```csharp
public class ApiResponse
{
    [Alias("user_id")]
    public int UserId { get; set; }

    [Alias("user_name")]
    public string UserName { get; set; }

    [Alias("is_verified")]
    public bool IsVerified { get; set; }
}

ApiResponse response = Json.To<ApiResponse>(@"{""user_id"":12345,""user_name"":""Mary"",""is_verified"":true}");
// response.UserId = 12345, response.UserName = "Mary", response.IsVerified = true
```

### Nested Objects

```csharp
public class Department
{
    public string Name { get; set; }
    public List<Employee> Employees { get; set; }
}

public class Employee
{
    public string Name { get; set; }
    public int Salary { get; set; }
}

Department dept = Json.To<Department>(@"{""Name"":""IT"",""Employees"":[{""Name"":""Anna"",""Salary"":100000},{""Name"":""Bob"",""Salary"":120000}]}");
// dept.Name = "IT"
// dept.Employees[0].Name = "Anna", Salary = 100000
// dept.Employees[1].Name = "Bob", Salary = 120000
```

---

## Type Casting & Checking

### Type Checking

```csharp
Json obj = Json.Parse(@"{
    ""integer"": 42,
    ""floating"": 3.14,
    ""text"": ""hello"",
    ""boolean"": true
}");

obj["integer"].IsLong();      // true
obj["integer"].IsDouble();    // false
obj["floating"].IsDouble();   // true
obj["text"].IsString();      // true
obj["boolean"].IsBool();      // true
```

### Type Casting

```csharp
string strVal = obj["text"];                          // Implicit cast to string
int intVal = obj["integer"].Int();                     // Explicit cast to int
long longVal = obj["integer"].Long(100);              // Explicit cast to long with default
double doubleVal = (double)obj["floating"];           // Explicit cast to double
decimal decimalVal = obj["floating"].Cast<decimal>(); // Explicit cast to decimal
```

### Wrong Type Casting

SilkJson handles type mismatches gracefully:

```csharp
int wrongInt = obj["floating"].Int();          // Not an integer, returns 3 (truncated)
bool wrongBool = obj["integer"].Bool();        // Not a boolean, returns false
bool wrongBool2 = obj["integer"].Bool(true);   // Returns default true instead of false
```

---

## JSON Queries

### By Key

```csharp
Json obj = Json.Parse(json);

obj["company"];                              // Direct key access
obj["departments", 0, "name"];              // Array index + key
```

### By Chain (Get Method)

```csharp
string itHead = obj.Get("departments", 0, "head", "name");
```

### FindDeep

Find all nodes matching a key or predicate:

```csharp
// Find all nodes with key "id"
obj.FindDeep("id").ForEach(id => Console.WriteLine($"id: {id}"));

// Find all active users
obj.FindDeep((node, _) => node["active"] == true)
    .ForEach(u => Console.WriteLine($"{u["name"]} ({u["email"] ?? u["phone"]})"));
```

### Indexer Query Syntax

| Syntax | Description |
|--------|-------------|
| `//key` | Deep search (all levels) |
| `/key` | Direct children only |
| `*` | Wildcard |

```csharp
// All names at any depth
obj["//name"].ForEach(n => Console.WriteLine(n));

// Team names at any depth
obj["//teams/*/name"].ForEach(n => Console.WriteLine(n));
```

---

## LINQ Queries

### Filter by Property

```csharp
Json users = Json.Parse(@"[
    {""name"": ""Alice"", ""age"": 25, ""department"": ""IT""},
    {""name"": ""Bob"", ""age"": 30, ""department"": ""HR""},
    {""name"": ""Charlie"", ""age"": 35, ""department"": ""IT""},
    {""name"": ""Diana"", ""age"": 28, ""department"": ""Finance""}
]");

// Filter users by department
users.Children()
    .Where(u => u["department"] == "IT")
    .ForEach(u => Console.WriteLine($"{u["name"]}, Age: {u["age"]}"));
```

### Filter by Value

```csharp
// Find users older than 27
users.Children()
    .Where(u => u["age"] > 27)
    .ForEach(u => Console.WriteLine($"{u["name"]}, Age: {u["age"]}"));
```

### Group By

```csharp
// Count users per department
users.Children()
    .GroupBy(u => u["department"].String())
    .ForEach((key, items) => Console.WriteLine($"{key}: {items.Count()}"));
```

### Order and Select

```csharp
// Order by age and select names
IEnumerable<string> orderedUsers = users.Children()
    .OrderBy(u => (int)u["age"])
    .Select(u => (string)u["name"]);

Console.WriteLine(string.Join(", ", orderedUsers));
// Output: Alice, Diana, Bob, Charlie
```

---

## API Reference

| Method | Description |
|--------|-------------|
| `Json.Parse(string)` | Parse JSON string to Json |
| `Json.From(object)` | Serialize object to JSON string |
| `Json.To<T>(string)` | Deserialize JSON string to T |
| `Json.Prettify(string)` | Pretty-format raw JSON string without parsing into `Json` tree |
| `Json.Compact(string)` | Compact raw JSON string without parsing into `Json` tree |
| `JsonObject.Set(key, value)` | Set key-value pair |
| `JsonArray.Add(value)` | Add element to array |
| `JsonArray.AddRange(values)` | Add multiple elements |
| `obj.Contains(key)` | Check if key exists |
| `obj.FindDeep(key)` | Deep search by key |
| `obj.Pretty()` | Format JSON with indentation |

---

## License

MIT License
