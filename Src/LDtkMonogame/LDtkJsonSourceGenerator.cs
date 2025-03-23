namespace LDtk;

using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Xna.Framework;

/// <summary> The json source generator for LDtk files. </summary>
/*[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(JsonElement), TypeInfoPropertyName = "JsonElement")]
[JsonSerializable(typeof(bool), TypeInfoPropertyName = "Bool")]
[JsonSerializable(typeof(bool[]), TypeInfoPropertyName = "BoolArray")]
[JsonSerializable(typeof(float), TypeInfoPropertyName = "Float")]
[JsonSerializable(typeof(float[]), TypeInfoPropertyName = "FloatArray")]
[JsonSerializable(typeof(int), TypeInfoPropertyName = "Int")]
[JsonSerializable(typeof(int[]), TypeInfoPropertyName = "IntArray")]
[JsonSerializable(typeof(string), TypeInfoPropertyName = "String")]
[JsonSerializable(typeof(string[]), TypeInfoPropertyName = "StringArray")]
[JsonSerializable(typeof(Color), TypeInfoPropertyName = "Color")]
[JsonSerializable(typeof(Color[]), TypeInfoPropertyName = "ColorArray")]
[JsonSerializable(typeof(Point), TypeInfoPropertyName = "Point")]
[JsonSerializable(typeof(Point[]), TypeInfoPropertyName = "PointArray")]
[JsonSerializable(typeof(Vector2), TypeInfoPropertyName = "Vector2")]
[JsonSerializable(typeof(Vector2[]), TypeInfoPropertyName = "Vector2Array")]
[JsonSerializable(typeof(EntityReference), TypeInfoPropertyName = "EntityReference")]
[JsonSerializable(typeof(EntityReference[]), TypeInfoPropertyName = "EntityReferenceArray")]
[JsonSerializable(typeof(TilesetRectangle), TypeInfoPropertyName = "TilesetRectangle")]
[JsonSerializable(typeof(TilesetRectangle[]), TypeInfoPropertyName = "TilesetRectangleArray")]
[JsonSerializable(typeof(LDtkFile), TypeInfoPropertyName = "LDtkFile")]*/
public partial class LDtkJsonSourceGenerator : JsonSerializerContext
{
    internal JsonSerializerOptions LDtkLevel;
    private JsonSerializerOptions serializeOptions;

    public LDtkJsonSourceGenerator(JsonSerializerOptions serializeOptions)
    {
        this.serializeOptions = serializeOptions;
    }
}
