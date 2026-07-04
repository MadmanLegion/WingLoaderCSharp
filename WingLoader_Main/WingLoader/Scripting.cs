using Generic_Generics.Audio;
using WingLoader.PollingDebugThread;
using WingLoader.Scripting;
using WingLoader_Generics;
using static WingLoader.Scripting.Checks;

namespace WingLoader_NS
{
    public partial class WingLoader_Main
    {
        /// <summary> Local Rules cache used for the active game logic. </summary>
        private List<WingLoaderRule> rules = new List<WingLoaderRule>();

        /// <summary>
        /// Determine the right script file to load into Rules based on the active game mode
        /// </summary>
        /// <param name="mode">The active game identifier.</param>
        private void setRules(WingLoader_Worker.GameMode mode)
        {
            if (rules is null)
            {
                RulesHelper.resetRules();
            }

            if (rules?.Count == 0)
            {
                logger.log("Setting rules for mode");
                string scriptFile = "";

                // Maps the appropriate dialogue speech script file based on game/expansion variant
                switch (mode)
                {
                    case WingLoader_Worker.GameMode.WC1AT:  scriptFile = "ScriptWC1.txt"; break;
                    case WingLoader_Worker.GameMode.WC1:    scriptFile = "ScriptWC1.txt"; break;
                    case WingLoader_Worker.GameMode.WC1KS:  scriptFile = "ScriptWC1.txt"; break;

                    case WingLoader_Worker.GameMode.SM2AT:  scriptFile = "ScriptSM2.txt"; break;
                    case WingLoader_Worker.GameMode.SM2:    scriptFile = "ScriptSM2.txt"; break;
                    case WingLoader_Worker.GameMode.SM2KS:  scriptFile = "ScriptSM2.txt"; break;

                    case WingLoader_Worker.GameMode.WC2AT:  scriptFile = "ScriptWC2.txt"; break;
                    case WingLoader_Worker.GameMode.WC2:    scriptFile = "ScriptWC2.txt"; break;
                    case WingLoader_Worker.GameMode.WC2KS:  scriptFile = "ScriptWC2.txt"; break;

                    case WingLoader_Worker.GameMode.SO1AT:  scriptFile = "ScriptSO1.txt"; break;
                    case WingLoader_Worker.GameMode.SO1:    scriptFile = "ScriptSO1.txt"; break;
                    case WingLoader_Worker.GameMode.SO1KS:  scriptFile = "ScriptSO1.txt"; break;

                    case WingLoader_Worker.GameMode.SO2AT:  scriptFile = "ScriptSO2.txt"; break;
                    case WingLoader_Worker.GameMode.SO2:    scriptFile = "ScriptSO2.txt"; break;
                    case WingLoader_Worker.GameMode.SO2KS:  scriptFile = "ScriptSO2.txt"; break;
                }
                if (scriptFile != "")
                {
                    scriptFile = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "GameScripts", scriptFile);
                    rules = RulesHelper.readRulesFromFile(scriptFile, logger.first_log_func());
                }
                else
                {
                    logger.log("Invalid scriptFile - gameMode may be wrong");
                }
            }
            //rules.Add(new Rule("tb_Message2", CheckTypes.match, "Company...", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funpc0009.wav", func_log)));
            //rules.Add(new Rule("tb_Message2", CheckTypes.match, "Atten-SHUN!", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funmi1.wav", func_log)));
            //rules.Add(new Rule("tb_Message2", CheckTypes.match, "Prepare arms!", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funmi2.wav", func_log)));
            //rules.Add(new Rule("tb_Message2", CheckTypes.match, "Fire!", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funpc0011.wav", func_log)));
            //rules.Add(new Rule("tb_Message1", CheckTypes.match, "We are gathered here to pay tribute to one of our own, 2ND LT. ${[String.Name]}.", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funpc0000.wav", func_log)));
            //rules.Add(new Rule("tb_Message1", CheckTypes.match, "We are gathered here to pay tribute to one of our own, 1ST LT. ${[String.Name]}.", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funpc0001.wav", func_log)));
            //rules.Add(new Rule("tb_Message1", CheckTypes.match, "We are gathered here to pay tribute to one of our own, CAPTAIN ${[String.Name]}.", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funpc0002.wav", func_log)));
            //rules.Add(new Rule("tb_Message1", CheckTypes.match, "We are gathered here to pay tribute to one of our own, MAJOR ${[String.Name]}.", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funpc0003.wav", func_log)));
            //rules.Add(new Rule("tb_Message1", CheckTypes.match, "We are gathered here to pay tribute to one of our own, LT. COL. ${[String.Name]}.", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funpc0004.wav", func_log)));
            //rules.Add(new Rule("tb_Message1", CheckTypes.match, "We are gathered here to pay tribute to one of our own, COL. ${[String.Name]}.", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funpc0005.wav", func_log)));
            //rules.Add(new Rule("tb_Message1", CheckTypes.match, "It is always sad to lose a pilot...", new Activity(ActionTypes.playAudio, "Data/Voice/Funeral/funpc0006.wav", func_log)));
        }

