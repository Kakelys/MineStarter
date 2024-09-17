namespace HandshakeHelper.StreamHelper;

public interface ITypeSerializer
{
    void WriteObject(Stream stream, object value);
    object ReadObject(Stream stream, Type targetType);
}
