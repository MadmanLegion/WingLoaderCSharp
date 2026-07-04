namespace WingLoader.PollingDebugThread
{
    /// <summary>
    /// Encapsulates all real-time process memory values, telemetry states, and logging indicators 
    /// parsed by the background debugger engine to pass along to user interface listeners.
    /// </summary>
    internal class DebugEventObject
    {
        public string msg1 = "";
        public string msg2 = "";
        public string firstName = "";
        public string surname = "";
        public string system = "";
        public string callsign = "";
        public string year = "";
        public string day = "";
        public string logMessage = "";
        public bool threadShouldStop = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugEventObject"/> class using dummy values.
        /// </summary>
        internal DebugEventObject() : this("", "", "", "", "", "", "", "", "") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugEventObject"/> class with explicit values.
        /// </summary>
        internal DebugEventObject(string Msg1, string Msg2, string FirstName, string Surname, string System, string Callsign, string Year, string Day, string LogMessage)
        {
            msg1 = Msg1;
            msg2 = Msg2;
            firstName = FirstName;
            surname = Surname;
            system = System;
            callsign = Callsign;
            year = Year;
            day = Day;
            logMessage = LogMessage;
        }
    }
}
