using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Invader : Drawable2DGameComponent, ICollideable, IShooter, IEnemy
    {
        private readonly Gun r_Gun;

        public int PointsValue { get; set; }

        public Color BulletsColor { get; } = Color.Blue;

        public Invader(Game i_Game, string i_SourceFileURL, Color i_Tint, int i_PointsValue) : base(i_Game, i_SourceFileURL)
        {
            Color = i_Tint;
            PointsValue = i_PointsValue;
            r_Gun = new Gun(this);
        }

        public void Shoot()
        {
            r_Gun.Shoot(eDirection.Down);
        }
    }
}