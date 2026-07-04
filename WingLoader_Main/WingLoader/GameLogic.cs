using WingLoader_Generics;
using WingLoader.Scripting;
using static WingLoader_Generics.WingLoader_Debugger;

namespace WingLoader_NS
{
    public partial class WingLoader_Main
    {


        #region gameOffsets
        public string mem_Message1 = "";
        public bool mem_Message2Indirect = false;
        public string mem_Message2 = "";
        public string mem_FirstName = "";
        public string mem_Surname = "";
        public string mem_Callsign = "";
        public string mem_System = "";
        public string mem_Year = "";
        public string mem_Day = "";

        /// <summary>
        /// Calculates and maps the architectural process memory pointers for dialogues, names etc. 
        /// depending on the active game selection context.
        /// </summary>
        /// <remarks>
        /// The offsets in this method are relative to either the application memory start in DOS (via DosBox) 
        /// irrespective of whether it is started tools like LoadFix.
        /// For Kilrathi Saga, the offsets are absolute (or as with mem_Message2) direct to an address that contains a pointer.
        /// </remarks>
        private void setGameOffsets()
        {
            // 1. Ensure the underlying base game process layout anchor offset has been resolved
            if (gameStartOffsetInt == 0)
            {
                try
                {
                    getGameStartOffset();
                }
                catch (Exception ex)
                {
                    logger.log(ex.Message);
                    //logger.log(ex.StackTrace);
                }
            }

            // 2. Early exit backout if the unmanaged memory start anchor pointer remains unassigned - true if the Dosbox Debugger failed to find the memory.
            // Win32 will be -1, Dos should be >50,000, 0 is the fallback when nothing found in Dos.
            if (gameStartOffsetInt == 0)
            {
                return;
            }

            // 3. Clear existing address states to prevent data contamination across different game instances
            mem_Message1 = "";
            mem_Message2 = "";
            mem_Message2Indirect = false;
            mem_FirstName = "";
            mem_Surname = "";
            mem_Callsign = "";
            mem_System = "";
            mem_Year = "";
            mem_Day = "";

            // 4. Map relative pointers onto the absolute process runtime structure based on selection
            if (activeGameMode == WingLoader_Worker.GameMode.WC1KS)
            {
                //WC1 KS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0x2154C0");   //0x6154C0; //Awaken   Hex example when using LoadFix -32...
                mem_Message2 = addHexStrings(gameStartOffsetHex, "0x2284E0");   //0x6284E0; //Awaken
                mem_Surname = addHexStrings(gameStartOffsetHex, "0x222F80");    //0x622F80; //OUR HERO
                mem_Callsign = addHexStrings(gameStartOffsetHex, "0x222F8E");   //0x622F8E; //BLUEHAIR
                mem_System = addHexStrings(gameStartOffsetHex, "0x21F280");     //0x61F280; //Enyo when View Medals
                mem_Year = addHexStrings(gameStartOffsetHex, "0x0AD00A");       //0x4AD00A; //2654
                mem_Day = addHexStrings(gameStartOffsetHex, "0x0AD008");        //0x4AD008; //110
            }
            else if (activeGameMode == WingLoader_Worker.GameMode.WC1 || activeGameMode == WingLoader_Worker.GameMode.WC1AT)
            {
                //WC1 DOS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0xB4E6");     //"0x259C2"; //Awaken   Hex example when using LoadFix -32...
                mem_Message2 = addHexStrings(gameStartOffsetHex, "C61A");       //"0x26af6"; //Awaken
                mem_Surname = addHexStrings(gameStartOffsetHex, "B7B6");        //"0x25C92"; //OUR HERO
                mem_Callsign = addHexStrings(gameStartOffsetHex, "B7C4");       //"0x25CA0"; //BLUEHAIR
                mem_System = addHexStrings(gameStartOffsetHex, "B2E8");         //"0x257C4"; //Enyo when View Medals
                mem_Year = addHexStrings(gameStartOffsetHex, "3572");           //"0x1DA4E"; //2654
                mem_Day = addHexStrings(gameStartOffsetHex, "3570");            //"0x1DA4C"; //110
            }
            else if (activeGameMode == WingLoader_Worker.GameMode.SM2KS)
            {
                // WC1 SM2 KS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0x2132F0");   //0x5CC520; //Awaken
                mem_Message2 = addHexStrings(gameStartOffsetHex, "0x1CC520");   //0x6132F0; //Awaken
                mem_Surname = addHexStrings(gameStartOffsetHex, "0xA5A16");     //0x4A5A16"; //Pelley
                mem_Callsign = addHexStrings(gameStartOffsetHex, "0xA5A0F");    //0x4A5A0F; //Goblin
                mem_System = addHexStrings(gameStartOffsetHex, "0x1CC6D0");     //0x5CC6D0; //Firekka when View Medals
                mem_Year = addHexStrings(gameStartOffsetHex, "0x1D0E9E");       //0x5D0E9E; //2654
                mem_Day = addHexStrings(gameStartOffsetHex, "0x1D0E9C");        //0x5D0E9C; //110
            }
            else if (activeGameMode == WingLoader_Worker.GameMode.SM2 || activeGameMode == WingLoader_Worker.GameMode.SM2AT)
            {
                // WC1 SM2 DOS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0xB7B0");     //0x2581C
                mem_Message2 = addHexStrings(gameStartOffsetHex, "0xC8E0");     //0x2694c
                mem_Surname = addHexStrings(gameStartOffsetHex, "BA80");        //0x2BAD6
                mem_Callsign = addHexStrings(gameStartOffsetHex, "BA8E");       //0x2BAE4
                mem_System = addHexStrings(gameStartOffsetHex, "B5B2");         //0x2561E
                mem_Year = addHexStrings(gameStartOffsetHex, "34A6");           //0x1D512
                mem_Day = addHexStrings(gameStartOffsetHex, "34A4");            //0x1D510
            }
            else if (activeGameMode == WingLoader_Worker.GameMode.WC2KS)
            {
                // WC2 KS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0x1D1C41");   //0x5D1C40 //For some reason the first character is garbled so we have to ignore that...
                mem_Message2 = addHexStrings(gameStartOffsetHex, "0x99DA4");   //0x499DA4 or 0x5D2ECC
                mem_Message2Indirect = true; //This is the address of the pointer to the conversation view...
                mem_FirstName = addHexStrings(gameStartOffsetHex, "0x99F28");   //0x499F28
                mem_Surname = addHexStrings(gameStartOffsetHex, "0x99F10");     //0x499F10
                mem_Callsign = addHexStrings(gameStartOffsetHex, "0x99EF8");    //0x499EF8
            }
            else if (activeGameMode == WingLoader_Worker.GameMode.WC2 || activeGameMode == WingLoader_Worker.GameMode.WC2AT)
            {
                // WC2 DOS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0xDDFC");     //0x2E8D8
                mem_Message2 = addHexStrings(gameStartOffsetHex, "0x11284");    //0x31D60
                mem_FirstName = addHexStrings(gameStartOffsetHex, "0x71ED");    //0x27CC9
                mem_Surname = addHexStrings(gameStartOffsetHex, "0x7206");      //0x27CE2
                mem_Callsign = addHexStrings(gameStartOffsetHex, "0x721F");     //0x27CFB
            }
            else if (activeGameMode == WingLoader_Worker.GameMode.SO1KS)
            {
                // WC2 SO1 KS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0x1D8141");   //0x5D1C40 //For some reason the first character is garbled so we have to ignore that...
                mem_Message2 = addHexStrings(gameStartOffsetHex, "0x97DDC");   //0x497DDC or 0x5D85A0
                mem_Message2Indirect = true; //This is the address of the pointer to the conversation view...
                mem_FirstName = addHexStrings(gameStartOffsetHex, "0x9A2F8");   //0x49A2F8
                mem_Surname = addHexStrings(gameStartOffsetHex, "0x9A311");     //0x49A311
                mem_Callsign = addHexStrings(gameStartOffsetHex, "0x9A32A");    //0x49A32A
            }
            else if (activeGameMode == WingLoader_Worker.GameMode.SO1 || activeGameMode == WingLoader_Worker.GameMode.SO1AT)
            {
                // WC2 SO1 DOS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0xDCF8");     //0x2E904
                mem_Message2 = addHexStrings(gameStartOffsetHex, "0x11154");    //0x31D60
                mem_FirstName = addHexStrings(gameStartOffsetHex, "0x71F7");    //0x27E03
                mem_Surname = addHexStrings(gameStartOffsetHex, "0x7210");      //0x27E1C
                mem_Callsign = addHexStrings(gameStartOffsetHex, "0x7229");     //0x27E35
            }
            else if (activeGameMode == WingLoader_Worker.GameMode.SO2KS)
            {
                // WC2 SO2 KS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0x1D6921");   //0x5D6921 //For some reason the first character is garbled so we have to ignore that...
                mem_Message2 = addHexStrings(gameStartOffsetHex, "0x9B4E4");   //0x49B4E4 or 0x5D6DF0
                mem_Message2Indirect = true; //This is the address of the pointer to the conversation view...
                mem_FirstName = addHexStrings(gameStartOffsetHex, "0x95210");   //0x495210
                mem_Surname = addHexStrings(gameStartOffsetHex, "0x95229");     //0x495229
                mem_Callsign = addHexStrings(gameStartOffsetHex, "0x95242");    //0x495242
            }
            else if (activeGameMode == WingLoader_Worker.GameMode.SO2 || activeGameMode == WingLoader_Worker.GameMode.SO2AT)
            {
                // WC2 SO2 DOS
                mem_Message1 = addHexStrings(gameStartOffsetHex, "0xD81E");     //0x2E54A
                mem_Message2 = addHexStrings(gameStartOffsetHex, "0x1100C");    //0x31D38
                mem_FirstName = addHexStrings(gameStartOffsetHex, "0xBC16");    //0x2C942
                mem_Surname = addHexStrings(gameStartOffsetHex, "0x6D3C");      //0x27A68
                mem_Callsign = addHexStrings(gameStartOffsetHex, "0x6D55");     //0x27A81
            }
        }
        #endregion gameOffsets

        #region gameModes
        /// <summary> Active version or edition category layout tracking flag for the currently executed target game. </summary>
        public WingLoader_Worker.GameMode activeGameMode;

        /// <summary>
        /// Intercepts option changes across radio toggles and platform checkboxes, 
        /// dynamically routing active executable paths and mapping corresponding game engine modes.
        /// </summary>
        /// <param name="sender">The active user interface component (RadioButton or CheckBox) that triggered the state change.</param>
        /// <param name="e">Standard system event arguments containing baseline notification metadata context.</param>
        /// <remarks>
        /// This central method automatically stops running background scan engines and flushes active script data 
        /// configurations whenever a new game variant selection is initiated to prevent cross-contamination of pointers.
        /// </remarks>
        public void selectGameMode(WingLoader_Worker.GameMode l_activeGameMode)
        {
            // Reset configurations immediately to enforce data boundary boundaries between varying game titles
            rules = RulesHelper.resetRules();
            StopDebugger_Click();
            activeGameMode = l_activeGameMode;
        }
        #endregion gameModes
    }
}