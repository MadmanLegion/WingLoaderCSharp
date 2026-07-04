using SDL2;

namespace Generic_Generics.Audio
{
    public class Generic_SDL2_Audio
    {

        /// <summary>
        /// Start the requested audio file and return control (A-Synchronous)
        /// Stop any playing audio first
        /// </summary>
        /// <param name="FileName">full path to file to play</param>
        public static void playAudio(string FileName)
        {
            if (sdl is null) //Use the singleton if it exists, else create it
            {
                sdl = new Generic_SDL2_Audio();
            }
            
            //Initialise SDL Audio subsystem
            sdl.createSDL();

            //Open Audio Device
            sdl.createAudioDevice();

            //Stop running audio if it exists:
            if (soundEffect != IntPtr.Zero)
            {
                sdl.stopAudio();
            }
            
            //Play and wait for user input or to completion:
            soundEffect = sdl.playAudioFile(FileName);
        }

        /// <summary>
        /// Stops any currently playing sound effects.
        /// </summary>
        public static void stopAudioPlayback()
        {
            if (sdl is not null)
            {
                if (soundEffect  != IntPtr.Zero)
                {
                    //Stop running audio if it exists:
                    if (soundEffect != 0x00)
                    {
                        sdl.stopAudio();
                    }
                }
            }
        }

        /// <summary>
        /// Start the requested audio file and allow it to play for defined time (Synchronous)
        /// Stop any playing audio first
        /// </summary>
        /// <param name="FileName">full path to file to play</param>
        /// <param name="Milliseconds">Time to wait</param>
        private static void playAudioFixedTime(string FileName, int Milliseconds)
        {
            if (sdl is null) //Use the singleton if it exists, else create it
            {
                sdl = new Generic_SDL2_Audio();
            }

            //Initialise SDL Audio subsystem
            sdl.createSDL();

            //Open Audio Device
            sdl.createAudioDevice();

            //Stop running audio if it exists:
            if (soundEffect != 0x00)
            {
                sdl.stopAudio();
            }

            //Play audio
            soundEffect = sdl.playAudioFile(FileName);
            
            //Dramatic pause
            Thread.Sleep(Milliseconds);

            //stop the audio and shutdown
            shutdownAudioPlayer();
        }

        /// <summary>
        /// Start the requested audio file and allow it to play until completion (Synchronous)
        /// Stop any playing audio first
        /// </summary>
        /// <param name="FileName">full path to file to play</param>
        private static void playAudioToEnd(string filename)
        {
            if (sdl is null) //Use the singleton if it exists, else create it
            {
                sdl = new Generic_SDL2_Audio();
            }

            //Initialise SDL Audio subsystem
            sdl.createSDL();

            //Open Audio Device
            sdl.createAudioDevice();

            //Stop running audio if it exists:
            if (soundEffect != 0x00)
            {
                sdl.stopAudio();
            }

            //Play audio
            soundEffect = sdl.playAudioFile(filename);
            
            //Wait for playback to complete
            while (SDL_mixer.Mix_Playing(-1) != 0)
            {
                SDL.SDL_Delay(100);
            }

            //Shutdown 
            shutdownAudioPlayer();
        }

        /// <summary>
        /// Shutdown the singleton if it exists - to be used with A-Synchronous methods.
        /// </summary>
        private static void shutdownAudioPlayer()
        {
            if (sdl is not null)
            {
                sdl.stopAudio();
                sdl.destroyAudioDevice();
                sdl.destroySDL(); //This is what ultimately destroys the audio if the stop function is not called...
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal Generic_SDL2_Audio() { }

        /// <summary>
        /// singleton
        /// </summary>
        private static Generic_SDL2_Audio? sdl;
        
        /// <summary>
        /// Pointer for playback
        /// </summary>
        private static IntPtr soundEffect;

        /// <summary>
        /// Initialise SDL Audio subsystem
        /// </summary>
        internal void createSDL()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_AUDIO) < 0)
            {
                Console.Write($"SDL Init Error: {SDL.SDL_GetError()}");
                return;
            }
        }

        /// <summary>
        /// Open Audio Device
        /// </summary>
        private void createAudioDevice()
        {
            if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
            {
                Console.Write($"Mixer Init Error: {SDL.SDL_GetError()}");
                return;
            }
        }

        /// <summary>
        /// Load Wav file and start it playing
        /// </summary>
        /// <returns>IntPtr for playback to be used to stop later on.</returns>
        private IntPtr playAudioFile(string filename)
        {
            IntPtr soundEffect = SDL_mixer.Mix_LoadWAV(filename);
            if (soundEffect == IntPtr.Zero)
            {
                Console.Write($"Failed to load sound: {SDL.SDL_GetError()}");
            }
            else
            {
                SDL_mixer.Mix_PlayChannel(-1, soundEffect, 0);
                Console.WriteLine("Playing audio... Press any key to exit.");
            }
            return soundEffect;
        }

        /// <summary>
        /// Cleanup: Stop playing Audio
        /// </summary>
        private void stopAudio()
        {
            SDL_mixer.Mix_HaltChannel(-1);
            if (soundEffect != IntPtr.Zero)
            {
                SDL_mixer.Mix_FreeChunk(soundEffect);
            }
            soundEffect = IntPtr.Zero;
        }

        /// <summary>
        /// Cleanup: Close Audio Subsystem
        /// </summary>
        private void destroyAudioDevice()
        {
            SDL_mixer.Mix_CloseAudio();
        }

        /// <summary>
        /// Cleanup: Shutdown SDL Subsystem
        /// </summary>
        private void destroySDL()
        {
            SDL.SDL_Quit();
        }
    }
}