namespace Generic_Generics.Natives
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Specifies the memory state used by Win32 memory management APIs 
    /// </summary>
    /// <remarks>
    /// These flags represent values commonly found in the <see cref="State"/> and <see cref="State"/> 
    /// fields of a Win32 <c>MEMORY_BASIC_INFORMATION</c> structural layout.
    /// </remarks>
    public enum MemoryState : uint
    {
        //Indicates committed memory pages for which physical storage has been allocated, either in RAM or inside the system paging file on disk.
        MEM_COMMIT = 0x1000, // 4096 - Allocations backed by storage
        
        // Indicates reserved address space pages where a range of the process's virtual address space is earmarked without any active physical storage allocations.
        MEM_RESERVE = 0x2000, // 8192 - Reserved address blocks
        
        // Indicates free pages that are currently inaccessible to the calling process and completely available to be allocated by the operating system.
        MEM_FREE = 0x10000, // 65536 - Empty, unallocated blocks
        
        // Indicates that the memory pages within the region are private (allocated exclusively for the process and not shared by other processes).
        MEM_PRIVATE = 0x20000, // 65536 - Empty, unallocated blocks

        //Custom End record used when handling VirtualQueryEx (I want a dummy record)
        MEM_END= 0xFFFFF
    }

    /// <summary>
    /// Specifies the type of memory allocationused by Win32 memory management APIs 
    /// </summary>
    /// <remarks>
    /// These flags represent values commonly found in the <see cref="State"/> and <see cref="Type"/> 
    /// fields of a Win32 <c>MEMORY_BASIC_INFORMATION</c> structural layout.
    /// </remarks>
    public enum MemoryType : uint
    {
        MEM_PRIVATE = 0x20000,  ///Private to the target process
        MEM_MAPPED = 0x40000,   //Mapped to a non-executable file on the disk
        MEM_IMAGE = 0x1000000,  //Mapped to an executable file on the disk
        //Custom End record used when handling VirtualQueryEx (I want a dummy record)
        MEM_END = 0xFFFFFFF
    }

    /// <summary>
    /// Indicates the protection status of the memory (can it be written to, read etc.)
    /// </summary>
    public enum MemoryProtection : uint
    {
        PAGE_NOACCESS = 0x01,   //Inaccessible
        PAGE_READONLY = 0x02,
        PAGE_READWRITE = 0x04,
        PAGE_WRITECOPY = 0x08,
        PAGE_EXECUTE = 0x10,
        PAGE_EXECUTE_READ = 0x20,
        PAGE_EXECUTE_READWRITE = 0x40,
        PAGE_EXECUTE_WRITECOPY = 0x80,
        PAGE_GUARD = 0x100,     //Inaccessible
        PAGE_NOCACHE = 0x200,
        PAGE_WRITECOMBINE = 0x400
    }

    /// <summary>
    /// Contains information about a range of pages in the virtual address space of a 32-bit process.
    /// Mapped directly from the native Win32 <c>MEMORY_BASIC_INFORMATION32</c> structure.
    /// </summary>
    /// <remarks>
    /// This structural layout is typically populated by calling native APIs like <c>VirtualQuery</c> or <c>VirtualQueryEx</c>.
    /// ensure <see cref="StructLayoutAttribute"/> with appropriate alignment packing is explicitly assigned if marshaling on 64-bit hosts.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_BASIC_INFORMATION32
    {
        // A pointer to the base address of the region of pages being evaluated.
        public IntPtr BaseAddress;

        // A pointer to the base address of a range of pages allocated by the <c>VirtualAlloc</c> function.
        public IntPtr AllocationBase;

        // The memory protection option specified when the region was initially allocated.
        // N.B. Matches the Win32 page protection constants (e.g., <c>PAGE_EXECUTE_READWRITE</c>).
        public MemoryProtection AllocationProtect;

        //private uint Alignment1;// 4-byte packing adjustment for 64-bit compilers

        // The total size of the memory region beginning at the base address, in bytes.
        public IntPtr RegionSize;

        // The current allocation state of the pages in the region.
        public MemoryState State;

        // The access protection of the pages in the region.
        // N.B. Represents the active runtime protection flags applied to the page block.
        public MemoryProtection Protect;

        // The type of pages inside the evaluated memory region.
        // N.B. Expected values include memory type constants such as <c>MEM_PRIVATE</c>, <c>MEM_MAPPED</c>, or <c>MEM_IMAGE</c>.
        public MemoryType Type;

        //private uint Alignment2;// Windows 10/11 64-bit memory tail alignment padding
    }

    /// <summary>
    /// Provides managed P/Invoke definitions for interacting with the native Windows Kernel32 subsystem.
    /// </summary>
    public static class Kernel32
    {
        // Required permission flag to read virtual memory in a process using <see cref="ReadProcessMemory"/>.
        public const uint PROCESS_VM_READ = 0x0010;

        // Required permission flag to retrieve basic process metadata using functions like <see cref="VirtualQueryEx"/>.
        public const uint PROCESS_QUERY_INFORMATION = 0x0400;

        /// <summary>
        /// Opens an existing local process object, returning an access handle to the process.
        /// </summary>
        /// <param name="dwDesiredAccess">The access rights requested for the target process (e.g., <see cref="PROCESS_VM_READ"/>).</param>
        /// <param name="bInheritHandle">If true, processes created by this process will inherit the handle.</param>
        /// <param name="dwProcessId">The unique operating system identifier of the target process to open.</param>
        /// <returns>A valid process access handle pointer, or <see cref="IntPtr.Zero"/> if the operation fails.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        /// <summary>
        /// Reads a block of physical memory data from a specified address range within a target process.
        /// </summary>
        /// <param name="hProcess">A valid handle to the process containing the memory to be read.</param>
        /// <param name="lpBaseAddress">The base memory address pointer to begin reading from inside the target process.</param>
        /// <param name="lpBuffer">The local application destination buffer array that receives the copied memory contents.</param>
        /// <param name="dwSize">The exact number of bytes to read from the target location address.</param>
        /// <param name="lpNumberOfBytesRead">Outputs the total number of bytes successfully transferred into the destination buffer.</param>
        /// <returns><c>true</c> if the memory copying operation succeeded; otherwise, <c>false</c>.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        /// <summary>
        /// Closes an open operating system object handle to release system kernel resources.
        /// </summary>
        /// <param name="hObject">A valid open handle pointer to an active system object.</param>
        /// <returns><c>true</c> if the handle was closed successfully; otherwise, <c>false</c>.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// Retrieves information about a range of pages within the virtual address space of a specified target process.
        /// </summary>
        /// <param name="hProcess">A handle to the process whose virtual memory page data is being queried.</param>
        /// <param name="lpAddress">A pointer to the base address of the region of pages to be queried.</param>
        /// <param name="lpBuffer">Outputs a populated <see cref="MEMORY_BASIC_INFORMATION32"/> structural layout details block.</param>
        /// <param name="dwLength">The structural memory size layout footprint bounds, in bytes.</param>
        /// <returns>The actual number of bytes returned in the information buffer block, or 0 on error.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern UIntPtr VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION32 lpBuffer, uint dwLength);
        
        /// <summary>
        /// Retrieves the File mapped to the memory address
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpv"></param>
        /// <param name="lpFilename"></param>
        /// <param name="nSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "K32GetMappedFileNameW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint GetMappedFileName(IntPtr hProcess, IntPtr lpAddress, [Out] byte[] lpFilename, uint nSize);
    }
}
