
using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.String;

namespace HandshakeHelper.Models;

public class Response : BasePacket
{
    [StreamSerialization(0, TypeSerializer = typeof(StringSerializer))]
    public string ResponseJson { get; set; }
}
