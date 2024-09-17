using System;

namespace HandshakeHelper.StreamHelper.DI;

public class ActivatorServiceProvider : IServiceProvider
{
    public object GetService(Type serviceType) => Activator.CreateInstance(serviceType);
}
