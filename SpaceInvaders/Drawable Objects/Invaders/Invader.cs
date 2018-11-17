using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Invader : Drawable2DGameComponent, ICollideable, IEnemy
    {
        private readonly Gun r_Gun;

        public int PointsValue { get; set; }

        public Invader(Game i_Game, string i_SourceFileURL, Color i_Tint, int i_PointsValue) : base(i_Game, i_SourceFileURL)
        {
            Tint = i_Tint;
            PointsValue = i_PointsValue;
            r_Gun = new Gun(i_Game, this);
        }

        public void Shoot()
        {
            Vector2 positionToShootFrom = new Vector2(this.PositionX + (0.5f * this.Width), this.Bottom + 1);
            r_Gun.Shoot(positionToShootFrom, eShootingDirection.Down, Color.Blue);
        }
    }
}