using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders
{
    public interface IShooter
    {
        Game Game { get; }

        Vector2 Position { get; }

        Color BulletsColor { get; }

        Rectangle Bounds { get; }

        SoundEffectInstance ShootingSoundEffectInstance { get; }
    }
}
