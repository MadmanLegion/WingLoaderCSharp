using System.Diagnostics;

namespace Generic_Generics.Processes
{
    public class ProcessStarter
    {
        /// <summary>
        /// Start a process found at fully resolved location filename and run as normal user
        /// Assumes working directory is the filename path (if qualified), or the current App Path if the filename is for a local file
        /// </summary>
        /// <param name="fileName">Path to application</param>
        /// <param name="args">Parameters to pass to the application</param>
        /// <returns>Status message indicating success or failure</returns>
        protected string StartProcessAsAdmin(string fileName, string args="")
        {
            return StartProcess(fileName, args, true);
        }

        /// <summary>
        /// Start a process found at fully resolved location filename and run as normal user
        /// Assumes working directory is the filename path (if qualified), or the current App Path if the filename is for a local file
        /// </summary>
        /// <param name="fileName">Path to application</param>
        /// <param name="args">Parameters to pass to the application</param>
        /// <returns>Status message indicating success or failure</returns>
        protected string StartProcess(string fileName, string args = "")
        {
            return StartProcess(fileName, args, false);
        }

        /// <summary>
        /// Start a process found at fully resolved location filename and run as normal user
        /// Assumes working directory is the filename path (if qualified), or the current App Path if the filename is for a local file
        /// </summary>
        /// <param name="fileName">Path to application</param>
        /// <param name="args">Parameters to pass to the application</param>
        /// <returns>Status message indicating success or failure</returns>
        private string StartProcess(string fileName, string args, bool elevated)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName; // Path to the executable
            startInfo.Arguments = args;
            string workingDirectory = System.IO.Directory.GetCurrentDirectory();
            if (fileName.Contains("\\"))
            {
                workingDirectory = Path.GetDirectoryName(fileName)!;
            }
            startInfo.WorkingDirectory = workingDirectory;

            string elevatedMessage = "";
            if (elevated)
            {
                startInfo.UseShellExecute = true;   // Required for elevation
                startInfo.Verb = "runas";           // Triggers the UAC prompt
                elevatedMessage = " elevated";
            }
            try
            {
                Process? proc = Process.Start(startInfo);

                try
                {
                    return string.Format("Started{1} process with ProcID {0}", proc?.Id, elevatedMessage);
                }
                catch (InvalidOperationException ioex)
                {
                    return string.Format("InvalidOperationException starting{1} process: {0}", ioex.Message, elevatedMessage);
                }
                catch (Exception ex)
                {
                    return string.Format("Exception starting{1} process: {0}", ex.Message, elevatedMessage);
                }
            }
            catch (System.ComponentModel.Win32Exception wex)
            {
                // Occurs if the user clicks "No" on the UAC prompt, but can also occur if the file is not found.
                return string.Format("Unable to start{1} process: {0}", wex.Message, elevatedMessage);
            }
        }
    }
}
