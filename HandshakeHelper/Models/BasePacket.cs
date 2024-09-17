using HandshakeHelper.StreamHelper;
using HandshakeHelper.StreamHelper.VarInt;

namespace HandshakeHelper.Models;

public class BasePacket
{
    [StreamSerialization(-1, TypeSerializer = typeof(VarIntSerializer))]
    public int PacketId { get; set; }
}
