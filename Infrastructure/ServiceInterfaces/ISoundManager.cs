using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceInterfaces
{
    public interface ISoundManager
    {
        bool IsAllSoundMuted { get; set; }
        float MediaVolume { get; set; }
        float SoundEffectsVolume { get; set; }
    }
}
