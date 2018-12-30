using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public interface ICollideable
    {
        Texture2D Texture { get; }

        Vector2 Position { get; }

        Rectangle Bounds { get; }


    }
}
