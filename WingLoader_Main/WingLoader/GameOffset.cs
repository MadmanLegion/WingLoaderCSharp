using WingLoader_Generics;
using WingLoader_Generics.Debugger;
using static WingLoader_Generics.WingLoader_Debugger;

namespace WingLoader_NS
{
    public partial class WingLoader_Main
    {
        /// <summary> The integer representation of the start of the game's memory block.</summary>
        private int gameStartOffsetInt = 0;
        /// <summary> The hexadecimal string representation of the start of the game's memory block.</summary>
        private string gameStartOffsetHex = "0x0";
        public string gameOffsetHex { get => gameStartOffsetHex; }

        /// <summary>
        /// Resets the internal game memory base location trackers back to empty baseline states.
        /// </summary>
        private void resetGameStartOffset()
        {
            gameStartOffsetInt = 0;
            gameStartOffsetHex = "0x0";
        }

        /// <summary>
        /// Dynamically scans and analyzes the unmanaged DOSBox memory matrix map to automatically detect 
        /// the absolute base execution pointer offset of the target game.
        /// </summary>
        /// <exception cref="Exception">Thrown if the emulator connection is active but the target application header cannot be located.</exception>
        /// <remarks>
        /// This method automatically handles memory offset mutations caused by the MS-DOS <c>LOADFIX.COM</c> utility allocation frames
        /// as well as handling the possibility that the game is being called indirectly through a batch file in the Launcher sub folder (e.g. WCAT)
        /// In Win32 Mode, this method serves as an inline placeholder, returning an offset of <c>0x00</c> immediately 
        /// since Win32 debuggers utilize absolute process indexing instead of segmented emulator structures.
        /// </remarks>
        private void getGameStartOffset()
        {
            if (debugger is Win32_Debugger)
            {
                //Need a placeholder here for the following logic - but Win32 mode doesn't need a gameStartOffset.
                // Win32 mode accesses memory maps using absolute process hooks; relative offsets are treated as baseline zero
                gameStartOffsetInt = -1;
                //Technically game starts at "0x400000", but we have to read memory starting there, so the relative offset of game from start of pulled memory is 0x00.
                gameStartOffsetHex = "0x00";
                return;
            }

            // Pull a 1,000,000 byte memory block from the active emulator instance layout
            Byte[] bytes = debugger.getMemory_Sync(debugger.initialOffset, "1000000");

            if (byteArraysEqual(bytes, new byte[] { 0x00, 0x00, 0x00 }))
            {
                gameStartOffsetInt = 0;
                gameStartOffsetHex = "0x00";
                return;
            }
            else
            {
                string headAppMemAddress = "0x18ED"; //The head address of the stack
                byte[] headAppBytes = trimFirstElementFromBytes(ReadMemoryBlock(bytes, headAppMemAddress, 50));
                string headApp = WingLoader_Debugger.BytesToString(headAppBytes);

                //If LoadFix IS NOT being used, the first appearance of the Executable name AFTER 0x18F0 is the start of the working memory
                int lookForCounter = 1;
                if (headApp == "Z:\\LOADFIX.COM")
                {
                    headAppMemAddress = "0x1B70"; //The second address of the stack
                    headAppBytes = trimFirstElementFromBytes(ReadMemoryBlock(bytes, headAppMemAddress, 50));

                    headApp = WingLoader_Debugger.BytesToString(headAppBytes);

                    //If LoadFix IS being used, the second appearance of the Executable name AFTER 0x1B70 is the start of the working memory (1st apperance as 0x?DE0 marks the end of the memory offset).
                    lookForCounter = 2;
                }
                if (headApp.StartsWith("C:\\Launcher"))
                {
                    //The WCAT game Launcher executable is running:
                }
                else
                {
                    headAppBytes = WingLoader_Debugger.StringToBytes(headApp.ToUpper()); //necessary because even if you start wc.exe, DOS will see WC.EXE.
                }

                //0x18F0 = 6384
                //0x1B70 = 7024 //still well before the working memory, so for ease we can scan from here...

                int initialIndex = hexStringToInt(headAppMemAddress);
                string foundOffset = "";

                // Scan the array data sequence to find the structural marker signature offset location indices
                while (lookForCounter > 0)
                {
                    // Performance notice: Enumerable.Range combined with Skip and Take allocates temporary objects.
                    initialIndex = Enumerable.Range(initialIndex + 1, bytes.Length - headAppBytes.Length + 1)
                        .FirstOrDefault(i => bytes.Skip(i)
                        .Take(headAppBytes.Length)
                        .SequenceEqual(headAppBytes), -1);

                    foundOffset = intToHexString(initialIndex);
                    lookForCounter--;   //"0x9DE0" then "0x1A4DF" when using Loadfix 32
                }

                if (initialIndex != -1)
                {
                    byte[] nullByte = new byte[] { 0x00 };

                    // Scans backwards from the match signature to locate the preceding null terminator delimiter byte
                    int address = (from i in Enumerable.Range(0, initialIndex)
                                   let segment = bytes.Skip(i)
                                   .Take(1)
                                   where nullByte
                                   .SequenceEqual(segment)
                                   select i).Reverse().FirstOrDefault(-1);
                    address++;

                    foundOffset = intToHexString(address);
                    headApp = WingLoader_Debugger.BytesToString(trimFirstElementFromBytes(ReadMemoryBlock(bytes, foundOffset, 20)));

                    // Assign resolved values back to the global variables
                    gameStartOffsetInt = address;
                    gameStartOffsetHex = foundOffset;

                    logger.log(string.Format("{0} = {1} @ {2}", headAppMemAddress, headApp, foundOffset));
                }
                else
                {
                    throw new Exception("Dosbox is started and connected, but unable to locate head app - is a game running?");
                }
            }
        }
    }
}
