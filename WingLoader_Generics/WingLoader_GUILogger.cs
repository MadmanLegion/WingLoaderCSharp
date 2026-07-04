using Generic_Generics.Logging;

namespace WingLoader_Generics
{
    public class WingLoader_GUILogger : Generic_Logger
    {
        /// <summary>
        /// Constructor and invoke the base constructor
        /// </summary>
        /// <param name="providedFunc"></param>
        public WingLoader_GUILogger(List<Action<string>> providedFunc) : base(providedFunc) { }

        /// <summary>
        /// GUI specific logging behaviour
        /// </summary>
        /// <param name="message"></param>
        public new void log(string? message)
        {
            if (message is not null)
            {
                message = FormatMessage(message);
                base.log(message);
            }
            
        }
    }
}
