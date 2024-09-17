namespace HandshakeHelper.StreamHelper;

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class StreamSerializationAttribute(int order) : Attribute
{
    public int Order { get; } = order;

    public Type TypeSerializer { get; set; }
}
