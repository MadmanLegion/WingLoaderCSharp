using FFmpeg.AutoGen;
using SDL2;

namespace Generic_Generics.Video
{
    public class SDL2_Video_Player
    {
        /// <summary>
        /// Test function to try different methods
        /// </summary>
        private static void testPlayVideo()
        {
            //playVideo("C:\\WingLoader 0.92\\Data\\Videos\\Origin_Intro.mp4");
            //playVideo("S:\\Video\\HIMYM CONVERTED MOVED\\Episodes\\1x01 - Pilot.mp4");
        }

        /// <summary>
        /// Plays a video file with synchronized audio using native FFmpeg decoders and hardware SDL2 rendering.
        /// </summary>
        /// <param name="filename">The relative or absolute file path of the multimedia video file to play.</param>
        /// <remarks>
        /// This method relies heavily on unmanaged pointers (<c>unsafe</c> blocks). 
        /// Note: Ensure proper resource disposal paths are added to prevent memory leaks if playback is aborted early.
        /// </remarks>
        public unsafe static void playVideo(string filename)
        {
            //Generic_SDL2_Video sdl = new Generic_SDL2_Video();

            // 1. Initialize
            Generic_SDL2_Video.setupFFMPEG();
            DynamicallyLoadedBindings.Initialize();


            // 1. Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO) < 0) return;

            // 2. Open MP4 file with FFmpeg
            AVFormatContext* pFormatContext = null;
            if (ffmpeg.avformat_open_input(&pFormatContext, filename, null, null) != 0) return;
            if (ffmpeg.avformat_find_stream_info(pFormatContext, null) < 0) return;

            // 3. Find the first VideoStream and first AudioStream
            int videoStreamIndex = -1;
            int audioStreamIndex = -1;
            for (int i = 0; i < pFormatContext->nb_streams; i++)
            {
                if ((pFormatContext->streams[i]->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO) && (videoStreamIndex == -1))
                {
                    videoStreamIndex = i;
                }
                if ((pFormatContext->streams[i]->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO) && (audioStreamIndex == -1))
                {
                    audioStreamIndex = i;
                }
                if (videoStreamIndex != -1 && audioStreamIndex != -1)
                {
                    break;
                }
            }

            //Initialise Video Decoder
            AVCodecContext* pVideoCodecContext = null;
            AVRational videoTimeBase = new AVRational();
            if (videoStreamIndex != -1)
            {
                AVCodecParameters* pCodecParameters = pFormatContext->streams[videoStreamIndex]->codecpar;
                AVCodec* pCodec = ffmpeg.avcodec_find_decoder(pCodecParameters->codec_id);
                pVideoCodecContext = ffmpeg.avcodec_alloc_context3(pCodec);
                ffmpeg.avcodec_parameters_to_context(pVideoCodecContext, pCodecParameters);
                ffmpeg.avcodec_open2(pVideoCodecContext, pCodec, null);
                videoTimeBase = pFormatContext->streams[videoStreamIndex]->time_base;
            }


            //Initialise Audio Decoder and Hardware Audio Specs
            AVCodecContext* pAudioCodecContext = null;
            AVRational audioTimeBase = new AVRational();
            uint audioDevice = 0;
            SwrContext* pSwrContext = null;
            byte* audioResampleBuffer = null;
            int maxTargetNbSamples = 0;

            if (audioStreamIndex != -1)
            {
                AVCodecParameters* aCodecParameters = pFormatContext->streams[audioStreamIndex]->codecpar;
                AVCodec* aCodec = ffmpeg.avcodec_find_decoder(aCodecParameters->codec_id);
                pAudioCodecContext = ffmpeg.avcodec_alloc_context3(aCodec);
                ffmpeg.avcodec_parameters_to_context(pAudioCodecContext, aCodecParameters);
                ffmpeg.avcodec_open2(pAudioCodecContext, aCodec, null);
                audioTimeBase = pFormatContext->streams[audioStreamIndex]->time_base;

                // Configure Target Hardware Audio Format
                SDL.SDL_AudioSpec desiredSpec = new SDL.SDL_AudioSpec();
                desiredSpec.freq = pAudioCodecContext->sample_rate; // Match source sampling frequency
                desiredSpec.format = SDL.AUDIO_S16SYS;              // Signed 16-bit Native Byte Order PCM
                desiredSpec.channels = (byte)pAudioCodecContext->ch_layout.nb_channels;
                desiredSpec.samples = 4096;                         // Buffer sizing
                desiredSpec.callback = null!;                        // Using QueueAudio instead of callbacks

                SDL.SDL_AudioSpec obtainedSpec;
                audioDevice = SDL.SDL_OpenAudioDevice(null!, 0, ref desiredSpec, out obtainedSpec, 0);
                if (audioDevice == 0) return;

                // Start hardware audio playback
                SDL.SDL_PauseAudioDevice(audioDevice, 0);

                // Configure Audio Resampler to match the target SDL PCM format
                AVChannelLayout outLayout;
                ffmpeg.av_channel_layout_default(&outLayout, obtainedSpec.channels);

                pSwrContext = ffmpeg.swr_alloc();
                ffmpeg.swr_alloc_set_opts2(
                    &pSwrContext,
                    &outLayout,
                    AVSampleFormat.AV_SAMPLE_FMT_S16,
                    obtainedSpec.freq, // Output configuration
                    &pAudioCodecContext->ch_layout,
                    pAudioCodecContext->sample_fmt,
                    pAudioCodecContext->sample_rate, // Input configuration
                    0,
                    null
                );
                ffmpeg.swr_init(pSwrContext);

                // Allocate a managed frame buffer area to store resampled outputs
                maxTargetNbSamples = (int)ffmpeg.av_rescale_rnd(4096
                    , obtainedSpec.freq
                    , pAudioCodecContext->sample_rate
                    , AVRounding.AV_ROUND_UP);
                int bufferSize = ffmpeg.av_samples_get_buffer_size(null, obtainedSpec.channels, maxTargetNbSamples, AVSampleFormat.AV_SAMPLE_FMT_S16, 1);
                audioResampleBuffer = (byte*)ffmpeg.av_malloc((nuint)bufferSize);
            }

            // 4. Initialize SDL Video Viewport and Graphics Pipeline
            IntPtr window = IntPtr.Zero;
            IntPtr renderer = IntPtr.Zero;
            IntPtr texture = IntPtr.Zero;
            SDL.SDL_Rect rect = new SDL.SDL_Rect();

            if (videoStreamIndex != -1)
            {
                window = SDL.SDL_CreateWindow("C# SDL MP4 Player", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, pVideoCodecContext->width, pVideoCodecContext->height, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
                renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
                texture = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_IYUV, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, pVideoCodecContext->width, pVideoCodecContext->height);
                rect = new SDL.SDL_Rect { x = 0, y = 0, w = pVideoCodecContext->width, h = pVideoCodecContext->height };
            }

            // Allocate core FFmpeg data structures for unmanaged data exchange
            AVPacket* pPacket = ffmpeg.av_packet_alloc();
            AVFrame* pFrame = ffmpeg.av_frame_alloc();

            // 5. Initialize Loop States and Core Synchronization Timers

            bool running = true;
            bool isPaused = false;
            ulong startTimeTicks = SDL.SDL_GetTicks64();
            ulong pausedTimeTicks = 0;

            // 6. Main Demuxing, Decoding, and Render Event Loop
            while (running)
            {
                // Poll and process SDL user inputs (Window control, Keyboard hotkeys)
                while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        running = false;
                        continue;
                    }
                    else if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                        {
                            running = false;
                            continue;
                        }
                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_SPACE)
                        {
                            isPaused = !isPaused;
                            SDL.SDL_PauseAudioDevice(audioDevice, isPaused ? 1 : 0);
                            if (isPaused)
                            {
                                // Track exact moment of pause to accurately offset the clock upon resume
                                pausedTimeTicks = SDL.SDL_GetTicks64();
                            }
                            else
                            {
                                // Shift anchor clock forward by the exact total duration spent paused
                                startTimeTicks += (SDL.SDL_GetTicks64() - pausedTimeTicks);
                            }
                        }
                    }
                }
                // Handle explicit global playback pause state
                if (isPaused)
                {
                    SDL.SDL_Delay(10); // Throttle CPU while paused
                    continue;
                }

                // Flow Control: Throttle file reads if the hardware audio queue is saturated
                // Prevents excessive memory growth from demuxing too far ahead of real-time audio
                if (audioDevice > 0 && SDL.SDL_GetQueuedAudioSize(audioDevice) > 256 * 1024) //was 512
                {
                    SDL.SDL_Delay(10);
                    continue;
                }

                // 7. Demux Next Data Packet from Multimedia Container
                if (ffmpeg.av_read_frame(pFormatContext, pPacket) >= 0)
                {
                    // Process Video Data Packet
                    if (pPacket->stream_index == videoStreamIndex && videoStreamIndex != -1)
                    {
                        // Send raw encoded packet to video decoder pipeline
                        ffmpeg.avcodec_send_packet(pVideoCodecContext, pPacket);
                        // Retrieve raw decompressed YUV frames from decoder
                        while (ffmpeg.avcodec_receive_frame(pVideoCodecContext, pFrame) == 0)
                        {
                            // Clock Synchronization: Convert Presentation Timestamp (PTS) units to absolute milliseconds
                            double videoPtsSeconds = pFrame->pts * ffmpeg.av_q2d(videoTimeBase);
                            ulong targetFrameTimeMs = (ulong)(videoPtsSeconds * 1000.0);
                            ulong elapsedPlaybackTimeMs = SDL.SDL_GetTicks64() - startTimeTicks;

                            // Evaluate if frame is ready or requires a render delay
                            if (targetFrameTimeMs > elapsedPlaybackTimeMs)
                            {
                                //We're early, wait a bit...
                                SDL.SDL_Delay((uint)(targetFrameTimeMs - elapsedPlaybackTimeMs));
                            }
                            // FIX: Only skip if the frame is massively late (e.g., more than 50ms), 
                            // otherwise just render it anyway to keep playback smooth.
                            if (targetFrameTimeMs < elapsedPlaybackTimeMs && (elapsedPlaybackTimeMs - targetFrameTimeMs) > 50)
                            {
                                continue;
                            }

                            // Update YUV texture safely with native parameters
                            SDL.SDL_UpdateYUVTexture(
                                texture,
                                ref rect,
                                (IntPtr)pFrame->data[0], pFrame->linesize[0],
                                (IntPtr)pFrame->data[1], pFrame->linesize[1],
                                (IntPtr)pFrame->data[2], pFrame->linesize[2]
                            );

                            // Render Frame
                            SDL.SDL_RenderClear(renderer);
                            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero);
                            SDL.SDL_RenderPresent(renderer);
                        }
                    }
                    // HANDLE AUDIO STREAM
                    else if (pPacket->stream_index == audioStreamIndex && audioStreamIndex != -1)
                    {
                        ffmpeg.avcodec_send_packet(pAudioCodecContext, pPacket);
                        while (ffmpeg.avcodec_receive_frame(pAudioCodecContext, pFrame) == 0)
                        {
                            // Resample arbitrary compressed format data array to standard 16-bit linear layout
                            int convertedSamples = ffmpeg.swr_convert(
                                pSwrContext,
                                &audioResampleBuffer,
                                maxTargetNbSamples,
                                ((byte**)&pFrame->data),
                                pFrame->nb_samples
                            );

                            if (convertedSamples > 0)
                            {
                                int rawAudioDataSize = ffmpeg.av_samples_get_buffer_size(null, pAudioCodecContext->ch_layout.nb_channels, convertedSamples, AVSampleFormat.AV_SAMPLE_FMT_S16, 1);

                                //// Capture the exact PTS when this specific block is pushed to the device
                                //if (pFrame->pts != ffmpeg.AV_NOPTS_VALUE)
                                //{
                                //    double currentAudioPts = pFrame->pts * ffmpeg.av_q2d(audioTimeBase);
                                //}
                                // Push the decoded chunk directly into SDL's unmanaged playback device queue
                                SDL.SDL_QueueAudio(audioDevice, (IntPtr)audioResampleBuffer, (uint)rawAudioDataSize); //
                            }
                        }
                    }

                    ffmpeg.av_packet_unref(pPacket);
                }

                else
                {
                    // End of File. Allow trailing audio bytes to flush completely out
                    if (audioDevice > 0 && SDL.SDL_GetQueuedAudioSize(audioDevice) > 0) SDL.SDL_Delay(10);
                    else running = false;
                }
            }

            // 6. Cleanup unmanaged blocks
            if (audioResampleBuffer != null) ffmpeg.av_free(audioResampleBuffer);
            if (pSwrContext != null) ffmpeg.swr_free(&pSwrContext);
            if (audioDevice > 0) SDL.SDL_CloseAudioDevice(audioDevice);
            ffmpeg.av_frame_free(&pFrame);
            ffmpeg.av_packet_free(&pPacket);
            if (pVideoCodecContext != null) ffmpeg.avcodec_free_context(&pVideoCodecContext);
            if (pAudioCodecContext != null) ffmpeg.avcodec_free_context(&pAudioCodecContext);
            ffmpeg.avformat_close_input(&pFormatContext);
            if (videoStreamIndex != -1)
            {

                SDL.SDL_DestroyTexture(texture);
                SDL.SDL_DestroyRenderer(renderer);
                SDL.SDL_DestroyWindow(window);
                SDL.SDL_Quit();
            }
        }
    }
}

        