using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Khourse.Api.Common;

public class JsonConfig
{
    public static readonly JsonSerializerSettings SnakeCaseSettings = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };
}
