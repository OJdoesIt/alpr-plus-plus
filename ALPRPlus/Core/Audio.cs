using Rage.Native;
using Stealth.Plugins.ALPRPlus.Common;
using System;

namespace Stealth.Plugins.ALPRPlus.Core
{
    internal static class Audio
    {
        private static void GetSound(ESounds selectedSound, out string soundSet, out string soundName)
        {
            switch (selectedSound)
            {
                case ESounds.PinButton:
                    {
                        soundSet = "ATM_SOUNDS";
                        soundName = "PIN_BUTTON";
                        break;
                    }
                case ESounds.TimerStop:
                    {
                        soundSet = "HUD_MINI_GAME_SOUNDSET";
                        soundName = "TIMER_STOP";
                        break;
                    }
                case ESounds.ThermalVisionOn:
                    {
                        soundSet = "";
                        soundName = "THERMAL_VISION_GOGGLES_ON_MASTER";
                        break;
                    }
                case ESounds.ThermalVisionOff:
                    {
                        soundSet = "";
                        soundName = "THERMAL_VISION_GOGGLES_OFF_MASTER";
                        break;
                    }
                default:
                    {
                        soundSet = "ATM_SOUNDS";
                        soundName = "PIN_BUTTON";
                        break;
                    }
            }
        }

        internal static void PlaySound(ESounds selectedSound)
        {
            string soundSet = "";
            string soundName = "";
            GetSound(selectedSound, out soundSet, out soundName);
            NativePlaySound(soundSet, soundName);
        }

        private static void NativePlaySound(string soundSet, string soundName)
        {
            try
            {
                if (soundSet == "")
                {
                    NativeFunction.Natives.x67C540AA08E4A6F5(-1, soundName, 0 , 1); //AUDIO::PLAY_SOUND_FRONTEND
                }
                else if (soundSet == "ATM_SOUNDS")
                {
                    NativeFunction.Natives.x67C540AA08E4A6F5(-1, soundName, soundSet, 1); //AUDIO::PLAY_SOUND_FRONTEND
                }
                else
                {
                    NativeFunction.Natives.x7FF4944CC209192D(-1, soundName, soundSet, 0, 0, 0); //AUDIO::PLAY_SOUND
                }
            }
            catch (Exception e)
            {
                Logger.LogTrivialDebug("Error calling sound native -- " + e.ToString());
            }
        }

        internal enum ESounds
        {
            PinButton = 1,
            TimerStop = 2,
            ThermalVisionOn = 3,
            ThermalVisionOff = 4
        }
    }
}
