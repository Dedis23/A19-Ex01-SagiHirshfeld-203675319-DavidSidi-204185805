using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Gun
    {
        private readonly HashSet<Bullet> r_BulletsFired;
        private readonly IShooter r_Shooter;

        public int NumberOfShotBulletsInScreen
        {
            get
            {
                return r_BulletsFired.Count;
            }
        }

        public Gun(IShooter i_Shooter)
        {
            r_BulletsFired = new HashSet<Bullet>();
            r_Shooter = i_Shooter;
        }

        public void Shoot(eDirection i_Direction)
        {
            Bullet newBullet = DrawableObjectsFactory.Create(r_Shooter.Game, DrawableObjectsFactory.eSpriteType.Bullet) as Bullet;

            Vector2 centralizedShootingPosition = getCentralizedShootingPosition(newBullet, i_Direction);
            newBullet.PositionX = centralizedShootingPosition.X;
            newBullet.PositionY = centralizedShootingPosition.Y;
            newBullet.Color = r_Shooter.BulletsColor;
            newBullet.Direction = i_Direction;
            newBullet.Shooter = r_Shooter;

            r_BulletsFired.Add(newBullet);
            newBullet.Killed += (bullet) => r_BulletsFired.Remove(bullet as Bullet);

            r_Shooter.Game.Components.Add(newBullet);
        }

        private Vector2 getCentralizedShootingPosition(Bullet i_Bullet, eDirection i_Direction)
        {
            float centralizedX = 0;
            float centralizedY = 0;

            switch (i_Direction)
            {
                case eDirection.Up:
                    centralizedX = r_Shooter.Position.X + (0.5f * r_Shooter.Width) - (0.5f * i_Bullet.Width);
                    centralizedY = r_Shooter.Top - 1 - i_Bullet.Height;
                    break;

                case eDirection.Down:
                    centralizedX = r_Shooter.Position.X + (0.5f * r_Shooter.Width) - (0.5f * i_Bullet.Width);
                    centralizedY = r_Shooter.Bottom + 1;
                    break;

                case eDirection.Left:
                    centralizedX = r_Shooter.Left - 1 - i_Bullet.Width;
                    centralizedY = r_Shooter.Position.Y + (0.5f * r_Shooter.Height) - (0.5f * i_Bullet.Height);
                    break;

                case eDirection.Right:
                    centralizedX = r_Shooter.Right + 1;
                    centralizedY = r_Shooter.Position.Y + (0.5f * r_Shooter.Height) - (0.5f * i_Bullet.Height);
                    break;
            }

            return new Vector2(centralizedX, centralizedY);
        }
    }
}
