using System.Diagnostics;

namespace MineStarter;

public static class LinuxCommands
{
    public static readonly string StartNewSessionCommand = $"tmux new-session -d -s {ServerConstants.TmuxSessionName}";
    public static readonly string GoToServerFolderCommand = $"tmux send-keys -t {ServerConstants.TmuxSessionName} 'cd {ServerConstants.PathToServerBash}' Enter";
    public static readonly string StartServerCommand = $"tmux send-keys -t {ServerConstants.TmuxSessionName} '{ServerConstants.StartServerCommand}' Enter";
    public static readonly string StopServerCommand = $"tmux send-keys -t {ServerConstants.TmuxSessionName} 'stop' Enter";
    public static readonly string KillTmuxSession = $"tmux kill-session -t {ServerConstants.TmuxSessionName}";

    public static readonly string GetActiveJavaSessions = $"lsof -i -P -n | grep -P -c 'java.*{ServerConstants.Port}'";

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