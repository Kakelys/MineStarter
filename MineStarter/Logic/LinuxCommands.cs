using System.Diagnostics;
using MineStarter.Utils.Settings;

namespace MineStarter;

public static class LinuxCommands
{
    public static readonly string StartNewSessionCommand = $"tmux new-session -d -s {Settings.Server.TmuxSessionName}";
    public static readonly string GoToServerFolderCommand = $"tmux send-keys -t {Settings.Server.TmuxSessionName} 'cd {Settings.Server.PathToServerBash}' Enter";
    public static readonly string StartServerCommand = $"tmux send-keys -t {Settings.Server.TmuxSessionName} '{Settings.Server.StartServerCommand}' Enter";
    public static readonly string StopServerCommand = $"tmux send-keys -t {Settings.Server.TmuxSessionName} 'stop' Enter";
    public static readonly string KillTmuxSession = $"tmux kill-session -t {Settings.Server.TmuxSessionName}";

    public static readonly string GetActiveJavaSessions = $"lsof -i -P -n | grep -P -c 'java.*{Settings.Server.Port}'";

    public static async Task<string> RunCommand(string command)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();

        var result = await process.StandardOutput.ReadToEndAsync();

        await process.WaitForExitAsync();

        return result;
    }
}