        /// <summary>
        /// Unpacks the messages EventObject into the seperate strings for execRules to act upon them.
        /// </summary>
        /// <param name="messages">The un-sanitized real-time process metadata argument bundle wrapper.</param>
        private void execRules(DebugEventObject messages)
        {
            execRules(messages.msg1, messages.msg2, messages.firstName, messages.surname, messages.system, messages.callsign, messages.year, messages.day);
        }

        /// <summary>
        /// Sweeps through the current active set of rules, applies any substitutions (e.g. for callsign), 
        /// Tests the current messages against the rule
        /// and fires off the configured action.
        /// </summary>
        /// <param name="tb_Message1">The sanitized subtitle message string from address slot 1.</param>
        /// <param name="tb_Message2">The sanitized subtitle message string from address slot 2.</param>
        /// <param name="tb_Firstname">The active player character given first name (WC2 only).</param>
        /// <param name="tb_Surname">The active player character last name.</param>
        /// <param name="tb_System">The active system (WC1 only).</param>
        /// <param name="tb_Callsign">The active player character callsign .</param>
        /// <param name="tb_Year">The in-game year identifier.</param>
        /// <param name="tb_Day">The in-game day identifier.</param>
        private void execRules(string tb_Message1, string tb_Message2, string tb_Firstname, string tb_Surname, string tb_System, string tb_Callsign, string tb_Year, string tb_Day)
        {
            if ((tb_Message1 == "") && (tb_Message2 == ""))
            {
                return;
            }
            else
            {
                bool handled = false;
                foreach (WingLoader.Scripting.WingLoaderRule rule in rules)
                {
                    string testvalue = "";

                    // 1. Identify which memory lane slot this rule targets
                    if (rule.memAddress == "tb_Message1")
                        testvalue = tb_Message1;
                    else if (rule.memAddress == "tb_Message2")
                        testvalue = tb_Message2;
                    else
                        continue; //Invalid rule

                    // 2. Append profile context variables if rule calls for structural string replacement tokens
                    if (rule.memCheckType == CheckTypes.matchWithCallsign)
                        testvalue = testvalue + "|" + tb_Callsign;
                    else if (rule.memCheckType == CheckTypes.matchWithSurname)
                        testvalue = testvalue + "|" + tb_Surname;
                    else if (rule.memCheckType == CheckTypes.matchWithFirstname)
                        testvalue = testvalue + "|" + tb_Firstname;

                    //Stop any running audio first... this also ensures the speech stops when the conversation ends.
                    Generic_SDL2_Audio.stopAudioPlayback();
                    
                    if (!string.IsNullOrEmpty(testvalue))
                    {
                        // 3. get the appropriate rule testing operator
                        Func<string, string, bool> operation = getOperation(rule.memCheckType);

                        if (operation != null)
                        {
                            // 4. Compute algorithm check; execute side effects and break loop execution on success
                            if (operation(testvalue, rule.memValue))
                            {
                                //Currently no way to define multiple activities in the script file, but handle the possibility all the same.
                                foreach (WingLoader.Scripting.Activity activity in rule.activities)
                                {
                                    activity.Act();
                                }
                                handled = true;
                                break;
                            }
                        }
                    }
                }

                // 5. Output unhandled dialogue subtitles to secondary diagnostic buffers for trace profiling
                if (!handled)
                {
                    tb_Message1 = RulesHelper.stripUnwantedMessage(tb_Message1);
                    RulesDebugLogger(string.Format("{0}:{1}", tb_Message1, handled.ToString()));
                    if (tb_Message2 != tb_Message1)
                    {
                        tb_Message2 = RulesHelper.stripUnwantedMessage(tb_Message2);
                        RulesDebugLogger(string.Format("{0}:{1}", tb_Message1, handled.ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// Logger for unhandled messages (rule is not present, but message is considered to be 'important'
        /// Items that are dumped here should either be added to the script with appropriate rules, or handled in the cleaning methods
        /// (WingLoader_Main.message_log and RulesHelper.readRulesFromFile
        /// </summary>
        /// <param name="message">The message to log.</param>
        private void RulesDebugLogger(string message)
        {
            if (message != "")
            {
                using (System.IO.StreamWriter writer = new StreamWriter(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "DebugRules.log"), true))
                {
                    writer.WriteLine(message);
                }
            }
        }
    }
}