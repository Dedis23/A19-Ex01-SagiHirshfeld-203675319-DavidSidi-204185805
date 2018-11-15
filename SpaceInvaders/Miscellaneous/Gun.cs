using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class Gun : GameComponent
    {
        private HashSet<Bullet> m_BulletsFired;
        private readonly Type k_TypeOfShooter;

        public int NumberOfShotBulletsInScreen
        {
            get
            {
                return m_BulletsFired.Count;
            }
        }

        public Gun(Game i_Game, object i_Shooter) : base(i_Game)
        {
            m_BulletsFired = new HashSet<Bullet>();
            k_TypeOfShooter = i_Shooter.GetType();
        }

        public void Shoot(Vector2 i_From, eShootingDirection i_Direction, Color i_BulletColor)
        {
            Bullet newBullet = DrawableObjectsFactory.Create(this.Game, DrawableObjectsFactory.eSpriteType.Bullet) as Bullet;
            newBullet.Position = getCentralizedShootingPosition(newBullet, i_From, i_Direction);    
            newBullet.Tint = i_BulletColor;
            newBullet.Direction = i_Direction;
            newBullet.TypeOfShooter = k_TypeOfShooter;
            this.Game.Components.Add(newBullet);
            m_BulletsFired.Add(newBullet);
            newBullet.Killed += (bullet) => m_BulletsFired.Remove(bullet as Bullet);
        }

        private Vector2 getCentralizedShootingPosition(Bullet i_Bullet, Vector2 i_From, eShootingDirection i_Direction)
        {
            float centralizedX = i_From.X - 0.5f * i_Bullet.Width;
            float centralizedY = i_Direction == eShootingDirection.Up ? i_From.Y - i_Bullet.Height : i_From.Y + i_Bullet.Height;
            return new Vector2(centralizedX, centralizedY);
        }
    }
}
