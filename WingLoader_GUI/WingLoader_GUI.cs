using Generic_Generics.Configurations;
using Generic_Generics.Processes;
using WingLoader_NS;
using WingLoader_Generics;
using WingLoader.Scripting;
using System.Drawing.Text;

namespace WingLoader_GUI
{
    internal partial class WingLoader_GUI_Form : Form
    {
        /// <summary>
        /// Global WingLoader class which abstracts away all real functionality from the UI
        /// </summary>
        private WingLoader_Main wl;
        ///MessageViewer is used to hand data back from process threads to the UI
        private MessageViewer mv;

        /// <summary>
        /// Initializes a new instance of the <see cref="WingLoader_GUI"/> class, 
        /// triggering layout component initialization, file structure setup, and system configuration parsing.
        /// </summary>
        internal WingLoader_GUI_Form()
        {
            InitializeComponent();

            mv = new MessageViewer();

            mv.ItemChanged += MessageViewer_Updated!;

            // Registers logging sinks and attaches delegates into the global logging router instance
            List<Action<string>> logfunctions = new List<Action<string>>();
            logfunctions.Add(func_log);

            wl = new WingLoader_Main(logfunctions, mv);

            if (wl.DebugMode)
                activateDebugWindows();
            else
                this.Size = new Size(382, 561);
        }

