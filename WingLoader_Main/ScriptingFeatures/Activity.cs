using Generic_Generics.Audio;
using Generic_Generics.Video;

namespace WingLoader.Scripting
{
    /// <summary>
    /// Specifies the execution behaviors triggered when an associated evaluation rule resolves successfully.
    /// </summary>
    internal enum ActionTypes { playAudio, playVideo, log }

    /// <summary>
    /// Represents a single, response payload wrapper containing specific action types, 
    /// relative folders for data to use.
    /// </summary>
    internal class Activity
    {
        /// <summary>
        /// Parses a raw configuration keyword string into its corresponding strongly typed <see cref="ActionTypes"/> identifier.
        /// </summary>
        /// <param name="actionType">The unparsed text keyword identifying the targeted action strategy.</param>
        /// <returns>The matching <see cref="ActionTypes"/> flag value, falling back to <see cref="ActionTypes.log"/> if unmapped.</returns>
        private ActionTypes getActionType(string actionType)
        {
            switch (actionType.ToLower())
            {
                case "playaudio": return ActionTypes.playAudio;
                case "playvideo": return ActionTypes.playVideo;
                case "log": return ActionTypes.log;
                default: return ActionTypes.log;
            }
        }

        /// <summary> The assigned action to be taken. </summary>
        ActionTypes actionType;
        /// <summary> The relative asset filename or raw string message payload. </summary>
        string actionValue;
        /// <summary> The function delegate to be called (mapped from the actionType. </summary>
        Action<string> activityFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Activity"/> class by resolving an action keyword string parameter.
        /// </summary>
        /// <param name="actionType">The string identifier representing the action type.</param>
        /// <param name="actionValue">The path description or other action specific payload.</param>
        /// <param name="activityFunction">The function delegate assigned to this activity.</param>
        internal Activity(string ActionType, string ActionValue, Action<string> ActivityFunction)
        {
            actionType = getActionType(ActionType);
            actionValue = ActionValue;
            activityFunction = ActivityFunction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Activity"/> class using a pre-resolved, strongly-typed <see cref="ActionTypes"/> flag.
        /// </summary>
        /// <param name="actionType">The ActionType identifier representing the action type.</param>
        /// <param name="actionValue">The path description or other action specific payload.</param>
        /// <param name="activityFunction">The function delegate assigned to this activity.</param>
        private Activity(ActionTypes ActionType, string ActionValue, Action<string> ActivityFunction)
        {
            actionType = ActionType;
            actionValue = ActionValue;
            activityFunction = ActivityFunction;
        }

        /// <summary>
        /// A blank fallback action delegate used to handle empty execution requests.
        /// </summary>
        private static void NullAction(string value)
        {
        }

        /// <summary>
        /// Generates a standardized, inert <see cref="Activity"/> instance designed to prevent crash states on blank entries.
        /// </summary>
        /// <returns>An empty logger-style activity mapped to an inactive feedback routine handler.</returns>
        public static Activity NullActivity()
        {
            Action<string> action = NullAction;
            return new Activity("", "", action);
        }

        /// <summary>
        /// Dispatches the required function delegate based on the Activity type.
        /// </summary>
        public void Act()
        {
            string lActionValue;
            switch (actionType)
            {
                case ActionTypes.playAudio:
                    lActionValue = string.Format("{0}\\{1}", System.IO.Directory.GetCurrentDirectory(), actionValue.Replace("/", "\\")).Replace("\\\\", "\\");
                    Generic_SDL2_Audio.playAudio(lActionValue);
                    break;
                case ActionTypes.playVideo:
                    lActionValue = string.Format("{0}\\{1}", System.IO.Directory.GetCurrentDirectory(), actionValue.Replace("/", "\\")).Replace("\\\\", "\\");
                    SDL2_Video_Player.playVideo(lActionValue);
                    break;
                case ActionTypes.log:
                    activityFunction(actionValue);
                    break;
            }
        }
    }
}
