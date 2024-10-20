using System.Net;
using System.Net.Sockets;
using HandshakeHelper.Models;
using HandshakeHelper.Models.ResponsePayload;
using HandshakeHelper.StreamHelper;
using MineStarter.Utils.Settings;
using Newtonsoft.Json;
using Version = HandshakeHelper.Models.ResponsePayload.Version;

namespace MineStarter;

public static class PortListener
{
    private const int _portInUseExceptionDelaySecs = 10;
    private const int taskLimit = 5;

    public static async Task<bool> UntilSomeoneLogin(int port)
    {
    startOfUntilSomeoneLoginMethod:
        try
        {
            using var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            await Logger.Log($"Started listening port {port}", Colors.Success);

            await HandleConnections(listener);

            listener.Stop();

            await Logger.Log($"Stopped listening port {port}", Colors.Success);

            return true;
        }
        catch (SocketException ex) when (ex.ErrorCode == 98)
        {
            await Logger.Log($"Port {port} already in use", Colors.Warning);
            await Logger.Log($"Start listening again via {_portInUseExceptionDelaySecs} secs", Colors.Info);

            await Task.Delay(_portInUseExceptionDelaySecs * 1000);
            goto startOfUntilSomeoneLoginMethod;
        }
    }

    private static async Task HandleConnections(TcpListener listener)
    {
        await Logger.Log("Started listening connections", Colors.Success);

        var tasks = new List<Task>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // for 'taskLimit' possible connection at the same time
        for (var i = 0; i < taskLimit; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                while (!await HandleTcpConnectWrapper(listener, cancellationToken)) { }

                await cancellationTokenSource.CancelAsync();
            }, cancellationToken));
        }

        await Task.WhenAll(tasks);
    }

    private static async Task<bool> HandleTcpConnectWrapper(TcpListener listener, CancellationToken cancellationToken)
    {
        try
        {
            return await HandleTcpConnect(listener, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return true;
        }
        catch (Exception ex)
        {
            await Logger.Log("Connection not handled", Colors.Warning);
            await Logger.Log($"{ex.Message}; {ex.StackTrace}", Colors.Error);

            return false;
        }
    }

    /// <summary>
    /// Returns true on login attempt
    /// </summary>
    private static async Task<bool> HandleTcpConnect(TcpListener listener, CancellationToken cancellationToken)
    {
        using var client = await listener.AcceptTcpClientAsync(cancellationToken);
        await Logger.Log("Received ping", Colors.Info);

        var stream = client.GetStream();

        var handshake = stream.ReadObject<Handshake>();
        _ = stream.ReadObject<Request>();

        if (handshake.NextState == HandshakeStates.Login)
        {
            stream.WriteObject(new Disconnect
            {
                Reason = $"'{Settings.Handshake.DisconnectReason}'"
            });

            await Logger.Log("Someone connected", Colors.Info);
            return true;
        }

        var moddedForge = handshake.ServerAddress.Contains("FML");
        stream.WriteObject(await GetResponse(moddedForge));

        return false;
    }

    private static async Task<Response> GetResponse(bool modded = false)
    {
        var favicon = "";
        if (File.Exists("Assets/favicon.png"))
        {
            byte[] imageArray = await File.ReadAllBytesAsync("Assets/favicon.png");
            favicon = Convert.ToBase64String(imageArray);
        }

        var resPayload = new ResponsePayload
        {
            Version = new Version
            {
                Name = Settings.Handshake.Name,
                Protocol = Settings.Handshake.Protocol
            },
            Description = new Description
            {
                Text = Settings.Handshake.Text
            },
            PlayerInfo = new PlayerInfo
            {
                Online = 0,
                Max = 0
            },
            Favicon = $"data:image/png;base64,{favicon}",
            ModpackData = modded ?
                new ModpackInfo
                {
                    Type = "FML",
                    ProjectId = "0",
                    Name = "MC Chocolate Edition",
                    Version = "1.5",
                    VersionId = "0",
                    IsMetadata = false
                }
                : null,
            PreviewsChat = false,
            EnforcesSecureChat = false,
        };

        var resPayloadJson = JsonConvert.SerializeObject(resPayload);

        return new Response
        {
            ResponseJson = resPayloadJson
        };
    }
}
