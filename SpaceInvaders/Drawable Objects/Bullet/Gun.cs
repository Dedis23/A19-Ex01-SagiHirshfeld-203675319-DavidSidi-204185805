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
        private bool m_Enabled;

        public int NumberOfShotBulletsInScreen
        {
            get
            {
                return r_BulletsFired.Count;
            }
        }

        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        public Gun(IShooter i_Shooter, int i_MaxBulletsInScreen)
        {
            r_BulletsFired = new HashSet<Bullet>();
            r_Shooter = i_Shooter;
            r_MaxBulletsInScreen = i_MaxBulletsInScreen;
            m_Enabled = true;

            r_BulletsFactory = r_Shooter.Game.Services.GetService(typeof(BulletsFactory)) as BulletsFactory;
            if (r_BulletsFactory == null)
            {
                r_BulletsFactory = new BulletsFactory(r_Shooter.Game);
            }
        }

        public void Shoot(Vector2 i_DirectionVector)
        {
            if (NumberOfShotBulletsInScreen < r_MaxBulletsInScreen && Enabled)
            { 
                shootBullet(i_DirectionVector);
            }
        }

        private void shootBullet(Vector2 i_DirectionVector)
        {
            Bullet newBullet = r_BulletsFactory.GetBullet();
            configureBullet(newBullet);
            newBullet.Fly(i_DirectionVector);
        }

        private void configureBullet(Bullet i_Bullet)
        {
            i_Bullet.Position = getBulletDeploymentPos(i_Bullet);
            i_Bullet.TintColor = r_Shooter.BulletsColor;
            i_Bullet.Shooter = r_Shooter;
            i_Bullet.SpriteKilled += onBulletDestroyed;
            r_BulletsFired.Add(i_Bullet);
        }

        private Vector2 getBulletDeploymentPos(Bullet i_Bullet)
        {
            Vector2 deploymentPos = Vector2.Zero;

            deploymentPos.X = r_Shooter.Position.X + (r_Shooter.Bounds.Width / 2) - (i_Bullet.Width / 2);
            if (r_Shooter is IEnemy)
            {
                deploymentPos.Y = r_Shooter.Bounds.Bottom + 1;
            }
            else
            {
                deploymentPos.Y = r_Shooter.Bounds.Top - 1 - i_Bullet.Height;
            }

            return deploymentPos;
        }

        private void onBulletDestroyed(object i_Bullet)
        {
            Bullet bullet = i_Bullet as Bullet;
            bullet.SpriteKilled -= onBulletDestroyed;
            r_BulletsFired.Remove(bullet);
        }
    }
}
