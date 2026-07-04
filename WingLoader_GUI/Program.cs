using System.Runtime.Loader;

namespace WingLoader_GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main runtime entry point responsible for initializing system configurations and launching the core user interface thread.
        /// </summary>
        /// <remarks>
        /// The <see cref="STAThreadAttribute"/> marks the primary thread model as Single-Threaded Apartment, 
        /// which is a mandatory architecture requirement for Windows Forms to safely handle native UI components, 
        /// clipboard mechanics, and OS file dialogue windows.
        /// </remarks>
        [STAThread]
        static void Main()
        {

            // Setup an alternate subscription to resolve missing assemblies from the libs subfolder (making our root path slightly tidier...)
            AssemblyLoadContext.Default.Resolving += (context, assemblyName) =>
            {
                // Define your subfolder path relative to the app base directory
                string assemblyPath = Path.Combine(AppContext.BaseDirectory, "libs", $"{assemblyName.Name}.dll");
                if (File.Exists(assemblyPath))
                {
                    return context.LoadFromAssemblyPath(assemblyPath);
                }
                return null;
            };
            


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //Add an unhandled exception handler here - so that we can nicely catch any background thread exceptions and log to file on the way down.
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Instantiates the primary form class and attaches it as the main UI loop window context anchor
            Application.Run(new WingLoader_GUI_Form());
        }


        

        /// <summary>
        /// Handle UI Thread exceptions and log to file and show a popup when something bad happens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Form_UIThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Log the exception details safely here
            LogError(e.Exception, "A UI error occurred:");
            MessageBox.Show($"A UI error occurred: {e.Exception.Message}");
        }

        /// <summary>
        /// Handle other exceptions and log to file and show a popup when something bad happens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log non-UI crashes (The app will terminate immediately after this block)
            Exception ex = (Exception)e.ExceptionObject;
            LogError(ex, "A Fatal error occurred:");
            System.Diagnostics.Debug.WriteLine($"Fatal Background Error: {ex.Message}");
        }

        //Helper method to log any unhandlded exceptions while crashing to desktop.
        static void LogError(Exception ex, string message)
        {
            string workingDirectory = System.IO.Directory.GetCurrentDirectory();
            string filename = string.Format("{0}-{1}", AppDomain.CurrentDomain.FriendlyName, "debug.log");
            string logFile = System.IO.Path.Combine(workingDirectory, filename);
            string logMessage = string.Format("{0} - {1} : {2}{3}", message, ex.Message, Environment.NewLine, ex.StackTrace);
            using (StreamWriter writer = new StreamWriter(logFile, true))
            {
                writer.WriteLine(logMessage);
            }
        }
    }
}