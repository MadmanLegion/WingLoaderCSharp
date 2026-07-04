using System.Data;
using static WingLoader.Scripting.Checks;

namespace WingLoader.Scripting
{
    internal static class RulesHelper
    {
        
        /// <summary>
        /// Deconstructs a pipe-separated script line string entry into a strongly typed, structured <see cref="WingLoaderRule"/> block object.
        /// </summary>
        /// <param name="rulestring">The raw text string line extracted from the tracking script database file.</param>
        /// <returns>A fully populated <see cref="WingLoaderRule"/> action container entity, or a <see cref="WingLoaderRule.NullRule"/> if layout limits are missing segments.</returns>
        private static WingLoaderRule parseRuleString(string rulestring, Action<string> func_log)
        {
            if (string.IsNullOrEmpty(rulestring)) return WingLoaderRule.NullRule();

            string[] ruleDetails = rulestring.Split("|");
            WingLoaderRule rule;

            if (ruleDetails.Length < 6)
            {
                //logger.log(string.Format("Invalid ruleString: {0}", rulestring));
                rule = WingLoaderRule.NullRule();
            }
            else
            {
                string address = ruleDetails[0];
                CheckTypes checktype = getCheckType(ruleDetails[1]);
                string ruleValue = ruleDetails[2];

                // Converts literal escaped characters text into actual unmanaged carriage line break formatting tokens
                if (ruleValue.Contains("\\n"))
                {
                    ruleValue = ruleValue.Replace("\\n", "\n");
                }
                
                string actionType = ruleDetails[3];
                string actionValue = ruleDetails[4];
                
                rule = new WingLoaderRule(
                   address,
                   checktype,
                   ruleValue,
                   new Activity(
                       actionType, 
                       actionValue,
                       func_log //ActionFunction (pass through for logger)
                       )
                   );
            }
            return rule;
        }


        /// <summary>
        /// Threaded read from file and call out to parseRuleString for lines that can be parsed.
        /// Leverages Parallel LINQ (PLINQ) pipelines to concurrently scan, filter, and parse dialogue script flat files 
        /// from disk without clogging or stuttering primary thread workflows.
        /// </summary>
        /// <param name="filename">The absolute path tracking where the local textual script database files reside on disk.</param>
        /// <remarks>
        /// Utilizes <see cref="ParallelEnumerable.AsOrdered"/> constraints to ensure the background processing pipeline 
        /// preserves the identical layout sequencing arrangement found inside the source dialogue script files.
        /// </remarks>
        public static List<WingLoaderRule> readRulesFromFile(string filename, Action<string> func_log)
        {
            if (!File.Exists(filename))
            {
                func_log($"Script file error: Target tracking path is missing -> {filename}");
                return new List<WingLoaderRule>();
            }

            List<WingLoaderRule> loadedRules = File.ReadLines(filename)
                .AsParallel()
                .AsOrdered() // Mandatory to protect original priority sorting sequences
                .Where(line => !(line =="") && !line.StartsWith("//") && line.Split("|").Length>=6)
                .Select(line => parseRuleString(line, func_log))
                .ToList();

            func_log(string.Format("Loaded {0} rules", loadedRules.Count));

            return loadedRules;
        }

        /// <summary>
        /// Returns an empty array of WingLoaderRules
        /// </summary>
        public static List<WingLoaderRule> resetRules()
        {
            return new List<WingLoaderRule>();
        }

        /// <summary>
        /// Remove various unwanted messages to avoid UI updates (and rule testing) that are not helpful
        /// </summary>
        public static string stripUnwantedMessage(string message)
        {
            if (message == ":False")
                return "";
            else if (message == "v:False")
                return "";
            else if (message == "Check pilot scores:False")
                return "";
            else if (message == "Fly training mission:False")
                return "";
            else if (message == "Check pilot scores:False")
                return "";
            else if (message.StartsWith("Talk to ") && message.EndsWith(":False"))
                return "";
            else if (message == " :False")
                return "";
            else if (message == "Enter barracks:False")
                return "";
            else if (message.StartsWith("Awaken ") && message.EndsWith(":False"))
                return "";
            else if (message == "Mission Hangar:False")
                return "";
            else if (message == "View your medals:False")
                return "";
            else if (message == "Return to the Bar:False")
                return "";
            else if (message == "Check poster out:False")
                return "";
            else if (message == "Quit Wing Commander:False")
                return "";
            else if (message == "Save this campaign  :False")
                return "";
            else if (message == "Load a game first.:False")
                return "";
            else if (message == "Loading Game...:False")
                return "";
            else return message;
        }
    }
}