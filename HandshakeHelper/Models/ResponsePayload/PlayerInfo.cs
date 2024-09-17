using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.VarInt;
using Newtonsoft.Json;

namespace HandshakeHelper.Models.ResponsePayload;

public class PlayerInfo
{
    [JsonProperty(PropertyName = "max")]
    [StreamSerialization(0, TypeSerializer = typeof(VarIntSerializer))]
    public int Max { get; set; }

    [JsonProperty(PropertyName = "online")]
    [StreamSerialization(0, TypeSerializer = typeof(VarIntSerializer))]
    public int Online { get; set; }
}
