using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Gun
    {
        private readonly HashSet<Bullet> r_BulletsFired;
        private readonly IShooter r_Shooter;
        private readonly int r_MaxBulletsInScreen;

        public int NumberOfShotBulletsInScreen
        {
            get
            {
                return r_BulletsFired.Count;
            }
        }

        public Gun(IShooter i_Shooter, int i_MaxBulletsInScreen)
        {
            r_BulletsFired = new HashSet<Bullet>();
            r_Shooter = i_Shooter;
            r_MaxBulletsInScreen = i_MaxBulletsInScreen;            
        }

        public void Shoot()
        {
            if (NumberOfShotBulletsInScreen < r_MaxBulletsInScreen)
            {
                shootBullet();
            }
        }

        private void shootBullet()
        {
            Bullet newBullet = BulletsFactory.Instance.GetBulletForShooter(r_Shooter);

            newBullet.Position = getCentralizedShootingPosition(newBullet, r_Shooter);
            newBullet.TintColor = r_Shooter.BulletsColor;
            newBullet.Shooter = r_Shooter;
            newBullet.Velocity = r_Shooter is IEnemy ? Bullet.FlyingVelocity : -Bullet.FlyingVelocity;

            r_BulletsFired.Add(newBullet);
            newBullet.SpriteKilled += onBulletDestroyed;
        }

        private Vector2 getCentralizedShootingPosition(Bullet i_Bullet, IShooter i_Shooter)
        {
            float centralizedX = 0;
            float centralizedY = 0;

            centralizedX = i_Shooter.Position.X + (0.5f * i_Shooter.Bounds.Width) - (0.5f * i_Bullet.Width);
            if (i_Shooter is IEnemy)
            {
                centralizedY = i_Shooter.Bounds.Bottom + 1;
            }
            else
            {
                centralizedY = i_Shooter.Bounds.Top - 1 - i_Bullet.Height;
            }

            return new Vector2(centralizedX, centralizedY);
        }

        private void onBulletDestroyed(object i_Bullet)
        {
            Bullet bullet = i_Bullet as Bullet;
            bullet.SpriteKilled -= onBulletDestroyed;
            r_BulletsFired.Remove(bullet);
        }
    }
}
