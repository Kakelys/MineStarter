using System.Buffers.Binary;
using System.Reflection;
using System.Runtime.CompilerServices;
using HandshakeHelper.Models.ResponsePayload;
using HandshakeHelper.StreamHelper.DI;
using HandshakeHelper.StreamHelper.VarInt;

namespace HandshakeHelper.StreamHelper;

public class ObjectStreamSerializer
{
    private readonly Dictionary<Type, ITypeSerializer> typeSerializers = new();
    private readonly IServiceProvider serviceProvider;
    private readonly byte[] byteBuffer = new byte[sizeof(long)];

    public ObjectStreamSerializer() : this(new ActivatorServiceProvider()) { }

    public ObjectStreamSerializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public T ReadObject<T>(Stream stream)
        => (T)ReadObject(stream, typeof(T));

    public object ReadObject(Stream stream, Type type)
    {
        var instance = Activator.CreateInstance(type);

        _ = stream.ReadVarInt32(out _);
        foreach ((var property, var attribute) in GetSerializationProperties(type))
        {
            var value = ReadValueFromStream(stream, property.PropertyType, attribute.TypeSerializer);

            if (!property.CanWrite)
            {
                throw new InvalidOperationException($"Cannot write to read-only property {property.Name} on type {type.FullName}");
            }

            property.SetValue(instance, value);
        }

        return instance;
    }

    public void WriteObject(Stream stream, object objectToWrite)
    {
        ArgumentNullException.ThrowIfNull(objectToWrite);

        var memoryStream = new MemoryStream();

        foreach ((var property, var attribute) in GetSerializationProperties(objectToWrite.GetType()))
        {
            var value = property.GetValue(objectToWrite);
            WriteValueToStream(memoryStream, value, attribute.TypeSerializer);
        }

        stream.WriteVarInt32((int)memoryStream.Position);
        memoryStream.Seek(0, SeekOrigin.Begin);
        memoryStream.CopyTo(stream);
    }

    private IEnumerable<(PropertyInfo property, StreamSerializationAttribute attribute)> GetSerializationProperties(Type type)
    {
        return type.GetProperties()
        .Select(p => new
        {
            property = p,
            attribute = p.GetCustomAttributes(typeof(StreamSerializationAttribute), true)
                .Cast<StreamSerializationAttribute>()!
                .FirstOrDefault()
        })
        .Where(x => x.attribute != null)
        .OrderBy(x => x.attribute!.Order)
        .Select(x => (x.property, x.attribute!));
    }

    private object ReadValueFromStream(Stream stream, Type propertyType, Type typeSerializerType)
    {
        if (typeSerializerType != null)
        {
            var typeSerializer = GetTypeSerializer(typeSerializerType);
            return typeSerializer.ReadObject(stream, propertyType);
        }

        if (propertyType == typeof(byte))
        {
            return stream.ReadByte();
        }
        else if (propertyType == typeof(short))
        {
            return BinaryPrimitives.ReadInt16BigEndian(ReadBytes(stream, sizeof(short)));
        }
        else if (propertyType == typeof(ushort))
        {
            return BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(stream, sizeof(ushort)));
        }
        else if (propertyType == typeof(int))
        {
            return BinaryPrimitives.ReadInt32BigEndian(ReadBytes(stream, sizeof(int)));
        }
        else if (propertyType == typeof(uint))
        {
            return BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(stream, sizeof(uint)));
        }
        else if (propertyType == typeof(long))
        {
            return BinaryPrimitives.ReadInt64BigEndian(ReadBytes(stream, sizeof(long)));
        }
        else if (propertyType == typeof(ulong))
        {
            return BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(stream, sizeof(ulong)));
        }
        else
        {
            throw new InvalidOperationException($"Cannot read type {propertyType.FullName} from stream without explicit serializer.");
        }
    }

    private Span<byte> ReadBytes(Stream stream, int length)
    {
        stream.Read(byteBuffer, 0, length);
        return new Span<byte>(byteBuffer, 0, length);
    }

    private void WriteValueToStream(Stream stream, object value, Type typeSerializerType)
    {
        if (typeSerializerType != null)
        {
            var typeSerializer = GetTypeSerializer(typeSerializerType);
            typeSerializer.WriteObject(stream, value);

            return;
        }

        var span = new Span<byte>(byteBuffer);

        if (value is byte byteValue)
        {
            stream.WriteByte(byteValue);
        }
        else if (value is short shortValue)
        {
            BinaryPrimitives.WriteInt16BigEndian(span, shortValue);
            stream.Write(span[..sizeof(short)]);
        }
        else if (value is ushort ushortValue)
        {
            BinaryPrimitives.WriteUInt16BigEndian(span, ushortValue);
            stream.Write(span[..sizeof(ushort)]);
        }
        else if (value is int intValue)
        {
            BinaryPrimitives.WriteInt32BigEndian(span, intValue);
            stream.Write(span[..sizeof(int)]);
        }
        else if (value is uint uintValue)
        {
            BinaryPrimitives.WriteUInt32BigEndian(span, uintValue);
            stream.Write(span[..sizeof(uint)]);
        }
        else if (value is long longValue)
        {
            BinaryPrimitives.WriteInt64BigEndian(span, longValue);
            stream.Write(span[..sizeof(long)]);
        }
        else if (value is ulong ulongValue)
        {
            BinaryPrimitives.WriteUInt64BigEndian(span, ulongValue);
            stream.Write(span[..sizeof(ulong)]);
        }
        else
        {
            throw new InvalidOperationException($"Cannot write type {value?.GetType()?.FullName ?? "null"} to stream without explicit serializer.");
        }
    }

    private ITypeSerializer GetTypeSerializer(Type typeSerializerType)
    {
        if (!typeof(ITypeSerializer).IsAssignableFrom(typeSerializerType))
        {
            throw new ArgumentException($"{typeSerializerType.FullName} does not implement {nameof(ITypeSerializer)}.");
        }

        if (typeSerializers.TryGetValue(typeSerializerType, out var cachedSerializer))
        {
            return cachedSerializer;
        }

        var serializer = (ITypeSerializer)serviceProvider.GetService(typeSerializerType);
        typeSerializers[typeSerializerType] = serializer;

        return serializer;
    }
}