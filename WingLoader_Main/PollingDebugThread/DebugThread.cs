using WingLoader_Generics;
using WingLoader_Generics.Debugger;
using WingLoader.PollingDebugThread;
using static WingLoader_Generics.WingLoader_Debugger;
using Generic_Generics.Audio;

namespace WingLoader_NS
{
    public partial class WingLoader_Main
    {
        /// <summary> Asynchronous tracking instance mapping the background polling execution thread. </summary>
        private System.Threading.Thread? messageLoggerThread;
        /// <summary> Thread flow control coordinator token indicating if the poller task loop should remain active. </summary>
        private bool messageLoggerThread_isLooping;

        /// <summary>
        /// Event channel triggered continuously at the completion of a background polling loop iteration, 
        /// transmitting refreshed unmanaged memory matrices to UI presentation methods.
        /// </summary>
        private event EventHandler<DebugEventObject>? messageLoggerEvent;

        /// <summary>
        /// Provisions, targets, and launches the unmanaged thread layer tasked with monitoring real-time game properties.
        /// </summary>
        private void StartDebuggerThread()
        {
            if (messageLoggerThread is null)
            {
                setDebugger();
                //Register the event handler which will handle main thread logging work...
                this.messageLoggerEvent += messageLogger_workCompleted;
                messageLoggerThread = new System.Threading.Thread(() => messageLoggerThreadPoller());
                messageLoggerThread.IsBackground = true; //So they shutdown when i close the app
                messageLoggerThread_isLooping = true;
                messageLoggerThread.Start();
                if (debugger is Win32_Debugger)
                {
                    mv.messages = $"Started Debugger Thread against ProcID {(debugger as Win32_Debugger)?.ProcessId}";
                }
            }
        }

        /// <summary>
        /// Terminates background engine execution loops, releases bound diagnostic instances, and flushes tracking contexts.
        /// </summary>
        public void StopDebuggerThread()
        {
            if (messageLoggerThread is not null)
            {
                messageLoggerThread_isLooping = false;
                messageLoggerThread = null;
            }
            message_log(); //set all the logging to Blank
            debugger.Dispose();
            Generic_SDL2_Audio.stopAudioPlayback();
        }

        /// <summary> Instance property for the required abstract polymorphic debugger. </summary>
        Debugger debugger = WingLoader_Debugger.Create(DebuggerTypes.Dummy);

        /// <summary>
        /// Continually samples live memory and pushes memory updates back to the UI
        /// </summary>
        /// <remarks>
        /// Leverages strict loop control tokens to monitor system connections. If an unmanaged memory scan 
        /// fails or target communication endpoints are dropped, the worker self-terminates by dropping its flag.
        /// </remarks>
        private void messageLoggerThreadPoller()
        {
            while (messageLoggerThread_isLooping)
            {
                // Safety Fix: Placed sleep at the absolute top of the loop context.
                // This ensures that hitting a 'continue' guard clause doesn't trigger 
                // an un-throttled infinite loop that pegs a CPU core at 100% load.

                Thread.Sleep(20);

                DebugEventObject returnObject = new DebugEventObject();
                
                if (string.IsNullOrEmpty(mem_Message1)) //Memory offsets are not yet setup, just wait
                    continue;

                if (debugger is DosBox_Debugger)
                {
                    var bytes = debugger.getMemory_Sync(debugger.initialOffset, "10000000");
                    if (bytes.Length != 3)
                    {
                        returnObject.msg1 = WingLoader_Debugger.BytesToString(getDosMemory(bytes, mem_Message1, -1));
                        returnObject.msg2 = WingLoader_Debugger.BytesToString(getDosMemory(bytes, mem_Message2, -1));
                        returnObject.firstName = WingLoader_Debugger.BytesToString(getDosMemory(bytes, mem_FirstName, -1));
                        returnObject.surname = WingLoader_Debugger.BytesToString(getDosMemory(bytes, mem_Surname, -1));
                        returnObject.callsign = WingLoader_Debugger.BytesToString(getDosMemory(bytes, mem_Callsign, -1));
                        returnObject.system = WingLoader_Debugger.BytesToString(getDosMemory(bytes, mem_System, -1));
                        returnObject.year = WingLoader_Debugger.BytesToInt16(getDosMemory(bytes, mem_Year, 2));
                        returnObject.day = WingLoader_Debugger.BytesToInt16(getDosMemory(bytes, mem_Day, 2));
                    }
                    else
                    {
                        returnObject.logMessage = "No memory debug returned (Dosbox is not running or is not setup to expose the debugger webservice), Debugger thread will be stopped";
                        returnObject.threadShouldStop = true; //Make sure the event handler stops the thread after processing...
                    }
                }
                else if (debugger is Win32_Debugger)
                {
                    //Need to read each section of memory independently for Win32....
                    var bytes = debugger.getMemory_Sync(debugger.initialOffset, "0x10");
                    if (bytes.Length != 3)
                    {
                        returnObject.msg1 = WingLoader_Debugger.BytesToString(getWin32Memory(mem_Message1, -1));
                        if (mem_Message2Indirect)
                        {
                            string memtarget = subtractHexStrings(BytesToHexString(getWin32Memory(mem_Message2, 4), true).Replace(" ", ""), debugger.initialOffset);
                            returnObject.msg2 = WingLoader_Debugger.BytesToString(getWin32Memory(memtarget, -1));
                        }
                        else
                        {
                            returnObject.msg2 = WingLoader_Debugger.BytesToString(getWin32Memory(mem_Message2, -1));
                        }
                        returnObject.firstName = WingLoader_Debugger.BytesToString(getWin32Memory(mem_FirstName, -1));
                        returnObject.surname = WingLoader_Debugger.BytesToString(getWin32Memory(mem_Surname, -1));
                        returnObject.callsign = WingLoader_Debugger.BytesToString(getWin32Memory(mem_Callsign, -1));
                        returnObject.system = WingLoader_Debugger.BytesToString(getWin32Memory(mem_System, -1));
                        returnObject.year = WingLoader_Debugger.BytesToInt16(getWin32Memory(mem_Year, 2));
                        returnObject.day = WingLoader_Debugger.BytesToInt16(getWin32Memory(mem_Day, 2));
                    }
                    else
                    {
                        returnObject.logMessage = "No memory debug returned (Win32 process is not running, or unable to attach to it), Debugger thread will be stopped";
                        returnObject.threadShouldStop = true; //Make sure the event handler stops the thread after processing...
                    }
                }

                // Finalize loop iteration and broadcast values up to registered UI listeners
                try
                {
                    messageLoggerEvent?.Invoke(this, returnObject);
                }
                catch { }

                //Disable looper so that no more messages are pushed to the UI after this one.
                if (returnObject.threadShouldStop)
                {
                    messageLoggerThread_isLooping = false;
                }
            }
        }

