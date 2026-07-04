using Generic_Generics.Logging;
using System.ComponentModel;
using Generic_Generics.Configurations;

namespace WingLoader_Generics.Debugger
{
    /// <summary>
    /// Base class structure for Debuggers to inherit from
    /// </summary>
    public abstract class Debugger
    {

        protected int processId = -1;
        public int ProcessId { get => processId; }

        /// <summary>
        /// Cleanup any instantiated objects
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// The initial memory offset (in hex format as a string) from which all other memory addresses are based.  
        /// By default in DOS this is 0x00 and in Windows it's 0x400000
        /// This value cannot be set by outsiders, but may need to be read
        /// </summary>
        public abstract string initialOffset { get; }

        /// <summary>
        /// Retrieve an array of bytes from the debugger - starting at offset "memory" and for length "size"
        /// offset and size are given in hex format as a string
        /// </summary>
        /// <param name="memory">offset to start reading from in 0x00 string format</param>
        /// <param name="size">number of bytes to read in 0000 string format</param>
        /// <returns></returns>
        public abstract Byte[] getMemory_Sync(string memory, string size);

        /// <summary>
        /// Retrieve an array of bytes from the debugger - starting at offset "memory" and for length "size"
        /// offset and size are given in hex format as a string
        /// </summary>
        /// <param name="memory">offset to start reading from in 0x00 string format</param>
        /// <param name="size">number of bytes to read in 0000 string format</param>
        /// <returns></returns>
        public abstract Task<Byte[]> getMemory_ASync(string memory, string size);


        /// <summary>
        /// Find the running instance of the executableName provided
        /// and assign the processID variable based on that running instance
        /// Implemented by Win32_Debugger only
        /// </summary>
        public virtual int setProcessID(string executableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A logger to be used by a Debugger when needing to describe a Win32 Error code.
        /// If the application configuration is not set to debug=true then this function does nothing.
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="errorCode"></param>
        protected static void LogError(string apiName, int errorCode)
        {
            // Convert the numeric error code into a human-readable system message
            string errorMessage = new Win32Exception(errorCode).Message;

            // Log the structural details
            Debug_Logger.log($"[ERROR] {apiName} failed.");
            Debug_Logger.log($"Error Code: 0x{errorCode:X} ({errorCode})");
            Debug_Logger.log($"Description: {errorMessage}");
        }
    }
}
