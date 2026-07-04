using System.Text;
using WingLoader_Generics;
using static WingLoader_Generics.WingLoader_Debugger;

namespace WingLoader_NS
{
    public partial class WingLoader_Main
    {
        /// <summary>
        /// UI click event handler that boots up background scanning worker, loads game specific 
        /// parsing rule flat files, and dynamic variable layout offset pointers.
        /// </summary>
        /// <remarks>
        /// If an initial connection failure or pointer resolution crash triggers an exception, 
        /// it executes defensive cleanup steps to guarantee background worker handles drop safely.
        /// </remarks>
        public void StartDebugger_Click()
        {
            StartDebuggerThread();
            try
            {
                setRules(activeGameMode);
                getGameStartOffset();
                setGameOffsets();

            }
            catch (Exception ex)
            {
                logger.log(string.Format("Debugger not started: {0}", ex.Message));

                //Shutdown cleanly
                StopDebuggerThread();
                message_log();
            }
        }

        /// <summary>
        /// UI click event handler that cleanly requests the background monitoring threads to shut down 
        /// and resets memory layout position variables back to fallback baseline states.
        /// </summary>
        public void StopDebugger_Click()
        {
            StopDebuggerThread();

            //For cleanliness...
            resetGameStartOffset();
            logger.log("Debugger has been stopped");
        }

        /// <summary>
        /// Evaluates the selected game version state and instantiates the matching polymorphic debugger type
        /// </summary>
        private void setDebugger()
        {
            if (WingLoader_Worker.gameModesDOS.Contains(activeGameMode))
            {
                debugger = WingLoader_Debugger.Create(WingLoader_Generics.DebuggerTypes.Dosbox);
            }
            else if (WingLoader_Worker.gameModesWin.Contains(activeGameMode))
            {
                debugger = WingLoader_Debugger.Create(WingLoader_Generics.DebuggerTypes.Win32);
                // Assign the running executable process Identifier to the debugger
                debugger.setProcessID(Path.GetFileNameWithoutExtension(WingLoader_Worker.getGameExecutable(activeGameMode)));
            }
        }

        /// <summary>
        /// UI click event handler that synchronously fetches a 100,000-byte block of process memory 
        /// and converts it to a raw hexadecimal string representation for display.
        /// Note that since this is debugger type independent, larger memory addresses break for Kilrathi Saga
        /// - ideally the memory pulled would be over the mapped process space (see VirtualQueryEx) but that's not required for this debug function.
        /// </summary>
        /// <remarks>
        /// Warning: Because this uses a synchronous memory fetch call, it can cause the UI 
        /// to freeze momentarily if the underlying debugger attachment takes time to resolve.
        /// However most of the slowness is logging the large dataset to the UI.
        /// </remarks>
        public void ExecuteDebugger_Sync_Click()
        {
            setDebugger();

            try
            {
                Byte[] bytes = debugger.getMemory_Sync(debugger.initialOffset, "100000");
                mv.hex = BytesToHexString(bytes);
            }
            catch (Exception ex)
            {
                logger.log(ex.Message);
            }
        }

        /// <summary>
        /// UI click event handler that synchronously fetches a 100,000-byte block of process memory 
        /// and converts it to a raw hexadecimal string representation for display.
        /// Note that since this is debugger type independent, larger memory addresses break for Kilrathi Saga
        /// - ideally the memory pulled would be over the mapped process space (see VirtualQueryEx) but that's not required for this debug function
        /// </summary>
        public async void ExecuteDebugger_Async_Click()
        {
            setDebugger();
            try
            {
                Byte[] bytes = await debugger.getMemory_ASync(debugger.initialOffset, "100000");
                mv.hex = BytesToHexString(bytes);
            }
            catch (Exception ex)
            {
                logger.log(ex.Message);
            }
        }
    }
}
