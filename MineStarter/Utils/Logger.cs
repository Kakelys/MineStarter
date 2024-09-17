namespace MineStarter;

public static class Logger
{
    public static async Task Log(string message, Colors color = Colors.Default)
    {
        if (!ServerConstants.LogEnabled)
            return;

        message = $"{DateTime.Now:[HH:mm:ss]} " + message;

        if (ServerConstants.IsDebug)
        {
            Console.ForegroundColor = (ConsoleColor)color;

            Console.WriteLine(message);

            Console.ForegroundColor = (ConsoleColor)Colors.Log;
        }
        else
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            await File.AppendAllLinesAsync("Logs/log.txt", [$"{message}\n"]);
        }
    }

    public static void DeleteLogFile()
    {
        if (File.Exists("Logs/log.txt"))
            File.Delete("Logs/log.txt");
    }
}