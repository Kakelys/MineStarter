namespace HandshakeHelper.StreamHelper;

public static class StreamExtensions
{
    private static readonly Lazy<ObjectStreamSerializer> serializer = new();

    public static T ReadObject<T>(this Stream stream)
        => serializer.Value.ReadObject<T>(stream);

    public static object ReadObject(this Stream stream, Type type)
        => serializer.Value.ReadObject(stream, type);

    public static void WriteObject(this Stream stream, object objectToWrite)
        => serializer.Value.WriteObject(stream, objectToWrite);
}
