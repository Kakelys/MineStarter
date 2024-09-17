using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.String;
using Newtonsoft.Json;

namespace HandshakeHelper.Models.ResponsePayload;

public class ResponsePayload
{
    [JsonProperty(PropertyName = "version")]
    public Version Version { get; set; }

    [JsonProperty(PropertyName = "description")]
    public Description Description { get; set; }

    [JsonProperty(PropertyName = "players")]
    public PlayerInfo PlayerInfo { get; set; }

    [JsonProperty(PropertyName = "favicon")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string Favicon { get; set; }
}
