using WingLoader_Generics;


namespace WingLoader_NS
{
    public partial class WingLoader_Main
    {
        /// <summary>
        /// UI click event handler that spins up the selected game profile engine under an elevated 
        /// administrative security privilege context, delaying briefly before activating memory debug hooks.
        /// </summary>
        public async void StartProcessElevated_Click()
        {
            try
            {
                WingLoader_Worker worker = new WingLoader_Worker();
                string message = worker.StartProcessAsAdmin(activeGameMode);

                // Non-blocking wait: Yields the thread back to the UI layout engine for 5 seconds 
                // while the emulator mounts handles in the background.
                if (WingLoader_Worker.gameModesDOS.Contains(activeGameMode))
                    await Task.Delay(5000); //Ensure Dosbox is running
                else if (WingLoader_Worker.gameModesWin.Contains(activeGameMode))
                    await Task.Delay(1000); //Ensure Game has started

                StartDebugger_Click();
                logger.log(message);
            }
            catch (Exception ex)
            {
                logger.log($"Elevation Launch Crash Encountered: {ex.Message}");
            }
        }

        /// <summary>
        /// UI click event handler that spins up the selected game profile engine under standard user 
        /// access security privileges, pausing briefly to let resources load before running memory scanners.
        /// </summary>
        public async void StartProcess_Click()
        {
            try
            {
                WingLoader_Worker worker = new WingLoader_Worker();
                string message = worker.StartProcess(activeGameMode);

                // Non-blocking wait: Yields the thread back to the UI layout engine for 5 seconds 
                // while the emulator mounts handles in the background.
                if (WingLoader_Worker.gameModesDOS.Contains(activeGameMode))
                    await Task.Delay(5000); //Ensure Dosbox is running
                else if (WingLoader_Worker.gameModesWin.Contains(activeGameMode))
                    await Task.Delay(1000); //Ensure Game has started

                StartDebugger_Click();
                logger.log(message);
            }
            catch (Exception ex)
            {
                logger.log($"Launch Crash Encountered: {ex.Message}");
            }
        }
    }
}
