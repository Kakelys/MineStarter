using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.String;
using Newtonsoft.Json;

namespace HandshakeHelper.Models.ResponsePayload;

public class Description
{
    [JsonProperty(PropertyName = "text")]
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string Text { get; set; }
}
