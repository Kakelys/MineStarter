namespace MineStarter;

public static class PlayerListener
{
    /// <summary>
    /// true if server running, false if server shut down
    /// </summary>
    public static async Task UntilNoPLayersForNMinutes(int minutes, int waitIntervalMins)
    {
        double tmpMinutes = minutes;

        // exit from method via 'delayOnServerShutdown' ms, check online in 'step' count
        const int step = 8;
        var delayOnServerShutdown = 120_000 / step;
        var stepCount = 1;

        while (tmpMinutes > 0)
        {
            var playersOnline = await GetPlayersOnline();

            if (playersOnline == -1)
            {
                await Logger.Log($"Seems, server not working [{stepCount++}/{step}]");
                tmpMinutes -= (double)minutes / step;

                if (tmpMinutes <= 0)
                    return;

                await Task.Delay(delayOnServerShutdown);
                continue;
            }

            if (playersOnline == 0)
            {
                tmpMinutes -= waitIntervalMins;
            }
            else
            {
                tmpMinutes = minutes;
                stepCount = 1;
            }

            await Logger.Log($"Players online: {playersOnline}", Colors.Info);
            await Logger.Log($"minutes until stop: {tmpMinutes} (+-{waitIntervalMins})", Colors.Log);

            await Task.Delay(waitIntervalMins * 60000);
        }
    }

    public static async Task<bool> IsOnline()
    {
        return await GetPlayersOnline() >= 0;
    }

    private static async Task<int> GetPlayersOnline()
    {
        var javaSessions = await LinuxCommands.RunCommand(LinuxCommands.GetActiveJavaSessions);

        if (!int.TryParse(javaSessions, out var sessions))
        {
            return -1;
        }

        return sessions - 1;
    }
}