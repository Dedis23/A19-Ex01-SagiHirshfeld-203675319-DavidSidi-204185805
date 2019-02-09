using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Infrastructure.Managers
{
    public class SoundManager : GameService, ISoundManager
    {
        public SoundManager(Game i_Game) : base(i_Game)
        {
        }

        protected override void RegisterAsService()
        {
            AddServiceToGame((typeof(ISoundManager)));
        }

        public bool MuteAllSound
        {
            get
            {
                return MuteMedia && MuteSoundEffects;
            }

            set
            {
                MuteMedia = MuteSoundEffects = value;
            }
        }

        public bool MuteMedia
        {
            get
            {
                return MediaPlayer.IsMuted;
            }

            set
            {
                MediaPlayer.IsMuted = value;
            }
        }

        private float m_SoundEffectsVolumeWhenMuted;
        private bool m_MuteSoundEffects;

        public bool MuteSoundEffects
        {
            get
            {
                return m_MuteSoundEffects;
            }

            set
            {
                if (m_MuteSoundEffects != value)
                {
                    m_MuteSoundEffects = value;
                    if (m_MuteSoundEffects)
                    {
                        m_SoundEffectsVolumeWhenMuted = SoundEffect.MasterVolume;
                        SoundEffect.MasterVolume = 0;
                    }

                    else
                    {
                        SoundEffect.MasterVolume = m_SoundEffectsVolumeWhenMuted;
                    }
                }
            }
            
        }

        public float MediaVolume
        {
            get
            {
                return MediaPlayer.Volume;
            }

            set
            {
                MediaPlayer.Volume = MathHelper.Clamp(value, 0, 1);
            }
        }

        public float SoundEffectsVolume
        {
            get
            {
                return MuteSoundEffects ? m_SoundEffectsVolumeWhenMuted : SoundEffect.MasterVolume;
            }

            set
            {
                value = MathHelper.Clamp(value, 0, 1);

                if (MuteSoundEffects)
                {
                    m_SoundEffectsVolumeWhenMuted = value;
                }

                else
                {
                    SoundEffect.MasterVolume = value;
                }
            }

        }
    }
}
