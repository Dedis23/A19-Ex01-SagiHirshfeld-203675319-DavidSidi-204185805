using Microsoft.Xna.Framework;
using System;

namespace SpaceInvaders
{
    public interface IPlayer : IDisposable, IGameComponent, IDrawable, IUpdateable
    {
        Game Game { get; }

        string Name { get; }

        string AssetName { get; }

        int Score { get; }

        Color ScoreColor { get; }

        int Lives { get; }

        event EventHandler<EventArgs> ScoreChanged;

        event EventHandler<EventArgs> LifeLost;

        event EventHandler<EventArgs> Disposed;
    }
}
