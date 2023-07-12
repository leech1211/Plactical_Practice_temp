using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum DataType
{
    UNDEFINED = 8,
    INT = 0,
    FLOAT = 1,
    OPERATOR = 2,
    BOOL = 3,
    PROPERTY = 4,
    FLOW = 5,
    CHAR = 6,
    STRING = 12,
    COLOR = 7,
    DRONE_FLOAT = 9,
    DRONE_INT = 10,
    DRONE_VAR = 11,
}