using Microsoft.Extensions.Configuration;

namespace MineStarter.Utils.Settings;

public class Settings
{
    public static ServerOptions Server = new();
    public static HandshakeOptions Handshake = new();
    public static ModdedOptions Modpack = new();

    static Settings()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>().Build();

        builder.GetSection("Server").Bind(Server);
        builder.GetSection("Handshake").Bind(Handshake);
        builder.GetSection("Modpack").Bind(Modpack);
    }
}
