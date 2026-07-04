using System.IO.Compression;

namespace Generic_Generics.Processes
{
    public static class ZipHandler
    {
        /// <summary>
        /// Unzips the file to the specified folder (removes any root folder that might exist in the archive)
        /// Will not do anything if the folder already exists
        /// </summary>
        /// <param name="filename">filename of the zip file relative to the App working directory</param>
        /// <param name="subfolder">filename of the extract target folder relative to the App working directory</param>
        /// <param name="force">optional parameter to force extraction if the folder exists</param>
        /// <returns></returns>
        public static bool unzipFileToFolder(string filename, string subfolder, bool force=false)
        {
            if (!force)
            {
                if (FileHandler.checkSubFolderExists(subfolder))
                {
                    return true;
                }
            }
            string workingFolder = Directory.GetCurrentDirectory();
            string sourceFile = Path.Combine(workingFolder, filename);
            string outputPath = Path.Combine(workingFolder, subfolder);



            if (FileHandler.checkFileExists("", filename))
            {
                using (ZipArchive archive = ZipFile.OpenRead(sourceFile))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries) //each file/folder in the zip
                    {

                        if (string.IsNullOrEmpty(entry.Name))
                            continue;

                        string relativepath = entry.FullName;
                        int slash = relativepath.IndexOf('/');
                        if (slash >= 0)
                        { 
                            relativepath = entry.FullName.Substring(slash + 1);
                        }

                        if (string.IsNullOrEmpty(relativepath))
                            continue;

                        string destPath = Path.Combine(outputPath, relativepath);

                        FileHandler.checkSubFolderExists(Path.GetDirectoryName(destPath)!, true);

                        entry.ExtractToFile(destPath, overwrite: true);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}