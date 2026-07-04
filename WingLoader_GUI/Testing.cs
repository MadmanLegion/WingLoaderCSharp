using Generic_Generics.Logging;
using Generic_Generics.Natives;
using System.Text;
using System.Text.Json.Serialization;
using WingLoader_Generics;
using WingLoader_Generics.Debugger;
using static WingLoader_Generics.WingLoader_Debugger;

namespace WingLoader_GUI
{
    internal partial class WingLoader_GUI_Form : Form
    {
        /// <summary>
        /// Polymorphic Debugger created with Dummy type - the test buttons will override this based on the active game mode.
        /// </summary>
        Debugger debugger = WingLoader_Debugger.Create(DebuggerTypes.Dummy);
        /// <summary>
        /// UI click event handler used to test unmanaged file extraction sequences 
        /// by unpacking a specific Wing Commander save game file and logging a target navigation status.
        /// </summary>
        /// <param name="sender">The user interface button control raising the click event.</param>
        /// <param name="e">Standard event notification arguments metadata.</param>
        private void btn_testDoSomething_Click(object sender, EventArgs e)
        {
            tb_messages.Text = "";
            // Pull memory and check for the string/Hex you're interested in.
            //setDebugger();
            if (WingLoader_Worker.gameModesDOS.Contains(wl.activeGameMode))
            {
                debugger = WingLoader_Debugger.Create(WingLoader_Generics.DebuggerTypes.Dosbox);
            }
            else if (WingLoader_Worker.gameModesWin.Contains(wl.activeGameMode))
            {
                debugger = WingLoader_Debugger.Create(WingLoader_Generics.DebuggerTypes.Win32);
                // Assign the running executable process Identifier to the debugger
                tb_messages.Text += $"Debugger attached to Process: {debugger.setProcessID(Path.GetFileNameWithoutExtension(WingLoader_Worker.getGameExecutable(wl.activeGameMode)))}";
            }

            string hexstring = "";
            try
            {
                Byte[] bytes = new byte[] { 0x00, 0x00, 0x00 };
                int dataspace = 250000;
                bool looping = true;
                while (looping)
                {
                    bytes = debugger.getMemory_Sync(debugger.initialOffset, dataspace.ToString());
                    if (bytes.Length > 3)
                        looping = false;
                    else if (dataspace < 50000)
                        looping = false;
                    else
                        dataspace = dataspace - 50000;//decrement by 50k and try again.
                }
                func_log($"Pulled {dataspace} bytes");

                hexstring = WingLoader_Debugger.BytesToHexString(bytes);
                //tb_Hex.Text = hexstring;
                tb_messages.Text += Environment.NewLine + (WingLoader_Debugger.BytesToString(trimFirstElementFromBytes(ReadMemoryBlock(bytes, tb_Address.Text, 100))));
            }
            catch
            {
                tb_messages.Text += "Failed to pull memory from debugger";
            }

            tb_hexstring.Text = WingLoader_Debugger.BytesToHexString(Encoding.UTF8.GetBytes(tb_string.Text));

            try
            {

                string lookingfor = tb_string.Text;
                string lookinghex = tb_hexstring.Text;
                string memoryhex = hexstring;

                int firstFound = memoryhex.IndexOf(lookinghex) / 3;
                int secondFound = memoryhex.IndexOf(lookinghex, (firstFound * 3) + 1) / 3;
                int thirdFound = memoryhex.IndexOf(lookinghex, (secondFound * 3) + 1) / 3;
                int lastFound = memoryhex.LastIndexOf(lookinghex) / 3;

                string firstfoundhex = intToHexString(firstFound);
                string secondfoundhex = intToHexString(secondFound);
                string thirdfoundhex = intToHexString(thirdFound);
                string lastfoundhex = intToHexString(lastFound);

                string firstOffset = subtractHexStrings(firstfoundhex, wl.gameOffsetHex);
                string secondOffset = subtractHexStrings(secondfoundhex, wl.gameOffsetHex);
                string thirdOffset = subtractHexStrings(thirdfoundhex, wl.gameOffsetHex);
                string lastOffset = subtractHexStrings(intToHexString(memoryhex.LastIndexOf(lookinghex)), wl.gameOffsetHex);

                tb_messages.Text += Environment.NewLine + $"Found '{lookingfor}' as '{lookinghex}' at " + Environment.NewLine +
                    $"'{firstFound}' " +
                    $"or '{secondFound}' " +
                    $"or '{thirdFound}' " +
                    $"or '{lastFound}' " + Environment.NewLine +
                    $"or in HEX '{firstfoundhex}' " +
                    $"or '{secondfoundhex}'" +
                    $"or '{thirdfoundhex}'" +
                    $"or '{lastfoundhex}'" + Environment.NewLine +
                    $"or as OffsetHex '{firstOffset}' " +
                    $"or '{secondOffset}'" +
                    $"or '{thirdOffset}'" +
                    $"or '{lastOffset}'";
            }
            catch
            {
                tb_messages.Text += "Failed badly";
            }

            // Test some Audio and Video
            //Generic_SDL2_Audio.playAudio();
            //Generic_SDL2_Video.playVideo();

            // Test unzipping
            //ZipHandler.unzipFileToFolder("dosbox-staging-windows-x64-v0.83.0-RC1.zip", "Dosbox", false);

            // Test Action to update a UI box
            //Action<string> updateHexTextBox = value =>
            //{
            //    tb_Hex.Text += value;
            //    tb_Hex.Refresh();
            //};


            // Test unpacking Unpacks a SaveGame file to an object
            //var returned = SaveFile_Struct.unpackSaveGameFile("C:\\DOSGames\\WING\\GAMEDAT\\SAVEGAME.WLD");
            //func_log($"{returned.Saves[5].LastMissionNavPoints[2].NavStatus}");

        }

