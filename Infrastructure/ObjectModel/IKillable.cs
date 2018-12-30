using System;

namespace Infrastructure.ObjectModel
{
    public interface IKillable
    {
        void Kill();

        event Action<object> Killed;
    }
}