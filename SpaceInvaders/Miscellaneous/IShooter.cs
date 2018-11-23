using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public interface IShooter
    {
        Game Game { get; }

        Vector2 Position { get; }

        Color BulletsColor { get; }

        int Width { get; }

        int Height { get; }

        int Top { get; }

        int Bottom { get; }

        int Left { get; }

        int Right { get; }
    }
}
