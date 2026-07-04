using System.Text;
using WingLoader_Generics.Debugger;

namespace WingLoader_Generics
{
    /// <summary>
    /// Different types of Debuggers that can be invoked - all must inherit from Debugger.cs, and should be defined in Debugger folder.
    /// </summary>
    public enum DebuggerTypes { Dummy, Dosbox, Win32 }

    /// <summary>
    /// Helper class for creating the required debugger and handling data conversions.
    /// </summary>
    public static class WingLoader_Debugger
    {
        /// <summary>
        /// Generic Constructor that will return a debugger of the specified DebuggerType.
        /// </summary>
        /// <param name="type">A member of DebuggerTypes e.g. Dummy, Dosbox, Win32 etc.</param>
        /// <returns>The created Debugger instance</returns>
        /// <exception cref="NotImplementedException">Returned when the requested debuggerType has not been implemented</exception>
        public static Debugger.Debugger Create(DebuggerTypes type)
        {
            switch (type)
            {
                case DebuggerTypes.Dummy: return new Dummy_Debugger();
                case DebuggerTypes.Dosbox: return new DosBox_Debugger();
                case DebuggerTypes.Win32: return new Win32_Debugger();
                default: throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Convert an array of bytes into a String
        /// </summary>
        public static string BytesToString(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Convert a string into an array of bytes
        /// </summary>
        public static byte[] StringToBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        /// <summary>
        /// Convert an array of bytes into a String indicating the hex in form "FF FF"
        /// If toBigEndian is set to true and this is a littleEndian system then it will reverse the array (convert to BigEndian).
        /// </summary>
        public static string BytesToHexString(byte[] bytes, bool toBigEndian=false)
        {
            if (toBigEndian & BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return string.Join(" ", bytes.Select(b => b.ToString("X2")));
        }

        /// <summary>
        /// Convert the first item in an array of bytes into an 8-bit integer
        /// </summary>
        public static string BytesToInt8(byte[] bytes)
        {
            return bytes[0].ToString();
        }

        /// <summary>
        /// Convert the first two items in an array of bytes into a 16-bit integer
        /// </summary>        
        public static string BytesToInt16(byte[] bytes)
        {
            return System.BitConverter.ToInt16(bytes[..2]).ToString();
        }

        /// <summary>
        /// Find the first instance in an array of bytes of the bytes representing a given string
        /// </summary>
        /// <param name="data">array of bytes to parse</param>
        /// <param name="lookupString">string, whose byte-wise representation is to be found</param>
        /// <returns>integer offset in the array of the desired string</returns>
        public static int findStringInBytes(byte[] data, string lookupString)
        {
            return data.IndexOf(Encoding.UTF8.GetBytes(lookupString));
        }

        /// <summary>
        /// convert an integer into a hex string (15 => 0xF)
        /// </summary>
        public static string intToHexString(int input)
        {
            return "0x" + input.ToString("X");
        }

        /// <summary>
        /// convert a hex string into an integer (0xF => 15)
        /// </summary>

        public static int hexStringToInt(string input)
        {
            return Convert.ToInt32(input, 16);
        }

        /// <summary>
        /// sum two hex-strings and return as a hext string (0xF + 0xF = 0x1E)
        /// </summary>
        public static string addHexStrings(string baseVal, string addVal)
        {
            int baseInt = hexStringToInt(baseVal);
            int addInt = hexStringToInt(addVal);
            return intToHexString(baseInt + addInt);
        }

        /// <summary>
        /// subtract one hex-string from another and return as a hext string (0x1E - 0xF = 0xF)
        /// </summary>
        public static string subtractHexStrings(string baseVal, string subtractVal)
        {
            int baseInt = hexStringToInt(baseVal);
            int subtractInt = hexStringToInt(subtractVal);
            return intToHexString(baseInt - subtractInt);
        }

        /// <summary>
        /// Check if the contents of two byte arrays are identical (iterate over each byte and check it's the same)
        /// </summary>
        public static bool byteArraysEqual(byte[] a, byte[] b)
        {
            int i;
            if (a.Length == b.Length)
            {
                i = 0;
                while (i < a.Length && (a[i] == b[i]))
                {
                    i++;
                }
                if (i == a.Length)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if every item in a byte array is null (0x00)
        /// </summary>
        public static bool IsByteArrayNull(byte[] buffer)
        {
            byte terminator = 00;
            bool returnstate = true;
            foreach (byte b in buffer)
            {
                if (b != terminator)
                {
                    returnstate = false;
                }
            }
            return returnstate;
        }

        /// <summary>
        /// Trims the byte array to either the fixed length, or the first null byte (for string parsing)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fixedLength"></param>
        /// <returns></returns>
        public static byte[] trimFirstElementFromBytes(Byte[] bytes, int fixedLength = -1)
        {
            int length = fixedLength;
            if (fixedLength == -1)
            {
                length = 100;
            }

            if (fixedLength == -1)
            {
                try
                {
                    byte terminator = 00;
                    bytes = bytes[..Array.IndexOf(bytes, terminator)];
                }
                catch
                { }
                return bytes;
            }
            else
            {
                try
                {
                    bytes = bytes[..length];
                }
                catch
                { }
                return bytes;
            }

            
        }

        /// <summary>
        /// Slices out a specific chunk of memory from a raw data snapshot using a hexadecimal string address offset.
        /// </summary>
        /// <param name="data">The source unmanaged process memory byte array snapshot buffer.</param>
        /// <param name="hexAddress">The target memory address string formatted in base-16 hexadecimal (e.g., '0x1D7A2').</param>
        /// <param name="length">The size footprint window bounds, in bytes, to slice out from the offset anchor.</param>
        /// <returns>
        /// A new byte array slice containing the extracted memory range; 
        /// otherwise, a 2-byte empty array safety placeholder (<c>0x00, 0x00</c>) if the address is blank.
        /// </returns>
        public static Byte[] ReadMemoryBlock(Byte[] data, string hexAddress, int length)
        {
            if (hexAddress == "")
            {
                return new byte[] { 0x00, 0x00 }; //return a nice safe placeholder when the requested memory is invalid.
            }
            int startAddress = hexStringToInt(hexAddress); //Convert from base 16 (Hex) to int32
            return ReadMemoryBlock(data, startAddress, startAddress + length);
        }

        /// <summary>
        /// Slices out a specific chunk of memory from a raw data snapshot using explicit numerical index boundaries.
        /// </summary>
        /// <param name="data">The source unmanaged process memory byte array snapshot buffer.</param>
        /// <param name="startAddress">The absolute starting index position within the source data array bounds.</param>
        /// <param name="endAddress">The non-inclusive ending index position within the source data array bounds.</param>
        /// <returns>
        /// A sub-array slice generated via modern C# range slicing indexers; 
        /// otherwise, a 2-byte fallback safety placeholder (<c>0x00, 0x00</c>) if index offsets overflow array boundaries.
        /// </returns>
        public static Byte[] ReadMemoryBlock(Byte[] data, int StartAddress, int EndAddress)
        {
            try
            {
                return data[StartAddress..EndAddress];
            }
            catch
            {
                return new byte[] { 0x00, 0x00 };
            }
        }
    }
}