using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.ObjectModel
{
    public class Sound
    {
        private SoundEffectInstance r_SoundEffectInstance;
        protected string m_AssetName;

        public string AssetName
        {
            get { return m_AssetName; }
            set { m_AssetName = value; }
        }

        public Sound(Game i_Game, string i_AssetName)
        {
            r_SoundEffectInstance = i_Game.Content.Load<SoundEffect>(i_AssetName).CreateInstance();
            r_SoundEffectInstance.Volume = 1.0f;
            r_SoundEffectInstance.IsLooped = false;
        }

        bool Loop
        {
            get { return r_SoundEffectInstance.IsLooped; }
            set { r_SoundEffectInstance.IsLooped = value; }
        }

        public void Play()
        {
            r_SoundEffectInstance.Play();
        }

        public void PlayLooped()
        {
            r_SoundEffectInstance.IsLooped = true;
            Play();
        }

        float Volume
        {
            get
            {
                return r_SoundEffectInstance.Volume;
            }
            set
            {
                r_SoundEffectInstance.Volume = value;
                r_SoundEffectInstance.Volume = MathHelper.Clamp(r_SoundEffectInstance.Volume, 0.0f, 1.0f);
            }
        }
    }
}