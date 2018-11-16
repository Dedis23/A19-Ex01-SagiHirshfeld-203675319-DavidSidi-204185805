using System;

namespace SpaceInvaders
{
    public interface IKillable
    {
        void Kill();

        event Action<object> Killed;
    }
}