        /// <summary>
        /// Slices out a specific data block from a larger pre-fetched DOSBox memory dump 
        /// and applies length-based trimming.
        /// </summary>
        /// <param name="bytes">The full raw binary byte array snapshot retrieved from the emulator wrapper.</param>
        /// <param name="memAddress">The hexadecimal target address string used to locate the data block offset.</param>
        /// <param name="length">The desired output size limit; passing -1 extracts data until a null-terminator is reached.</param>
        /// <returns>A trimmed byte array slice, or a 3-byte empty array (<c>0x00, 0x00, 0x00</c>) if the address is blank.</returns>
        private byte[] getDosMemory(byte[] bytes, string memAddress, int length)
        {
            if (string.IsNullOrEmpty(memAddress))
                return new byte[] { 0x00, 0x00, 0x00 };
            var data = ReadMemoryBlock(bytes, memAddress, 100);
            return WingLoader_Debugger.trimFirstElementFromBytes(data, length);
        }

        /// <summary>
        /// Directly queries the active native Win32 process memory at a dynamically calculated 
        /// offset and trims the result block.
        /// </summary>
        /// <param name="memAddress">The relative hexadecimal offset string of the desired field.</param>
        /// <param name="length">The target output formatting size constraints passed to the tracking trimmer engine.</param>
        /// <returns>A validated and trimmed raw byte data array segment from the target process.</returns>
        private byte[] getWin32Memory(string memAddress, int length)
        {
            if (string.IsNullOrEmpty(memAddress))
                return new byte[] { 0x00, 0x00, 0x00 };
            string offset = WingLoader_Debugger.addHexStrings(debugger.initialOffset, memAddress);
            var data = debugger.getMemory_Sync(offset, "0x100");
            return WingLoader_Debugger.trimFirstElementFromBytes(data, length);
        }

        /// <summary>
        /// Handles the completion callback event dispatched by the background debugger poller thread, 
        /// safely marshaling execution back to the primary UI thread to handle UI controls, logging, and rules.
        /// </summary>
        /// <param name="sender">The source component object that raised the event channel.</param>
        /// <param name="messages">The populated <see cref="MyEventArgs"/> state telemetry and textual message data instance payload.</param>
        /// <remarks>
        /// Leverages <see cref="Control.InvokeRequired"/> to detect cross-thread execution. 
        /// Fixed: Adjusted the Invoke target to invoke this method directly instead of re-raising the event, 
        /// which prevents a recursive loop event storm across multiple form listeners.
        /// </remarks>
        private void messageLogger_workCompleted(object? sender, DebugEventObject messages)
        {
            //// Check if we are running on a non-UI threadhead - Not required since this isn't part of the main UI, we'll handle it instead in MessageViewer - which is updated in message_log
            //if (this.InvokeRequired)
            //{
            //    if (messageLoggerThread_isLooping)
            //    {
            //        this.Invoke(new Action(() => messageLogger_workCompleted(sender, messages)));
            //    }
            //    return;
            //}

            // Safe to interact with UI and show MessageBox here
            //MessageBox.Show(this, message, "Worker Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (messages.threadShouldStop)
            {
                StopDebuggerThread();
            }
            //Clean unchanged values and trigger scripting execution for changed values.
            DebugEventObject cleanMessages = message_log(messages);
            execRules(cleanMessages);
        }
    }
}
