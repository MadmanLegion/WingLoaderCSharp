using Generic_Generics.Logging;

namespace WingLoader_Generics.Debugger
{
    internal class Dummy_Debugger : Debugger
    {

        /// <summary>
        /// Cleanup any instantiated objects
        /// For Dummy this is nothing
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// The initial memory offset (in hex format as a string) from which all other memory addresses are based.  
        /// By default in DOS this is 0x00 and in Windows it's 0x400000
        /// This value cannot be set by outsiders, but may need to be read
        /// </summary>
        public override string initialOffset { get => "0x00"; }

        /// <summary>
        /// Retrieve an array of bytes from the debugger - starting at offset "memory" and for length "size"
        /// offset and size are given in hex format as a string
        /// </summary>
        /// <param name="memory">offset to start reading from in 0x00 string format</param>
        /// <param name="size">number of bytes to read in 0000 string format</param>
        /// <returns></returns>
        public override Byte[] getMemory_Sync(string memory, string size)
        {
            byte[] bytes = new byte[] { 0x00, 0x00, 0x00 }; ;
            try
            {
                if (DateTime.Now.Second % 10 < 5)
                {
                    bytes = WingLoader_Debugger.StringToBytes("This is a low test");
                }
                else
                {
                    bytes = WingLoader_Debugger.StringToBytes("This is a high test");
                }
            }
            catch (Exception e)
            {
                Debug_Logger.log(e.Message);
            }
            return bytes;
        }

        /// <summary>
        /// Retrieve an array of bytes from the debugger - starting at offset "memory" and for length "size"
        /// offset and size are given in hex format as a string
        /// </summary>
        /// <param name="memory">offset to start reading from in 0x00 string format</param>
        /// <param name="size">number of bytes to read in 0000 string format</param>
        /// <returns></returns>
        public override async Task<Byte[]> getMemory_ASync(string memory, string size)
        {
            byte[] bytes = new byte[] { 0x00, 0x00, 0x00 }; ;
            try
            {
                bytes = WingLoader_Debugger.StringToBytes("This is a test");
            }
            catch (Exception e)
            {
                Debug_Logger.log(e.Message);
            }
            return bytes;
        }
    }
}
