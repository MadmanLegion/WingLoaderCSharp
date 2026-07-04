namespace Generic_Generics.Processes
{
    internal static class FileHandler
    {
        /// <summary>
        /// Checks if a subFolder of the app working directory exists
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        public static bool checkFileExists(string subFolder, string filename)
        {
            string workingFolder = Directory.GetCurrentDirectory();
            string file = Path.Combine(workingFolder, subFolder, filename);
            if (File.Exists(file))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if a subFolder of the app working directory exists
        /// </summary>
        /// <param name="subFolder">folder to check existence of </param>
        /// <param name="create">optional boolean to create the folder if required (defaults to false)</param>
        /// <returns></returns>
        public static bool checkSubFolderExists(string subFolder, bool create=false)
        {
            string workingFolder = Directory.GetCurrentDirectory();
            string folder = Path.Combine(workingFolder, subFolder);
            if (Directory.Exists(folder))
                return true;
            else
                if (create)
                {
                    Directory.CreateDirectory(folder);
                    return true;
                }
                return false;
        }
    }
}
