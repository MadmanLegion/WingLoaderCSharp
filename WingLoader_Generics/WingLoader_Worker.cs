using Generic_Generics.Processes;

namespace WingLoader_Generics
{
    /// <summary>
    /// Manages launching processes for various iterations of the Wing Commander franchise, 
    /// resolving execution folders, configurations, and administrative elevation privileges.
    /// </summary>
    public class WingLoader_Worker : ProcessStarter
    {
        /// <summary>
        /// Specifies the specific version, expansion pack, or platform port layout 
        /// of the game targeted for execution.
        /// </summary>
        public enum GameMode { DEFAULT
                , WC1, SM15, SM2, WC2, SO1, SO2
                , WC1AT, SM15AT, SM2AT, WC2AT, SO1AT, SO2AT
                , WC1KS, SM1KS, SM15KS, SM2KS, WC2KS, SO1KS, SO2KS
        }
        /// <summary>
        /// A lookup array matching game entries that require a DOS emulator wrapper to execute.
        /// </summary>
        public static GameMode[] gameModesDOS = { GameMode.WC1, GameMode.SM15, GameMode.SM2, GameMode.WC2, GameMode.SO1, GameMode.SO2
                , GameMode.WC1AT, GameMode.SM15AT, GameMode.SM2AT, GameMode.WC2AT, GameMode.SO1AT, GameMode.SO2AT };

        /// <summary>
        /// A lookup array matching game entries engineered to run natively inside modern Windows environments.
        /// </summary>
        public static GameMode[] gameModesWin = { GameMode.WC1KS, GameMode.SM1KS, GameMode.SM15KS, GameMode.SM2KS, GameMode.WC2KS, GameMode.SO1KS, GameMode.SO2KS };

        /// <summary>
        /// Orchestrates launching a specific game variant using administrative UAC elevated security credentials.
        /// </summary>
        /// <param name="mode">The target game edition variation to execute.</param>
        /// <returns>A string response payload detailing process operational tracing results.</returns>
        public string StartProcessAsAdmin(GameMode mode)
        {
            return StartProcessLevel(mode, true);
        }

        /// <summary>
        /// Orchestrates launching a specific game variant under standard user security context restrictions.
        /// </summary>
        /// <param name="mode">The target game edition variation to execute.</param>
        /// <returns>A string response payload detailing process operational tracing results.</returns>
        public string StartProcess(GameMode mode)
        {
            return StartProcessLevel(mode, false);
        }

        /// <summary>
        /// Normalizes shared launcher properties, maps configuration config args, and dispatches the execution request.
        /// </summary>
        /// <param name="mode">The target game edition variation to execute.</param>
        /// <param name="administrator">True if the task tracking wrapper requires full administrative privilege overrides.</param>
        /// <returns>A string confirmation containing the underlying process initialization tracing output status logs.</returns>
        private string StartProcessLevel(GameMode mode, bool Administrator)
        {
            string fileName = getGameExecutable(mode);
            string param = getGameParameter(mode);
            string args="";
            if (gameModesDOS.Contains(mode))
            {
                args = $"-conf {System.IO.Directory.GetCurrentDirectory()}\\GameConfigs\\Dosbox.conf ";
            }
            if (param != "")
            {
                args = args + $" -conf {System.IO.Directory.GetCurrentDirectory()}\\GameConfigs\\" + param;
            }
            if (Administrator)
            {
                return base.StartProcessAsAdmin(fileName, args);
            }
            else
            {
                return base.StartProcess(fileName, args);
            }
        }

        /// <summary>
        /// Resolves the file name string corresponding to the secondary emulator configuration file target.
        /// </summary>
        /// <param name="mode">The game context profile currently being initialized.</param>
        /// <returns>The string configuration filename, or an empty string if no sub-parameter mapping exists.</returns>
        private string getGameParameter(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.WC1:      return "WC1.conf";
                case GameMode.SM15:     return "WC1_SM15.conf";
                case GameMode.SM2:      return "WC1_SM2.conf";
                case GameMode.WC2:      return "WC2.conf";
                case GameMode.SO1:      return "WC2_SO1.conf";
                case GameMode.SO2:      return "WC2_SO2.conf";
                case GameMode.WC1AT:    return "WC1_WCAT.conf";
                case GameMode.SM15AT:   return "SM1_SM15_WCAT.conf";
                case GameMode.SM2AT:    return "WC1_SM2_WCAT.conf";
                case GameMode.WC2AT:    throw new Exception("WC2AT is not yet available"); //return "WC2_WCAT.conf";
                case GameMode.SO1AT:    throw new Exception("WC2AT is not yet available"); //return "WC2_SO1_WCAT.conf";
                case GameMode.SO2AT:    throw new Exception("WC2AT is not yet available"); //return "WC2_SO2_WCAT.conf";
                default:                return "";
            }
        }

        /// <summary>
        /// Evaluates local workspace environment appsettings configuration paths to pinpoint the absolute path location layout 
        /// of the executable or script loader.
        /// </summary>
        /// <param name="mode">The requested deployment mode engine parameter.</param>
        /// <returns>The combined path location pointing directly to the target operational executable asset.</returns>
        /// <exception cref="ArgumentException">Thrown if an unmapped or invalid game mode identifier index is processed.</exception>
        public static string getGameExecutable(GameMode mode)
        {
            if (gameModesDOS.Contains(mode))
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                    //ConfigurationHandler.getConfig("appsettings", "executableFolders:dosBoxFolder");
                return Path.Combine(path,"Dosbox","dosbox_with_debugger.exe");
            }
            else if (gameModesWin.Contains(mode))
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                    //ConfigurationHandler.getConfig("appsettings", "executableFolders:gameFolderRoot");
                string subPath = "";
                string executable = "";

                switch (mode) {
                    case GameMode.WC1KS: 
                        subPath = "WingKS";
                        executable = "wc_wcdx.exe"; 
                        break;
                    case GameMode.SM1KS: 
                        subPath = "WingKS";
                        executable = "sm1_wcdx.exe";
                        break;
                    case GameMode.SM15KS:
                        subPath = "WingKS";
                        executable = "WC1SM15Run.bat";
                        executable = "sm2_wcdx.exe";
                        break;
                    case GameMode.SM2KS:
                        subPath = "WingKS";
                        executable = "WC1SM2Run.bat";
                        executable = "sm2_wcdx.exe";
                        break;
                    case GameMode.WC2KS:
                        subPath = "Wing2KS";
                        executable = "wc2_wcdx.exe";
                        break;
                    case GameMode.SO1KS:
                        subPath = "Wing2KS";
                        executable = "so1_wcdx.exe";
                        break;
                    case GameMode.SO2KS:
                        subPath = "Wing2KS";
                        executable = "so2_wcdx.exe";
                        break;
                }
                return Path.Combine(path, subPath, executable);
            }
            else
            {
                throw new ArgumentException(string.Format("Invalid Game Mode: {0}", mode));
            }
        }
    }
}
