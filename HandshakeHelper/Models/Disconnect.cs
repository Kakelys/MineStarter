using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.String;

namespace HandshakeHelper.Models;

public class Disconnect : BasePacket
{
    [StreamSerialization(1, TypeSerializer = typeof(StringSerializer))]
    public string Reason { get; set; }
}
