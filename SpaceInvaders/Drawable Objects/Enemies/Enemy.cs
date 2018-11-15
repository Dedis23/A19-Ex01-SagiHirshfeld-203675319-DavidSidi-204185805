using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Enemy : Drawable2DGameComponent, ICollideable
    {
        private Gun m_Gun;
        public int PointsValue { get; set; }

        public Enemy(Game i_Game, string i_SourceFileURL, Color i_Tint, int i_PointsValue) : base(i_Game, i_SourceFileURL)
        {
            Tint = i_Tint;
            PointsValue = i_PointsValue;
            m_Gun = new Gun(i_Game, this);
        }

        public void Shoot()
        {
            Vector2 positionToShootFrom = new Vector2(this.Position.X + 0.5f * this.Width, this.Bottom + 1);
            m_Gun.Shoot(positionToShootFrom, eShootingDirection.Down, Color.Blue);
        }
    }
}