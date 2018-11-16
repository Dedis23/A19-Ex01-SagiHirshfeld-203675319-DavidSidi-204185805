using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Gun : GameComponent
    {
        private readonly HashSet<Bullet> r_BulletsFired;
        private readonly object r_Shooter;

        public int NumberOfShotBulletsInScreen
        {
            get
            {
                return r_BulletsFired.Count;
            }
        }

        public Gun(Game i_Game, object i_Shooter) : base(i_Game)
        {
            r_BulletsFired = new HashSet<Bullet>();
            r_Shooter = i_Shooter;
        }

        public void Shoot(Vector2 i_From, eShootingDirection i_Direction, Color i_BulletColor)
        {
            Bullet newBullet = DrawableObjectsFactory.Create(this.Game, DrawableObjectsFactory.eSpriteType.Bullet) as Bullet;
            Vector2 centralizedShootingPosition = getCentralizedShootingPosition(newBullet, i_From, i_Direction);
            newBullet.PositionX = centralizedShootingPosition.X;
            newBullet.PositionY = centralizedShootingPosition.Y;
            newBullet.Tint = i_BulletColor;
            newBullet.Direction = i_Direction;
            newBullet.Shooter = r_Shooter;
            this.Game.Components.Add(newBullet);
            r_BulletsFired.Add(newBullet);
            newBullet.Killed += (bullet) => r_BulletsFired.Remove(bullet as Bullet);
        }

        private Vector2 getCentralizedShootingPosition(Bullet i_Bullet, Vector2 i_From, eShootingDirection i_Direction)
        {
            float centralizedX = i_From.X - (0.5f * i_Bullet.Width);
            float centralizedY = i_Direction == eShootingDirection.Up ? i_From.Y - i_Bullet.Height : i_From.Y + i_Bullet.Height;
            return new Vector2(centralizedX, centralizedY);
        }
    }
}
