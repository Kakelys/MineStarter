namespace MineStarter.Utils.Settings;

public class ServerOptions
{
    public string PathToServerBash { get; set; }
    public string StartServerCommand { get; set; }
    public string TmuxSessionName { get; set; }
    public int MinutesUntilShutdown { get; set; }
    public int IntervalsBetweenOnlineCheck { get; set; }
    public int Port { get; set; }
    public bool IsDebug { get; set; }
    public bool LogEnabled { get; set; }
    public bool PreviewsChat { get; set; }
    public bool EnforcesSecureChat { get; set; }

}
