using SDL2;

namespace Generic_Generics.Audio
{
    internal class SDL2_Audio_Player
    {

        /// <summary>
        /// Start the requested audio file in a window and allow it to play until completion (Synchronous)
        /// Can be aborted with Escape, and paused/started with space
        /// Note. Will not stop audio played under Generic_SDL2_Audio.
        /// </summary>
        /// <param name="FileName">full path to file to play</param>
        private static IntPtr playAudioFileWithControl(string filename)
        {
            Generic_SDL2_Audio sdl = new Generic_SDL2_Audio();

            //Initialise SDL Audio subsystem
            sdl.createSDL();

            IntPtr window = SDL.SDL_CreateWindow("Audio player", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 640, 480, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            IntPtr soundEffect = SDL_mixer.Mix_LoadWAV(filename);
            if (soundEffect == IntPtr.Zero)
            {
                Console.Write($"Failed to load sound: {SDL.SDL_GetError()}");
            }
            else
            {
                int channel = SDL_mixer.Mix_PlayChannel(-1, soundEffect, 0);
                Console.WriteLine("Playing audio... Press any key to exit.");

                bool audioFileRunning = true;
                bool isPaused = false;
                ulong startTimeTicks = SDL.SDL_GetTicks64();

                while (audioFileRunning)
                {
                    //Handle event loop (input and pauses)
                    while (SDL.SDL_PollEvent(out SDL.SDL_Event sdl_event) != 0)
                    {
                        switch (sdl_event.type)
                        {
                            case SDL.SDL_EventType.SDL_QUIT:
                                audioFileRunning = false;
                                continue;

                            case SDL.SDL_EventType.SDL_KEYDOWN:

                                if (sdl_event.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                                {
                                    audioFileRunning = false;
                                    continue;
                                }
                                else if (sdl_event.key.keysym.sym == SDL.SDL_Keycode.SDLK_SPACE)
                                {
                                    if (isPaused)
                                    {
                                        SDL_mixer.Mix_Resume(channel);
                                        isPaused = false;
                                    }
                                    else
                                    {
                                        SDL_mixer.Mix_Pause(channel);
                                        isPaused = true;
                                    }
                                }
                                break;
                        }
                    }
                    //if (isPaused)
                    //{
                    //    SDL.SDL_Delay(10);
                    //    continue;
                    //}
                }
            }
            return soundEffect;
        }
    }
}
