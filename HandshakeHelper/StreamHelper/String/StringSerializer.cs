using System.Text;
using HandshakeHelper.StreamHelper.VarInt;

namespace HandshakeHelper.StreamHelper.String;

public class StringSerializer : ITypeSerializer
{
    public object ReadObject(Stream stream, Type targetType)
    {
        if(targetType == typeof(string))
        {
            var byteLength = stream.ReadVarInt32(out _);

            var buffer = new byte[byteLength];
            stream.Read(buffer, 0, byteLength);
            return Encoding.UTF8.GetString(buffer);
        }
        else
        {
            throw new InvalidOperationException($"{nameof(StringSerializer)} cannot read type {targetType.FullName}.");
        }
    }

    public void WriteObject(Stream stream, object value)
    {
        if(value is string stringValue)
        {
            stream.WriteVarInt64(stringValue.Length);
            stream.Write(Encoding.UTF8.GetBytes(stringValue));
        }
        else
        {
            throw new InvalidOperationException($"{nameof(StringSerializer)} cannot serialize type {value?.GetType()?.FullName}.");
        }
    }
}
