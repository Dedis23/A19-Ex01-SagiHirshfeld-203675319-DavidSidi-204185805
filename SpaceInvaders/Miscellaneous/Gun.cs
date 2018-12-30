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

        public void Shoot()
        {
            Bullet newBullet = new Bullet(r_Shooter.Game);
            Vector2 centralizedShootingPosition = getCentralizedShootingPosition(newBullet);
            newBullet.Position = centralizedShootingPosition;
            newBullet.TintColor = r_Shooter.BulletsColor;         
            newBullet.Shooter = r_Shooter;

            if( r_Shooter is IEnemy)
            {
                newBullet.Velocity *= -1;
            }

            r_BulletsFired.Add(newBullet);
            newBullet.SpriteKilled += (bullet) => r_BulletsFired.Remove(bullet as Bullet);
        }

        private Vector2 getCentralizedShootingPosition(Bullet i_Bullet)
        {
            float centralizedX = 0;
            float centralizedY = 0;
            
            centralizedX = r_Shooter.Position.X + (0.5f * r_Shooter.Bounds.Width) - (0.5f * i_Bullet.Width);
            if (r_Shooter is IEnemy)
            {
                centralizedY = r_Shooter.Bounds.Bottom + 1;                
            }
            else
            {
                centralizedY = r_Shooter.Bounds.Top - 1 - i_Bullet.Height;
            }            

            return new Vector2(centralizedX, centralizedY);
        }
    }
}
