using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public class Invader : Sprite, ICollideable, IShooter, IEnemy
    {
        private readonly Gun r_Gun;

        public int PointsValue { get; set; }

        public Color BulletsColor { get; } = Color.Blue;

        public Invader(Game i_Game, string i_SourceFileURL, Color i_Tint, int i_PointsValue) : base(i_SourceFileURL ,i_Game)
        {
            TintColor = i_Tint;
            PointsValue = i_PointsValue;
            r_Gun = new Gun(this);
        }

        public void Shoot()
        {
            r_Gun.Shoot(eDirection.Down);
        }
    }
}