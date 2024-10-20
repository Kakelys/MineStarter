using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.String;
using Newtonsoft.Json;

namespace HandshakeHelper.Models.ResponsePayload;

public class ModpackInfo
{
    [JsonProperty(PropertyName = "type")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string Type { get; set; }

    [JsonProperty(PropertyName = "projectID")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string ProjectId { get; set; }

    [JsonProperty(PropertyName = "name")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "version")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string Version { get; set; }

    [JsonProperty(PropertyName = "versionID")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string VersionId { get; set; }

    [JsonProperty(PropertyName = "isMetadata")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public bool IsMetadata { get; set; }
}
