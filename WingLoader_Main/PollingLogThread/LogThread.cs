namespace WingLoader_NS
{
    public partial class WingLoader_Main
    {
        /// <summary> Asynchronous thread instance dedicated to background polling and runtime log generation. </summary>
        private System.Threading.Thread? pollingLoggerThread;

        /// <summary> Thread flow controller flag indicating if the status monitoring loop should remain active. </summary>
        private bool pollingLoggerThread_isLooping;

        /// <summary>
        /// Event channel triggered periodically by the background logger thread 
        /// to pass status tracking strings up to interface listeners.
        /// </summary>
        private event EventHandler<string>? mainLoggerEvent;


        /// <summary>
        /// Provisions, hooks up event handlers for, and starts up the independent background thread 
        /// tasked with periodic engine status monitoring.
        /// </summary>
        private void StartLoggerThread()
        {
            //Option 1:
            //Register the event handler which will handle main thread logging work...
            this.mainLoggerEvent += mainLogger_workCompleted;
            pollingLoggerThread = new System.Threading.Thread(() => mainLoggerThreadPoller());
            pollingLoggerThread.IsBackground = true; //So they shutdown when i close the app
            pollingLoggerThread_isLooping = true;
            pollingLoggerThread.Start();
            
            //Option 2:
            //Task.Run(() => mainLoggerThreadPoller()); //Alternate to the 2 lines above... could replace explicit thread creation if migrating to async/await structural patterns later.
        }

        /// <summary>
        /// Requests the background status logger thread to gracefully exit by flipping its looping control flag.
        /// </summary>
        private void StopLoggerThread()
        {
            pollingLoggerThread_isLooping = false;
        }

        /// <summary>
        /// A background tracking loop that rests for 2 seconds per cycle and broadcasts 
        /// active state telemetry data back up to the interface layer.
        /// </summary>
        private void mainLoggerThreadPoller()
        {
            while (pollingLoggerThread_isLooping)
            {
                Thread.Sleep(2000);
                mainLoggerEvent?.Invoke(this, string.Format("{0}", pollingLoggerThread_isLooping));
            }
        }

        /// <summary>
        /// Event completion consumer method that intercepts backgorund activities
        /// and securely wraps cross-thread handoffs to safely interact with UI components.
        /// </summary>
        /// <param name="sender">The component engine source that raised the event notifications.</param>
        /// <param name="message">The text status parameter or tracking string dispatched by the worker thread.</param>
        private void mainLogger_workCompleted(object? sender, string message)
        {
            //// Check if we are running on a non-UI threadhead - Not required since this isn't part of the main UI, we'll handle it instead in MessageViewer - which is updated in message_log
            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new Action(() => mainLogger_workCompleted(sender, message)));
            //    return;
            //}

            // Safe to interact with UI and show MessageBox here
            //MessageBox.Show(this, message, "Worker Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            logger.log(message);
        }
    }
}
