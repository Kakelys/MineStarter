namespace MineStarter;

public static class ServerConstants
{
    public const string PathToServerBash = "~/tmpServer";
    public const string StartServerCommand = "sh start.sh";
    public const string TmuxSessionName = "minecraft";
    public const int MinutesUntilShutdown = 10;
    public const int IntervalsBetweenOnlineCheck = 2;
    public const int Port = 25565;
    public const bool IsDebug = true;
    public const bool LogEnabled = true;
}