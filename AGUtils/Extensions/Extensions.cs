using Newtonsoft.Json;

namespace Static.Extensions
{
    public static class Extensions
    {
        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented);
    }
}
