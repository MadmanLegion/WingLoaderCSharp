using Generic_Generics.Configurations;
using Generic_Generics.Processes;
using System;
using System.Collections.Generic;
using System.Text;
using WingLoader_Generics;

namespace WingLoader_NS
{
    public partial class WingLoader_Main
    {
        /// <summary> Flag indicating whether debug mode is enabled (enables extra UI functionality, configurable from the appsettings file. </summary>
        public bool DebugMode = false;

        /// <summary> Local logger instance used for quick logging. </summary>
        private WingLoader_GUILogger logger;

        /// <summary>Local MessageViewer instance used for routing updates back to the UI</summary>
        private MessageViewer mv;

        /// <summary>
        /// Initializes the <see cref="WingLoader_Main"/> class, 
        /// setup the logger and message viewer instances, and call the asynchronous startup.
        /// </summary>
        public WingLoader_Main(List<Action<string>> logfunctions, MessageViewer l_messages)
        {
            logger = new WingLoader_GUILogger(logfunctions);
            mv = l_messages;
            startup();
        }

        /// <summary>
        /// Handle any startup functionality for our main thread without needing the GUI to wait.
        /// </summary>
        private async void startup()
        {
            setDebugConfig();
            firstTimeSetup();
            await checkLatestCodeVersion("bekenn\\wcdx");
            await checkLatestCodeVersion("dosbox-staging\\dosbox-staging");
            resetGameStartOffset();
        }

        /// <summary>
        /// Unpack any archive files that are deployed but not extracted (part of my build process, but not required for a user).
        /// </summary>
        private void firstTimeSetup()
        {
            // Deploys DosBox
            ZipHandler.unzipFileToFolder("dosbox-staging-windows-x64-v0.83.0-RC1.zip", "Dosbox");
            ZipHandler.unzipFileToFolder("FFMPEG.zip", "FFMPEG");
            ZipHandler.unzipFileToFolder("Data.zip", "Data");
            ZipHandler.unzipFileToFolder("Wing.zip", "Wing");
            ZipHandler.unzipFileToFolder("WingAT.zip", "WingAT");
            ZipHandler.unzipFileToFolder("WingKS.zip", "WingKS");
            ZipHandler.unzipFileToFolder("Wing2.zip", "Wing2");
            ZipHandler.unzipFileToFolder("Wing2AT.zip", "Wing2AT");
            ZipHandler.unzipFileToFolder("Wing2KS.zip", "Wing2KS");
        }

        /// <summary>
        /// Sets up the debugMode in appsettings.json (defaults to false)
        /// </summary>
        private void setDebugConfig()
        {
            DebugMode = false;

            // read (or assign default) configuration value for each config setting and then save the instantiated value back to the config file
            DebugMode = string.Equals(ConfigurationHandler.getConfig("appsettings:debug"), "true", StringComparison.OrdinalIgnoreCase);
            ConfigurationHandler.writeConfig("appsettings:debug", DebugMode.ToString());
        }


        /// <summary>
        /// Sets up the given github project 'latest version' in appsettings.json - and logs a warning if it has changed (not expecting the user to do anything, but helpful information perhaps...)
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private async Task<bool> checkLatestCodeVersion(string project)
        {
            string currentUrl = await getURLofWebsite(project);
            string previousUrl = ConfigurationHandler.getConfig($"appsettings:{project}");
            if (string.Equals(currentUrl, previousUrl, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else
            {
                ConfigurationHandler.writeConfig($"appsettings:{project}", currentUrl);
                mv.messages = ($"Warning there may be a new version of the required project {project} at {currentUrl}");
                return true;
            }
        }

        /// <summary>
        /// Connect to a github project latest release page and returns the resolved URL (i.e. the current project release version).
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private async Task<string> getURLofWebsite(string project)
        {
            using var client = new HttpClient();
            string initialUrl = $"https://github.com/{project}/releases/latest";
            using HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, initialUrl));
            Uri finalUrl = response!.RequestMessage!.RequestUri!;
            return finalUrl.ToString();
        }
    }
}
