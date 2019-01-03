using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Gun
    {
        private readonly HashSet<Bullet> r_BulletsFired;       
        private readonly IShooter r_Shooter;
        private readonly int r_MaxBulletsInScreen;
        private readonly BulletsFactory r_BulletsFactory;

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

            r_BulletsFactory = r_Shooter.Game.Services.GetService(typeof(BulletsFactory)) as BulletsFactory;
            if (r_BulletsFactory == null)
            {
                r_BulletsFactory = new BulletsFactory(r_Shooter.Game);
            }
        }

        public void Shoot(Vector2 i_DirectionVector)
        {
            if (NumberOfShotBulletsInScreen < r_MaxBulletsInScreen)
            { 
                shootBullet(i_DirectionVector);
            }
        }

        private void shootBullet(Vector2 i_DirectionVector)
        {
            Bullet newBullet = r_BulletsFactory.GetBullet();

            newBullet.Position = getCentralizedShootingPosition(newBullet, r_Shooter);
            newBullet.TintColor = r_Shooter.BulletsColor;
            newBullet.Shooter = r_Shooter;
            newBullet.SpriteKilled += onBulletDestroyed;

            newBullet.Fly(i_DirectionVector);
            r_BulletsFired.Add(newBullet);
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
