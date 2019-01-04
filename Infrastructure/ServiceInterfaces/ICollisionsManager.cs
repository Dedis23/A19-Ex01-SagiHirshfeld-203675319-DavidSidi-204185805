//*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ServiceInterfaces
{
    public interface ICollidable
    {
        event EventHandler<EventArgs> PositionChanged;
        event EventHandler<EventArgs> SizeChanged;
        event EventHandler<EventArgs> VisibleChanged;
        event EventHandler<EventArgs> VulnerableChanged;
        event EventHandler<EventArgs> Disposed;        
        bool Visible { get; }
        bool Vulnerable { get; }
        bool CheckCollision(ICollidable i_Source);
        void Collided(ICollidable i_Collidable);
    }

    public interface ICollidable2D : ICollidable
    {
        Rectangle Bounds { get; }
        Vector2 Velocity { get; }
        Texture2D Texture { get; }        
    }

    public interface ICollidable3D : ICollidable
    {
        BoundingBox Bounds { get; }
        Vector3 Velocity { get; }
    }

    public interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
}
