using WingLoader.PollingDebugThread;

namespace WingLoader_NS
{
    public partial class WingLoader_Main
    {
        /// <summary>
        /// Flushes and blanks out all UI configuration textboxes by feeding an empty dataset into the logging pipeline.
        /// </summary>
        private void message_log()
        {
            message_log(new DebugEventObject());
        }

        /// <summary>
        /// Unpacks a messages object into the specific sub-assets for the UI.
        /// </summary>
        /// <param name="messages">The aggregate <see cref="DebugEventObject"/> object populated by the debugger loop.</param>
        /// <returns>A separated instance layout of the <see cref="DebugEventObject"/> dataset.</returns>
        private DebugEventObject message_log(DebugEventObject messages)
        {
            return message_log(messages.msg1, messages.msg2, messages.firstName, messages.surname, messages.system, messages.callsign, messages.year, messages.day, messages.logMessage);
        }

        /// <summary>
        /// Validates, filters, and applies elements into the UI textboxes while filtering out redundant loop frames and spaceflight radar noise.
        /// Note that in updating the MessageViewer fields - we trigger updates back to the UI.
        /// </summary>
        /// <param name="msg1">The active raw textual message string extracted from process buffer location 1.</param>
        /// <param name="msg2">The active raw textual message string extracted from process buffer location 2.</param>
        /// <param name="firstName">The pilot's parsed legal given first name data value string.</param>
        /// <param name="surname">The pilot's parsed legal family last name data value string.</param>
        /// <param name="system">The localized active narrative space galaxy or sector location string name.</param>
        /// <param name="callsign">The pilot's tactical combat flight identity nickname callsign string.</param>
        /// <param name="year">The calculated numerical in-game narrative calendar timeline tracker year.</param>
        /// <param name="day">The calculated numerical in-game narrative calendar timeline tracker day.</param>
        /// <param name="logMessage">Diagnostic operational alerts or errors captured during unmanaged sampling stages.</param>
        /// <returns>
        /// A sanitized <see cref="DebugEventObject"/> instance data pack where unchanged values or flight HUD telemetry 
        /// are set to empty strings (<c>""</c>) to prevent redundant script execution loops.
        /// </returns>
        private DebugEventObject message_log(string msg1, string msg2, string firstName, string surname, string system, string callsign, string year, string day, string logMessage)
        {
            //We're going to return this for use in script processing, but don't want to process every time we poll, only on updates, so blank the messages if they're unchanged.
            if (mv.message1 != msg1)
            {
                mv.message1 = msg1;
            }
            else
            {
                msg1 = "";
            }
            if (mv.message2 != msg2)
            {
                mv.message2 = msg2;
            }
            else
            {
                msg2 = "";
            }

            //During spaceflight WC1 uses tb_Message 1 for occasional updates to speed (e.g. 200) armour (e.g. 40) and distance to target (e.g. 43782 km or 816 m) - want to ignore these
            int dummy = 0;
            if (msg1.EndsWith(" m"))
            {
                msg1 = "";
            }
            else if (msg1.EndsWith(" km"))
                msg1 = "";
            else if (int.TryParse(msg1, out dummy))
            {
                msg1 = "";
            }

            // Bind values directly into active form layout components
            mv.firstName = firstName;
            mv.surName = surname;
            mv.system = system;
            mv.callSign = callsign;
            mv.year = year;
            mv.day = day;
            if (logMessage != "")
            {
                logger.log(logMessage);
            }
            return new DebugEventObject(msg1, msg2, firstName, surname, system, callsign, year, day, logMessage);
        }
    }
}
