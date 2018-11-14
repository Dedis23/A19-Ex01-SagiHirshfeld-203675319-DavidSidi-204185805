using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Enemy : Drawable2DGameComponent, ICollideable
    {
        public int PointsValue { get; set; }
        
        private Gun m_Gun;
        private const float k_ChanceToShoot = 10;
        private const float k_TimeBetweenRollsInSeconds = 1;
        private float m_RemainingDelay;
        private Random m_RandomGenerator;

        public Enemy(Game i_Game, string i_SourceFileURL, Color i_Tint, int i_PointsValue) : base(i_Game, i_SourceFileURL)
        {
            Tint = i_Tint;
            PointsValue = i_PointsValue;
            m_Gun = new Gun(i_Game);

            m_RandomGenerator = new Random((int)DateTime.Now.Ticks);
        }

        public override void Update(GameTime i_GameTime)
        {
            rollForShoot(i_GameTime);
            base.Update(i_GameTime);
        }

        private void rollForShoot(GameTime i_GameTime)
        {
            m_RemainingDelay += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_RemainingDelay >= k_TimeBetweenRollsInSeconds)
            {
                if ((m_RandomGenerator.Next(1, 100) <= k_ChanceToShoot))
                {
                    Shoot();
                }

                m_RemainingDelay = 0;
            }
        }

        public void Shoot()
        {
            Vector2 positionToShootFrom = new Vector2(this.Position.X + 0.5f * this.Width, this.Bottom + 1);
            m_Gun.Shoot(positionToShootFrom, eShootingDirection.Down, Color.Blue);
        }
    }
}
