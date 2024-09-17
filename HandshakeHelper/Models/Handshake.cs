using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.String;
using HandshakeHelper.StreamHelper.VarInt;

namespace HandshakeHelper.Models;

public class Handshake : BasePacket
{
    [StreamSerialization(0, TypeSerializer = typeof(VarIntSerializer))]
    public int ProtocolVersion { get; set; }

    [StreamSerialization(1, TypeSerializer = typeof(StringSerializer))]
    public string ServerAddress { get; set; }

    [StreamSerialization(2)]
    public ushort ServerPort { get; set; }

    [StreamSerialization(3, TypeSerializer = typeof(VarIntSerializer))]
    public HandshakeStates NextState { get; set; }
}