        /// <summary>
        /// UI click event handler that maps a Win32 process target, initializes page queries, 
        /// and walks the process memory space using sequential native system API calls.
        /// </summary>
        /// <param name="sender">The user interface button control raising the click event.</param>
        /// <param name="e">Standard event notification arguments metadata.</param>
        /// <remarks>
        /// This method dumps page blocks flagged as <see cref="MemoryState.MEM_COMMIT"/>. 
        /// Warning: The native API loop evaluation relies on the return value of <see cref="Kernel32.VirtualQueryEx"/> 
        /// matching an exact zero condition (<c>!= 0</c>) to break out. On some 64-bit systems processing a 32-bit target, 
        /// an edge-case memory boundary failure can return a non-zero system error pointer, creating an infinite execution loop.
        /// </remarks>
        private void btn_testDoSomething2_Click(object sender, EventArgs e)
        {
            Action<string> logger = value =>
            {
                Debug_Logger.log(value);
                tb_Hex.Text += value+ Environment.NewLine;
                //tb_Hex.Refresh();
            };

            Action<string> nologger = value => { };

             tb_Hex.Text = "";
            // Test dumping the contents of a Win32 process space to file.
            if (WingLoader_Worker.gameModesWin.Contains(wl.activeGameMode))
            {
                Win32_Debugger debugger = (WingLoader_Debugger.Create(WingLoader_Generics.DebuggerTypes.Win32) as Win32_Debugger)!;
                // Assign the running executable process Identifier to the debugger
                tb_messages.Text += $"Debugger attached to Process: {debugger.setProcessID(Path.GetFileNameWithoutExtension(WingLoader_Worker.getGameExecutable(wl.activeGameMode)))}";

                debugger.getMemory_Sync("0x0", "0x1");

                ///Scan the entire Memory space and list out the allocations:
                List<Win32_Debugger.MemorySpace> memorySpaceList = debugger.ParseWin32MemoryAllocations(nologger);


                //Now for those memory allocations - check for instances of 'jeff' to indicate the WC2 memory manager allocations....
                foreach (Win32_Debugger.MemorySpace memorySpaceItem in memorySpaceList.ToList()) //Making a copy of memorySpaceList so we can iterate over it, and add items in flight...
                {
                    if (((memorySpaceItem.BaseAddress.StartsWith("0x00000A")) & (memorySpaceItem.RegionSize == 1044480))
                        || ((memorySpaceItem.BaseAddress.StartsWith("0x00000B")) & (memorySpaceItem.RegionSize == 2072576)))
                    {
                        List<Win32_Debugger.MemorySpace> patchList = debugger.parseMemorySpaceForSegments(nologger, memorySpaceItem);
                        memorySpaceList.AddRange(patchList);
                    }
                }

                //memorySpaceList.Sort();
                logger.Invoke(Win32_Debugger.printMemorySpaceHeader());
                logger.Invoke(Win32_Debugger.printMemorySpaceSeperator());
                foreach (Win32_Debugger.MemorySpace ms in memorySpaceList)
                    logger.Invoke(Win32_Debugger.printMemorySpace(ms));

            }
        }
    }
}
