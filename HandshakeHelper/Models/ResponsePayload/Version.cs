using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.String;
using Newtonsoft.Json;

namespace HandshakeHelper.Models.ResponsePayload;

public class Version
{
    [JsonProperty(PropertyName = "name")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "protocol")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public long Protocol { get; set; }
}
