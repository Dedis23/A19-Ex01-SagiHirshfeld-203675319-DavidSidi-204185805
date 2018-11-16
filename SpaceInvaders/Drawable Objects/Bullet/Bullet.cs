using Microsoft.Xna.Framework;
using System;

namespace SpaceInvaders
{
    public class Bullet : Drawable2DGameComponent, ICollideable
    {
        private const int k_BulletsVelocity = 155;
        public eShootingDirection Direction { get; set; }
        public object Shooter { get; set; }

        public Bullet(Game game, string i_SourceFileURL) : base(game, i_SourceFileURL)
        {
            this.Velocity = k_BulletsVelocity;
        }

        public override void Update(GameTime i_GameTime)
        {
            float yDelta = (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
            m_Position.Y = Direction == eShootingDirection.Up ? m_Position.Y - yDelta : m_Position.Y + yDelta;

            if (!IsInScreen)
            {
                this.Kill();
            }

            base.Update(i_GameTime);
        }
    }
}
