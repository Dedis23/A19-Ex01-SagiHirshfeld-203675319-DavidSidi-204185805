using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool IsAllSoundMuted
        {
            get
            {
                return IsMediaMuted && AreSoundEffectsMuted;
            }

            set
            {
                IsMediaMuted = AreSoundEffectsMuted = value;
            }
        }

        public bool IsMediaMuted
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

        private float m_SoundEffectsMasterVolumeBeforeMute;
        private bool m_AreSoundEffectsMuted;

        public bool AreSoundEffectsMuted
        {
            get
            {
                return m_AreSoundEffectsMuted;
            }

            set
            {
                if (m_AreSoundEffectsMuted != value)
                {
                    m_AreSoundEffectsMuted = value;
                    if (m_AreSoundEffectsMuted)
                    {
                        m_SoundEffectsMasterVolumeBeforeMute = SoundEffect.MasterVolume;
                        SoundEffect.MasterVolume = 0;
                    }

                    else
                    {
                        SoundEffect.MasterVolume = m_SoundEffectsMasterVolumeBeforeMute;
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
                return SoundEffect.MasterVolume;
            }

            set
            {
                SoundEffect.MasterVolume = MathHelper.Clamp(value, 0, 1);
            }

        }
    }
}
