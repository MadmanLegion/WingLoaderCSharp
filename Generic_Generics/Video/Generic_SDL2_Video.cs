using FFmpeg.AutoGen;

namespace Generic_Generics.Video
{

    internal class Generic_SDL2_Video
    {
        

        /// <summary>
        /// Configures the root path for the FFMPEG library binaries
        /// </summary>
        /// <param name="path">
        /// The directory path where FFMPEG resides.
        /// Pass 'local' to use the 'FFMPEG' folder inside the applications current working directory.
        /// </param>
        /// <exception cref="FileNotFoundException">Thrown when ffmpeg.exe is not found</exception>"
        internal static void setupFFMPEG(string path="local")
        {
            string rootPath; // @"S:\P4WS\Source\3rd Party Packages\ffmpeg"
            if (string.Equals(path, "local", StringComparison.OrdinalIgnoreCase))
            {
                string workingPath = Directory.GetCurrentDirectory();
                rootPath = Path.Combine(workingPath, "FFMPEG");
            }
            else
            {
                rootPath = path;
            }
            // Ensure the executable exists in the resolved path
            if (File.Exists(Path.Combine(rootPath, "ffmpeg.exe")))
            {
                ffmpeg.RootPath = rootPath;
            }
            else
            {
                throw new FileNotFoundException($"FFMPEG not found at {rootPath}");
            }
        }
    }
}