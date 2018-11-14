using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public interface ICollideable
    {
        Texture2D Texture { get; }
        Vector2 Position { get; }
        int Width { get; }
        int Height { get; }
        int Top { get; }
        int Bottom { get; }
        int Left { get; }
        int Right { get; }
    }
}
