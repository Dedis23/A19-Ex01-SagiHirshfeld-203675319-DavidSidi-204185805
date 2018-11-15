using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public interface IKillable
    {
        void Kill();
        event Action<object> Killed;
    }
}
