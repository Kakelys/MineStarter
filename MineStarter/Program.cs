using MineStarter;
using MineStarter.Utils.Settings;

Logger.DeleteLogFile();
await Logger.Log("\n");

while (true)
{
    await Start();
}

static async Task Start()
{
    // listen for players
    await PlayerListener.UntilNoPLayersForNMinutes(Settings.Server.MinutesUntilShutdown, Settings.Server.IntervalsBetweenOnlineCheck);

    if (await PlayerListener.IsOnline())
    {
        // stop server
        await Logger.Log("Stop server", Colors.Log);
        await LinuxCommands.RunCommand(LinuxCommands.StopServerCommand);

        // wait until server full stop and free port
        await Logger.Log("Waiting until server full stop", Colors.Log);
        while (await PlayerListener.IsOnline())
        {
            await Task.Delay(5000);
        }

        await Logger.Log("Kill tmux session", Colors.Log);
        await LinuxCommands.RunCommand(LinuxCommands.KillTmuxSession);
    }

    // wait until ping
    await PortListener.UntilSomeoneLogin(Settings.Server.Port);

    // stop if server is working
    if (await PlayerListener.IsOnline())
        return;

    await Logger.Log("Start new session", Colors.Log);
    await LinuxCommands.RunCommand(LinuxCommands.StartNewSessionCommand);

    await Logger.Log("Goto server folder", Colors.Log);
    await LinuxCommands.RunCommand(LinuxCommands.GoToServerFolderCommand);

    await Logger.Log("Start server", Colors.Log);
    await LinuxCommands.RunCommand(LinuxCommands.StartServerCommand);
}