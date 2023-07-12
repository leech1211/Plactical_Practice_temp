using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum BulletType
{
    NONE,
    NORMAL,
    FIRE,
    WATER,
    LEAF,
    BOUNCE,
    SERCH,
    RECURSIVE,
}