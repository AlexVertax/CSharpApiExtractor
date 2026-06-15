# CSharpApiExtractor

`CSharpApiExtractor` scans C# source files with Roslyn and builds a JSON description of the public API.

It extracts:

- namespaces
- classes, structs, interfaces, records, enums, and delegates
- constructors, methods, properties, indexers, and fields
- method parameters
- XML documentation summaries
- undocumented items in a separate `MissedItems` list

## Projects

- `CSharpApiExtractor` - the main library
- `CSharpApiExtractor.Example` - generic console example
- `SilkJson` - local JSON serializer used by the extractor

## Library Usage

```csharp
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CSharpApiExtractor;

ExtractorOptions options = new ExtractorOptions();
options.SourcePaths.Add(@"D:\Projects\MyLibrary");
options.ExcludedPaths.Add(@"D:\Projects\MyLibrary\Editor");

Extractor extractor = new Extractor(options);
ApiDocument document = await extractor.ExtractAsync();

File.WriteAllText("api.json", document.ToJsonString(pretty: true), Encoding.UTF8);

if (document.MissedItems.Count > 0)
{
    File.WriteAllLines("missed-items.txt", document.MissedItems, Encoding.UTF8);
}
```

## Output Shape

```json
{
  "MyLibrary.Core": {
    "UserService": {
      "declaration": "MyLibrary.Core.UserService",
      "summary": "Provides user-related operations.",
      "members": [
        {
          "name": "GetById",
          "type": 3,
          "declaration": "MyLibrary.Core.User GetById(int)",
          "summary": "Returns a user by identifier.",
          "valueType": "MyLibrary.Core.User",
          "parameters": [
            {
              "name": "id",
              "type": "int"
            }
          ]
        }
      ]
    }
  }
}
```

## Notes

- The extractor reads `.cs` files directly.
- It does not require parsing a `.csproj`.
- XML summaries are taken from `///` comments.
- JSON output is produced through `SilkJson`.

## License

MIT
