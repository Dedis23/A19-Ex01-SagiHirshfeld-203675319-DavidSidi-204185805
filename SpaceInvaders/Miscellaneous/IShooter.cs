using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public interface IShooter
    {
        Game Game { get; }

        Vector2 Position { get; }

        Color BulletsColor { get; }

        Rectangle Bounds { get; }

    }
}
