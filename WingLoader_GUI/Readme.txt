WingLoader-C# is a ground up re-implementation of the excellent work done by Destro in WingLoader - it's not as pretty as Destro's work, but it has some new tricks!

This tool exists for two reasons:
1) To see if i could do it
2) To implement the same logic Destro had implemented but using the Dosbox Staging 0.83 RC1 debugger API and thus support both original Dos releases and All-Tinker's amazing WCAT work.

What 3rd party code does this package already contain:
- Libraries for SDL2-2.30.11-win32-x64 sourced from here - https://github.com/libsdl-org/SDL/releases/download/release-2.30.11/SDL2-2.30.11-win32-x64.zip
	If you want to update the SDL version - then get a later SDL2 package (NOT SDL3) e.g. https://github.com/libsdl-org/SDL/releases/download/release-2.32.10/SDL2-2.32.10-win32-x64.zip and extract it to the libs folder.
	However, I have not tested this, and make not guarantees that it will work!
- FFmpeg.AutoGen 8.1.0 by Ruslan Balanuhkin (from NuGet)
- Sayers.SDL2.Core 1.0.11 by Jeremy Sayers (from NuGet

To get this going:
- Extract this package to a folder somewhere (e.g. Games\WingLoaderC#
- Download the Dosbox Staging 0.83.0-RC1 package (e.g. https://github.com/dosbox-staging/dosbox-staging/releases/download/v0.83.0-rc1/dosbox-staging-windows-x64-v0.83.0-RC1.zip)
	Simply place this into your WingLoaderC# folder as WingLoaderC#\dosbox-staging-windows-x64-v0.83.0-RC1.zip - the application will unzip it for you.
- Download the FFMPEG package (e.g. https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl-shared.zip) and extract the bin folder to the FFMPEG folder
	You should now have Games\WingLoaderC#\FFMPEG\ffmpeg.exe
- The application will automatically create game folders for the 6 supported versions of games when it starts the first time. HOWEVER you will need to extract the games themselves to these folders.
	Wing - contains the original DOS version of Wing Commander 1 - ensure the game executables exist in the folder like WingLoaderC#\Wing\WC.EXE
	WingAT - contains the All-Tinker patched DOS version of Wing Commander 1 - ensure the game executables exist in the folder like WingLoaderC#\WingAT\WC.BAT
	WingKS - contains the Kilrathi Saga version of Wing Commander 1 - ensure the game executables exist in the folder like WingLoaderC#\WingKS\wc_wcdx.exe
	Wing2 - contains the original DOS version of Wing Commander 2 - ensure the game executables exist in the folder like WingLoaderC#\Wing2\WC2.EXE
	Wing2AT - contains the All-Tinker patched DOS version of Wing Commander 2 - ensure the game executables exist in the folder like WingLoaderC#\Wing2AT\WC2.BAT - note this does not exist yet!
	Wing2KS - contains the Kilrathi Saga version of Wing Commander 2 - ensure the game executables exist in the folder like WingLoaderC#\Wing2KS\wc2_wcdx.
- The application will automatically extract the Data.zip to the required folders (e.g. WingLoaderC#\Data\Voice\Angel\bri0137.wav)


Start the application by running WingLoader_GUI.exe
- When it starts the first time it will create a config file, and will let you know of the latest version of Dosbox-Staging and WCDX in github.
- Select the game you want using the radio buttons on the bottom right 
	By default the game will start WC1 KS, 
	If you UNCHECK the Kilrathi Saga option it will start WC1 with WCAT installed, 
	If you also UNCHECK the WCAT option it will start the original DOS version of WC1.
- Start the game with the Start Game button.  The game will start and after a few moments the Debugger should start too.  
	If it does not, then you can use the Start Debugger button to force it to start (note it will only start when the right game mode is running).


Immense appreciation and recognition must go to Destro for his work in the WingLoader tool and particularly because i've built off his script for WC1 and his Audio package.
Also to the many others in the WC community whose work over the years have made playing these games in 2026 possible - particularly:
- HCL for his ground breaking work
- Bekenn for WCDX
- All-Tinker for WCAT
- Delmar for WCWorkshop, but also for collating lots of older tools and analysis onto github
- UnnamedCharater for WCToolbox
