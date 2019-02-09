using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace Infrastructure.ServiceInterfaces
{
    public interface ISoundManager
    {
        bool MuteAllSound { get; set; }
        bool MuteMedia { get; set; }
        bool MuteSoundEffects { get; set; }
        float MediaVolume { get; set; }
        float SoundEffectsVolume { get; set; }
    }
}
