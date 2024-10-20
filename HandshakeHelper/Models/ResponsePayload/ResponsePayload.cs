using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.String;
using Newtonsoft.Json;

namespace HandshakeHelper.Models.ResponsePayload;

public class ResponsePayload
{
    [JsonProperty(PropertyName = "previewsChat")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public bool PreviewsChat { get; set; }

    [JsonProperty(PropertyName = "enforcesSecureChat")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public bool EnforcesSecureChat { get; set; }

    [JsonProperty(PropertyName = "description")]
    public Description Description { get; set; }

    [JsonProperty(PropertyName = "players")]
    public PlayerInfo PlayerInfo { get; set; }

    [JsonProperty(PropertyName = "version")]
    public Version Version { get; set; }

    [JsonProperty(PropertyName = "favicon")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string Favicon { get; set; }

    [JsonProperty(PropertyName = "modpackData")]
    public ModpackInfo ModpackData { get; set; }
}
