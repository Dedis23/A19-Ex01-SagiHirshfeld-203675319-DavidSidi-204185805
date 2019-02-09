using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace Infrastructure
{
    public static class ExtensionMethods
    {
        private static readonly ArgumentOutOfRangeException sr_ArgumentOutOfRangeException = new ArgumentOutOfRangeException();

        public static bool IsInRange(this float s_TheNum, float i_Low, float i_High)
        {
            return s_TheNum <= i_High && s_TheNum >= i_Low;
        }

        public static void ThrowIfNotInRange(this float s_TheNum, float i_Low, float i_High)
        {
            if (s_TheNum > i_High || s_TheNum < i_Low)
            {
                throw sr_ArgumentOutOfRangeException;
            }
        }

        public static void PauseAndThenPlay(this SoundEffectInstance i_SoundEffectInstance)
        {
            i_SoundEffectInstance.Pause();
            i_SoundEffectInstance.Play();
        }

        public static void Play(this Song i_Song, bool i_PlayLooped)
        {
            MediaPlayer.Play(i_Song);
            MediaPlayer.IsRepeating = i_PlayLooped;
        }
    }
}
