using System.Text.Json;
using System.Text.Json.Serialization;

namespace EF_Demo
{
    public static class Extensions
    {
        public static string ToJson(this object source)
        {
            return JsonSerializer.Serialize(source);
        }
        
        public static bool Compare(this string source, string target)
        {
            return source.Equals(target);
        }
    }
}