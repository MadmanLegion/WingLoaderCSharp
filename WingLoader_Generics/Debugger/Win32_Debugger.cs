using Generic_Generics.Logging;
using Generic_Generics.Natives;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WingLoader_Generics.Debugger
{
    public class Win32_Debugger : Debugger
    {
        private IntPtr hProcess = IntPtr.Zero;

        /// <summary>
        /// Cleanup any instantiated objects
        /// For Win32 this is the handle to the process
        /// </summary>
        public override void Dispose()
        {
            Kernel32.CloseHandle(hProcess);
            hProcess = IntPtr.Zero;
            processId = -1;
        }

        /// <summary>
        /// The initial memory offset (in hex format as a string) from which all other memory addresses are based.  
        /// By default in DOS this is 0x00 and in Windows it's 0x400000
        /// This value cannot be set by outsiders, but may need to be read
        /// </summary>
        public override string initialOffset { get => "0x400000"; }

        /// <summary>
        /// Retrieve an array of bytes from the debugger - starting at offset "memory" and for length "size"
        /// offset and size are given in hex format as a string
        /// </summary>
        /// <param name="memory">offset to start reading from in 0x00 string format</param>
        /// <param name="size">number of bytes to read in 0000 string format</param>
        /// <returns></returns>
        public override Byte[] getMemory_Sync(string memory, string size)
        {
            byte[] buffer = new byte[] { 0x00, 0x00, 0x00 };
            try
            {
                if (processId == -1)
                {
                    Debug_Logger.log("No Process has been initialised");
                    return buffer;
                }

                if (hProcess == IntPtr.Zero)
                {
                    setupProcess();
                }

                if (setupProcess())
                {
                    buffer = readMemory(WingLoader_Debugger.hexStringToInt(memory), WingLoader_Debugger.hexStringToInt(size));
                }
            }
            catch (Exception e)
            {
                Debug_Logger.log(e.Message);
            }
            return buffer;
        }

        /// <summary>
        /// Retrieve an array of bytes from the debugger - starting at offset "memory" and for length "size"
        /// offset and size are given in hex format as a string
        /// </summary>
        /// <param name="memory">offset to start reading from in 0x00 string format</param>
        /// <param name="size">number of bytes to read in 0000 string format</param>
        /// <returns></returns>
        public override Task<Byte[]> getMemory_ASync(string memory = "0x0", string size = "100")
        {

            byte[] bytes = new byte[] { 0x00, 0x00, 0x00 }; ;
            try
            {
                bytes = getMemory_Sync(memory, size);
            }
            catch (Exception e)
            {
                Debug_Logger.log(e.Message);
            }
            return Task.FromResult(bytes);
        }

        /// <summary>
        /// Find the running instance of the executableName provided
        /// and assign the processID variable based on that running instance
        /// Implemented by Win32_Debugger only
        /// </summary>
        public override int setProcessID(string executableName)
        {
            processId = -1;
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                if (process.ProcessName == executableName)
                {
                    processId = process.Id;
                    break;
                }
            }
            //Debug_Logger.log(string.Format("Found process {0}:{1}", executableName, processId));
            return processId;
        }


        /// <summary>
        /// Attempts to open a native handle to the target process with read-only memory privileges.
        /// </summary>
        /// <returns><c>true</c> if the process handle was successfully acquired; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Captures and logs structural Win32 system errors using <see cref="Marshal.GetLastWin32Error"/> if the allocation fails.
        /// </remarks>
        private bool setupProcess()
        {
            hProcess = Kernel32.OpenProcess(Kernel32.PROCESS_QUERY_INFORMATION | Kernel32.PROCESS_VM_READ, false, processId);
            //hProcess = Kernel32.OpenProcess(Kernel32.PROCESS_VM_READ, false, processId);

            if (hProcess == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                string errorMessage = new Win32Exception(errorCode).Message;
                LogError("OpenProcess", errorCode);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads a raw byte array from a specific memory address pointer inside the opened process.
        /// </summary>
        /// <param name="offset">The memory address pointer inside the target process to begin reading from.</param>
        /// <param name="length">The exact total number of bytes to extract from the address block.</param>
        /// <returns>A byte array containing the read memory data, or a 3-byte empty footprint array on system failure.</returns>
        private byte[] readMemory(int offset, int length)
        {
            byte[] buffer = new byte[length];
            int numberOfBytesRead = 0;

            bool success = Kernel32.ReadProcessMemory(hProcess, offset, buffer, buffer.Length, out numberOfBytesRead);
            if (!success)
            {
                int errorCode = Marshal.GetLastWin32Error();
                LogError("ReadProcessMemory", errorCode);
                buffer = new byte[] { 0x00, 0x00, 0x00 };
            }
            return buffer;
        }
        
        /// <summary>
        /// Opens the target system process, extracts a 100-byte memory block from the specified address, and automatically handles clean-up on failure.
        /// </summary>
        /// <param name="offset">The target memory address pointer to query.</param>
        /// <param name="logger">An action callback delegate mechanism used to pass operational status tracing data logs.</param>
        /// <returns>The read memory structure data buffer array, or a 3-byte empty array block on error states.</returns>
        private byte[] TestDebugger(int offset, Action<string> logger)
        {
            int length = 100;
            byte[] buffer = new byte[length];
            if (setupProcess())
            {
                try
                {
                    buffer = readMemory(offset, length);
                    return buffer;
                }
                finally
                {
                    Dispose();
                }
            }
            else
            {
                logger("Failed to setup process handle. Returning fallback buffer.");
                buffer = new byte[] { 0x00, 0x00, 0x00 };
                return buffer;
            }
        }

        #region Win32MemorySpaceReview
        /// <summary>
        /// Structure to wrap the Memory information retrieved;
        /// </summary>
        public struct MemorySpace
        {
            public string BaseAddress;
            public int RegionSize;
            public MemoryState MemoryState;
            public MemoryType MemoryType;
            public string rawPath;
        }

        /// <summary>
        /// Log a header that can be used when printing the contents of the MemorySpace
        /// </summary>
        public static string printMemorySpaceHeader()
        {
            return $"{"Base Address",-16} | {"Region Size (Bytes)",-20} | {"State",-12} | {"Type",-12}";
        }

        /// <summary>
        /// Log a delimiter line to be used when printing the MemorySpace in a neat format
        /// </summary>
        public static string printMemorySpaceSeperator()
        {
            return new string('-', 55);
        }

        /// <summary>
        /// Log the contents of the MemorySpace in a neat format
        /// </summary>
        public static string printMemorySpace(MemorySpace ms)
        {
            return $"{ms.BaseAddress,-16} | {ms.RegionSize,-20} | {ms.MemoryState,-12} | {ms.MemoryType,-12} | {ms.rawPath}";
        }

        /// <summary>
        /// Iterate over the Process Memory and retrieve the allocated memory blocks into a list of MemorySpace structures
        /// </summary>
        /// <param name="func_log"></param>
        /// <returns></returns>
        public List<MemorySpace> ParseWin32MemoryAllocations(Action<string> func_log)
        {
            List<MemorySpace> memorySpaceResults = new List<MemorySpace>();
            if (hProcess == IntPtr.Zero)
            {
                func_log($"Win32 Debugger has not been connected - attach it first");
                return memorySpaceResults;
            }

            IntPtr currentAddress = 4194304; // "0x400000" - This is where the process space for Win32 apps starts.
            uint structSize = (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION32));

            func_log(printMemorySpaceHeader());

            // 2. Loop until VirtualQueryEx fails (reaches end of address space)
            while (Kernel32.VirtualQueryEx(hProcess, currentAddress, out MEMORY_BASIC_INFORMATION32 mbi, structSize) != UIntPtr.Zero)
            {
                MemorySpace memorySpaceItem = new MemorySpace();
                memorySpaceItem.BaseAddress = "0x" + mbi.BaseAddress.ToString("X12");
                memorySpaceItem.RegionSize = (int)mbi.RegionSize;
                memorySpaceItem.MemoryState = mbi.State;
                memorySpaceItem.MemoryType = mbi.Type;


                // Process the retrieved data (filtering strictly for active committed memory pages)
                if (mbi.State == MemoryState.MEM_COMMIT)
                {
                    if (mbi.Type == MemoryType.MEM_MAPPED || mbi.Type == MemoryType.MEM_IMAGE)
                    {
                        uint safe32BitAddress = (uint)(mbi.BaseAddress & 0xFFFFFFFFL);
                        //IntPtr queryAddress = new IntPtr(safe32BitAddress + 1);
                        byte[] pathBuffer = new byte[512];
                        uint charsCopied = Kernel32.GetMappedFileName(hProcess, mbi.BaseAddress, pathBuffer, (uint)pathBuffer.Length);
                        if (Marshal.GetLastWin32Error() != 1006 & Marshal.GetLastWin32Error() != 0) //Empty/Invalid file view or Anonymous PageFile sharing
                            func_log($"{Marshal.GetLastWin32Error()}" + Environment.NewLine);
                        if (charsCopied > 0)
                        {
                            memorySpaceItem.rawPath = System.Text.Encoding.Unicode.GetString(pathBuffer, 0, (int)charsCopied * 2);
                        }
                    }

                    func_log(printMemorySpace(memorySpaceItem));
                    memorySpaceResults.Add(memorySpaceItem);
                }

                // Increment pointer locations to point straight onto the next memory segment boundary
                // 4. Move to the next memory chunk
                // Use checked block to prevent overflow on 32-bit systems reaching the end of memory
                try
                {
                    uint nextAddress = (uint)(mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64());

                    if ((nextAddress < currentAddress)  //Overrun of process memory
                        || (nextAddress >= 0xFFFFFFFFU)) //End of 4GB memory space limit
                    {
                        //Print last line indicating where we got to (end of memory)
                        memorySpaceItem = new MemorySpace();
                        memorySpaceItem.BaseAddress = "0x" + mbi.BaseAddress.ToString("X12");
                        memorySpaceItem.RegionSize = 0;
                        memorySpaceItem.MemoryState = MemoryState.MEM_END;
                        memorySpaceItem.MemoryType = MemoryType.MEM_END;
                        func_log(printMemorySpace(memorySpaceItem));
                        break;
                    }
                    currentAddress = (IntPtr)nextAddress;
                }
                catch (OverflowException)
                {
                    break;
                }
            }

            if (Marshal.GetLastWin32Error() != 87 & Marshal.GetLastWin32Error() != 0) //ERROR_INVALID_PARAMETER returned at the end of the loop.
                func_log($"Returned Win32 Error: {Marshal.GetLastWin32Error()}");

            return memorySpaceResults;
        }

        /// <summary>
        /// Iterate over the memorySpace and break it into chunks starting with the magic string (jeff) used in the Kilrathi Saga
        /// </summary>
        /// <param name="func_log"></param>
        /// <param name="ms"></param>
        /// <returns></returns>
        public List<MemorySpace> parseMemorySpaceForSegments(Action<string> func_log, MemorySpace ms)
        {
            List<MemorySpace> memorySpaceResults = new List<MemorySpace>();
            byte[] buffer = getMemory_Sync(ms.BaseAddress, WingLoader_Debugger.intToHexString(ms.RegionSize));

            int baseOffset = WingLoader_Debugger.hexStringToInt(ms.BaseAddress);

            byte[] pattern = WingLoader_Debugger.StringToBytes("jeff");

            // Find all starting indices using Enumerable
            List<int> indices = Enumerable.Range(0, buffer.Length - pattern.Length + 1)
                .Where(i => buffer.Skip(i)
                .Take(pattern.Length)
                .SequenceEqual(pattern))
                .ToList();

            for (int i=0; i<indices.Count; i++)
            {
                int startIndex = indices[i];
                int endIndex = startIndex;
                if (i==indices.Count-1)
                    endIndex = ms.RegionSize;
                else 
                    endIndex = indices[i + 1];
                int length = endIndex - startIndex - 1;

                int currentOffset = baseOffset + startIndex;

                buffer = getMemory_Sync(WingLoader_Debugger.intToHexString(currentOffset), WingLoader_Debugger.intToHexString(10));

                ////Want to skip if everything is either AB (ALLOC_DEAD_BUFFER) or 00 (Null) or DD (DEAD/Freed by C++ heap)
                //if ((buffer[0] == (byte)0xAB || buffer[0] == (byte)0x00 || buffer[0] == (byte)0xDD)
                //                        && buffer.All(b => b == buffer[0]))
                //{ continue; }


                MemorySpace memorySpaceItem = new MemorySpace();
                memorySpaceItem.BaseAddress = "0x" + currentOffset.ToString("X12"); ;
                memorySpaceItem.RegionSize = length;
                memorySpaceItem.MemoryState = ms.MemoryState;
                memorySpaceItem.MemoryType = ms.MemoryType;
                memorySpaceItem.rawPath = "Game chunk";

                func_log(printMemorySpace(memorySpaceItem));

                memorySpaceResults.Add(memorySpaceItem);
            }
            return memorySpaceResults;
        }
        #endregion Win32MemorySpaceReview
    }
}
