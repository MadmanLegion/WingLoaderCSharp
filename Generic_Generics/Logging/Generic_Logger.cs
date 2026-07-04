namespace Generic_Generics.Logging
{
    public class Generic_Logger
    {
        /// <summary>
        /// An action that takes a string parameter and returns nothing (use Func to return something - last parameter is output)
        /// </summary>
        private List<Action<string>> targetfunction;

        /// <summary>
        /// Constructor saves the provided logging function to the local variable
        /// </summary>
        /// <param name="providedFunc"></param>
        public Generic_Logger(List<Action<string>> providedFunc)
        {
            targetfunction = new List<Action<string>>(providedFunc);
        }

        /// <summary>
        /// Allow a caller to retrieve the logging function, not entirely safe (e.g. outside of main UI threads) but handy.
        /// </summary>
        /// <returns>An Action<string> representing the primary log function of this logger.</string></returns>
        public Action<string> first_log_func()
        {
            return targetfunction[0];
        }

        /// <summary>
        /// Prepend the message with the time
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected string FormatMessage(string message)
        {
            message = string.Format("{0}: {1}", DateTime.Now.ToString("HH:mm:ss"), message);
            if (!message.EndsWith(Environment.NewLine))
            {
                message += Environment.NewLine;
            }
            return message;
        }

        /// <summary>
        /// invoke the logging function to write the message to the log.
        /// </summary>
        /// <param name="message"></param>
        protected void log(string message)
        {
            foreach (Action<string> function in targetfunction)
            {
                function(message);
            }
        }
    }
}