        /// <summary>
        /// Action to handle updates being called back to the UI - receives a KeyValuePair (entries in the MessageViewer object) which will (once on the main UI thread) trigger GUI updates.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="updateItem"></param>
        private void MessageViewer_Updated(object sender, KeyValuePair<string, string> updateItem)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI(updateItem)));
            }
            else
            {
                UpdateUI(updateItem);
            }
        }

        /// <summary>
        /// Receive a KeyValuePair and update the UI object indicated by the 'Key' to reflect the 'Value'.
        /// </summary>
        /// <param name="item"></param>
        private void UpdateUI(KeyValuePair<string, string> item)
        {

            switch (item.Key)
            {
                case "tb_Message1": tb_Message1.Text = item.Value; break;
                case "tb_Message2": tb_Message2.Text = item.Value; break;
                case "tb_Firstname": tb_Firstname.Text = item.Value; break;
                case "tb_Surname": tb_Surname.Text = item.Value; break;
                case "tb_Callsign": tb_Callsign.Text = item.Value; break;
                case "tb_System": tb_System.Text = item.Value; break;
                case "tb_Year": tb_Year.Text = item.Value; break;
                case "tb_Day": tb_Day.Text = item.Value; break;

                case "tb_Hex": tb_Hex.Text = item.Value; break;
                case "tb_Address": tb_Address.Text = item.Value; break;
                case "tb_string": tb_string.Text = item.Value; break;
                case "tb_hexstring": tb_hexstring.Text = item.Value; break;
                
                case "tb_messages": func_log(item.Value); break;
                default: break;
            }
        }

        /// <summary>
        /// Appends a new tracing or status text message to the top of the interface log display text box control.
        /// </summary>
        /// <param name="message">The text string payload to append to the user view display console window.</param>
        /// <remarks>
        /// Warning: This method updates UI components directly and must be called exclusively 
        /// from the primary user interface thread context.
        /// </remarks>
        private void func_log(string message)
        {
            tb_messages.Text = message + Environment.NewLine + tb_messages.Text;
        }

        /// <summary>
        /// UI click event handler that spins up the selected game profile engine under an elevated 
        /// administrative security privilege context, delaying briefly before activating memory debug hooks.
        /// </summary>
        private async void btn_StartProcessElevated_Click(object sender, System.EventArgs e)
        {
            wl.StartProcessElevated_Click();
        }

        /// <summary>
        /// UI click event handler that spins up the selected game profile engine under standard user 
        /// access security privileges, pausing briefly to let resources load before running memory scanners.
        /// </summary>
        private async void btn_StartProcess_Click(object sender, EventArgs e)
        {
            wl.StartProcess_Click();
        }

        /// <summary>
        /// UI click event handler that synchronously fetches a 100,000-byte block of process memory 
        /// and converts it to a raw hexadecimal string representation for display.
        /// </summary>
        /// <remarks>
        /// Warning: Because this uses a synchronous memory fetch call, it can cause the UI 
        /// to freeze momentarily if the underlying debugger attachment takes time to resolve.
        /// </remarks>
        private void btn_ExecuteDebugger_Sync_Click(object sender, EventArgs e)
        {
            wl.ExecuteDebugger_Sync_Click();
        }

        /// <summary>
        /// UI click event handler that asynchronously fetches a 1,000,000-byte memory block, 
        /// extracts a targeted segment using the address inside <see cref="tb_Address"/>, and logs the textual output.
        /// </summary>
        private void btn_ExecuteDebugger_Async_Click(object sender, EventArgs e)
        {
            wl.ExecuteDebugger_Async_Click();
        }

        /// <summary>
        /// UI click event handler that boots up background scanning workers, loads game specific 
        /// parsing rule flat files, and dynamic variable layout offset pointers.
        /// </summary>
        /// <remarks>
        /// If an initial connection failure or pointer resolution crash triggers an exception, 
        /// it executes defensive cleanup steps to guarantee background worker handles drop safely.
        /// </remarks>
        private void btn_StartDebugger_Click(object sender, EventArgs e)
        {
            wl.StartDebugger_Click();
        }

        /// <summary>
        /// UI click event handler that cleanly requests the background monitoring threads to shut down 
        /// and resets memory layout position variables back to fallback baseline states.
        /// </summary>
        private void btn_StopDebugger_Click(object sender, EventArgs e)
        {
            wl.StopDebugger_Click();
        }

        /// <summary>
        /// Change of checkbox indicating whether the desired game mode is Kilrathi Saga
        /// </summary>
        private void cb_WCKS_CheckedChanged(object sender, EventArgs e)
        {
            //If we're activating KS then disable WCAT - temporarily disable the event handler while we do that...
            if ((sender is System.Windows.Forms.CheckBox cbSender) && (cbSender.Checked == true))
            {
                try
                {
                    cb_WCAT.CheckedChanged -= cb_WCAT_CheckedChanged!;
                    cb_WCAT.Checked = false;
                }
                finally
                {
                    cb_WCAT.CheckedChanged += cb_WCAT_CheckedChanged!;
                }
            }
            rb_selectGameMode(sender, e);
        }

        /// <summary>
        /// Event listener intercepting checked option status shifts on All-Tinkers DOSBox selection UI controller toggles.
        /// </summary>
        private void cb_WCAT_CheckedChanged(object sender, EventArgs e)
        {
            //If we're activating WCAT then disable KS - temporarily disable the event handler while we do that...
            if ((sender is System.Windows.Forms.CheckBox cbSender) && (cbSender.Checked == true))
            {
                try
                {
                    cb_WCKS.CheckedChanged -= cb_WCKS_CheckedChanged!;
                    cb_WCKS.Checked = false;
                }
                finally
                {
                    cb_WCKS.CheckedChanged += cb_WCKS_CheckedChanged!;
                }
            }
            rb_selectGameMode(sender, e);
        }

        /// <summary>
        /// Intercepts option changes across radio toggles and platform checkboxes, 
        /// dynamically routing active executable paths and mapping corresponding game engine modes.
        /// </summary>
        /// <param name="sender">The active user interface component (RadioButton or CheckBox) that triggered the state change.</param>
        /// <param name="e">Standard system event arguments containing baseline notification metadata context.</param>
        /// <remarks>
        /// This central method automatically stops running background scan engines and flushes active script data 
        /// configurations whenever a new game selection is made to prevent cross-contamination of pointers.
        /// </remarks>
        private void rb_selectGameMode(object sender, EventArgs e)
        {
            WingLoader_Worker.GameMode activeGameMode;
            bool processUpdates = false;

            if (sender is System.Windows.Forms.RadioButton rb && rb.Checked) //This awesome little feature casts the sender inline!
            {
                //Only update when activated (avoid double hit when moving radioButton)
                processUpdates = true;
            }
            else if (sender is System.Windows.Forms.CheckBox cbSender) //This awesome little feature casts the sender inline!
            {
                processUpdates = true;
                if (cbSender.Text == "Kilrathi Saga")
                {
                    if (cbSender.Checked)
                    {
                        //Update when WCKS box is checked...
                    }
                    else if (!cbSender.Checked)
                    {
                        //Update when WCKS box is un-checked...
                    }
                }
                if (cbSender.Text == "WCAT")
                {
                    if (cbSender.Checked)
                    {
                        //Update when WCAT box is checked...
                    }
                    else if (!cbSender.Checked)
                    {
                        //Update when WCAT box is un-checked...
                    }
                }
            }

            if (processUpdates)
            {
                // 3. Evaluate selected options and compile active directory targeting profiles
                if (rb_WC1.Checked)
                {
                    if (cb_WCKS.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.WC1KS;
                    }
                    else if (cb_WCAT.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.WC1AT;
                    }
                    else
                    {
                        activeGameMode = WingLoader_Worker.GameMode.WC1;
                    }
                }
                else if (rb_SM1.Checked)
                {
                    if (cb_WCKS.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SM1KS;
                    }
                    else if (cb_WCAT.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.WC1AT;
                    }
                    else
                    {
                        activeGameMode = WingLoader_Worker.GameMode.WC1;
                    }
                }
                else if (rb_SM2.Checked)
                {
                    if (cb_WCKS.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SM2KS;
                    }
                    else if (cb_WCAT.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SM2AT;
                    }
                    else
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SM2;
                    }
                }
                else if (rb_WC2.Checked)
                {
                    if (cb_WCKS.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.WC2KS;
                    }
                    else if (cb_WCAT.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.WC2AT;
                    }
                    else
                    {
                        activeGameMode = WingLoader_Worker.GameMode.WC2;
                    }
                }
                else if (rb_SO1.Checked)
                {
                    if (cb_WCKS.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SO1KS;
                    }
                    else if (cb_WCAT.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SO1AT;
                    }
                    else
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SO1;
                    }
                }
                else if (rb_SO2.Checked)
                {
                    if (cb_WCKS.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SO2KS;
                    }
                    else if (cb_WCAT.Checked)
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SO2AT;
                    }
                    else
                    {
                        activeGameMode = WingLoader_Worker.GameMode.SO2;
                    }
                }
                else
                {
                    activeGameMode = WingLoader_Worker.GameMode.DEFAULT;
                }

                //Trigger the updates in the main code:
                wl.selectGameMode(activeGameMode);
            }
        }

        /// <summary>
        /// Called before Dispose - we should shut down all the subthreads first to avoid any awkward thread-level exceptions.
        /// </summary>
        /// <param name="e">The event details container containing form termination data flags.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            func_log("Shutting down");
            wl.StopDebuggerThread();

            // Call the base class method
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Enable the debug options for users.
        /// </summary>
        private void activateDebugWindows()
        {
            btn_ExecuteDebugger.Visible = true;
            btn_ExecuteDebuggerAsync.Visible = true;
            btn_testDoSomething.Visible = true;
            btn_testDoSomething2.Visible = true;
            tb_Address.Visible = true;
            tb_string.Visible = true;
            tb_hexstring.Visible = true;
            tb_Hex.Visible = true;
            this.Size = new Size(1116, 579);
        }
    }
}
